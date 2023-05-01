using NetDimension.NanUI;
using System.Data;

namespace Mind_Map_Tool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            // WinFormium 类是 NanUI 的初始化启动器，使用 CreateRuntimeBuilder 方法来启动运行时初始化过程
            WinFormium.CreateRuntimeBuilder(env =>
            {
                // 初始化CGF
            }, app =>
            {
                // 初始化应用程序

                app.UseDebuggingMode();

                // 映射一个假资源
                app.UseEmbeddedFileResource("http", "res.app.local", "Web");

                app.UseMainWindow(_ => new MainWindow());
            }).Build().Run();
        }
    }
}