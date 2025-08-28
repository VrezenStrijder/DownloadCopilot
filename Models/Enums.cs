using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCopilot.Models
{
    public enum SiteType
    {
        HuggingFace,
        GitHub,
        Custom
    }


    /// <summary>
    /// 下载器类型
    /// </summary>
    public enum DownloaderType
    {
        IDM,
        Thunder,
        Aria2c,
        BuiltIn
    }

    public enum DownloadStatus
    {
        Pending,
        Downloading,
        Completed,
        Failed,
        Paused
    }

    public enum FileType
    {
        File,
        Directory,
        Submodule,
        Symlink,
        Archive
    }

}
