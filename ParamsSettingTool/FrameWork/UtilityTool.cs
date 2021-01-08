using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ITL.Framework
{
    public static class UtilityTool
    {
        public const int SW_RESTORE = 9;

        #region  判断当前是否是设计时期
        private static bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
        /// <summary>
        /// 判断当前是否是设计时期
        /// </summary>
        /// <returns></returns>
        public static bool IsDesignMode()
        {
            return designMode;
        }
        #endregion

        /// <summary>
        /// 该函数确定给定窗口是否是最小化（图标化）的窗口
        /// </summary>
        /// <param name="hWnd">被测试窗口的句柄</param>
        /// <returns>如果窗口未最小化，返回值为零；如果窗口已最小化，返回值为非零</returns>
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        /// <summary>
        /// 该函数设置由不同线程产生的窗口的显示状态
        /// 备注：这个函数向给定窗口的消息队列发送show-window事件。应用程序可以使用这个函数避免在等待一个挂起的应用程序完成处理show-window事件时也被挂起。
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="nCmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// 该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
        /// 系统给创建前台窗口的线程分配的权限稍高于其他线程。
        /// </summary>
        /// <param name="hWnd">指定的窗口的窗口句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 根据进程映像路径名称激活指定进程
        /// </summary>
        /// <param name="aProcessPath"></param>
        public static void BringProcessToFrontByPath(string aProcessPath)
        {
            //检测进程是否已经重复启动
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            Process current = Process.GetCurrentProcess();
            foreach (System.Diagnostics.Process process in processList)
            {
                if (process.Id != current.Id)
                {
                    //比较进程名如"CenterSystem"
                    if (process.ProcessName.ToUpper() == System.IO.Path.GetFileNameWithoutExtension(aProcessPath).ToUpper())
                    {
                        IntPtr hWnd = process.MainWindowHandle;
                        // if iconic, we need to restore the window
                        if (UtilityTool.IsIconic(hWnd)) //如果窗体最小化,则恢复窗体
                        {
                            UtilityTool.ShowWindowAsync(hWnd, UtilityTool.SW_RESTORE);
                        }
                        //SetWindowPos(hWnd, new IntPtr(-1), 0, 0, 0, 0, 1 | 2);
                        //激活窗体并置前
                        UtilityTool.SetForegroundWindow(hWnd);

                        return;
                    }
                }

            }
        }

        /// <summary>
        /// 获取字符串像素尺寸
        /// </summary>
        /// <param name="str"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Size GetStrPixelSize(string str, Font font)
        {
            if (font == null)
            {
                return new Size(0, 0);
            }
            var size = TextRenderer.MeasureText(str, font);
            //var sizeF = graphics.MeasureString(str, font);
            return size;
        }

        /// <summary>
        /// 显示Dialog
        /// </summary>
        /// <typeparam name="T">窗体类型</typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static T ShowDialogForm<T>(EventArgs e, Form owner = null) where T : Form
        {
            T oneFrom = Activator.CreateInstance<T>();  //创建窗体实例
            if (owner != null)
            {
                oneFrom.Owner = owner;
            }
            oneFrom.ShowDialog();
            return oneFrom;
        }

        /// <summary>
        /// 显示UserControl，创建新实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aParentControl"></param>
        /// <param name="dsSytle"></param>
        /// <returns></returns>
        public static T ShowUserControl<T>(Control aParentControl, DockStyle dsSytle = DockStyle.Fill, bool visible = true) where T : UserControl
        {
            T userControl = Activator.CreateInstance<T>();
            if ((userControl == null) || (aParentControl == null))
            {
                return null;
            }
            userControl.Visible = visible;
            userControl.Parent = aParentControl;
            userControl.Dock = dsSytle;
            userControl.BringToFront();
            return userControl;
        }
        /// <summary>
        /// 显示UserControl，不创建新实例
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="parentCtrl"></param>
        /// <param name="dock"></param>
        public static void ShowUserControl(Control ctrl, Control parentCtrl, DockStyle dock = DockStyle.Fill)
        {
            ctrl.Parent = parentCtrl;
            ctrl.Dock = dock;
            ctrl.Visible = true;
            ctrl.BringToFront();
        }

        /// <summary>
        /// 获取系统错误码
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("kernel32.dll")]
        private static extern uint FormatMessage(uint dwFlags, IntPtr lpSource,
          uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer,
          uint nSize, IntPtr Arguments);

        /// <summary>
        /// 获取系统错误信息描述
        /// </summary>
        /// <returns></returns>
        public static string GetSysErrMsg()
        {
            uint errCode = GetLastError();
            //string msg = null;
            uint strLen = 0x100;
            var strBuilder = new StringBuilder((int)strLen);
            FormatMessage(0x1000, IntPtr.Zero, errCode, 0, strBuilder, strLen, IntPtr.Zero);
            return string.Format("ErrorCode:{0},ErrorInfo:{1}", errCode, strBuilder.ToString());
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetDllDirectory(int bufsize, StringBuilder buf);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string librayName);

        public static void LoadDllFile(string dllfolder, string libname)
        {
            var currentpath = new StringBuilder(255);
            GetDllDirectory(currentpath.Length, currentpath);

            // use new path
            SetDllDirectory(dllfolder);

            LoadLibrary(libname);

            // restore old path
            SetDllDirectory(currentpath.ToString());
        }

    }
}
