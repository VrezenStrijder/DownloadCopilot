namespace DownloadCopilot
{
    partial class MainForm
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
            scMain = new SplitContainer();
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)scMain).BeginInit();
            scMain.Panel2.SuspendLayout();
            scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            SuspendLayout();
            // 
            // scMain
            // 
            scMain.Dock = DockStyle.Fill;
            scMain.FixedPanel = FixedPanel.Panel1;
            scMain.Location = new Point(0, 0);
            scMain.Name = "scMain";
            // 
            // scMain.Panel2
            // 
            scMain.Panel2.Controls.Add(webView);
            scMain.Size = new Size(1362, 561);
            scMain.SplitterDistance = 870;
            scMain.SplitterWidth = 2;
            scMain.TabIndex = 0;
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Dock = DockStyle.Fill;
            webView.Location = new Point(0, 0);
            webView.Name = "webView";
            webView.Size = new Size(490, 561);
            webView.TabIndex = 0;
            webView.ZoomFactor = 1D;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1362, 561);
            Controls.Add(scMain);
            Name = "MainForm";
            Text = "Form2";
            scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scMain).EndInit();
            scMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer scMain;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
    }
}