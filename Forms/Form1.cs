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
            // ��ʼ�����ع�����
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
            // ��ȡģ���б�
            var models = lbModels.Items.Cast<string>().ToList();

            cts = new CancellationTokenSource();

            // �������ذ�ť
            btnDownload.Enabled = false;
            progressBar.Value = 0;

            try
            {
                foreach (var model in models)
                {
                    lblStatus.Text = $"��������: {model}";

                    var progress = new Progress<DownloadProgressEventArgs>(p =>
                    {
                        progressBar.Value = (int)p.OverallProgress;
                        lblProgress.Text = $"{p.CompletedFiles}/{p.TotalFiles} �ļ������";
                        lblSpeed.Text = $"�ٶ�: {p.Speed:F2} MB/s";
                    });

                    var success = await downloadManager.DownloadModelAsync(
                        model,
                        onlyOnnx: chkOnlyOnnx.Checked,
                        progress: progress
                    );

                    if (!success)
                    {
                        MessageBox.Show($"����ʧ��: {model}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                MessageBox.Show("����ģ��������ɣ�", "�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                btnDownload.Enabled = true;
            }
        }

        private void OnProgressChanged(object sender, DownloadProgressEventArgs e)
        {
            // ��UI�̸߳��½���
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
                lblStatus.Text = "������ɣ�";
            }
            else
            {
                lblStatus.Text = $"����ʧ��: {e.Error}";
            }
        }

    }
}
