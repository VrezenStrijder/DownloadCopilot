using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DownloadCopilot.Models;
using Ookii.Dialogs.WinForms;

namespace DownloadCopilot
{
    public partial class DownloadConfigForm : Form
    {
        public DownloadConfigForm()
        {
            InitializeComponent();
            this.Icon = Utility.FromBytes(Properties.Resources.Icon);
            Init();
            InitEvents();
        }

        private void Init()
        {
            Config = ConfigManager.LoadDownloadConfig();
        }

        private void InitEvents()
        {
            this.Load += (s, e) =>
            {
                if (Config != null)
                {
                    tbCodeSaveFolder.Text = Config.CodeSavePath;
                    tbModelSaveFolder.Text = Config.ModelSavePath;
                    tbIDMPath.Text = Config.IDMFilePath;
                    tbThunderPath.Text = Config.ThunderFilePath;
                    tbAria2Path.Text = Config.Aria2FilePath;
                }

                this.btnCodeSaveFolder.Focus();
            };

            this.btnCodeSaveFolder.Click += (s, e) =>
            {
                using (var dlg = new VistaFolderBrowserDialog()
                {
                    Description = "选择代码文件导出目录",
                    ShowNewFolderButton = true,
                    UseDescriptionForTitle = true,
                })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        tbCodeSaveFolder.Text = dlg.SelectedPath;
                    }
                }
            };

            this.btnModelSaveFolder.Click += (s, e) =>
            {
                using (var dlg = new VistaFolderBrowserDialog()
                {
                    Description = "选择代码文件导出目录",
                    ShowNewFolderButton = true,
                    UseDescriptionForTitle = true,
                })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        tbModelSaveFolder.Text = dlg.SelectedPath;
                    }
                }
            };

            this.btnIDMPath.Click += (s, e) =>
            {
                using (var dlg = new VistaOpenFileDialog()
                {
                    Title = "选择IDM路径",
                    Filter = "exe文件(*.exe)|*.exe|所有文件(*.*)|*.*",
                    DefaultExt = "exe",
                    Multiselect = false
                })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        tbIDMPath.Text = dlg.FileName;
                    }
                }
            };

            this.btnThunderPath.Click += (s, e) =>
            {
                using (var dlg = new VistaOpenFileDialog()
                {
                    Title = "选择迅雷路径",
                    Filter = "exe文件(*.exe)|*.exe|所有文件(*.*)|*.*",
                    DefaultExt = "exe",
                    Multiselect = false
                })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        tbThunderPath.Text = dlg.FileName;
                    }
                }
            };

            this.btnAria2Path.Click += (s, e) =>
            {
                using (var dlg = new VistaOpenFileDialog()
                {
                    Title = "选择Aria2路径",
                    Filter = "exe文件(*.exe)|*.exe|所有文件(*.*)|*.*",
                    DefaultExt = "exe",
                    Multiselect = false
                })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        tbAria2Path.Text = dlg.FileName;
                    }
                }
            };

            this.btnSave.Click += (s, e) =>
            {
                DownloadConfig item = new DownloadConfig()
                {
                    CodeSavePath = tbCodeSaveFolder.Text,
                    ModelSavePath = tbModelSaveFolder.Text,
                    IDMFilePath = tbIDMPath.Text,
                    ThunderFilePath = tbThunderPath.Text,
                    Aria2FilePath = tbAria2Path.Text,
                };
                ConfigManager.SaveDownloadConfig(item);
                MessageBox.Show("保存成功.");
            };
        }

        public DownloadConfig Config { get; set; }

    }
}
