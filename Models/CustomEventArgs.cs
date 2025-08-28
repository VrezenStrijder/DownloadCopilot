using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCopilot.Models
{

    /// <summary>
    /// 下载进度事件参数
    /// </summary>
    public class DownloadProgressEventArgs : EventArgs
    {
        public int TotalFiles { get; set; }
        public int CompletedFiles { get; set; }
        public string CurrentFile { get; set; }
        public double OverallProgress { get; set; }
        public double CurrentFileProgress { get; set; }
        public long BytesReceived { get; set; }
        public long TotalBytes { get; set; }
        public double Speed { get; set; } // MB/s
    }

    /// <summary>
    /// 下载完成事件参数
    /// </summary>
    public class DownloadCompletedEventArgs : EventArgs
    {
        public bool Success { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalTasks { get; set; }
        public string Error { get; set; }
    }
}
