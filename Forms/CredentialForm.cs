using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DownloadCopilot.Models;

namespace DownloadCopilot
{
    public partial class CredentialForm : Form
    {
        public CredentialForm()
        {
            InitializeComponent();
            this.Icon = Utility.FromBytes(Properties.Resources.Icon);

            InitEvents();

            Credentials = new List<SiteCredential>();
        }

        public List<SiteCredential> Credentials { get; set; }

        private void InitEvents()
        {
            this.Load += (s, e) =>
            {
                cmbSite.SelectedIndex = 0;
                Credentials = ConfigManager.LoadSiteCredential() ?? new List<SiteCredential>();
            };

            cmbSite.SelectedIndexChanged += (s, e) =>
            {
                UpdateMirrorList();
            };

            cmbMirror.SelectedIndexChanged += (s, e) =>
            {
                if (Credentials != null)
                {
                    var credential = Credentials.FirstOrDefault(x => cmbMirror.SelectedItem.ToString().Contains(x.SiteName));
                    if (credential != null)
                    {
                        tbUsername.Text = credential.Username;
                        tbPwd.Text = credential.Password;
                    }
                }
                else
                {
                    tbUsername.Clear();
                    tbPwd.Clear();
                    tbUsername.Focus();
                }
            };

            btnSave.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(tbUsername.Text))
                {
                    tbUsername.Focus();
                }
                else if (string.IsNullOrEmpty(tbPwd.Text))
                {
                    tbPwd.Focus();
                }

                if (Uri.TryCreate(cmbMirror.SelectedItem.ToString(), UriKind.Absolute, out var uri))
                {
                    string siteName = uri.Host;
                    var item = Credentials.Find(t => t.SiteName.Equals(siteName, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                    {
                        item.Username = tbUsername.Text.Trim();
                        item.Password = tbPwd.Text.Trim();
                    }
                    else
                    {
                        item = new SiteCredential()
                        {
                            SiteName = siteName,
                            Username = tbUsername.Text.Trim(),
                            Password = tbPwd.Text.Trim()
                        };
                        Credentials.Add(item);
                    }

                    ConfigManager.SaveCalibrations(Credentials);

                    MessageBox.Show("保存成功.");
                }

            };
        }

        private void UpdateMirrorList()
        {
            cmbMirror.Items.Clear();
            var sites = ConfigManager.LoadMirrorSites();

            if (cmbSite.SelectedIndex == 0)
            {
                cmbMirror.Items.AddRange(sites.GithubMirrorSites.ToArray());
            }
            else // HuggingFace
            {
                cmbMirror.Items.AddRange(sites.HuggingfaceMirrorSites.ToArray());
            }

            cmbMirror.SelectedIndex = 0;
        }

    }
}
