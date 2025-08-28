using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCopilot.Models
{
    /// <summary>
    /// 下载任务
    /// </summary>
    public class DownloadTask
    {
        public string Url { get; set; }
        public string LocalPath { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DownloadStatus Status { get; set; }
        public double Progress { get; set; }
        public string Error { get; set; }
    }

}
