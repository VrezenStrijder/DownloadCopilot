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
            // 创建下载管理器
            var manager = new UniversalDownloadManager(@"D:\Downloads");

            // 示例1：下载GitHub仓库
            manager.SetSite(SiteType.GitHub);
            manager.SetMirror("https://ghproxy.com");

            await manager.DownloadRepositoryAsync(
                "microsoft/vscode",
                DownloaderType.IDM,
                branch: "main",
                fileFilter: new List<string> { ".md", ".json" }
            );

            // 示例2：下载HuggingFace模型
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