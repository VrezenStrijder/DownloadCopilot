namespace DownloadCopilot
{
    partial class SiteConfigForm
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
            components = new System.ComponentModel.Container();
            groupBox1 = new GroupBox();
            lbGithub = new ListBox();
            cmsCommand = new ContextMenuStrip(components);
            splitContainer1 = new SplitContainer();
            btnAdd = new Button();
            tbUrl = new TextBox();
            cmbSite = new ComboBox();
            btnSave = new Button();
            groupBox2 = new GroupBox();
            lbHuggingface = new ListBox();
            tsmiDel = new ToolStripMenuItem();
            groupBox1.SuspendLayout();
            cmsCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lbGithub);
            groupBox1.Dock = DockStyle.Left;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 6, 3, 3);
            groupBox1.Size = new Size(350, 317);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Github站点";
            // 
            // lbGithub
            // 
            lbGithub.ContextMenuStrip = cmsCommand;
            lbGithub.Dock = DockStyle.Fill;
            lbGithub.FormattingEnabled = true;
            lbGithub.ItemHeight = 17;
            lbGithub.Location = new Point(3, 22);
            lbGithub.Name = "lbGithub";
            lbGithub.SelectionMode = SelectionMode.MultiExtended;
            lbGithub.Size = new Size(344, 292);
            lbGithub.TabIndex = 0;
            // 
            // cmsCommand
            // 
            cmsCommand.Items.AddRange(new ToolStripItem[] { tsmiDel });
            cmsCommand.Name = "cmsCommand";
            cmsCommand.Size = new Size(181, 48);
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(btnAdd);
            splitContainer1.Panel1.Controls.Add(tbUrl);
            splitContainer1.Panel1.Controls.Add(cmbSite);
            splitContainer1.Panel1.Controls.Add(btnSave);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(groupBox2);
            splitContainer1.Panel2.Controls.Add(groupBox1);
            splitContainer1.Size = new Size(719, 382);
            splitContainer1.SplitterDistance = 63;
            splitContainer1.SplitterWidth = 2;
            splitContainer1.TabIndex = 2;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(502, 12);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(70, 30);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "添加";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // tbUrl
            // 
            tbUrl.Location = new Point(12, 16);
            tbUrl.Name = "tbUrl";
            tbUrl.Size = new Size(357, 23);
            tbUrl.TabIndex = 0;
            // 
            // cmbSite
            // 
            cmbSite.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSite.FormattingEnabled = true;
            cmbSite.Items.AddRange(new object[] { "Github", "Huggingface" });
            cmbSite.Location = new Point(375, 16);
            cmbSite.Name = "cmbSite";
            cmbSite.Size = new Size(121, 25);
            cmbSite.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Location = new Point(616, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 3;
            btnSave.Text = "保 存";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lbHuggingface);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(350, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 6, 3, 3);
            groupBox2.Size = new Size(369, 317);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Huggingface站点";
            // 
            // lbHuggingface
            // 
            lbHuggingface.ContextMenuStrip = cmsCommand;
            lbHuggingface.Dock = DockStyle.Fill;
            lbHuggingface.FormattingEnabled = true;
            lbHuggingface.ItemHeight = 17;
            lbHuggingface.Location = new Point(3, 22);
            lbHuggingface.Name = "lbHuggingface";
            lbHuggingface.SelectionMode = SelectionMode.MultiExtended;
            lbHuggingface.Size = new Size(363, 292);
            lbHuggingface.TabIndex = 0;
            // 
            // tsmiDel
            // 
            tsmiDel.Name = "tsmiDel";
            tsmiDel.Size = new Size(180, 22);
            tsmiDel.Text = "删除";
            // 
            // SiteConfigForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(719, 382);
            Controls.Add(splitContainer1);
            Name = "SiteConfigForm";
            Text = "站点配置";
            groupBox1.ResumeLayout(false);
            cmsCommand.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private SplitContainer splitContainer1;
        private Button btnSave;
        private ListBox lbGithub;
        private Button btnAdd;
        private TextBox tbUrl;
        private ComboBox cmbSite;
        private GroupBox groupBox2;
        private ListBox lbHuggingface;
        private ContextMenuStrip cmsCommand;
        private ToolStripMenuItem tsmiDel;
    }
}