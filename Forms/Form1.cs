using DownloadCopilot.Manager;
using DownloadCopilot.Models;

namespace DownloadCopilot
{
    public partial class Form1 : Form
    {

        private HuggingFaceDownloadManager downloadManager;
        private CancellationTokenSource cts;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbMirror.SelectedIndex = 0;
            cmbDownloader.SelectedIndex = 0;
        }


        private void btnAddModel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtModelKey.Text))
            {
                return;
            }
            lbModels.Items.Add(txtModelKey.Text.Trim());
            txtModelKey.Clear();
        }

        private void InitializeDownloadManager()
        {
            // 初始化下载管理器
            downloadManager = new HuggingFaceDownloadManager(
                downloadDir: txtDownloadPath.Text,
                mirrorUrl: cmbMirror.Text,
                downloaderType: (DownloaderType)cmbDownloader.SelectedIndex
            );

            downloadManager.ProgressChanged += OnProgressChanged;
            downloadManager.DownloadCompleted += OnDownloadCompleted;
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            InitializeDownloadManager();
            // 获取模型列表
            var models = lbModels.Items.Cast<string>().ToList();

            cts = new CancellationTokenSource();

            // 禁用下载按钮
            btnDownload.Enabled = false;
            progressBar.Value = 0;

            try
            {
                foreach (var model in models)
                {
                    lblStatus.Text = $"正在下载: {model}";

                    var progress = new Progress<DownloadProgressEventArgs>(p =>
                    {
                        progressBar.Value = (int)p.OverallProgress;
                        lblProgress.Text = $"{p.CompletedFiles}/{p.TotalFiles} 文件已完成";
                        lblSpeed.Text = $"速度: {p.Speed:F2} MB/s";
                    });

                    var success = await downloadManager.DownloadModelAsync(
                        model,
                        onlyOnnx: chkOnlyOnnx.Checked,
                        progress: progress
                    );

                    if (!success)
                    {
                        MessageBox.Show($"下载失败: {model}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                MessageBox.Show("所有模型下载完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                btnDownload.Enabled = true;
            }
        }

        private void OnProgressChanged(object sender, DownloadProgressEventArgs e)
        {
            // 在UI线程更新进度
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnProgressChanged(sender, e)));
                return;
            }

            progressBar.Value = (int)e.OverallProgress;
            lblCurrentFile.Text = e.CurrentFile;
            lblProgress.Text = $"{e.CompletedFiles}/{e.TotalFiles}";

            if (e.Speed > 0)
            {
                lblSpeed.Text = $"{e.Speed:F2} MB/s";
            }
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
                lblStatus.Text = "下载完成！";
            }
            else
            {
                lblStatus.Text = $"下载失败: {e.Error}";
            }
        }

    }
}
