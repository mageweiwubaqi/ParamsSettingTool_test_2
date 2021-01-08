using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ITL.General
{
    public partial class DragPanelControl : PanelControl
    {
        Control f_DragControl = null;  //拖动的载体
        public DragPanelControl()
        {
            InitializeComponent();
        }

        public DragPanelControl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;    //向窗口发送消息
        public const int SC_MOVE = 0xF010;          //向窗口发送移动的消息
        public const int HTCAPTION = 0x0002;

        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    base.OnMouseDown(e);
        //    ReleaseCapture();
        //    if (f_DragControl == null)
        //    {
        //        return;
        //    }
        //    SendMessage(f_DragControl.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        //}

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                ReleaseCapture();
                if (f_DragControl != null)
                {
                    SendMessage(f_DragControl.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0); //给指定控件发送移动消息
                    SendMessage(this.Handle, 0x0202, 0, 0);  //给当前控件重新发送被截获的消息
                }

            }


        }

        public Control DragControl
        {
            get
            {
                return f_DragControl;
            }
            set
            {
                f_DragControl = value;
            }
        }
    }
}
