using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCopilot.Models
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    public class RepositoryInfo
    {
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; } = "main";
        public string Tag { get; set; }
        public SiteType SiteType { get; set; }
        public long TotalSize { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<FileItem> Files { get; set; } = new List<FileItem>();
    }

    /// <summary>
    /// 文件项
    /// </summary>
    public class FileItem
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Sha { get; set; }
        public string DownloadUrl { get; set; }
        public FileType Type { get; set; }
        public bool Selected { get; set; } = true;
    }

}
