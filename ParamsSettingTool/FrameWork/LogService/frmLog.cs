using DevExpress.Utils.About;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ITL.Framework
{
    public partial class frmLog : Form
    {
        private SynchronizationContext CurrentUIContext;
        private const int LOG_WINDOW_HEIGHT = 200;
        private bool f_IsStopAutoScroll = false;
        private int f_LastSelPos = 0;
        public frmLog()
        {
            InitializeComponent();
            CurrentUIContext = SynchronizationContext.Current;
        }

        private void frmLog_Shown(object sender, EventArgs e)
        {
            bool designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            if (!designMode)
            {
                this.InitUI();
            }
        }

        //初始化界面
        private void InitUI()
        {
            this.Left = 0;
            this.Top =  Screen.PrimaryScreen.WorkingArea.Height - LOG_WINDOW_HEIGHT;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = LOG_WINDOW_HEIGHT;
            //this.TopMost = true;
        }



         public void WriteLog(string logInfo)
        {
            if (this.IsHandleCreated)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    if (tbInfo.Lines.Count() > 10000)
                    {
                        tbInfo.Clear();
                    }
                    tbInfo.AppendText(string.Format("{0}{1}", logInfo, Environment.NewLine));
                    if (!f_IsStopAutoScroll)
                    {
                        tbInfo.ScrollToCaret();
                        f_LastSelPos = tbInfo.SelectionStart + tbInfo.SelectionLength;
                    }
                    else
                    {
                        tbInfo.SelectionStart = f_LastSelPos;
                    }
                }));
            }

        }

        private void frmLog_FormClosing(object sender, FormClosingEventArgs e)
        {
          //  Application.Exit();
        }

        private void btnStopScroll_Click(object sender, EventArgs e)
        {
            //f_IsStopAutoScroll = !f_IsStopAutoScroll;

            tbInfo.Clear();
        }
    }
}
