using ITL.Framework;
using ITL.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITL.ParamsSettingTool
{
 //   F2010321010
 //00
 //   010000240000261A3A000C0A87BD3
 //       FFFFFF00
 //       C0A87BFE
 //       CA608685
 //       C0A87B96
 //       3125EB2A000001220C1C00000F59514B30313130205631303030423140F3
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createNew = false;
            ////系统能够识别有名称的互斥，因此可以使用它禁止应用程序启动两次           
            ////第二个参数可以设置为产品的名称:Application.ProductName            
            ////每次启动应用程序，都会验证程序名称的互斥是否存在
            Mutex mutex = new Mutex(true, "ParamsSettingTool", out createNew);

            try
            {
                if (!createNew)
                {
                    UtilityTool.BringProcessToFrontByPath(Application.ExecutablePath); //若程序已启动，则激活程序并置前

                    Application.Exit();
                    return;
                }
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.UserSkins.BonusSkins.Register();
             //   HintProvider.StartWaiting(null, "正在启动参数设置工具", "", Application.ProductName, showDelay: 0, showCloseButtonDelay: int.MaxValue);
                var main = new MainForm();
                Application.Run(main);
                //var Login = new InputPsdForm();
                //Application.Run(Login);
            }
            finally
            {
                if (createNew)
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}
