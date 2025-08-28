using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DownloadCopilot.Models;
using Microsoft.Web.WebView2.Core;
using static System.Net.Mime.MediaTypeNames;

namespace DownloadCopilot.Manager
{
    public class UniversalDownloadManager : BaseDownloader
    {
        private readonly Dictionary<SiteType, IDownloadSite> sites;
        private IDownloadSite currentSite;

        public UniversalDownloadManager(string downloadDir = @"D:\Models")
            : base(downloadDir)
        {
            sites = new Dictionary<SiteType, IDownloadSite>
            {
                [SiteType.GitHub] = new GitHubSite(),
                [SiteType.HuggingFace] = new HuggingFaceSite()
            };

            currentSite = sites[SiteType.GitHub];
        }

        /// <summary>
        /// 设置当前站点
        /// </summary>
        public void SetSite(SiteType siteType)
        {
            if (sites.ContainsKey(siteType))
            {
                currentSite = sites[siteType];
            }
            else
            {
                throw new ArgumentException($"不支持的站点类型: {siteType}");
            }
        }

        /// <summary>
        /// 设置镜像URL
        /// </summary>
        public void SetMirror(string mirrorUrl)
        {
            currentSite.MirrorUrl = mirrorUrl;
        }

        public void SetSaveDir(string saveDir)
        {
            downloadDir = saveDir;
        }

        /// <summary>
        /// 获取仓库信息
        /// </summary>
        public async Task<RepositoryInfo> GetRepositoryInfoAsync(string repoPath)
        {
            return await currentSite.GetRepositoryInfoAsync(repoPath);
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        public async Task<List<FileItem>> GetFileListAsync(string repoPath, string branch = "main")
        {
            return await currentSite.GetFileListAsync(repoPath, branch);
        }

        /// <summary>
        /// 下载仓库
        /// </summary>
        public async Task<bool> DownloadRepositoryAsync(
            string repoPath,
            DownloaderType downloaderType = DownloaderType.IDM,
            string branch = "main",
            List<string> fileFilter = null,
            IProgress<DownloadProgressEventArgs> progress = null)
        {
            try
            {
                // 获取仓库信息
                var repoInfo = await GetRepositoryInfoAsync(repoPath);

                // 获取文件列表
                var files = await GetFileListAsync(repoPath, branch);

                // 应用文件过滤
                if (fileFilter != null && fileFilter.Any())
                {
                    files = files.Where(f =>
                        fileFilter.Any(filter =>
                            f.Path.EndsWith(filter, StringComparison.OrdinalIgnoreCase)
                        )
                    ).ToList();
                }

                // 创建本地目录
                var localDir = Path.Combine(downloadDir,
                    currentSite.Name,
                    repoInfo.Owner ?? "",
                    repoInfo.Name);
                Directory.CreateDirectory(localDir);

                // 创建下载任务
                var tasks = files.Select(file => new DownloadTask
                {
                    Url = file.DownloadUrl,
                    LocalPath = Path.Combine(localDir, file.Path.Replace('/', Path.DirectorySeparatorChar)),
                    FileName = file.Name,
                    FileSize = file.Size,
                    Status = DownloadStatus.Pending
                }).ToList();

                // 执行下载
                return await DownloadFilesAsync(tasks, downloaderType, progress);
            }
            catch (Exception ex)
            {
                OnDownloadCompleted(new DownloadCompletedEventArgs
                {
                    Success = false,
                    Error = ex.Message
                });
                return false;
            }
        }

        public async Task<bool> DownloadFileAsync(List<FileItem> fileItems,
            DownloaderType downloaderType = DownloaderType.IDM,
            string repo = "",
            IProgress<DownloadProgressEventArgs> progress = null)
        {
            var localDir = Path.Combine(downloadDir, currentSite.Name, repo);
            Directory.CreateDirectory(localDir);

            try
            {
                var tasks = fileItems.Select(fileItem => new DownloadTask
                {
                    Url = fileItem.DownloadUrl,
                    LocalPath = Path.Combine(localDir, fileItem.Path.Replace('/', Path.DirectorySeparatorChar)),
                    FileName = Path.GetFileName(fileItem.Path.Replace('/', Path.DirectorySeparatorChar)),
                    FileSize = fileItem.Size,
                    Status = DownloadStatus.Pending
                }).ToList();

                // 执行下载
                return await DownloadFilesAsync(tasks, downloaderType, progress);
            }
            catch (Exception ex)
            {
                OnDownloadCompleted(new DownloadCompletedEventArgs
                {
                    Success = false,
                    Error = ex.Message
                });
                return false;
            }
        }

        public async Task<bool> DownloadSingleFileAsync(FileItem fileItem,
            DownloaderType downloaderType = DownloaderType.IDM,
            string repo = "",
            IProgress<DownloadProgressEventArgs> progress = null)
        {
            var localDir = Path.Combine(downloadDir, currentSite.Name, repo);
            Directory.CreateDirectory(localDir);

            try
            {
                var task = new DownloadTask
                {
                    Url = fileItem.DownloadUrl,
                    LocalPath = Path.Combine(localDir, fileItem.Path.Replace('/', Path.DirectorySeparatorChar)),
                    FileName = Path.GetFileName(fileItem.Path.Replace('/', Path.DirectorySeparatorChar)),
                    FileSize = fileItem.Size,
                    Status = DownloadStatus.Pending
                };

                var tasks = new List<DownloadTask> { task };
                // 执行下载
                return await DownloadFilesAsync(tasks, downloaderType, progress);
            }
            catch (Exception ex)
            {
                OnDownloadCompleted(new DownloadCompletedEventArgs
                {
                    Success = false,
                    Error = ex.Message
                });
                return false;
            }
        }

        /// <summary>
        /// 实现下载文件
        /// </summary>
        protected override async Task<bool> DownloadFilesAsync(
            List<DownloadTask> tasks,
            DownloaderType downloaderType,
            IProgress<DownloadProgressEventArgs> progress)
        {
            switch (downloaderType)
            {
                case DownloaderType.IDM:
                    return await DownloadWithIDMAsync(tasks, progress);

                case DownloaderType.Thunder:
                    return await DownloadWithThunderAsync(tasks, progress);

                case DownloaderType.Aria2c:
                    return await DownloadWithAria2cAsync(tasks, progress);

                case DownloaderType.BuiltIn:
                    return await DownloadWithHttpClientAsync(tasks, progress);

                default:
                    throw new NotSupportedException($"不支持的下载器: {downloaderType}");
            }
        }

        private async Task<bool> DownloadWithIDMAsync(List<DownloadTask> tasks, IProgress<DownloadProgressEventArgs> progress)
        {
            var idmPath = GetDownloaderPath(DownloaderType.IDM);
            if (!File.Exists(idmPath))
            {
                MessageBox.Show("IDM未安装或路径不正确");
                return false;
            }

            var totalTasks = tasks.Count;
            var completedTasks = 0;

            foreach (var task in tasks)
            {
                // 创建目录
                var dir = Path.GetDirectoryName(task.LocalPath);
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                // 检查文件是否已存在
                if (File.Exists(task.LocalPath))
                {
                    var fileInfo = new FileInfo(task.LocalPath);
                    if (fileInfo.Length > 0)
                    {
                        task.Status = DownloadStatus.Completed;
                        completedTasks++;

                        progress?.Report(new DownloadProgressEventArgs
                        {
                            TotalFiles = totalTasks,
                            CompletedFiles = completedTasks,
                            CurrentFile = task.FileName,
                            OverallProgress = (double)completedTasks / totalTasks * 100
                        });

                        continue;
                    }
                }

                // IDM命令行参数
                var arguments = $"/d \"{task.Url}\" /p \"{dir}\" /f \"{task.FileName}\" /n /q";

                var processInfo = new ProcessStartInfo
                {
                    FileName = idmPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    await Task.Run(() => process.WaitForExit());

                    if (process.ExitCode == 0)
                    {
                        task.Status = DownloadStatus.Completed;
                        completedTasks++;
                    }
                    else
                    {
                        task.Status = DownloadStatus.Failed;
                        task.Error = $"IDM退出代码: {process.ExitCode}";
                    }
                }

                progress?.Report(new DownloadProgressEventArgs
                {
                    TotalFiles = totalTasks,
                    CompletedFiles = completedTasks,
                    CurrentFile = task.FileName,
                    OverallProgress = (double)completedTasks / totalTasks * 100
                });
            }

            return tasks.All(t => t.Status == DownloadStatus.Completed);
        }

        private async Task<bool> DownloadWithThunderAsync(List<DownloadTask> tasks, IProgress<DownloadProgressEventArgs> progress)
        {
            var thunderPath = GetDownloaderPath(DownloaderType.Thunder);
            if (!File.Exists(thunderPath))
            {
                MessageBox.Show("Thunder未安装或路径不正确");
                return false;
            }

            var lines = tasks.Select(t => t.Url).ToArray();
            string downloadList = string.Join("\r\n", lines);


            try
            {
                foreach (var line in lines)
                {
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = thunderPath,
                        Arguments = line,
                        UseShellExecute = true
                    };

                    using (var process = Process.Start(processInfo))
                    {
                        await Task.Run(() => process.WaitForExit());
                    }
                }

            }
            catch
            {
                return false;
            }

            return true;
        }

        private async Task<bool> DownloadWithAria2cAsync(List<DownloadTask> tasks, IProgress<DownloadProgressEventArgs> progress)
        {
            var aria2cPath = GetDownloaderPath(DownloaderType.Aria2c);
            if (!File.Exists(aria2cPath))
            {
                MessageBox.Show("Aria2c未安装或路径不正确");
                return false;
            }

            // 创建输入文件
            var inputFile = Path.Combine(Path.GetTempPath(), $"aria2_input_{Guid.NewGuid()}.txt");

            try
            {
                var lines = new List<string>();
                foreach (var task in tasks)
                {
                    var dir = Path.GetDirectoryName(task.LocalPath);
                    if (!string.IsNullOrEmpty(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    lines.Add(task.Url);
                    lines.Add($"  out={task.FileName}");
                    lines.Add($"  dir={dir}");
                }

                await File.WriteAllLinesAsync(inputFile, lines);

                var arguments = $"-i \"{inputFile}\" -x 16 -s 16 -j 5 --file-allocation=none";

                var processInfo = new ProcessStartInfo
                {
                    FileName = aria2cPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    await Task.Run(() => process.WaitForExit());
                    return process.ExitCode == 0;
                }
            }
            finally
            {
                if (File.Exists(inputFile))
                {
                    File.Delete(inputFile);
                }
            }
        }

        private async Task<bool> DownloadWithHttpClientAsync(List<DownloadTask> tasks, IProgress<DownloadProgressEventArgs> progress)
        {
            var totalTasks = tasks.Count;
            var completedTasks = 0;
            var semaphore = new SemaphoreSlim(3); // 最多3个并发

            var downloadTasks = tasks.Select(async task =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var dir = Path.GetDirectoryName(task.LocalPath);
                    if (!string.IsNullOrEmpty(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    if (File.Exists(task.LocalPath))
                    {
                        task.Status = DownloadStatus.Completed;
                        Interlocked.Increment(ref completedTasks);
                        return true;
                    }

                    using (var response = await httpClient.GetAsync(task.Url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(task.LocalPath, FileMode.Create))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }

                        task.Status = DownloadStatus.Completed;
                        Interlocked.Increment(ref completedTasks);

                        progress?.Report(new DownloadProgressEventArgs
                        {
                            TotalFiles = totalTasks,
                            CompletedFiles = completedTasks,
                            CurrentFile = task.FileName,
                            OverallProgress = (double)completedTasks / totalTasks * 100
                        });

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    task.Status = DownloadStatus.Failed;
                    task.Error = ex.Message;
                    return false;
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var results = await Task.WhenAll(downloadTasks);
            return results.All(r => r);
        }
    }
}
