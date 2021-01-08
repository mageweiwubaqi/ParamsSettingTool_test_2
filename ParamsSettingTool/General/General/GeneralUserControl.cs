using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ITL.Framework;
using DevExpress.XtraEditors;
using ITL.Public;

namespace ITL.General
{
    public partial class GeneralUserControl : BaseUserControl
    {
        public GeneralUserControl()
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

        ///// <summary>
        ///// 解决界面变化时闪烁的问题
        ///// </summary>
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;////用双缓冲从下到上绘制窗口的所有子孙
        //        return cp;
        //    }
        //}

        private void GeneralUserControl_Load(object sender, EventArgs e)
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
                this.InitUIEvents();
            }
        }

        protected virtual void InitUIOnLoad()
        {
            
        }

        protected virtual void InitUIEvents()
        {
            
        }
    }
}
