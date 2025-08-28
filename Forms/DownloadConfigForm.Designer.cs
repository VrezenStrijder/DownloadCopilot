namespace DownloadCopilot
{
    partial class DownloadConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            tbCodeSaveFolder = new TextBox();
            btnCodeSaveFolder = new Button();
            btnIDMPath = new Button();
            tbIDMPath = new TextBox();
            label2 = new Label();
            btnThunderPath = new Button();
            tbThunderPath = new TextBox();
            label3 = new Label();
            btnAria2Path = new Button();
            tbAria2Path = new TextBox();
            label4 = new Label();
            btnSave = new Button();
            btnModelSaveFolder = new Button();
            tbModelSaveFolder = new TextBox();
            label5 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 33);
            label1.Name = "label1";
            label1.Size = new Size(80, 17);
            label1.TabIndex = 0;
            label1.Text = "代码保存目录";
            // 
            // tbCodeSaveFolder
            // 
            tbCodeSaveFolder.Location = new Point(119, 30);
            tbCodeSaveFolder.Name = "tbCodeSaveFolder";
            tbCodeSaveFolder.Size = new Size(330, 23);
            tbCodeSaveFolder.TabIndex = 1;
            // 
            // btnCodeSaveFolder
            // 
            btnCodeSaveFolder.Location = new Point(455, 27);
            btnCodeSaveFolder.Name = "btnCodeSaveFolder";
            btnCodeSaveFolder.Size = new Size(80, 28);
            btnCodeSaveFolder.TabIndex = 7;
            btnCodeSaveFolder.Text = "选 择";
            btnCodeSaveFolder.UseVisualStyleBackColor = true;
            // 
            // btnIDMPath
            // 
            btnIDMPath.Location = new Point(455, 125);
            btnIDMPath.Name = "btnIDMPath";
            btnIDMPath.Size = new Size(80, 28);
            btnIDMPath.TabIndex = 10;
            btnIDMPath.Text = "选 择";
            btnIDMPath.UseVisualStyleBackColor = true;
            // 
            // tbIDMPath
            // 
            tbIDMPath.Location = new Point(119, 128);
            tbIDMPath.Name = "tbIDMPath";
            tbIDMPath.Size = new Size(330, 23);
            tbIDMPath.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 131);
            label2.Name = "label2";
            label2.Size = new Size(57, 17);
            label2.TabIndex = 8;
            label2.Text = "IDM路径";
            // 
            // btnThunderPath
            // 
            btnThunderPath.Location = new Point(455, 173);
            btnThunderPath.Name = "btnThunderPath";
            btnThunderPath.Size = new Size(80, 28);
            btnThunderPath.TabIndex = 13;
            btnThunderPath.Text = "选 择";
            btnThunderPath.UseVisualStyleBackColor = true;
            // 
            // tbThunderPath
            // 
            tbThunderPath.Location = new Point(119, 176);
            tbThunderPath.Name = "tbThunderPath";
            tbThunderPath.Size = new Size(330, 23);
            tbThunderPath.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 179);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 11;
            label3.Text = "迅雷路径";
            // 
            // btnAria2Path
            // 
            btnAria2Path.Location = new Point(455, 221);
            btnAria2Path.Name = "btnAria2Path";
            btnAria2Path.Size = new Size(80, 28);
            btnAria2Path.TabIndex = 16;
            btnAria2Path.Text = "选 择";
            btnAria2Path.UseVisualStyleBackColor = true;
            // 
            // tbAria2Path
            // 
            tbAria2Path.Location = new Point(119, 224);
            tbAria2Path.Name = "tbAria2Path";
            tbAria2Path.Size = new Size(330, 23);
            tbAria2Path.TabIndex = 15;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(27, 227);
            label4.Name = "label4";
            label4.Size = new Size(62, 17);
            label4.TabIndex = 14;
            label4.Text = "Aria2路径";
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(461, 278);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 17;
            btnSave.Text = "保 存";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnModelSaveFolder
            // 
            btnModelSaveFolder.Location = new Point(455, 74);
            btnModelSaveFolder.Name = "btnModelSaveFolder";
            btnModelSaveFolder.Size = new Size(80, 28);
            btnModelSaveFolder.TabIndex = 20;
            btnModelSaveFolder.Text = "选 择";
            btnModelSaveFolder.UseVisualStyleBackColor = true;
            // 
            // tbModelSaveFolder
            // 
            tbModelSaveFolder.Location = new Point(119, 77);
            tbModelSaveFolder.Name = "tbModelSaveFolder";
            tbModelSaveFolder.Size = new Size(330, 23);
            tbModelSaveFolder.TabIndex = 19;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(27, 80);
            label5.Name = "label5";
            label5.Size = new Size(80, 17);
            label5.TabIndex = 18;
            label5.Text = "模型保存目录";
            // 
            // DownloadConfigForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(573, 320);
            Controls.Add(btnModelSaveFolder);
            Controls.Add(tbModelSaveFolder);
            Controls.Add(label5);
            Controls.Add(btnSave);
            Controls.Add(btnAria2Path);
            Controls.Add(tbAria2Path);
            Controls.Add(label4);
            Controls.Add(btnThunderPath);
            Controls.Add(tbThunderPath);
            Controls.Add(label3);
            Controls.Add(btnIDMPath);
            Controls.Add(tbIDMPath);
            Controls.Add(label2);
            Controls.Add(btnCodeSaveFolder);
            Controls.Add(tbCodeSaveFolder);
            Controls.Add(label1);
            Name = "DownloadConfigForm";
            Text = "下载配置";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox tbCodeSaveFolder;
        private Button btnCodeSaveFolder;
        private Button btnIDMPath;
        private TextBox tbIDMPath;
        private Label label2;
        private Button btnThunderPath;
        private TextBox tbThunderPath;
        private Label label3;
        private Button btnAria2Path;
        private TextBox tbAria2Path;
        private Label label4;
        private Button btnSave;
        private Button btnModelSaveFolder;
        private TextBox tbModelSaveFolder;
        private Label label5;
    }
}