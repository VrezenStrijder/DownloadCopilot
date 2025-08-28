namespace DownloadCopilot
{
    partial class CredentialForm
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
            cmbSite = new ComboBox();
            cmbMirror = new ComboBox();
            label1 = new Label();
            tbUsername = new TextBox();
            label2 = new Label();
            tbPwd = new TextBox();
            btnSave = new Button();
            SuspendLayout();
            // 
            // cmbSite
            // 
            cmbSite.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSite.FormattingEnabled = true;
            cmbSite.Items.AddRange(new object[] { "Github", "HuggingFace" });
            cmbSite.Location = new Point(23, 28);
            cmbSite.Name = "cmbSite";
            cmbSite.Size = new Size(121, 25);
            cmbSite.TabIndex = 0;
            // 
            // cmbMirror
            // 
            cmbMirror.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMirror.FormattingEnabled = true;
            cmbMirror.Items.AddRange(new object[] { "Github", "HuggingFace" });
            cmbMirror.Location = new Point(162, 28);
            cmbMirror.Name = "cmbMirror";
            cmbMirror.Size = new Size(221, 25);
            cmbMirror.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 84);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 2;
            label1.Text = "用户名";
            // 
            // tbUsername
            // 
            tbUsername.Location = new Point(91, 81);
            tbUsername.Name = "tbUsername";
            tbUsername.Size = new Size(150, 23);
            tbUsername.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 134);
            label2.Name = "label2";
            label2.Size = new Size(36, 17);
            label2.TabIndex = 4;
            label2.Text = "密 码";
            // 
            // tbPwd
            // 
            tbPwd.Location = new Point(91, 131);
            tbPwd.Name = "tbPwd";
            tbPwd.PasswordChar = '*';
            tbPwd.Size = new Size(150, 23);
            tbPwd.TabIndex = 5;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(283, 198);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 6;
            btnSave.Text = "保 存";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // CredentialForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(399, 240);
            Controls.Add(btnSave);
            Controls.Add(tbPwd);
            Controls.Add(label2);
            Controls.Add(tbUsername);
            Controls.Add(label1);
            Controls.Add(cmbMirror);
            Controls.Add(cmbSite);
            Name = "CredentialForm";
            Text = "登录配置";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbSite;
        private ComboBox cmbMirror;
        private Label label1;
        private TextBox tbUsername;
        private Label label2;
        private TextBox tbPwd;
        private Button btnSave;
    }
}