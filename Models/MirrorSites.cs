using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCopilot.Models
{
    public class MirrorSites
    {

        public MirrorSites()
        {
            GithubMirrorSites = new List<string>();
            HuggingfaceMirrorSites = new List<string>();
        }

        public List<string> GithubMirrorSites { get; set; }

        public List<string> HuggingfaceMirrorSites { get; set; }
    }
}
