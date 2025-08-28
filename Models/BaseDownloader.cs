using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCopilot.Models
{
    /// <summary>
    /// 下载器基类
    /// </summary>
    public abstract class BaseDownloader
    {
        protected readonly HttpClient httpClient;
        protected string downloadDir;

        public event EventHandler<DownloadProgressEventArgs> ProgressChanged;
        public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;

        protected BaseDownloader(string downloadDir)
        {
            this.downloadDir = downloadDir;
            httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(30) };
        }

        protected abstract Task<bool> DownloadFilesAsync(List<DownloadTask> tasks, DownloaderType downloaderType, IProgress<DownloadProgressEventArgs> progress);

        protected string GetDownloaderPath(DownloaderType downloaderType)
        {
            var config = ConfigManager.LoadDownloadConfig();

            switch (downloaderType)
            {
                case DownloaderType.IDM:
                    return config?.IDMFilePath ?? "";
                case DownloaderType.Thunder:
                    return config?.ThunderFilePath ?? "";
                case DownloaderType.Aria2c:
                    return config?.Aria2FilePath ?? "";
                default:
                    return config?.IDMFilePath ?? "";
            }
        }


        protected void OnProgressChanged(DownloadProgressEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }

        protected void OnDownloadCompleted(DownloadCompletedEventArgs e)
        {
            DownloadCompleted?.Invoke(this, e);
        }
    }
}

