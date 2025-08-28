using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCopilot.Models
{
    /// <summary>
    /// 下载站点接口
    /// </summary>
    public interface IDownloadSite
    {
        SiteType Type { get; }
        string Name { get; }
        string BaseUrl { get; }
        string MirrorUrl { get; set; }

        Task<RepositoryInfo> GetRepositoryInfoAsync(string repoPath);
        Task<List<FileItem>> GetFileListAsync(string repoPath, string branch = "main");
        string BuildDownloadUrl(string repoPath, string filePath, string branch = "main");
        Task<List<string>> GetBranchesAsync(string repoPath);
        Task<List<string>> GetTagsAsync(string repoPath);
    }

}
