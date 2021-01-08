using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITL.Public
{
    public partial class WaitForm : HintGeneralForm
    {

        /// <summary>
        /// 手动关闭后触发事件
        /// </summary>
        public Action HandleCloseEvent;
        public Func<Dictionary<object, Action>> GetUncloseControl { get; set; }

        /// <summary>
        /// 延迟N毫秒显示关闭按钮
        /// </summary>
        public int ShowCloseButtonDelay { get; set; }


        public WaitForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;

            InitializeComponent();
            bool designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            if (!designMode)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
                this.KeyPreview = true;
                picClose.Visible = false;
                lblCaption.Text = "";
                lblDescription.Text = "";
                InitResource();
            }
        }

        private void InitResource()
        {
            this.Paint += WaitForm_Paint;
            this.Shown += WaitForm_Shown;
            picClose.Click += PicClose_Click;
        }


        private void WaitForm_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid, //左边
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid, //上边
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid, //右边
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid);//底边)
        }

        private void PicClose_Click(object sender, EventArgs e)
        {
            HandleCloseEvent?.Invoke(); 
            this.Close();
        }

        private void WaitForm_Shown(object sender, EventArgs e)
        {
            SetForegroundWindow(this.Handle);

            DateTime start = DateTime.Now;
            bool closeVisible = false;
            //延迟显示关闭按钮
            Task tsk = new Task(() =>
            {
                while (true)
                {
                    if (Disposing || IsDisposed)
                    {
                        return;
                    }
                    if (!closeVisible)
                    {
                        TimeSpan ts = DateTime.Now - start;
                        if (ts.TotalMilliseconds >= ShowCloseButtonDelay)
                        {
                            this.Invoke((MethodInvoker)(() =>
                            {
                                picClose.Visible = true;
                                closeVisible = true;
                            }));
                        }
                    }

                    var dic = GetUncloseControl?.Invoke();
                    if (dic == null || dic.Count == 0)
                    {
                        UIContext?.Post((o) =>
                        {
                            if (!Disposing && !IsDisposed)
                            {
                                this.Close();
                            }
                        }, null);
                        break;
                    }
                    Thread.Sleep(100);
                }
            });
            tsk.Start();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            //SetForegroundWindow(this.Handle);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void UpdateText(string caption, string description)
        {
            lblCaption.Text = (caption != null) ? caption : lblCaption.Text;
            lblDescription.Text = (description != null) ? description : lblDescription.Text;
            this.AdjustLabelPosition();
        }
        //调整Caption和Description的位置
        private void AdjustLabelPosition()
        {
            bool capEmpty = string.IsNullOrWhiteSpace(lblCaption.Text);
            bool descEmpty = string.IsNullOrWhiteSpace(lblDescription.Text);
            lock (this)
            {
                if (!capEmpty && descEmpty)
                {
                    lblDescription.Visible = false;

                    lblCaption.Visible = true;
                    lblCaption.Location = new Point(4, 30);
                }
                else if (capEmpty && !descEmpty)
                {
                    lblCaption.Visible = false;

                    lblDescription.Visible = true;
                    lblDescription.Location = new Point(4, 35);
                }
                else
                {
                    lblCaption.Visible = true;
                    lblDescription.Visible = true;
                    lblCaption.Location = new Point(4, 20);
                    lblDescription.Location = new Point(4, 50);
                }
            }
        }

        //更新等待条界面信息
        public void Waiting(string caption, string description, object ctrl)
        {
            if (this.InvokeRequired)
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        UpdateText(caption, description);
                    }));

                }
            }
            else
            {
                UpdateText(caption, description);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //Ctrl + Shift + Esc，显示正在进行等待提示的Controls
            if (e.Control & e.Shift & e.KeyCode == Keys.D)
            {
                var list = GetUncloseControl?.Invoke();
                if (list != null && list.Count > 0)
                {
                    MessageBox.Show(string.Join(Environment.NewLine, list.Keys));
                }
            }
        }

        private bool leftMouseDown = false;
        private Point originalPt = new Point();
        private void picWaiting_MouseDown(object sender, MouseEventArgs e)
        {
            SetForegroundWindow(this.Handle);
            if (e.Button == MouseButtons.Left)
            {
                leftMouseDown = true;
                originalPt = new Point(-e.X, -e.Y);
            }
        }

        private void picWaiting_MouseUp(object sender, MouseEventArgs e)
        {
            leftMouseDown = false;
        }

        private void picWaiting_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftMouseDown)
            {
                Point pt = Control.MousePosition;
                pt.Offset(originalPt.X, originalPt.Y);
                Location = pt;
            }
        }
    }


    /// <summary>
    /// 无进度等待提示框管理类
    /// </summary>
    public class WaitFormManager
    {
        #region 属性及变量定义
        
        private static bool f_IsShowingWaitForm = false;
        /// <summary>
        /// 当前是否正在显示等待窗（配合PreFilterMessage进行鼠标等事件过滤，模拟独占）
        /// </summary>
        public static bool IsShowingWaitForm
        {
            get
            {
                lock (f_LockStatic)
                {
                    return f_IsShowingWaitForm;
                }
            }
            set
            {
                lock (f_LockStatic)
                {
                    f_IsShowingWaitForm = value;
                }
            }
        }

        private object f_Lock = new object();
        private static object f_LockStatic = new object();
        private volatile static WaitFormManager _SingleWaitMgr;
        private Task f_Task;
        private WaitForm f_WaitForm = null;
        private readonly Dictionary<object, Action> f_Identities = new Dictionary<object, Action>();
        private Task WaitTask
        {
            get
            {
                lock (f_Lock)
                {
                    return f_Task;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_Task = value;
                }
            }
        }
        private WaitForm WaitingForm
        {
            get
            {
                lock (f_Lock)
                {
                    return f_WaitForm;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_WaitForm = value;
                }
            }
        }
        //等待方标识
        private Dictionary<object, Action> WaitIdentities
        {
            get
            {
                lock (f_Lock)
                {
                    return f_Identities;
                }
            }
        }

        private WaitFormManager()
        {

        }

        //重置Task为null
        private void RevertTask()
        {
            WaitTask = null;
            WaitingForm = null;
            WaitIdentities.Clear();
        }        
        #endregion

        //找到窗体的最顶层父窗体
        private Form FindTopParentForm(Form frm)
        {
            if (frm?.ParentForm == null)
            {
                return frm;
            }
            return FindTopParentForm(frm.ParentForm);
        }

        //启动无进度等待条
        public void StartWaiting(Form owner, string caption, string description, object ctrl, Action handleCloseCallback, 
            int showDelay, int showCloseButtonDelay)
        {
            if (WaitTask != null)
            {
                if (WaitIdentities.ContainsKey(ctrl))
                {
                    WaitIdentities[ctrl] = handleCloseCallback;
                }
                else
                {
                    WaitIdentities.Add(ctrl, handleCloseCallback);
                }
                Waiting(caption, description, ctrl);
                return;
            }

            //计算owner相对于屏幕的起始位置及宽高
            Point leftTopPt = new Point();
            int ownerWidth = 0, ownerHeight = 0;
            owner = FindTopParentForm(owner);
            if (owner != null && owner.IsHandleCreated && owner.Visible)
            {
                leftTopPt = owner.Location;
                ownerWidth = owner.Width;
                ownerHeight = owner.Height;
            }


            WaitTask = new Task(() =>
            {
                try
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;
                    if (showDelay > 0)
                    {
                        Thread.Sleep(showDelay);
                    }
                    //启动时发现没有等待方标识，则重置并退出
                    if (WaitIdentities.Count == 0)
                    {
                        RevertTask();
                        return;
                    }
                    WaitingForm = new WaitForm();
                    if (!leftTopPt.IsEmpty && ownerWidth > WaitingForm.Width && ownerHeight > WaitingForm.Height)
                    {
                        WaitingForm.StartPosition = FormStartPosition.Manual;
                        Point pt = new Point(leftTopPt.X + (ownerWidth - WaitingForm.Width) / 2, leftTopPt.Y + (ownerHeight - WaitingForm.Height) / 2);

                        WaitingForm.Location = pt;
                    }

                    WaitingForm.ShowCloseButtonDelay = showCloseButtonDelay;
                    WaitingForm.HandleCloseEvent = () =>
                    {
                        lock (f_Lock)
                        {
                            List<Action> acts = WaitIdentities.Select(p => p.Value).ToList();
                            foreach (var callback in acts)
                            {
                                callback?.Invoke();
                            } 
                        }
                    };
                    WaitingForm.Shown += (sender, e) =>
                    {
                        IsShowingWaitForm = true;
                    };
                    WaitingForm.FormClosing += (sender, e) =>
                    {
                        RevertTask();
                        IsShowingWaitForm = false;
                    };
                    WaitingForm.GetUncloseControl = () =>
                    {
                        return WaitIdentities;
                    };
                    WaitingForm.Waiting(caption, description, ctrl);
                    //WaitingForm.ShowDialog();
                    Application.Run(WaitingForm);
                }
                catch (Exception)
                {
                    RevertTask();
                    IsShowingWaitForm = false;
                }
            });
            WaitIdentities.Clear();
            WaitIdentities.Add(ctrl, handleCloseCallback);
            WaitTask.Start();
        }

        //更新等待条界面
        public void Waiting(string caption, string description, object ctrl)
        {
            if (WaitTask == null || WaitingForm == null)
            {
                return;
            }
            if (!WaitIdentities.ContainsKey(ctrl))
            {
                WaitIdentities.Add(ctrl, null);
            }
            WaitingForm?.Waiting(caption, description, ctrl);
        }

        //完成当前等待方下的等待更新
        public void Done(object ctrl, bool isForceClose)
        {
            if (isForceClose)
            {
                WaitIdentities.Clear();
            }
            else
            {
                WaitIdentities.Remove(ctrl);
            }
        }

        /// <summary>
        /// 无进度等待条管理类单例
        /// </summary>
        public static WaitFormManager Singleton
        {
            get
            {
                if (_SingleWaitMgr == null)
                {
                    lock (f_LockStatic)
                    {
                        if (_SingleWaitMgr == null)
                        {
                            _SingleWaitMgr = new WaitFormManager();
                        }
                    }
                }
                return _SingleWaitMgr;
            }
        }
    }
}
