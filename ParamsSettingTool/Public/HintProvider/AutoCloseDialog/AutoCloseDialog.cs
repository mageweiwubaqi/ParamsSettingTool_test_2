using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITL.Public
{
    public partial class AutoCloseDialog : HintGeneralForm
    {

        private int SHOW_TIME = 200;
        private int CLOSE_TIME = 500;

        private System.Windows.Forms.Timer f_CountTimer;
        private int f_DurationTime; //窗体可持续时间
        private int f_MaxDurationTime;//窗体最大持续时间
        private int f_LeftTime; //窗体剩余可持续时间
        private bool f_IsShowLeftTime = false; //是否显示剩余关闭时间
        private bool f_CanClose = true; //展示时间到后是否允许关闭，可能人为阻止关闭
        private string f_Text = string.Empty;
        private HintIconType f_IconType = HintIconType.OK;


        public AutoCloseDialog()
        {
            InitializeComponent();
            bool designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            if (!designMode)
            {
                this.InitUI();
            }
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private void SetMouseControl(Control control)
        {
            control.MouseHover += DoMouseHover;
            control.MouseLeave += DoMouseLeave;
            if (!control.HasChildren)
            {
                return;
            }
            foreach (Control subControl in control.Controls)
            {
                //control.MouseHover += DoMouseHover;
                //control.MouseLeave += DoMouseLeave;
                SetMouseControl(subControl);

            }
        }



        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            SetForegroundWindow(this.Handle);
        }

        private void DoMouseHover(object sender, EventArgs e)
        {
            f_CanClose = false;
        }

        private void DoMouseLeave(object sender, EventArgs e)
        {
            f_CanClose = true;
        }
        private void InitUI()
        {
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            lblInfo.MaximumSize = new Size(320,600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            SetMouseControl(this);
            this.Paint += (senderPaint, ePaint) =>
            {

                ControlPaint.DrawBorder(ePaint.Graphics, this.ClientRectangle,
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid, //左边
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid, //上边
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid, //右边
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid);//底边)
            };

          

            f_CountTimer = new System.Windows.Forms.Timer();
            f_CountTimer.Interval = 100;
            f_CountTimer.Tick += OnCountTimer_Tick;
            f_CountTimer.Enabled = false;
        }

        private void OnCountTimer_Tick(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer messageTimer = (sender as System.Windows.Forms.Timer);
            //timeout--;
            f_LeftTime = f_LeftTime - messageTimer.Interval;
            int leftTime = f_LeftTime / 1000;
            if ((f_IsShowLeftTime) && (leftTime > 0))
            {
                lblInfo.Text = string.Format("{0} [{1}]", f_Text, leftTime);
            } else
            {
                lblInfo.Text = f_Text;
            }

            if (f_LeftTime <= 0)
            {
                if (f_CanClose)
                {
                    messageTimer.Stop();
                    messageTimer.Enabled = false;
                    this.Close();
                  
                }

            }
            else
            {
            }
        }



        private void ShowClose( string text,HintIconType iconType, 
            int atLeastDuration, int atMostDuration, Form parentForm)
        {
            //this.Text = caption;
            this.TopMost = true;
            f_Text = text;
            //AdjustUIByHintInfo(f_Text,ref atLeastDuration);
            lblInfo.Text = text;
            this.Height = pnlTop.Height + pnlBottom.Height + lblInfo.Height + 15;
            this.Width = pnlIcon.Width + pnlRight.Width + lblInfo.Width + 10;
            f_IconType = iconType;
            pnlIcon.BackgroundImage = null;
            switch (f_IconType)
            {
                case HintIconType.OK:
                    pnlIcon.BackgroundImage = ParamsSettingTool.Properties.Resources.OK_32;
                    break;
                case HintIconType.Warning:
                    pnlIcon.BackgroundImage = ParamsSettingTool.Properties.Resources.warning_32;
                    break;
                case HintIconType.Err:
                    pnlIcon.BackgroundImage = ParamsSettingTool.Properties.Resources.Error_32;
                    break;
                default:
                    pnlIcon.BackgroundImage = null;
                    break;
            }
            int byteLen = Encoding.GetEncoding("GBK").GetByteCount(text);
            int addDuration = 0;
            if (byteLen > 10)
            {
                addDuration = (byteLen - 10) * 75;
            }
           
            f_DurationTime = atLeastDuration + addDuration;
            f_LeftTime = f_DurationTime - SHOW_TIME - CLOSE_TIME;
            f_MaxDurationTime = atMostDuration;
            if (f_LeftTime < 0)
            {
                f_LeftTime = 0;
            }
            else if (f_LeftTime > f_MaxDurationTime)
            {
                f_LeftTime = f_MaxDurationTime;
            }
            f_CountTimer.Start();

            parentForm = FindTopParentForm(parentForm);
            if (parentForm != null && parentForm.IsHandleCreated && parentForm.Visible)
            {
                this.StartPosition = FormStartPosition.Manual;
                Point pt = new Point((parentForm.Width - this.Width) / 2, (parentForm.Height - this.Height) / 2);
                this.Location = new Point(pt.X + parentForm.Left, pt.Y + parentForm.Top);
            }
            this.ShowDialog();
        }

        //找到窗体的最顶层父窗体
        private Form FindTopParentForm(Form frm)
        {
            if (frm?.ParentForm == null)
            {
                return frm;
            }
            return FindTopParentForm(frm.ParentForm);
        }

        public static void ShowHint(string text, HintIconType iconType,int atLeastDuration, int atMostDuration,
            Form parentForm, bool waitForClose)
        {
            var tsk = new Task(() => {

                AutoCloseDialog dlg = new AutoCloseDialog();

                dlg.ShowClose(text, iconType, atLeastDuration, atMostDuration, parentForm);
            });
            tsk.Start();
            if (waitForClose)
            {
                tsk.Wait();
            }
        }

        private void AutoCloseDialog_Load(object sender, EventArgs e)
        {
    
            //关于Environment.OSVersion https://stackoverflow.com/questions/2819934/detect-windows-version-in-net
            if (Environment.OSVersion.Version.Major >= 6)
            {
                //AutoCloseDialogHelper.AnimateWindow(this.Handle, SHOW_TIME, 
                //    AutoCloseDialogHelper.AW_SLIDE | AutoCloseDialogHelper.AW_ACTIVATE | AutoCloseDialogHelper.AW_VER_NEGATIVE);
                AutoCloseDialogHelper.AnimateWindow(this.Handle, SHOW_TIME, AutoCloseDialogHelper.AW_BLEND);
            }
          
        }

        private void AutoCloseDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                AutoCloseDialogHelper.AnimateWindow(this.Handle, CLOSE_TIME,
                   AutoCloseDialogHelper.AW_HIDE | AutoCloseDialogHelper.AW_BLEND);
            }

        }
    }
}
