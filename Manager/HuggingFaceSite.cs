using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DownloadCopilot.Models;

namespace DownloadCopilot.Manager
{
    public class HuggingFaceSite : IDownloadSite
    {
        private readonly HttpClient httpClient;
        private readonly List<string> mirrorUrls;

        public HuggingFaceSite()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "UniversalDownloader/1.0");

            // HuggingFace镜像站点
            mirrorUrls = new List<string>
            {
                "https://huggingface.co",
                "https://hf-mirror.com"
            };

            MirrorUrl = mirrorUrls.First();
        }

        public SiteType Type => SiteType.HuggingFace;
        public string Name => "HuggingFace";
        public string BaseUrl => "https://huggingface.co";
        public string MirrorUrl { get; set; }


        public async Task<RepositoryInfo> GetRepositoryInfoAsync(string repoPath)
        {
            var apiUrl = $"{MirrorUrl}/api/models/{repoPath}";
            var response = await httpClient.GetStringAsync(apiUrl);
            var json = JsonDocument.Parse(response);

            var repoInfo = new RepositoryInfo
            {
                Name = repoPath,
                SiteType = SiteType.HuggingFace,
                LastUpdated = json.RootElement.TryGetProperty("lastModified", out var lastMod)
                    ? lastMod.GetDateTime()
                    : DateTime.Now
            };

            return repoInfo;
        }

        public async Task<List<FileItem>> GetFileListAsync(string repoPath, string branch = "main")
        {
            var files = new List<FileItem>();

            try
            {
                var apiUrl = $"{MirrorUrl}/api/models/{repoPath}";
                var response = await httpClient.GetStringAsync(apiUrl);
                var json = JsonDocument.Parse(response);

                if (json.RootElement.TryGetProperty("siblings", out var siblings))
                {
                    foreach (var file in siblings.EnumerateArray())
                    {
                        var filename = file.GetProperty("rfilename").GetString();
                        files.Add(new FileItem
                        {
                            Path = filename,
                            Name = Path.GetFileName(filename),
                            Size = file.TryGetProperty("size", out var size) ? size.GetInt64() : 0,
                            DownloadUrl = BuildDownloadUrl(repoPath, filename, branch),
                            Type = FileType.File
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"获取模型文件列表失败: {ex.Message}");
            }

            return files;
        }

        public string BuildDownloadUrl(string repoPath, string filePath, string branch = "main")
        {
            return $"{MirrorUrl}/{repoPath}/resolve/{branch}/{filePath}";
        }

        public async Task<List<string>> GetBranchesAsync(string repoPath)
        {
            // HuggingFace通常只有main分支
            return new List<string> { "main" };
        }

        public async Task<List<string>> GetTagsAsync(string repoPath)
        {
            var tags = new List<string>();

            try
            {
                var apiUrl = $"{MirrorUrl}/api/models/{repoPath}/refs";
                var response = await httpClient.GetStringAsync(apiUrl);
                var json = JsonDocument.Parse(response);

                if (json.RootElement.TryGetProperty("tags", out var tagsArray))
                {
                    foreach (var tag in tagsArray.EnumerateArray())
                    {
                        tags.Add(tag.GetString());
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
