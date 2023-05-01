using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CPF.Reflection;
using Mind_Map_Tool.src;
using NetDimension.NanUI;
using NetDimension.NanUI.HostWindow;
using NetDimension.NanUI.JavaScript;

namespace Mind_Map_Tool
{
    internal class MainWindow : Formium
    {
        //指定主页跳转地址
        public override string StartUrl { get; } = "http://res.app.local/index.html";

        // 指定窗体样式为无边框样式
        public override HostWindowType WindowType { get; } = HostWindowType.Borderless;

        public MainWindow()
        {
            Size = new Size(1024, 668);
            AllowFullScreen = true;
            // 设置标题
            Title = "思维导图工具";
            StartPosition = FormStartPosition.CenterScreen;

            // 正常情况下启动画面会在页面加载完成后自动关闭。
            // 禁用启动画面的自动隐藏。
            SplashScreen.AutoHide = false;

            // 定制启动画面

            // 设置在启动画面默认显示的图片
            SplashScreen.Image = Properties.Resources.Logo;

            // 设置启动画面的布局方式
            //SplashScreen.ImageLayout = ImageLayout.Tile;

            // 设置在启动画面默认的背景色
            SplashScreen.BackColor = ColorTranslator.FromHtml("#8368A6");

            // 添加 PictureBox 来加载 Gif 动画
            var loaderGif = new PictureBox
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Size = new Size(50, 50),
                Image = Properties.Resources.Indicator,
                SizeMode = PictureBoxSizeMode.CenterImage,
                BorderStyle = BorderStyle.None,
                BackColor = Color.Transparent,

            };

            loaderGif.Location = new Point(Width - loaderGif.Width - 20, Height - loaderGif.Height - 20);

            SplashScreen.Content.Add(loaderGif);

            // 特别提示：如果使用SystemBorderless样式，请勿将此属性设置为null。
            // 因为DWM渲染机制，如果图片为空那么整个Panel都将停止渲染，所以这里使用一张透明的png图片来骗过DWM。

            //SplashScreen.Image = Properties.Resources.TransparentBG;

            // 关闭启动画面功能
            //EnableSplashScreen = false;

        }

        protected override void OnReady()
        {
            // 页面加载完成事件
            LoadEnd += PageLoadEnd;
            // 加载Excel
            BuildTreeInfo.buildTreeInfo(@"E:\c#space\Mind_Map_Tool\Mind_Map_Tool\Resources\基构云数据指标概览.xlsx");

            // 注册方法到JS
            RegisterJavaScript();
        }

        private void PageLoadEnd(object? sender, NetDimension.NanUI.Browser.LoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                // 隐藏启动画面
                SplashScreen.Hide();
            }
        }

        private void RegisterJavaScript()
        {
            // 创建 JS 对象
            var obj = new JavaScriptObject();

            obj.Add("GetTreeList", args =>
            {
                var keyWord = args.FirstOrDefault(x => x.IsString);
                if (null == keyWord)
                {
                    return null;
                }
                var result = WebController.GetTreeList(keyWord);
                return new JavaScriptValue(result);
            });

            obj.Add("GetTreeMap", args =>
            {
                var keyWord = args.FirstOrDefault(x => x.IsString);
                if (null == keyWord)
                {
                    return null;
                }
                var result = WebController.GetTreeMap(keyWord);
                return new JavaScriptValue(result);
            });
            // 把对象注册到 JS 环境
            RegisterJavaScriptObject("tree", obj);
        }
    }
}
