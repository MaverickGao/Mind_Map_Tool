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
            // WinFormium ���� NanUI �ĳ�ʼ����������ʹ�� CreateRuntimeBuilder ��������������ʱ��ʼ������
            WinFormium.CreateRuntimeBuilder(env =>
            {
                // ��ʼ��CGF
            }, app =>
            {
                // ��ʼ��Ӧ�ó���

                app.UseDebuggingMode();

                // ӳ��һ������Դ
                app.UseEmbeddedFileResource("http", "res.app.local", "Web");

                app.UseMainWindow(_ => new MainWindow());
            }).Build().Run();
        }
    }
}