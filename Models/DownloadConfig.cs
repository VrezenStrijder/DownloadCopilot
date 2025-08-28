using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCopilot.Models
{
    public class DownloadConfig
    {
        public string CodeSavePath { get; set; }

        public string ModelSavePath { get; set; }

        public string IDMFilePath { get; set; }

        public string ThunderFilePath { get; set; }

        public string Aria2FilePath { get; set; }

    }
}
