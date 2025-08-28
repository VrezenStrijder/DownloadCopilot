using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCopilot
{
    public class Utility
    {
        public static Icon FromBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return new Icon(ms);
            }
        }
    }
}
