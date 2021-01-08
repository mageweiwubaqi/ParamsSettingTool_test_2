using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ITL.Framework
{
    //public class LogConsole:IDisposable
    //{

    //    public struct RECT
    //    {
    //        public int Left;        // x position of upper-left corner
    //        public int Top;         // y position of upper-left corner
    //        public int Right;       // x position of lower-right corner
    //        public int Bottom;      // y position of lower-right corner
    //    }

    //    [DllImport("user32.dll")]
    //    public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

    //    [DllImport("user32.dll")]
    //    public static extern bool GetWindowRect(HandleRef hwnd, out RECT lpRect);

    //    [DllImport("kernel32.dll")]
    //    [return: MarshalAs(UnmanagedType.Bool)]
    //    private static extern bool AllocConsole();

    //    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    //    private static extern bool FreeConsole();

    //    [DllImport("kernel32.dll", SetLastError = true)]
    //    private static extern IntPtr GetConsoleWindow();
    //    [DllImport("user32.dll", SetLastError = true)]
    //    private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
    //    [DllImport("user32.dll", SetLastError = true)]
    //    private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);

    //    private object f_Lock = new object();

    //    public LogConsole()
    //    {
    //        AllocConsole();
           
    //        this.SetConsoleWindow();
           
    //    }


    //    private void SetConsoleWindow()
    //    {
    //        try
    //        {
    //            Console.ForegroundColor = ConsoleColor.Yellow;
    //            int width = Console.LargestWindowWidth > 0 ? Console.LargestWindowWidth : 240;
    //            int height = Console.LargestWindowHeight > 0 ? Console.LargestWindowHeight : 63;
    //            Console.SetWindowSize(width, height * 1 / 4);
    //            BottomConsole();
    //        }
    //        catch (Exception)
    //        {
    //        }
    //    }


    //    private static void CenterConsole()
    //    {
    //        IntPtr hWin = GetConsoleWindow();
    //        RECT rc;
    //        GetWindowRect(hWin, out rc);
    //        Screen scr = Screen.FromPoint(new Point(rc.Left, rc.Top));
    //        int x = scr.WorkingArea.Left + (scr.WorkingArea.Width - (rc.Right - rc.Left)) / 2;
    //        int y = scr.WorkingArea.Top + (scr.WorkingArea.Height - (rc.Bottom - rc.Top)) / 2;
    //        MoveWindow(hWin, x, y, rc.Right - rc.Left, rc.Bottom - rc.Top, false);
    //    }

    //    private static void BottomConsole()
    //    {
    //        IntPtr hWin = GetConsoleWindow();
    //        RECT consoleRect;
    //        GetWindowRect(hWin, out consoleRect);
    //        Screen windowScreen = Screen.FromPoint(new Point(consoleRect.Left, consoleRect.Top));
    //        int x = windowScreen.WorkingArea.Left + (windowScreen.WorkingArea.Width - (consoleRect.Right - consoleRect.Left)) / 2;
    //        int y = windowScreen.WorkingArea.Top + windowScreen.WorkingArea.Height - (consoleRect.Bottom - consoleRect.Top) ;
    //        MoveWindow(hWin, x, y, consoleRect.Right - consoleRect.Left, consoleRect.Bottom - consoleRect.Top, false);
    //    }

    //    public void WriteLog(string logInfo)
    //    {
    //        lock (f_Lock)
    //        {
    //            Console.WriteLine(logInfo);
    //        }
               
    //    }



    //    public void Dispose()
    //    {
    //        FreeConsole();
    //    }


    //}
}
