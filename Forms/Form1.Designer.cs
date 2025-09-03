namespace DownloadCopilot
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtDownloadPath = new TextBox();
            cmbMirror = new ComboBox();
            label2 = new Label();
            cmbDownloader = new ComboBox();
            label3 = new Label();
            btnDownload = new Button();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            lblSpeed = new Label();
            lblProgress = new Label();
            chkOnlyOnnx = new CheckBox();
            label4 = new Label();
            lblCurrentFile = new Label();
            groupBox1 = new GroupBox();
            txtModelKey = new TextBox();
            label5 = new Label();
            btnAddModel = new Button();
            lbModels = new ListBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 78);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 0;
            label1.Text = "下载路径";
            // 
            // txtDownloadPath
            // 
            txtDownloadPath.Location = new Point(90, 75);
            txtDownloadPath.Name = "txtDownloadPath";
            txtDownloadPath.Size = new Size(222, 23);
            txtDownloadPath.TabIndex = 1;
            txtDownloadPath.Text = "F:\\Models\\HuggingFace";
            // 
            // cmbMirror
            // 
            cmbMirror.FormattingEnabled = true;
            cmbMirror.Items.AddRange(new object[] { "https://hf-mirror.com" });
            cmbMirror.Location = new Point(90, 32);
            cmbMirror.Name = "cmbMirror";
            cmbMirror.Size = new Size(139, 25);
            cmbMirror.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(28, 35);
            label2.Name = "label2";
            label2.Size = new Size(56, 17);
            label2.TabIndex = 3;
            label2.Text = "目标网站";
            // 
            // cmbDownloader
            // 
            cmbDownloader.FormattingEnabled = true;
            cmbDownloader.Items.AddRange(new object[] { "IDM", "Thunder", "Aria2c", "BuiltIn" });
            cmbDownloader.Location = new Point(90, 119);
            cmbDownloader.Name = "cmbDownloader";
            cmbDownloader.Size = new Size(139, 25);
            cmbDownloader.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(28, 122);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 5;
            label3.Text = "下载方式";
            // 
            // btnDownload
            // 
            btnDownload.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnDownload.Location = new Point(32, 198);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(100, 30);
            btnDownload.TabIndex = 6;
            btnDownload.Text = "开始下载";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(22, 61);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(400, 10);
            progressBar.TabIndex = 7;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(37, 400);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 17);
            lblStatus.TabIndex = 8;
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Location = new Point(22, 29);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(35, 17);
            lblSpeed.TabIndex = 9;
            lblSpeed.Text = "速度:";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(140, 29);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(0, 17);
            lblProgress.TabIndex = 10;
            // 
            // chkOnlyOnnx
            // 
            chkOnlyOnnx.AutoSize = true;
            chkOnlyOnnx.Location = new Point(32, 150);
            chkOnlyOnnx.Name = "chkOnlyOnnx";
            chkOnlyOnnx.Size = new Size(93, 21);
            chkOnlyOnnx.TabIndex = 11;
            chkOnlyOnnx.Text = "只下载Onnx";
            chkOnlyOnnx.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(22, 85);
            label4.Name = "label4";
            label4.Size = new Size(59, 17);
            label4.TabIndex = 12;
            label4.Text = "当前下载:";
            // 
            // lblCurrentFile
            // 
            lblCurrentFile.AutoSize = true;
            lblCurrentFile.Location = new Point(87, 85);
            lblCurrentFile.Name = "lblCurrentFile";
            lblCurrentFile.Size = new Size(0, 17);
            lblCurrentFile.TabIndex = 13;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblCurrentFile);
            groupBox1.Controls.Add(progressBar);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(lblSpeed);
            groupBox1.Controls.Add(lblProgress);
            groupBox1.Location = new Point(28, 255);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(546, 129);
            groupBox1.TabIndex = 14;
            groupBox1.TabStop = false;
            groupBox1.Text = "下载进度";
            // 
            // txtModelKey
            // 
            txtModelKey.Location = new Point(413, 29);
            txtModelKey.Name = "txtModelKey";
            txtModelKey.Size = new Size(222, 23);
            txtModelKey.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(363, 32);
            label5.Name = "label5";
            label5.Size = new Size(44, 17);
            label5.TabIndex = 16;
            label5.Text = "模型Id";
            // 
            // btnAddModel
            // 
            btnAddModel.Location = new Point(641, 29);
            btnAddModel.Name = "btnAddModel";
            btnAddModel.Size = new Size(53, 23);
            btnAddModel.TabIndex = 17;
            btnAddModel.Text = "添加";
            btnAddModel.UseVisualStyleBackColor = true;
            btnAddModel.Click += btnAddModel_Click;
            // 
            // lbModels
            // 
            lbModels.FormattingEnabled = true;
            lbModels.ItemHeight = 17;
            lbModels.Location = new Point(363, 58);
            lbModels.Name = "lbModels";
            lbModels.Size = new Size(272, 140);
            lbModels.TabIndex = 18;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lbModels);
            Controls.Add(btnAddModel);
            Controls.Add(label5);
            Controls.Add(txtModelKey);
            Controls.Add(btnDownload);
            Controls.Add(groupBox1);
            Controls.Add(chkOnlyOnnx);
            Controls.Add(lblStatus);
            Controls.Add(label3);
            Controls.Add(cmbDownloader);
            Controls.Add(label2);
            Controls.Add(cmbMirror);
            Controls.Add(txtDownloadPath);
            Controls.Add(label1);
            MaximumSize = new Size(816, 489);
            MinimumSize = new Size(816, 489);
            Name = "Form1";
            Text = "Code/Model Downloader";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtDownloadPath;
        private ComboBox cmbMirror;
        private Label label2;
        private ComboBox cmbDownloader;
        private Label label3;
        private Button btnDownload;
        private ProgressBar progressBar;
        private Label lblStatus;
        private Label lblSpeed;
        private Label lblProgress;
        private CheckBox chkOnlyOnnx;
        private Label label4;
        private Label lblCurrentFile;
        private GroupBox groupBox1;
        private TextBox txtModelKey;
        private Label label5;
        private Button btnAddModel;
        private ListBox lbModels;
    }
}
