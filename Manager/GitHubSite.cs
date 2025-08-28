using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DownloadCopilot.Models;

namespace DownloadCopilot.Manager
{
    public class GitHubSite : IDownloadSite
    {
        private readonly HttpClient httpClient;
        private readonly List<string> mirrorUrls;
        private string githubToken; // 添加GitHub Token支持

        public GitHubSite(string token = null)
        {
            httpClient = new HttpClient();

            // 设置请求头
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

            // 如果有token，添加认证
            if (!string.IsNullOrEmpty(token))
            {
                githubToken = token;
                httpClient.DefaultRequestHeaders.Add("Authorization", $"token {token}");
            }

            // GitHub镜像站点
            mirrorUrls = new List<string>
            {
                "https://ghproxy.com",
                "https://mirror.ghproxy.com",
                "https://github.moeyy.xyz",
                "https://gh-proxy.com",
                "https://gh.api.99988866.xyz",
                "https://github.91chi.fun"
            };

            MirrorUrl = string.Empty;
        }

        public SiteType Type => SiteType.GitHub;
        public string Name => "GitHub";
        public string BaseUrl => "https://github.com";
        public string ApiUrl => "https://api.github.com";
        public string MirrorUrl { get; set; }

        /// <summary>
        /// 解析仓库路径
        /// </summary>
        private (string owner, string repo) ParseRepoPath(string repoPath)
        {
            // 支持的输入格式
            // 1. owner/repo
            // 2. https://github.com/owner/repo
            // 3. git@github.com:owner/repo.git

            repoPath = repoPath.Trim();

            // 移除.git后缀
            if (repoPath.EndsWith(".git"))
            {
                repoPath = repoPath[..^4];
            }

            if (repoPath.StartsWith("http"))
            {
                var uri = new Uri(repoPath);
                var parts = uri.AbsolutePath.Trim('/').Split('/');
                if (parts.Length >= 2)
                {
                    return (parts[0], parts[1]);
                }
            }

            // 处理SSH格式
            if (repoPath.StartsWith("git@"))
            {
                var match = Regex.Match(repoPath, @"git@github\.com:(.+)/(.+)");
                if (match.Success)
                {
                    return (match.Groups[1].Value, match.Groups[2].Value);
                }
            }

            // 处理简单格式 owner/repo
            var simpleParts = repoPath.Split('/');
            if (simpleParts.Length == 2)
            {
                return (simpleParts[0], simpleParts[1]);
            }
            throw new ArgumentException($"无法解析仓库路径: {repoPath}");
        }

        /// <summary>
        /// 获取仓库信息
        /// </summary>
        public async Task<RepositoryInfo> GetRepositoryInfoAsync(string repoPath)
        {
            var (owner, repo) = ParseRepoPath(repoPath);

            try
            {
                var apiUrl = $"{ApiUrl}/repos/{owner}/{repo}";
                var response = await httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"获取仓库信息失败: {response.StatusCode} - {response.ReasonPhrase}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);

                var repoInfo = new RepositoryInfo
                {
                    Owner = owner,
                    Name = repo,
                    Branch = json.RootElement.GetProperty("default_branch").GetString(),
                    SiteType = SiteType.GitHub,
                    TotalSize = json.RootElement.GetProperty("size").GetInt64() * 1024, // KB to Bytes
                    LastUpdated = json.RootElement.GetProperty("updated_at").GetDateTime()
                };

                return repoInfo;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取仓库信息失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取分支列表
        /// </summary>
        public async Task<List<string>> GetBranchesAsync(string repoPath)
        {
            var (owner, repo) = ParseRepoPath(repoPath);
            var branches = new List<string>();

            try
            {
                var page = 1;
                var perPage = 100;
                bool hasMore = true;

                while (hasMore)
                {
                    var url = $"{ApiUrl}/repos/{owner}/{repo}/branches?page={page}&per_page={perPage}";
                    var response = await httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        // 如果失败，返回默认分支
                        if (branches.Count == 0)
                        {
                            branches.Add("main");
                            branches.Add("master");
                            branches.Add("dev");
                            branches.Add("develop");
                        }
                        break;
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    var json = JsonDocument.Parse(content);
                    var items = json.RootElement.EnumerateArray().ToList();

                    if (items.Count == 0)
                    {
                        hasMore = false;
                    }
                    else
                    {
                        foreach (var branch in items)
                        {
                            var branchName = branch.GetProperty("name").GetString();
                            if (!string.IsNullOrEmpty(branchName))
                            {
                                branches.Add(branchName);
                            }
                        }

                        // 检查是否还有更多页
                        hasMore = items.Count == perPage;
                        page++;
                    }
                }
            }
            catch (Exception ex)
            {
                // 如果API调用失败，添加常见的分支名称
                if (branches.Count == 0)
                {
                    branches.Add("main");
                    branches.Add("master");
                    branches.Add("dev");
                    branches.Add("develop");
                    branches.Add("v10"); // 特定项目的分支
                }
            }

            return branches;
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        public async Task<List<FileItem>> GetFileListAsync(string repoPath, string branch = "main")
        {
            var (owner, repo) = ParseRepoPath(repoPath);
            var files = new List<FileItem>();

            try
            {
                // 首先尝试获取树
                var treeUrl = $"{ApiUrl}/repos/{owner}/{repo}/git/trees/{branch}?recursive=1";
                var response = await httpClient.GetAsync(treeUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JsonDocument.Parse(content);

                    if (json.RootElement.TryGetProperty("tree", out var tree))
                    {
                        foreach (var item in tree.EnumerateArray())
                        {
                            if (item.GetProperty("type").GetString() == "blob")
                            {
                                var path = item.GetProperty("path").GetString();
                                files.Add(new FileItem
                                {
                                    Path = path,
                                    Name = Path.GetFileName(path),
                                    Size = item.TryGetProperty("size", out var size) ? size.GetInt64() : 0,
                                    Sha = item.GetProperty("sha").GetString(),
                                    DownloadUrl = BuildDownloadUrl(owner, repo, path, branch),
                                    Type = FileType.File
                                });
                            }
                        }
                    }
                }
                else
                {
                    // 如果获取树失败，尝试其他方法
                    files = await GetFileListAlternativeAsync(owner, repo, branch);
                }
            }
            catch (Exception ex)
            {
                // 使用备用方法
                files = await GetFileListAlternativeAsync(owner, repo, branch);
            }

            // 总是添加分支压缩包选项
            files.Insert(0, new FileItem
            {
                Name = $"[整个分支压缩包] {repo}-{branch}.zip",
                Path = $"{repo}-{branch}.zip",
                DownloadUrl = GetBranchArchiveUrl(owner, repo, branch),
                Type = FileType.Archive,
                Size = -1 // 表示大小未知
            });

            return files;
        }

        /// <summary>
        /// 备用获取文件列表方法
        /// </summary>
        private async Task<List<FileItem>> GetFileListAlternativeAsync(string owner, string repo, string branch)
        {
            var files = new List<FileItem>();

            try
            {
                // 尝试从releases获取
                var releasesUrl = $"{ApiUrl}/repos/{owner}/{repo}/releases/latest";
                var response = await httpClient.GetAsync(releasesUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JsonDocument.Parse(content);

                    if (json.RootElement.TryGetProperty("assets", out var assets))
                    {
                        foreach (var asset in assets.EnumerateArray())
                        {
                            files.Add(new FileItem
                            {
                                Name = asset.GetProperty("name").GetString(),
                                Path = asset.GetProperty("name").GetString(),
                                Size = asset.GetProperty("size").GetInt64(),
                                DownloadUrl = asset.GetProperty("browser_download_url").GetString(),
                                Type = FileType.File
                            });
                        }
                    }
                }
            }
            catch
            {

            }

            return files;
        }

        /// <summary>
        /// 获取分支压缩包URL
        /// </summary>
        public string GetBranchArchiveUrl(string owner, string repo, string branch, bool useZip = true)
        {
            var format = useZip ? "zip" : "tar.gz";
            var archiveUrl = $"{BaseUrl}/{owner}/{repo}/archive/refs/heads/{branch}.{format}";

            // 如果使用镜像
            if (!string.IsNullOrEmpty(MirrorUrl))
            {
                return $"{MirrorUrl}/{archiveUrl}";
            }

            return archiveUrl;
        }

        /// <summary>
        /// 构建下载URL
        /// </summary>
        public string BuildDownloadUrl(string owner, string repo, string filePath, string branch = "main")
        {
            // 使用镜像加速
            if (!string.IsNullOrEmpty(MirrorUrl))
            {
                if (MirrorUrl.Contains("ghproxy") || MirrorUrl.Contains("mirror.ghproxy"))
                {
                    return $"{MirrorUrl}/https://raw.githubusercontent.com/{owner}/{repo}/{branch}/{filePath}";
                }
                else
                {
                    return $"{MirrorUrl}/https://github.com/{owner}/{repo}/raw/{branch}/{filePath}";
                }
            }

            // 直接访问
            return $"https://raw.githubusercontent.com/{owner}/{repo}/{branch}/{filePath}";
        }

        public string BuildDownloadUrl(string repoPath, string filePath, string branch = "main")
        {
            if (!string.IsNullOrEmpty(MirrorUrl))
            {
                if (MirrorUrl.Contains("ghproxy") || MirrorUrl.Contains("mirror.ghproxy"))
                {
                    return $"{MirrorUrl}/https://raw.githubusercontent.com/{repoPath}/{branch}/{filePath}";
                }
                else
                {
                    return $"{MirrorUrl}/https://github.com/{repoPath}/raw/{branch}/{filePath}";
                }
            }

            // 直接访问
            return $"https://raw.githubusercontent.com/{repoPath}/{branch}/{filePath}";
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        public async Task<List<string>> GetTagsAsync(string repoPath)
        {
            var (owner, repo) = ParseRepoPath(repoPath);
            var tags = new List<string>();

            try
            {
                var url = $"{ApiUrl}/repos/{owner}/{repo}/tags";
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var json = JsonDocument.Parse(content);

                    foreach (var tag in json.RootElement.EnumerateArray())
                    {
                        tags.Add(tag.GetProperty("name").GetString());
                    }
                }
            }
            catch
            {
                // 忽略错误
            }

            return tags;
        }

        
    }
}