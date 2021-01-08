using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ITL.Framework;
using ITL.Public;
using System.Runtime.InteropServices;

namespace ITL.General
{
    public partial class GeneralForm : BaseForm
    {
        public GeneralForm()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                SetStyle(
                     ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.Selectable
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.SupportsTransparentBackColor,
                     true);
            }
        }

        private void GeneralForm_Load(object sender, EventArgs e)
        {
            //设置默认字体，仅在设计时期使用，实现拖控件自动加载字体，运行时期不可运行
            if (UtilityTool.IsDesignMode())
            {
                WindowsFormsSettings.DefaultFont = new Font(ControlUtilityTool.PubFontFamily, 10.5F, FontStyle.Regular);
            }
            else
            {
                //初始化UI
                this.InitUIOnLoad();
            }
        }

        private void GeneralForm_Shown(object sender, EventArgs e)
        {
            this.InitUIOnShown();
        }

        protected virtual void InitUIOnLoad()
        {
            
        }

        protected virtual void InitUIOnShown()
        {
            if (UtilityTool.IsDesignMode())
            {
                return;
            }
            this.InitUIEvents();
            this.SetCaptionPosition();
            ControlUtilityTool.SetITLSimpleButtonFlatStyle(this.btnOK);
            ControlUtilityTool.SetPanelControlBorderLines(this.pnlButtom, false, true, true, true);
            ControlUtilityTool.SetPanelControlBorderLines(this.pnlMain, false, true, true, true);
            ControlUtilityTool.SetControlDefaultFont(this.lblCaption, 12, FontStyle.Bold);
            ControlUtilityTool.SetControlDefaultColor(this.lblCaption, Color.White, ColorType.ctForeColor);
            ControlUtilityTool.SetControlDefaultColor(this.pnlMain, ControlUtilityTool.PubBackColorNormal);
            ControlUtilityTool.SetControlDefaultColor(this.pnlButtom, ControlUtilityTool.PubBackColorNormal);
            ControlUtilityTool.SetSuperToolTip(this.btnMin, "最小化");
            ControlUtilityTool.SetSuperToolTip(this.btnMax, "最大化");
            ControlUtilityTool.SetSuperToolTip(this.btnClose, "关闭");
            this.btnOK.Top = (this.pnlButtom.Height - this.btnOK.Height) / 2;
            this.btnOK.Left = this.pnlButtom.Width - 25 - this.btnOK.Width;
            this.btnMin.Visible = false;
            this.btnMax.Visible = false;
        }

        protected virtual void InitUIEvents()
        {
            this.btnMin.Click += this.btnMin_Click;
            this.btnMax.Click += this.btnMax_Click;
            this.btnClose.Click += this.btnClose_Click;
            this.pnlTitle.MouseDoubleClick += this.pnlTitle_MouseDoubleClick;
            this.SizeChanged += this.GeneralForm_SizeChanged;
            this.btnOK.Click += this.btnOK_Click;
        }
      
        private void btnMin_Click(object sender, EventArgs e)
        {
            //窗体最小化
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            //窗体最大化与恢复
            if (this.WindowState == FormWindowState.Normal)
            {
                this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlTitle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (btnMax.Visible)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }
                else if (WindowState == FormWindowState.Normal)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }
        }

        private void GeneralForm_SizeChanged(object sender, EventArgs e)
        {
            //变更最大化按钮图标
            if (this.WindowState == FormWindowState.Normal)
            {
                this.btnMax.Image = ParamsSettingTool.Properties.Resources.Max_16;
                if (!UtilityTool.IsDesignMode())
                {
                    ControlUtilityTool.SetSuperToolTip(this.btnMax, "最大化");
                }
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.btnMax.Image = ParamsSettingTool.Properties.Resources.MaxEx_16;
                if (!UtilityTool.IsDesignMode())
                {
                    ControlUtilityTool.SetSuperToolTip(this.btnMax, "还原");
                }
            }
            //调整标题位置
            this.SetCaptionPosition();
        }

        /// <summary>
        /// 设置窗体标题位置
        /// </summary>
        protected virtual void SetCaptionPosition()
        {
            this.lblCaption.Left = (this.pnlTitle.Width - this.lblCaption.Width) / 2;
            this.lblCaption.Top = (this.pnlTitle.Height - this.lblCaption.Height) / 2;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.ExcuteOKPerformClick();
        }

        protected virtual void ExcuteOKPerformClick()
        {

        }

        #region 解决FormBorderStyle设置为None后点击任务栏无法自动最小化问题
        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        static extern int GetWindowLong(HandleRef hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        void SetForm()
        {
            if (this.IsHandleCreated)
            {
                int WS_SYSMENU = 0x00080000;//系统菜单
                int WS_MINIMIZEBOX = 0x00020000;//最大最小按钮
                int windowLong = (GetWindowLong(new HandleRef(this, this.Handle), -16));
                SetWindowLong32(new HandleRef(this, this.Handle), -16, windowLong | WS_SYSMENU | WS_MINIMIZEBOX);
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetForm();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;//最大最小化按钮
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX;//允许最小化操作
                //cp.ExStyle |= 0x02000000;////用双缓冲从下到上绘制窗口的所有子孙
                return cp;
            }
        }

        #endregion

        /// <summary>
        /// 实现按ESC键关闭窗体，回车确定
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;
            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        this.Close();
                        break;
                    case Keys.Enter:
                        this.ExcuteOKPerformClick();
                        break;
                }
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            return false;
        }
    }
}
