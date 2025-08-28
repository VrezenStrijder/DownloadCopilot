using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DownloadCopilot.Models;

namespace DownloadCopilot
{
    public partial class SiteConfigForm : Form
    {

        private const string PragmaticPattern =
                            @"^(https?):\/\/" +                              // scheme
                            @"(?:" +
                                @"localhost|" +                              // localhost
                                @"(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}|" +       // domain
                                @"(?:\d{1,3}\.){3}\d{1,3}" +                 // ipv4 (宽松)
                            @")" +
                            @"(?::\d{2,5})?" +                               // :port
                            @"(?:\/[^\s?#]*)?" +                             // /path
                            @"(?:\?[^\s#]*)?" +                              // ?query
                            @"(?:\#[^\s]*)?" +                               // #fragment
                            @"$";

        private static readonly Regex PragmaticRegex = new Regex(PragmaticPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public SiteConfigForm()
        {
            InitializeComponent();
            this.Icon = Utility.FromBytes(Properties.Resources.Icon);
            InitEvents();
            MirrorSites = new MirrorSites();
        }

        public MirrorSites MirrorSites { get; set; }


        private void InitEvents()
        {
            this.Load += (s, e) =>
            {
                tbUrl.Focus();
                cmbSite.SelectedIndex = 0;
                MirrorSites = ConfigManager.LoadMirrorSites();
                if (MirrorSites != null)
                {
                    lbGithub.Items.AddRange(MirrorSites.GithubMirrorSites.ToArray());
                    lbHuggingface.Items.AddRange(MirrorSites.HuggingfaceMirrorSites.ToArray());
                }
                else
                {
                    MirrorSites = new MirrorSites();
                }

                if (lbGithub.Items.Count == 0)
                {
                    lbGithub.Items.AddRange(new[]
                   {
                        "https://github.com",
                        "https://ghproxy.com",
                        "https://mirror.ghproxy.com",
                        "https://github.moeyy.xyz",
                        "https://gh-proxy.com"
                    });
                }

                if (lbHuggingface.Items.Count == 0)
                {
                    lbHuggingface.Items.AddRange(new[]
                    {
                        "https://huggingface.co",
                        "https://hf-mirror.com"
                    });
                }
            };

            lbGithub.Enter += (s, e) =>
            {
                lbHuggingface.ClearSelected();
            };

            lbHuggingface.Enter += (s, e) =>
            {
                lbGithub.ClearSelected();
            };

            btnAdd.Click += (s, e) =>
            {
                var url = tbUrl.Text;
                if (!string.IsNullOrEmpty(url) && PragmaticRegex.IsMatch(url))
                {
                    if (cmbSite.SelectedIndex == 0)
                    {
                        lbGithub.Items.Add(url);
                    }
                    else
                    {
                        lbHuggingface.Items.Add(url);
                    }
                }
            };


            tsmiDel.Click += (s, e) =>
            {
                if (lbGithub.SelectedItem != null)
                {
                    var items = lbGithub.SelectedItems.Cast<string>().ToList();
                    for (int i = 0; i < items.Count; i++)
                    {
                        lbGithub.Items.Remove(items[i]);
                    }
                }
                else if (lbHuggingface.SelectedItem != null)
                {
                    var items = lbHuggingface.SelectedItems.Cast<string>().ToList();
                    for (int i = 0; i < items.Count; i++)
                    {
                        lbHuggingface.Items.Remove(items[i]);
                    }
                }
            };

            btnSave.Click += (s, e) =>
            {
                MirrorSites.GithubMirrorSites = lbGithub.Items.Cast<string>().ToList();
                MirrorSites.HuggingfaceMirrorSites = lbHuggingface.Items.Cast<string>().ToList();
                ConfigManager.SaveMirrorSites(MirrorSites);

                MessageBox.Show("保存成功.");
            };
        }


    }
}
