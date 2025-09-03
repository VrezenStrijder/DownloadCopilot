using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DownloadCopilot.Manager;
using DownloadCopilot.Models;

namespace DownloadCopilot
{
    public partial class MainForm : Form
    {
        private UniversalDownloadManager downloadManager;
        private CancellationTokenSource cts;
        private DownloadConfig downloadConfig;

        public MainForm()
        {
            downloadConfig = ConfigManager.LoadDownloadConfig();

            InitializeComponent();
            InitializeUI();
            InitializeDownloadManager();
        }

        public SiteType SiteType { get; set; } = SiteType.GitHub;

        public RepositoryInfo SiteInfo { get; set; }

        private void InitializeUI()
        {
            // 设置窗体
            this.Text = "Download Copilot";
            this.Size = new Size(1378, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Icon = Utility.FromBytes(Properties.Resources.Icon);

            // 初始化 WebView
            InitializeWebView();
            // 创建控件
            CreateControls();
        }

        private async void InitializeWebView()
        {
            // 等待 CoreWebView2 初始化完成
            await webView.EnsureCoreWebView2Async(null);

            webView.SourceChanged += (s, e) =>
            {
                if (webView.CoreWebView2 != null)
                {
                    var currentUrl = webView.CoreWebView2.Source;
                    ExtractRepoInfo(currentUrl);
                }
            };

            webView.NavigationCompleted += async (s, e) =>
            {
                if (e.IsSuccess)
                {
                    // 检查是否需要自动填充登录信息
                    var url = webView.CoreWebView2.Source;
                    if (url.Contains("github.com/login") || url.Contains("github.com/session"))
                    {
                        // 自动登录
                        await AutoLoginGitHub();
                    }
                }
            };

            // 初始化站点
            var sites = ConfigManager.LoadMirrorSites();
            var defaultMirror = sites?.GithubMirrorSites?.First();
            if (!string.IsNullOrEmpty(defaultMirror))
            {
                webView.CoreWebView2.Navigate(defaultMirror);
            }
            else
            {
                webView.CoreWebView2.Navigate("about:blank");
            }
        }

        private void CreateControls()
        {
            // 顶部面板
            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                Padding = new Padding(10)
            };

            // 站点选择
            var lblSite = new Label
            {
                Text = "选择站点:",
                Location = new Point(10, 15),
                Size = new Size(80, 25)
            };

            var cmbSite = new ComboBox
            {
                Name = "cmbSite",
                Location = new Point(100, 12),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSite.Items.AddRange(new[] { "GitHub", "HuggingFace" });
            cmbSite.SelectedIndex = 0;
            cmbSite.SelectedIndexChanged += CmbSite_SelectedIndexChanged;

            // 镜像选择
            var lblMirror = new Label
            {
                Text = "镜像站点:",
                Location = new Point(270, 15),
                Size = new Size(80, 25)
            };

            var cmbMirror = new ComboBox
            {
                Name = "cmbMirror",
                Location = new Point(360, 12),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            UpdateMirrorList(cmbMirror, "GitHub");
            cmbMirror.SelectedIndexChanged += (s, e) =>
            {
                // 清除选中状态
                var fileList = this.Controls.Find("fileList", true).FirstOrDefault() as ListView;
                foreach (ListViewItem item in fileList.Items)
                {
                    item.Checked = false;
                }

                if (cmbMirror.SelectedIndex > 0)
                {
                    downloadManager.SetMirror(cmbMirror.SelectedItem.ToString());
                }

                if (cmbSite.SelectedIndex == 0)
                {
                    // 如果是github,则只导航至主站(镜像站访问仓库无效)
                    webView.CoreWebView2.Navigate(cmbMirror.Items?[0]?.ToString());
                }
                else
                {
                    webView.CoreWebView2.Navigate(cmbMirror.SelectedItem.ToString());
                }
            };

            // 站点配置按钮
            var btnSiteConfig = new Button
            {
                Text = "站点配置",
                Location = new Point(cmbMirror.Right + 20, 10),
                Size = new Size(80, 27)
            };
            btnSiteConfig.Click += (s, e) =>
            {
                var frm = new SiteConfigForm();
                frm.ShowDialog();
            };

            // 登录信息配置按钮
            var btnCredentialConfig = new Button
            {
                Text = "登录配置",
                Location = new Point(btnSiteConfig.Right + 10, 10),
                Size = new Size(80, 27)
            };
            btnCredentialConfig.Click += (s, e) =>
            {
                var frm = new CredentialForm();
                frm.ShowDialog();
            };

            // 下载配置按钮
            var btnDownloadConfig = new Button
            {
                Text = "下载配置",
                Location = new Point(btnCredentialConfig.Right + 10, 10),
                Size = new Size(80, 27)
            };
            btnDownloadConfig.Click += (s, e) =>
            {
                var frm = new DownloadConfigForm();
                frm.ShowDialog();
            };

            // 仓库输入
            var lblRepo = new Label
            {
                Text = "仓库地址:",
                Location = new Point(10, 50),
                Size = new Size(80, 25)
            };

            var txtRepo = new TextBox
            {
                Name = "txtRepo",
                Location = new Point(100, 47),
                Size = new Size(460, 25),
                PlaceholderText = "输入仓库地址，如: microsoft/vscode 或 https://github.com/microsoft/vscode"
            };
            txtRepo.Leave += (s, e) =>
            {
                string url = $"{cmbMirror.SelectedItem.ToString()}/{txtRepo.Text.Trim()}";
                webView.CoreWebView2.Navigate(url);

                // 执行一次获取分支
                var btnGetBranches = this.Controls.Find("btnGetBranches", true).FirstOrDefault() as Button;
                btnGetBranches?.PerformClick();
            };

            // 分支选择
            var lblBranch = new Label
            {
                Text = "分支/标签:",
                Location = new Point(10, 85),
                Size = new Size(80, 25)
            };

            var cmbBranch = new ComboBox
            {
                Name = "cmbBranch",
                Location = new Point(100, 82),
                Size = new Size(150, 25),
                Text = "main"
            };

            // 获取分支按钮
            var btnGetBranches = new Button
            {
                Name = "btnGetBranches",
                Text = "获取分支",
                Location = new Point(260, 81),
                Size = new Size(80, 27)
            };
            btnGetBranches.Click += BtnGetBranches_Click;

            // 下载器选择
            var lblDownloader = new Label
            {
                Text = "下载工具:",
                Location = new Point(360, 85),
                Size = new Size(80, 25)
            };

            var cmbDownloader = new ComboBox
            {
                Name = "cmbDownloader",
                Location = new Point(450, 82),
                Size = new Size(110, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDownloader.Items.AddRange(new[] { "IDM", "Thunder", "Aria2c", "内置下载" });
            cmbDownloader.SelectedIndex = 0;

            // 操作按钮
            var btnGetFiles = new Button
            {
                Name = "btnGetFiles",
                Text = "获取文件列表",
                Location = new Point(580, 45),
                Size = new Size(100, 30)
            };
            btnGetFiles.Click += BtnGetFiles_Click;

            var btnDownload = new Button
            {
                Name = "btnDownload",
                Text = "开始下载",
                Location = new Point(690, 45),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnDownload.Click += BtnDownload_Click;

            var btnSelectAll = new Button
            {
                Name = "btnSelectAll",
                Text = "全选",
                Location = new Point(580, 80),
                Size = new Size(50, 27)
            };
            btnSelectAll.Click += (s, e) => SelectAllFiles(true);

            var btnDeselectAll = new Button
            {
                Name = "btnDeselectAll",
                Text = "全不选",
                Location = new Point(635, 80),
                Size = new Size(65, 27)
            };
            btnDeselectAll.Click += (s, e) => SelectAllFiles(false);

            var btnFilterOnnx = new Button
            {
                Name = "btnFilterOnnx",
                Text = "仅ONNX",
                Location = new Point(705, 80),
                Size = new Size(80, 27)
            };
            btnFilterOnnx.Click += (s, e) => FilterFiles(".onnx");

            var btnFilterZip = new Button
            {
                Name = "btnFilterZip",
                Text = "仅ZIP",
                Location = new Point(790, 80),
                Size = new Size(65, 27)
            };
            btnFilterZip.Click += (s, e) => FilterFiles(".zip");

            // 下载路径
            var lblPath = new Label
            {
                Text = "保存路径:",
                Location = new Point(10, 120),
                Size = new Size(80, 25)
            };

            var txtPath = new TextBox
            {
                Name = "txtPath",
                Location = new Point(100, 117),
                Size = new Size(460, 25),
                Text = downloadConfig?.CodeSavePath ?? @"D:\Codes"
            };

            var btnBrowse = new Button
            {
                Text = "浏览...",
                Location = new Point(570, 116),
                Size = new Size(70, 27)
            };
            btnBrowse.Click += BtnBrowse_Click;

            topPanel.Controls.AddRange(new Control[]
            {
                lblSite, cmbSite, lblMirror, cmbMirror, btnSiteConfig, btnCredentialConfig, btnDownloadConfig,
                lblRepo, txtRepo,
                lblBranch, cmbBranch, btnGetBranches,
                lblDownloader, cmbDownloader,
                btnGetFiles, btnDownload,
                btnSelectAll, btnDeselectAll, btnFilterOnnx, btnFilterZip,
                lblPath, txtPath, btnBrowse
            });

            // 文件列表
            var fileList = new ListView
            {
                Name = "fileList",
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                CheckBoxes = true
            };
            fileList.Columns.Add("文件名", 400);
            fileList.Columns.Add("大小", 100);
            fileList.Columns.Add("路径", 300);

            // 底部状态栏
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                Padding = new Padding(10)
            };

            var progressBar = new ProgressBar
            {
                Name = "progressBar",
                Location = new Point(10, 10),
                Size = new Size(860, 15)
            };

            var lblStatus = new Label
            {
                Name = "lblStatus",
                Location = new Point(10, 30),
                Size = new Size(400, 25),
                Text = "就绪"
            };

            var lblProgress = new Label
            {
                Name = "lblProgress",
                Location = new Point(420, 30),
                Size = new Size(200, 25),
                Text = ""
            };

            var lblSpeed = new Label
            {
                Name = "lblSpeed",
                Location = new Point(630, 40),
                Size = new Size(200, 25),
                Text = ""
            };

            bottomPanel.Controls.AddRange(new Control[]
            {
                progressBar, lblStatus, lblProgress, lblSpeed
            });

            // 添加到窗体
            scMain.Panel1.Controls.Add(fileList);
            scMain.Panel1.Controls.Add(topPanel);
            scMain.Panel1.Controls.Add(bottomPanel);
        }

        private void InitializeDownloadManager()
        {
            var txtPath = this.Controls.Find("txtPath", true).FirstOrDefault() as TextBox;
            downloadManager = new UniversalDownloadManager(txtPath?.Text ?? (downloadConfig?.CodeSavePath ?? @"D:\Codes"));

            downloadManager.ProgressChanged += OnProgressChanged;
            downloadManager.DownloadCompleted += OnDownloadCompleted;
        }

        private void CmbSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmbSite = sender as ComboBox;
            var cmbMirror = this.Controls.Find("cmbMirror", true).FirstOrDefault() as ComboBox;

            if (cmbSite != null && cmbMirror != null)
            {
                UpdateMirrorList(cmbMirror, cmbSite.SelectedItem.ToString());

                // 更新下载管理器的站点
                SiteType = cmbSite.SelectedItem.ToString() == "GitHub"
                    ? SiteType.GitHub
                    : SiteType.HuggingFace;
                downloadManager.SetSite(SiteType);
            }

            string codesDir = downloadConfig?.CodeSavePath ?? @"D:\Codes";
            string modelsDir = downloadConfig?.ModelSavePath ?? @"D:\Models";
            string downloadDir = (cmbSite.SelectedIndex == 0) ? codesDir : modelsDir;

            var txtPath = this.Controls.Find("txtPath", true).FirstOrDefault() as TextBox;
            txtPath.Text = downloadDir;
            downloadManager.SetSaveDir(downloadDir);
        }

        private void UpdateMirrorList(ComboBox cmbMirror, string site)
        {
            cmbMirror.Items.Clear();
            var sites = ConfigManager.LoadMirrorSites();

            if (site == "GitHub")
            {
                cmbMirror.Items.AddRange(sites.GithubMirrorSites.ToArray());
            }
            else // HuggingFace
            {
                cmbMirror.Items.AddRange(sites.HuggingfaceMirrorSites.ToArray());
            }

            cmbMirror.SelectedIndex = 0;
        }


        private async void BtnGetBranches_Click(object sender, EventArgs e)
        {
            var txtRepo = this.Controls.Find("txtRepo", true).FirstOrDefault() as TextBox;
            var cmbBranch = this.Controls.Find("cmbBranch", true).FirstOrDefault() as ComboBox;

            if (string.IsNullOrEmpty(txtRepo?.Text))
            {
                MessageBox.Show("请输入仓库地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                UpdateStatus("正在获取分支列表...");

                if (SiteType == SiteType.GitHub)
                {
                    var githubSite = new GitHubSite();

                    // 调用正确的方法获取分支
                    var branches = await githubSite.GetBranchesAsync(txtRepo.Text);

                    cmbBranch.Items.Clear();
                    foreach (var branch in branches)
                    {
                        cmbBranch.Items.Add(branch);
                    }

                    if (cmbBranch.Items.Count > 0)
                    {
                        // 优先选择main或master
                        var defaultBranch = branches.FirstOrDefault(b => b == "main")
                                         ?? branches.FirstOrDefault(b => b == "master")
                                         ?? branches[0];

                        cmbBranch.SelectedItem = defaultBranch;
                    }

                    UpdateStatus($"获取到 {branches.Count} 个分支");

                    Task.Delay(300).Wait();
                    var btnGetFiles = this.Controls.Find("btnGetFiles", true).FirstOrDefault() as Button;
                    btnGetFiles?.PerformClick();
                }
                else if (SiteType == SiteType.HuggingFace)
                {
                    cmbBranch.Items.Clear();
                    cmbBranch.Items.Add("main");
                    cmbBranch.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取分支失败: {ex.Message}\n\n建议：\n1. 检查网络连接\n2. 确认仓库地址正确\n3. 可能需要GitHub Token进行认证",
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("获取分支列表失败");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void BtnGetFiles_Click(object sender, EventArgs e)
        {
            var txtRepo = this.Controls.Find("txtRepo", true).FirstOrDefault() as TextBox;
            var cmbBranch = this.Controls.Find("cmbBranch", true).FirstOrDefault() as ComboBox;
            var fileList = this.Controls.Find("fileList", true).FirstOrDefault() as ListView;

            if (string.IsNullOrEmpty(txtRepo?.Text))
            {
                MessageBox.Show("请输入仓库地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                UpdateStatus("正在获取文件列表...");

                var branch = cmbBranch?.Text;
                if (string.IsNullOrEmpty(branch))
                {
                    branch = "main";
                }
                List<FileItem> files = new List<FileItem>();

                IDownloadSite site = SiteType == SiteType.GitHub ? new GitHubSite() : new HuggingFaceSite();
                SiteInfo = await site.GetRepositoryInfoAsync(txtRepo.Text);
                files = await site.GetFileListAsync(txtRepo.Text, branch);

                fileList.Items.Clear();

                foreach (var file in files)
                {
                    var item = new ListViewItem(file.Name);

                    // 格式化文件大小
                    if (file.Size >= 0)
                    {
                        item.SubItems.Add(FormatFileSize(file.Size));
                    }
                    else
                    {
                        item.SubItems.Add("未知");
                    }

                    item.SubItems.Add(file.Path);
                    item.Tag = file;

                    // 默认不选中，让用户自己选择
                    item.Checked = file.Type == FileType.Archive; // 默认选中压缩包

                    // 如果是压缩包，使用不同的颜色
                    if (file.Type == FileType.Archive)
                    {
                        item.ForeColor = Color.Blue;
                        item.Font = new Font(item.Font, FontStyle.Bold);
                    }

                    fileList.Items.Add(item);
                }

                UpdateStatus($"获取到 {files.Count} 个文件/选项");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取文件列表失败: {ex.Message}\n\n可能的原因：\n1. 分支名称不正确\n2. 网络问题\n3. API限制（需要Token）", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("获取文件列表失败");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void BtnDownload_Click(object sender, EventArgs e)
        {
            var txtRepo = this.Controls.Find("txtRepo", true).FirstOrDefault() as TextBox;
            var cmbBranch = this.Controls.Find("cmbBranch", true).FirstOrDefault() as ComboBox;
            var cmbDownloader = this.Controls.Find("cmbDownloader", true).FirstOrDefault() as ComboBox;
            var fileList = this.Controls.Find("fileList", true).FirstOrDefault() as ListView;

            if (string.IsNullOrEmpty(txtRepo?.Text))
            {
                MessageBox.Show("请输入仓库地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 获取选中的文件
            var selectedItems = new List<FileItem>();
            bool hasArchive = false;

            foreach (ListViewItem item in fileList.CheckedItems)
            {
                if (item.Tag is FileItem file)
                {
                    selectedItems.Add(file);
                    if (file.Type == FileType.Archive)
                    {
                        hasArchive = true;
                    }
                }
            }

            if (!selectedItems.Any())
            {
                MessageBox.Show("请选择要下载的文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 如果选择了压缩包，询问是否只下载压缩包
            if (hasArchive && selectedItems.Count > 1)
            {
                var result = MessageBox.Show(
                    "您选择了分支压缩包和其他文件。\n\n" +
                    "是 - 只下载压缩包\n" +
                    "否 - 下载所有选中的文件\n" +
                    "取消 - 取消下载",
                    "下载确认",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    return;
                }
                if (result == DialogResult.Yes)
                {
                    selectedItems = selectedItems.Where(f => f.Type == FileType.Archive).ToList();
                }
            }

            try
            {
                var btnDownload = sender as Button;
                btnDownload.Enabled = false;

                var downloaderType = (DownloaderType)(cmbDownloader?.SelectedIndex ?? 0);
                var branch = cmbBranch?.Text ?? "main";

                var githubSite = new GitHubSite();

                // 设置镜像
                var cmbMirror = this.Controls.Find("cmbMirror", true).FirstOrDefault() as ComboBox;

                if (cmbMirror != null && cmbMirror.SelectedIndex > 0)
                {
                    githubSite.MirrorUrl = cmbMirror.SelectedItem.ToString();
                    selectedItems.ForEach(t =>
                    {
                        if (!t.DownloadUrl.StartsWith(githubSite.MirrorUrl))
                        {
                            t.DownloadUrl = $"{githubSite.MirrorUrl}/{t.DownloadUrl}";
                        }
                    });
                }

                // 下载文件
                bool flag = await DownloadFilesAsync(selectedItems, downloaderType);

                MessageBox.Show("下载完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"下载失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                var btnDownload = this.Controls.Find("btnDownload", true).FirstOrDefault() as Button;
                if (btnDownload != null)
                {
                    btnDownload.Enabled = true;
                }
            }
        }


        private async Task<bool> DownloadFilesAsync(List<FileItem> files, DownloaderType downloaderType)
        {
            var txtRepo = this.Controls.Find("txtRepo", true).FirstOrDefault() as TextBox;
            string repo = txtRepo.Text.Replace('/', Path.DirectorySeparatorChar);
            var progress = new Progress<DownloadProgressEventArgs>(OnProgressUpdate);
            var success = await downloadManager.DownloadFileAsync(files, downloaderType, repo, progress);

            return success;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择下载保存路径";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var txtPath = this.Controls.Find("txtPath", true).FirstOrDefault() as TextBox;
                    if (txtPath != null)
                    {
                        txtPath.Text = dialog.SelectedPath;
                    }
                }
            }
        }

        private void SelectAllFiles(bool selected)
        {
            var fileList = this.Controls.Find("fileList", true).FirstOrDefault() as ListView;
            if (fileList != null)
            {
                foreach (ListViewItem item in fileList.Items)
                {
                    item.Checked = selected;
                }
            }
        }

        private void FilterFiles(string extension)
        {
            var fileList = this.Controls.Find("fileList", true).FirstOrDefault() as ListView;
            if (fileList != null)
            {
                foreach (ListViewItem item in fileList.Items)
                {
                    var file = item.Tag as FileItem;
                    item.Checked = file != null && file.Name.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        private void OnProgressUpdate(DownloadProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnProgressUpdate(e)));
                return;
            }

            var progressBar = this.Controls.Find("progressBar", true).FirstOrDefault() as ProgressBar;
            var lblProgress = this.Controls.Find("lblProgress", true).FirstOrDefault() as Label;
            var lblSpeed = this.Controls.Find("lblSpeed", true).FirstOrDefault() as Label;

            if (progressBar != null)
                progressBar.Value = (int)e.OverallProgress;

            if (lblProgress != null)
                lblProgress.Text = $"{e.CompletedFiles}/{e.TotalFiles} 文件";

            if (lblSpeed != null && e.Speed > 0)
            {
                lblSpeed.Text = $"速度: {e.Speed:F2} MB/s";
            }
            UpdateStatus($"正在下载: {e.CurrentFile}");
        }

        private void OnProgressChanged(object sender, DownloadProgressEventArgs e)
        {
            OnProgressUpdate(e);
        }

        private void OnDownloadCompleted(object sender, DownloadCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnDownloadCompleted(sender, e)));
                return;
            }

            if (e.Success)
            {
                UpdateStatus($"下载完成! 共 {e.CompletedTasks}/{e.TotalTasks} 个文件");
            }
            else
            {
                UpdateStatus($"下载失败: {e.Error}");
            }
        }

        private async void ExtractRepoInfo(string url)
        {
            var txtRepo = this.Controls.Find("txtRepo", true).FirstOrDefault() as TextBox;

            if (SiteType == SiteType.GitHub)
            {
                var githubRepoPattern = @"github\.com/([^/]+)/([^/\?]+)";
                var match = Regex.Match(url, githubRepoPattern);

                if (match.Success && match.Groups.Count >= 3)
                {
                    var owner = match.Groups[1].Value;
                    var repo = match.Groups[2].Value;

                    // 清理仓库名（去除.git后缀等）
                    repo = repo.Replace(".git", "");

                    txtRepo.Text = $"{owner}/{repo}";
                }
            }
            else if (SiteType == SiteType.HuggingFace)
            {
                var uri = new Uri(url);
                var segments = uri.AbsolutePath.Trim('/').Split('/');

                if (segments.Length < 2)
                {
                    return;
                }
                // 排除 datasets / spaces / docs / models 等非模型目录
                string first = segments[0].ToLowerInvariant();
                if (first is "datasets" or "spaces" or "docs" or "models")
                {
                    return;
                }
                string owner = segments[0];
                string model = segments[1];

                txtRepo.Text = $"{owner}/{model}";
            }


        }

        private void UpdateStatus(string message)
        {
            var lblStatus = this.Controls.Find("lblStatus", true).FirstOrDefault() as Label;
            if (lblStatus != null)
            {
                lblStatus.Text = message;
            }
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }


        #region 自动登录(github)

        private async Task AutoLoginGitHub()
        {
            try
            {
                var crdentials = ConfigManager.LoadSiteCredential();
                var githubCreds = crdentials.Find(t => t.SiteName.Equals("github.com", StringComparison.OrdinalIgnoreCase));
                if (githubCreds == null)
                {
                    return;
                }

                // JavaScript代码来填充表单并提交
                string script = $@"
                    (function() {{
                        // 等待页面加载
                        function fillLoginForm() {{
                            // 查找用户名输入框（GitHub的多种可能选择器）
                            var usernameInput = document.querySelector('#login_field') || 
                                               document.querySelector('input[name=""login""]') ||
                                               document.querySelector('input[type=""text""][name=""user[login]""]') ||
                                               document.querySelector('input[autocomplete=""username""]');

                            // 查找密码输入框
                            var passwordInput = document.querySelector('#password') || 
                                               document.querySelector('input[name=""password""]') ||
                                               document.querySelector('input[type=""password""]');

                            // 查找提交按钮
                            var submitButton = document.querySelector('input[type=""submit""]') ||
                                              document.querySelector('button[type=""submit""]') ||
                                              document.querySelector('.btn-primary');

                            if (usernameInput && passwordInput) {{
                                // 填充用户名
                                usernameInput.value = '{EscapeJavaScriptString(githubCreds.Username)}';
                                usernameInput.dispatchEvent(new Event('input', {{ bubbles: true }}));
                                usernameInput.dispatchEvent(new Event('change', {{ bubbles: true }}));

                                // 填充密码
                                passwordInput.value = '{EscapeJavaScriptString(githubCreds.Password)}';
                                passwordInput.dispatchEvent(new Event('input', {{ bubbles: true }}));
                                passwordInput.dispatchEvent(new Event('change', {{ bubbles: true }}));

                                // 可选：自动提交表单
                                if (submitButton) {{
                                    setTimeout(() => {{
                                        // submitButton.click(); // 如果需要自动提交，取消注释
                                        console.log('表单已填充，可以手动点击登录');
                                    }}, 500);
                                }}

                                return 'success';
                            }} else {{
                                return 'not_found';
                            }}
                        }}

                        return fillLoginForm();
                    }})();
                ";

                var result = await webView.CoreWebView2.ExecuteScriptAsync(script);

                //if (result.Contains("success"))
                //{
                //    MessageBox.Show("登录信息已填充，请点击登录按钮或等待自动登录");
                //}
                //else if (result.Contains("not_found"))
                //{
                //    MessageBox.Show("未找到登录表单，请确保在登录页面");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"自动登录失败: {ex.Message}");
            }
        }

        private string EscapeJavaScriptString(string str)
        {
            return str.Replace("\\", "\\\\")
                     .Replace("'", "\\'")
                     .Replace("\"", "\\\"")
                     .Replace("\r", "\\r")
                     .Replace("\n", "\\n");
        }

        #endregion

    }
}
