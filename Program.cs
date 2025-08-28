using DownloadCopilot.Manager;
using DownloadCopilot.Models;

namespace DownloadCopilot
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

        static async Task TestAsync()
        {
            // �������ع�����
            var manager = new UniversalDownloadManager(@"D:\Downloads");

            // ʾ��1������GitHub�ֿ�
            manager.SetSite(SiteType.GitHub);
            manager.SetMirror("https://ghproxy.com");

            await manager.DownloadRepositoryAsync(
                "microsoft/vscode",
                DownloaderType.IDM,
                branch: "main",
                fileFilter: new List<string> { ".md", ".json" }
            );

            // ʾ��2������HuggingFaceģ��
            manager.SetSite(SiteType.HuggingFace);
            manager.SetMirror("https://hf-mirror.com");

            await manager.DownloadRepositoryAsync(
                "bert-base-chinese",
                DownloaderType.Aria2c,
                fileFilter: new List<string> { ".onnx", ".json" }
            );

        }
    }
}