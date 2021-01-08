using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ITL.Public
{
    public partial class ConfirmForm : HintGeneralForm
    {
        private string FMT_STR = "{0}({1})";


        private Size f_MinSize = new Size(313, 252);
        private Size f_MaxSize = new Size(434, 343);

        private int f_LeftSeconds = 0; //剩余的描述
        private Action f_DefaultExcute = null;  //默认按钮执行的事件

        private string f_HintInfo = string.Empty;
        private ConfirmFormIcons f_HintIcon = ConfirmFormIcons.Hint;
        private ConfirmFormButtons f_Buttons = ConfirmFormButtons.OK;
        private ConfirmFormDefaultButton f_DefaultButton = ConfirmFormDefaultButton.OK;
        private int f_ShowMSeconds = 0;

        /// <summary>
        /// 提示信息
        /// </summary>
        public string HintInfo
        {
            get
            {
                lock(f_Lock)
                {
                    return f_HintInfo;
                }
            }
            set
            {
                lock(f_Lock)
                {
                    f_HintInfo = value;
                }
            }
        }

        /// <summary>
        /// 窗体图标
        /// </summary>
        public ConfirmFormIcons HintIcon
        {
            get
            {
                lock (f_Lock)
                {
                    return f_HintIcon;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_HintIcon = value;
                }
            }
        }

        /// <summary>
        /// 窗体按钮
        /// </summary>
        public ConfirmFormButtons Buttons
        {
            get
            {
                lock (f_Lock)
                {
                    return f_Buttons;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_Buttons = value;
                }
            }
        }

        /// <summary>
        /// 默认按钮
        /// </summary>
        public ConfirmFormDefaultButton DefaultButton
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DefaultButton;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DefaultButton = value;
                }
            }
        }

        /// <summary>
        /// 显示时间(豪秒),为0时表示不进行倒计时
        /// </summary>
        public int ShowMSeconds
        {
            get
            {
                lock (f_Lock)
                {
                    return f_ShowMSeconds;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_ShowMSeconds = value;
                }
            }
        }

        public ConfirmForm()
        {
            InitializeComponent();
            InitUI();
        }
        private void InitUI()
        {
            //picClose.Parent = this;
            //picClose.Width = pnlTitle.Height;
            //picClose.Height = pnlTitle.Height;
            //picClose.BackColor = pnlTitle.BackColor;
            //picClose.Location = new Point(this.Width - picClose.Width, 0);
            //picClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            //picClose.BringToFront();
        }


        //找到窗体的最顶层父窗体
        private static Form FindTopParentForm(Form frm)
        {
            if (frm?.ParentForm == null)
            {
                return frm;
            }
            return FindTopParentForm(frm.ParentForm);
        }

        /// <summary>
        /// 外部调用接口
        /// </summary>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <param name="buttons"></param>
        /// <param name="defaultButton"></param>
        /// <returns></returns>
        public static DialogResult ShowConfirmForm(string text, ConfirmFormIcons icon, ConfirmFormButtons buttons, ConfirmFormDefaultButton defaultButton, int showMSeconds, Form owner)
        {
            ConfirmForm dialog = new ConfirmForm()
            {
                HintInfo = text,
                HintIcon = icon,
                Buttons = buttons,
                DefaultButton = defaultButton,
                ShowMSeconds = showMSeconds
            };

            if (owner != null && owner.IsHandleCreated && owner.Visible)
            {
                owner = FindTopParentForm(owner);
                return dialog.ShowDialog(owner);
            }
            else
            {
                dialog.StartPosition = FormStartPosition.CenterScreen;
                return dialog.ShowDialog();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            this.AddEventToControl(this.pnlTitle);
            this.AddEventToControl(this.picIcon);
            this.AddEventToControl(this.lblCaption);

            //由于ShowHint()会调整窗体大小，ShowButtons()会调整按钮位置，故两个函数顺序不能调换
            this.ShowHintIcon();
            this.ShowHint();
            this.ShowButtons();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (ownerForm != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                Point pt = new Point(ownerForm.Location.X + (ownerForm.Width - Width) / 2, ownerForm.Location.Y + (ownerForm.Height - Height) / 2);

                this.Location = pt;
            }
        }

        private Form ownerForm;
        private DialogResult ShowDialog(Form owner)
        {
            ownerForm = owner;
            return this.ShowDialog((IWin32Window)(owner));
        }

        #region 显示信息相关代码
        /// <summary>
        /// 
        /// </summary>
        private void ShowHint()
        {
            if(this.ShowMSeconds == 0)
            {
                this.SetHintText(this.HintInfo);
                return;
            }
            //倒计时显示
            f_LeftSeconds = this.ShowMSeconds;// / 1000;
            f_LeftSeconds = f_LeftSeconds < 3 ? 3 : f_LeftSeconds;
            this.SetHintText(string.Format(FMT_STR, this.HintInfo, f_LeftSeconds));
            this.tmrCountDown.Interval = 1000;
            this.tmrCountDown.Start();
        }

        private void tmrCountDown_Tick(object sender, EventArgs e)
        {
            f_LeftSeconds--;
            if (f_LeftSeconds <= 0)
            {
                this.tmrCountDown.Stop();
                f_DefaultExcute?.Invoke();
            }
            this.SetHintText(string.Format(FMT_STR, this.HintInfo, f_LeftSeconds));
        }

        /// <summary>
        /// 计算窗体大小
        /// </summary>
        /// <param name="hint"></param>
        private void CalcFormSize(string hint)
        {
            //窗体最小尺寸：303×163,每行显示26个字符，可显示2行，超过2行后加宽，2行组多显示2×36个字符，之后需加行
            //窗体最大尺寸：377×274,每行显示36个字符，可显示8行，最多显示8×36个字符，超过最大字符数可
            int txtLength = Encoding.Default.GetBytes(hint).Length;

            //小于2×26个字符时，使用最小尺寸
            if (txtLength <= 2 * 26)
            {
                this.Width = 303;
                this.Height = 163;
            }
            //小于2×36个字符时，使用最小高度，最大宽度
            else if (txtLength <= 2 * 36)
            {
                this.Width = 377;
                this.Height = 163;
            }
            //大于8×36个字符时，使用最大尺寸
            else if (txtLength >= 8 * 36)
            {
                this.Width = 377;
                this.Height = 274;
            }
            else //其余使用最大宽度，高度自动调整
            {
                this.Width = 377;
                int rowCount = ((txtLength / 36) * 36 < txtLength) ? ((txtLength / 36) + 1) : (txtLength / 36);
                switch (rowCount)
                {
                    case 3:
                        {
                            this.Height = 175;
                        }
                        break;
                    case 4:
                        {
                            this.Height = 194;
                        }
                        break;
                    case 5:
                        {
                            this.Height = 207;
                        }
                        break;
                    case 6:
                        {
                            this.Height = 230;
                        }
                        break;
                    case 7:
                        {
                            this.Height = 252;
                        }
                        break;
                    default:
                        {
                            this.Height = 274;
                        }
                        break;
                }
            }
        }

        private void SetHintText(string hint)
        {
            this.CalcFormSize(hint);
            this.lblHint.Text = hint;
            //this.tipHint.SetToolTip(this.lblHint, hint);
        }

        /// <summary>
        /// 显示图标
        /// </summary>
        private void ShowHintIcon()
        {
            switch (this.HintIcon)
            {
                case ConfirmFormIcons.Hint:
                    {
                        this.picIcon.Image = ParamsSettingTool.Properties.Resources.Hint_32;
                        this.lblCaption.Text = "提示信息";
                    }
                    break;
                //case ConfirmFormIcons.Error:
                //    {
                //        this.picIcon.Image = Properties.Resources.Error_32;
                //        this.lblCaption.Text = MultLang.GetLangValue("c8e241c693804ca18ef400211151b5d6", "错误信息");
                //    }
                //    break;
                case ConfirmFormIcons.Warning:
                    {
                        this.picIcon.Image = ParamsSettingTool.Properties.Resources.warning_32;
                        this.lblCaption.Text = "警告信息";
                    }
                    break;
                case ConfirmFormIcons.Question:
                    {
                        this.picIcon.Image = ParamsSettingTool.Properties.Resources.Question_32;
                        this.lblCaption.Text = "提示信息";
                    }
                    break;
                default:
                    break;
            }
            //this.lblCaption.Left = (this.Width - this.lblCaption.Width) / 2;
        }

        /// <summary>
        /// 显示按钮
        /// </summary>
        private void ShowButtons()
        {
            switch (this.Buttons)
            {
                case ConfirmFormButtons.OK:
                    {
                        this.btnLeft.Visible = false;
                        this.btnMiddle.Visible = true;
                        this.btnRight.Visible = false;
                        this.btnMiddle.Text = "确  定";
                        if(this.DefaultButton == ConfirmFormDefaultButton.OK)
                        {
                            //this.btnMiddle.FlatAppearance.BorderSize = 1;
                            this.SetFocus(this.btnMiddle);
                            f_DefaultExcute = this.ExcuteMiddleButtonClick;
                        }
                        this.btnMiddle.Left = (this.pnlButtons.Width - this.btnMiddle.Width) / 2;
                    }
                    break;
                case ConfirmFormButtons.OKCancel:
                    {
                        this.btnLeft.Visible = false;
                        this.btnMiddle.Visible = true;
                        this.btnRight.Visible = true;
                        this.btnMiddle.Text = "确  定";
                        this.btnRight.Text = "取  消";
                        if(this.DefaultButton == ConfirmFormDefaultButton.OK)
                        {
                            this.btnMiddle.FlatAppearance.BorderSize = 1;
                            this.SetFocus(this.btnMiddle);
                            f_DefaultExcute = this.ExcuteMiddleButtonClick;
                        }
                        else
                        {
                            this.btnRight.FlatAppearance.BorderSize = 1;
                            this.SetFocus(this.btnRight);
                            f_DefaultExcute = this.ExcuteRightButtonClick;
                        }
                        this.btnMiddle.Left = (this.pnlButtons.Width - this.btnMiddle.Width - this.btnRight.Width - 12) / 2;
                        this.btnRight.Left = this.btnMiddle.Left + this.btnRight.Width + 12;
                    }
                    break;
                case ConfirmFormButtons.YesNoCancal:
                    {
                        this.btnLeft.Visible = true;
                        this.btnMiddle.Visible = true;
                        this.btnRight.Visible = true;
                        this.btnLeft.Text = "是";
                        this.btnMiddle.Text = "否";
                        this.btnRight.Text = "取  消";
                        switch (this.DefaultButton)
                        {
                            case ConfirmFormDefaultButton.Yes:
                                {
                                    this.btnLeft.FlatAppearance.BorderSize = 1;
                                    this.SetFocus(this.btnLeft);
                                    f_DefaultExcute = this.ExcuteLeftButtonClick;
                                }
                                break;
                            case ConfirmFormDefaultButton.No:
                                {
                                    this.btnMiddle.FlatAppearance.BorderSize = 1;
                                    this.SetFocus(this.btnMiddle);
                                    f_DefaultExcute = this.ExcuteMiddleButtonClick;
                                }
                                break;
                            default:
                                {
                                    this.btnRight.FlatAppearance.BorderSize = 1;
                                    this.SetFocus(this.btnRight);
                                    f_DefaultExcute = this.ExcuteRightButtonClick;
                                }
                                break;
                        }
                        this.btnMiddle.Left = (this.pnlButtons.Width - this.btnMiddle.Width) / 2;
                        this.btnLeft.Left = this.btnMiddle.Left - this.btnLeft.Width - 12;
                        this.btnRight.Left = this.btnMiddle.Left + this.btnRight.Width + 12;
                    }
                    break;
                default:
                    break;
            }


        }

        /// <summary>
        /// 设置焦点
        /// </summary>
        /// <param name="control"></param>
        private void SetFocus(Control control)
        {
            if((control.Visible)&&(control.Enabled))
            {
                control.Focus();
            }
        }
        #endregion

        #region 拖动相关代码

        private void AddEventToControl(Control control)
        {
            control.MouseDown += this.CustomMouseDown;
            control.MouseMove += this.CustomMouseMove;
            control.MouseUp += this.CustomMouseUp;
        }

        /// <summary>
        /// 鼠标按下为true
        /// </summary>
        private bool f_Mousedown;
        private Point f_Location = new Point(0, 0);
        private Point f_MouseLocation = new Point(0, 0);

        private void CustomMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                f_MouseLocation.X = MousePosition.X;
                f_MouseLocation.Y = MousePosition.Y;
                f_Location = this.Location;
                f_Mousedown = true;
                ((Control)sender).Cursor = Cursors.SizeAll;
                this.BringToFront();
            }
        }

        private void CustomMouseMove(object sender, MouseEventArgs e)
        {
            if (f_Mousedown)
            {
                // 获取当前屏幕的光标坐标
                Point pTemp = new Point(Cursor.Position.X, Cursor.Position.Y);
                // 定位事件源的 Location
                this.Location = new Point(f_Location.X + MousePosition.X - f_MouseLocation.X, f_Location.Y + MousePosition.Y - f_MouseLocation.Y);
            }
        }

        private void CustomMouseUp(object sender, MouseEventArgs e)
        {
            f_Mousedown = false;
            ((Control)sender).Cursor = Cursors.Arrow;
        }
        #endregion

        #region 执行事件相关代码
        private void ExcuteLeftButtonClick()
        {
            if(this.tmrCountDown.Enabled)
            {
                this.tmrCountDown.Stop();
            }
            switch (this.Buttons)
            {
                case ConfirmFormButtons.YesNoCancal:
                    {
                        this.DialogResult = DialogResult.Yes;
                    }
                    break;
                default:
                    {
                        this.DialogResult = DialogResult.None;
                    }
                    break;
            }
            this.Close();
        }

        private void ExcuteMiddleButtonClick()
        {
            if (this.tmrCountDown.Enabled)
            {
                this.tmrCountDown.Stop();
            }
            switch (this.Buttons)
            {
                case ConfirmFormButtons.OK:
                case ConfirmFormButtons.OKCancel:
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    break;
                case ConfirmFormButtons.YesNoCancal:
                    {
                        this.DialogResult = DialogResult.No;
                    }
                    break;
                default:
                    {
                        this.DialogResult = DialogResult.None;
                    }
                    break;
            }
            this.Close();
        }

        private void ExcuteRightButtonClick()
        {
            if (this.tmrCountDown.Enabled)
            {
                this.tmrCountDown.Stop();
            }
            switch (this.Buttons)
            {
                case ConfirmFormButtons.OKCancel:
                case ConfirmFormButtons.YesNoCancal:
                    {
                        this.DialogResult = DialogResult.Cancel;
                    }
                    break;
                default:
                    {
                        this.DialogResult = DialogResult.None;
                    }
                    break;
            }
            this.Close();
        }
        #endregion

        private void picClose_MouseEnter(object sender, EventArgs e)
        {
            this.picClose.BackColor = Color.Red;
        }

        private void picClose_MouseLeave(object sender, EventArgs e)
        {
            this.picClose.BackColor = Color.Transparent;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            this.ExcuteRightButtonClick();
        }

        private void btnMiddle_Click(object sender, EventArgs e)
        {
            this.ExcuteMiddleButtonClick();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            this.ExcuteLeftButtonClick();
        }

        private void ConfirmForm_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid, //左边
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid, //上边
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid, //右边
                    Color.FromArgb(9, 163, 220), 1, ButtonBorderStyle.Solid);//底边)
        }

        /// <summary>
        /// 快捷键-复制文本
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.C))
            {
                Clipboard.SetDataObject(lblHint.Text, true);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
