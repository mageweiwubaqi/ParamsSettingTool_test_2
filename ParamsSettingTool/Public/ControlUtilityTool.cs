using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraNavBar;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using ITL.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ITL.Public
{
    //提示信息类型
    public enum HintType : byte
    {
        htNone = 0,
        htInfo = 1,
        htAsk = 2,
        htWarn = 3,
        htErr = 4,
        htOK,
        htUnLock,
    }

    //控件颜色类型
    public enum ColorType
    {
        ctBackColor = 0, //背景颜色
        ctForeColor  //字体颜色
    }

    //按钮大小
    public enum ButtonSize
    {
        bsNone = 0, //自定义
        bsNormal = 28, //height-28
        bsLarge = 35,  //height-35
        bsSuperLarge = 56  //56
    }

    public static class ControlUtilityTool
    {

        /// <summary>
        /// 名称规范正则：以中文、字母、数字开头，中间允许有 ' '、'.'、'-'、'/'
        /// </summary>
        public const string NameEditMask = @"[\u4e00-\u9fa5a-zA-Z0-9\-]([ ]*[\u4e00-\u9fa5\-./a-zA-Z0-9][ ]*)*";

        public static FontFamily PubFontFamily = GetDefaultFont();
        public const string DEFAULT_SKIN_NAME = "Liquid Sky"; //默认皮肤

        public static readonly Color PubBackColorNormal = Color.FromArgb(250, 253, 255);   //背景色
        public static readonly Color PubBackColorFunction = Color.FromArgb(225, 243, 255);  //第一梯度色
        public static readonly Color PubBackColorTitle = Color.FromArgb(9, 163, 220);   //标题色
        public static readonly Color PubBackColorEdit = Color.White;  //输入框颜色
        public static readonly Color PubLineColor = Color.FromArgb(85, 191, 230); //线条颜色

        public static readonly Color PubForeColorTitle = Color.White;  //标题文字颜色
        public static readonly Color PubForeColorNormal = Color.FromArgb(32, 31, 53);  //普通文字颜色，控件默认，目前无需设置
        public static readonly Color PubForeColorActive = Color.FromArgb(0, 109, 176); //激活字体颜色

        public static readonly Color PubForeClorMustInput = Color.FromArgb(163, 21, 21);//必填项使用暗红色*标注

        public static readonly Color PubTableBackColorBlue = Color.AliceBlue;  //表格背景颜色1
        public static readonly Color PubTableBackColorWhite = Color.GhostWhite;  //表格背景颜色2

        public static FontFamily GetDefaultFont()
        {
            FontFamily font = null;
            try
            {
                font = new FontFamily("Microsoft YaHei");  //默认字体，中文
            }
            catch (Exception )
            {
                font = SystemFonts.DefaultFont.FontFamily;
            }

            return font;
        }

        //设置控件的Hint提示信息
        public static SuperToolTip SetSuperToolTip(object aControl, string hintInfo, string hintCaption = "", HintType hintType = HintType.htNone,
            string footerInfo = "", Image titleImage = null, Image contentImage = null, Image footerImage = null)
        {
            var toolTipProp = aControl.GetType().GetProperty(nameof(BaseControl.SuperTip));
            if (toolTipProp == null)
            {
                return null;
            }

            //if ((!(aControl is System.ComponentModel.Component)) && (!(aControl is DevExpress.XtraEditors.Controls.EditorButton)))
            //{
            //    return null;
            //}
            SuperToolTip superTip = new SuperToolTip();
            superTip.MaxWidth = 250;

            ToolTipTitleItem titleItem = null; //标题
            ToolTipItem contentItem = new ToolTipItem(); //内容
            contentItem.AllowHtmlText = DefaultBoolean.True;
            ToolTipItem footerItem = null; //页脚

            Icon InfoIcon = null;
            switch (hintType)
            {
                case HintType.htInfo:
                    InfoIcon = SystemIcons.Information;
                    break;
                case HintType.htAsk:
                    InfoIcon = SystemIcons.Question;
                    break;
                case HintType.htWarn:
                    InfoIcon = SystemIcons.Warning;
                    break;
                case HintType.htErr:
                    InfoIcon = SystemIcons.Error;
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(hintCaption))
            {
                titleItem = new ToolTipTitleItem();
                titleItem.Text = hintCaption;
                if (titleImage != null)
                {
                    titleItem.Appearance.Options.UseImage = true;
                    titleItem.ImageToTextDistance = 4;
                    titleItem.Appearance.Image = titleImage;
                }
            }

            contentItem.Font = new Font(PubFontFamily, 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            contentItem.Text = hintInfo;
            if (InfoIcon != null)
            {
                contentItem.Appearance.Options.UseImage = true;
                contentItem.ImageToTextDistance = 4;
                if (titleItem != null)
                {
                    titleItem.Appearance.Image = GetSmailSizeIcon(InfoIcon, 16, 16).ToBitmap();
                }
            }
            if (contentImage != null)
            {
                contentItem.Appearance.Options.UseImage = true;
                contentItem.ImageToTextDistance = 4;
                contentItem.Appearance.Image = contentImage;
            }

            if (!string.IsNullOrEmpty(footerInfo))
            {
                footerItem = new ToolTipItem();
                footerItem.AllowHtmlText = DefaultBoolean.True;
                footerItem.Text = footerInfo;
                footerItem.Appearance.Options.UseImage = true;
                footerItem.ImageToTextDistance = 4;
                footerItem.Appearance.Image = footerImage;
                footerItem.Font = new Font(PubFontFamily, 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            }

            if (titleItem != null)
            {
                superTip.Items.Add(titleItem);
            }
            superTip.Items.Add(contentItem);
            if (footerItem != null)
            {
                superTip.Items.AddSeparator();
                superTip.Items.Add(footerItem);
            }
            if (aControl is BaseControl)
            {
                BaseControl ctrl = aControl as BaseControl;
                ctrl.ToolTipController = new DevExpress.Utils.ToolTipController();
                ctrl.ToolTipController.AutoPopDelay = 5000 + hintInfo.Length * 300; ;
                ctrl.SuperTip = superTip;
            }
            else
            {
                toolTipProp.SetValue(aControl, superTip, null);
            }


            return superTip;
        }

        //根据已有的Icon获取指定Size的Icon
        public static Icon GetSmailSizeIcon(Icon systemIcon, int width, int height)
        {
            Size iconSize = new Size(width, height);
            Bitmap bitmap = new Bitmap(iconSize.Width, iconSize.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(systemIcon.ToBitmap(), new Rectangle(Point.Empty, iconSize));
            }

            return Icon.FromHandle(bitmap.GetHicon());
        }

        /// <summary>
        /// 设置NavBarControl风格
        /// </summary>
        /// <param name="nbControl"></param>
        public static void SetITLNavBarControlStyle(NavBarControl nbControl)
        {
            nbControl.PaintStyleKind = NavBarViewKind.NavigationPane;  //展示风格
            nbControl.OptionsNavPane.ShowOverflowButton = false; //不显示下方按钮
            nbControl.OptionsNavPane.ShowOverflowPanel = false;  //不显示下方区域
            nbControl.BorderStyle = BorderStyles.Office2003;  //边框风格
            nbControl.LinkSelectionMode = LinkSelectionModeType.OneInControl;  //只允许同时选中一个Item

            nbControl.Font = new Font(ControlUtilityTool.PubFontFamily, 12F, FontStyle.Bold);
            nbControl.Appearance.GroupHeader.Font = new Font(ControlUtilityTool.PubFontFamily, 12F, FontStyle.Bold);
            nbControl.Appearance.GroupHeaderActive.Font = new Font(ControlUtilityTool.PubFontFamily, 12F, FontStyle.Bold);
            nbControl.Appearance.GroupHeaderHotTracked.Font = new Font(ControlUtilityTool.PubFontFamily, 12F, FontStyle.Bold);
            nbControl.Appearance.GroupHeaderPressed.Font = new Font(ControlUtilityTool.PubFontFamily, 12F, FontStyle.Bold);
            nbControl.Appearance.NavigationPaneHeader.Font = new Font(ControlUtilityTool.PubFontFamily, 13F, FontStyle.Bold);
            nbControl.Appearance.Item.Font = new Font(ControlUtilityTool.PubFontFamily, 10.5F, FontStyle.Bold);
            nbControl.Appearance.ItemActive.Font = new Font(ControlUtilityTool.PubFontFamily, 10.5F, FontStyle.Bold);
            nbControl.Appearance.ItemHotTracked.Font = new Font(ControlUtilityTool.PubFontFamily, 10.5F, FontStyle.Bold);
            nbControl.Appearance.ItemPressed.Font = new Font(ControlUtilityTool.PubFontFamily, 10.5F, FontStyle.Bold);
            nbControl.Appearance.GroupHeaderActive.ForeColor = ControlUtilityTool.PubForeColorActive;
            nbControl.Appearance.NavigationPaneHeader.ForeColor = ControlUtilityTool.PubForeColorActive;
            //nbControl.BackColor = ControlUtilityTool.PubBackColorNormal;
        }

        /// <summary>
        /// 调整NavBarControl宽度
        /// </summary>
        /// <param name="nbControl"></param>
        /// <param name="minWidth"></param>
        public static int AdjustNavBarControlWidth(NavBarControl nbControl, int minWidth, bool shouldAdjustW = true)
        {
            //调整Navigation宽度
            int nvWidth = minWidth;
            foreach (NavBarGroup nvGrp in nbControl.Groups)
            {
                if (!nvGrp.Visible)
                {
                    continue;
                }
                var size = UtilityTool.GetStrPixelSize(nvGrp.Caption, nbControl.Font);
                int width = size.Width + nvGrp.GetImageSize().Width + 20;
                nvWidth = width > nvWidth ? width : nvWidth;
            }
            if (shouldAdjustW)
            {
                nbControl.Width = nvWidth;
            }
            return nvWidth;
        }

        //设置PanelControl边框线
        public static void SetPanelControlBorderLines(PanelControl pnl, bool top, bool left, bool bottom, bool right)
        {
            ButtonBorderStyle topBorder = top ? ButtonBorderStyle.Solid : ButtonBorderStyle.None;
            ButtonBorderStyle leftBorder = left ? ButtonBorderStyle.Solid : ButtonBorderStyle.None;
            ButtonBorderStyle bottomBorder = bottom ? ButtonBorderStyle.Solid : ButtonBorderStyle.None;
            ButtonBorderStyle rightBorder = right ? ButtonBorderStyle.Solid : ButtonBorderStyle.None;
            pnl.BorderStyle = BorderStyles.NoBorder;
            pnl.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, pnl.ClientRectangle,
                   Color.FromArgb(135, 182, 236), 1, leftBorder, //左边
                   Color.FromArgb(135, 182, 236), 1, topBorder, //上边
                   Color.FromArgb(135, 182, 236), 1, rightBorder, //右边
                   Color.FromArgb(135, 182, 236), 1, bottomBorder);//底边)
            };
        }

        /// <summary>
        /// 设置文本框水印文字
        /// </summary>
        /// <param name="textEdit">文本框</param>
        /// <param name="watermark">水印文字</param>
        public static void SetWatermark(TextEdit textEdit, string watermark)
        {
            textEdit.Properties.NullValuePromptShowForEmptyValue = true;
            //textEdit.Properties.ShowNullValuePromptWhenFocused = true;
            textEdit.Properties.NullValuePrompt = watermark;
        }

        /// <summary>
        /// 清空水印文字
        /// </summary>
        /// <param name="textEdit">文本框</param>
        public static void ClearWatermark(TextEdit textEdit)
        {
            if (textEdit.Properties.NullValuePromptShowForEmptyValue)
            {
                textEdit.Properties.NullValuePrompt = string.Empty;
            }
        }

        /// <summary>
        /// 设置PictureEdit风格
        /// </summary>
        /// <param name="pictureEdit"></param>
        public static void SetITLPictureEditStyle(PictureEdit pictureEdit)
        {
            pictureEdit.Properties.ShowMenu = false;  //不显示右键菜单
            pictureEdit.Properties.AllowFocused = false;  //不允许有焦点
            pictureEdit.Properties.Appearance.BackColor = Color.Transparent;  //透明背景色
            pictureEdit.Properties.SizeMode = PictureSizeMode.Stretch;  //图片拉伸
        }

        /// <summary>
        /// 设置TextEdit风格
        /// </summary>
        /// <param name="textEdit"></param>
        public static void SetITLTextEditStyle(TextEdit textEdit)
        {
            textEdit.Properties.AllowFocused = false;  //不允许有焦点
            textEdit.Properties.AutoHeight = false;  //不自动调整高度
            textEdit.Properties.AppearanceDisabled.ForeColor = Color.Black;               
            textEdit.Properties.ContextMenu = new ContextMenu();  //屏蔽右键菜单
        }

        /// <summary>
        /// 设置ComboBoxEdit风格
        /// </summary>
        /// <param name="comboBoxEdit"></param>
        public static void SetITLComboBoxEditStyle(ComboBoxEdit comboBoxEdit)
        {
            comboBoxEdit.Properties.AllowFocused = false;  //不允许有焦点
            comboBoxEdit.Properties.AutoHeight = false;  //不自动调整高度
            comboBoxEdit.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;  //只选不输入
            comboBoxEdit.Properties.PopupSizeable = false;
        }

        /// <summary>
        /// 设置控件的默认字体,大小,风格
        /// 适用于控件：LabelControl, SimpleButton, TextEdit, ComboboxEdit，CheckEdit, SpinEdit, DateEdit, RadioGroup, TreeList
        /// </summary>
        /// <param name="control"></param>
        public static void SetControlDefaultFont(Control control, float fontSize = 10.5F, FontStyle fontStyle = FontStyle.Regular)
        {
            if (control is BaseStyleControl)
            {
                BaseStyleControl button = (BaseStyleControl)control;
                button.Font = new Font(PubFontFamily, fontSize, fontStyle);
            }
            else if (control is BaseEdit)
            {
                BaseEdit edit = (BaseEdit)control;
                edit.Font = new Font(PubFontFamily, fontSize, fontStyle);
                //如果是ComboxEdit，则设置其他字体
                if (control is ComboBoxEdit)
                {
                    ComboBoxEdit comboBoxEdit = (ComboBoxEdit)control;
                    comboBoxEdit.Properties.AppearanceDropDown.Font = new Font(PubFontFamily, fontSize, fontStyle);
                    comboBoxEdit.Properties.AppearanceFocused.Font = new Font(PubFontFamily, fontSize, fontStyle);
                }
            }
            else if (control is TreeList)
            {
                (control as TreeList).Appearance.Row.Font = new Font(PubFontFamily, fontSize, fontStyle);
                (control as TreeList).Appearance.HeaderPanel.Font = new Font(PubFontFamily, fontSize, FontStyle.Bold);
            }




            else //Control的字体
            {
                control.Font = new Font(PubFontFamily, fontSize, fontStyle);
            }
        }

        /// <summary>
        /// 设置TextEdit风格，以Label形式显示
        /// </summary>
        /// <param name="edit"></param>
        public static void SetTextEditLabelStyle(TextEdit edit)
        {
            edit.ReadOnly = true;
            edit.BorderStyle = BorderStyles.NoBorder;
            edit.TabStop = false;
            edit.Properties.AllowFocused = false;
            edit.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, edit.ClientRectangle,
                    Color.Transparent, 1, ButtonBorderStyle.Solid,//左
                    Color.Transparent, 1, ButtonBorderStyle.Solid,//上
                    Color.Transparent, 1, ButtonBorderStyle.Solid,//右
                    Color.DarkGray, 1, ButtonBorderStyle.Solid); //下

            };

            edit.Cursor = Cursors.Arrow;
            edit.MaskBox.Cursor = Cursors.Arrow;
        }

        //设置SimpleButton风格
        public static void SetITLSimpleButtonFlatStyle(SimpleButton button, ButtonSize buttonSize = ButtonSize.bsNormal)
        {
            button.BorderStyle = BorderStyles.UltraFlat;
            switch (buttonSize)
            {
                case ButtonSize.bsNone:
                    break;
                default:
                    button.Height = (int)buttonSize;
                    break;
            }
            button.AllowFocus = false;
            button.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, button.ClientRectangle,
                    Color.FromArgb(135, 182, 236), 1, ButtonBorderStyle.Solid, //左边
                    Color.FromArgb(135, 182, 236), 1, ButtonBorderStyle.Solid, //上边
                    Color.FromArgb(135, 182, 236), 1, ButtonBorderStyle.Solid, //右边
                    Color.FromArgb(135, 182, 236), 1, ButtonBorderStyle.Solid);//底边)
            };
        }

        /// <summary>
        /// 设置控件背景色
        /// 适用于控件：PanelControl, LabelControl, SimpleButton, TextEdit, ComboboxEdit, CheckEdit, SpinEdit, DateEdit, RadioGroup
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="controlColor">控件颜色</param>
        /// <param name="colorType">颜色类型</param>
        public static void SetControlDefaultColor(Control control, Color controlColor, ColorType colorType = ColorType.ctBackColor)
        {

            switch (colorType)
            {
                case ColorType.ctBackColor:
                    {
                        if (control is PanelControl)
                        {
                            PanelControl pnl = (PanelControl)control;
                            pnl.BackColor = controlColor;
                        }
                        else if (control is TextEdit)
                        {
                            TextEdit edt = (TextEdit)control;
                            edt.BackColor = controlColor;
                        }
                        else if (control is SimpleButton)
                        {
                            SimpleButton btn = (SimpleButton)control;
                            btn.Appearance.BackColor = controlColor;
                        }
                        else if (control is XtraScrollableControl)
                        {
                            XtraScrollableControl scl = control as XtraScrollableControl;
                            scl.BackColor = controlColor;
                        }
                        break;
                    }
                case ColorType.ctForeColor:
                    {
                        if (control is BaseStyleControl)
                        {
                            BaseStyleControl bsControl = (BaseStyleControl)control;
                            bsControl.ForeColor = controlColor;
                        }
                        else if (control is BaseEdit)
                        {
                            BaseEdit edt = (BaseEdit)control;
                            edt.ForeColor = controlColor;
                            //如果是ComboxEdit，则设置其他字体颜色
                            if (control is ComboBoxEdit)
                            {
                                ComboBoxEdit comboBoxEdit = (ComboBoxEdit)control;
                                comboBoxEdit.Properties.AppearanceDropDown.ForeColor = controlColor;
                                comboBoxEdit.Properties.AppearanceFocused.ForeColor = controlColor;
                            }
                        }
                        break;
                    }
            }

        }

        public static void SetFocus(DevExpress.XtraEditors.BaseControl control)
        {
            if ((control.Enabled) && (control.Visible))
            {
                control.Focus();
            }
        }

        //调整最后一列的宽度，使其充满剩余空间 aOffset 未位置偏移调整值
        public static void MakeLastColumnClientInGridView(GridView aGridView)
        {
            //if ((aParentControl != null) && (aGridView.Columns.Count > 0))
            //{
            //    int width = 0;
            //    for (int index = 0; index < aGridView.Columns.Count - 1; index++)
            //    {
            //        if (aGridView.Columns[index].Visible)
            //        {
            //            width += aGridView.Columns[index].Width;
            //        }
            //    }
            //    width += aGridView.IndicatorWidth;
            //    aGridView.Columns[aGridView.Columns.Count - 1].Width = aParentControl.Width - width - aOffset;
            //}

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewRects rects =
             ((DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo)aGridView.GetViewInfo()).ViewRects;

            if (aGridView.VisibleColumns.Count > 0 && rects.ColumnTotalWidth < rects.ColumnPanelWidth)
            {
                aGridView.VisibleColumns[aGridView.VisibleColumns.Count - 1].Width += (rects.ColumnPanelWidth - rects.ColumnTotalWidth);
            }

            //else if (aGridView.VisibleColumns.Count > 0)
            //{
            //    aGridView.VisibleColumns[aGridView.VisibleColumns.Count - 1].Width -= (rects.ColumnTotalWidth - rects.ColumnPanelWidth);
            //}

        }

        //根据GridView当前的数据量调整GridView的Indicator(行指示器)宽度,若指定了aParentControl,则同时调整最后一列的宽度，使其充满剩余空间
        public static void AdjustIndicatorWidth(GridView aGridView)
        {
            int rowCount = aGridView.RowCount;
            if (rowCount <= 0)
            {
                aGridView.IndicatorWidth = -1;
            }
            else
            {
                aGridView.IndicatorWidth = 23 + rowCount.ToString().Length * 9;
            }
            //MakeLastColumnClientInGridView(aGridView);
        }

        /// <summary>
        /// 向指定GridView中增加一列
        /// </summary>
        /// <param name="gridView">目标gridview</param>
        /// <param name="fieldName">列名</param>
        /// <param name="capiton">显示名</param>
        /// <returns></returns>
        public static GridColumn AddGridViewColum(ColumnView gridView, string fieldName, string capiton = "")
        {
            GridColumn oneCol = gridView.Columns.Add();
            if (capiton == "")
            {
                oneCol.Caption = fieldName;
            }
            else
            {
                oneCol.Caption = capiton;
            }
            oneCol.FieldName = fieldName;
            oneCol.Visible = true;   //动态添加列 时：注意visible默认为false
            oneCol.OptionsColumn.AllowEdit = false;    //不允许编辑
            oneCol.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            oneCol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            oneCol.VisibleIndex = gridView.Columns.Count - 1;
            return oneCol;
        }

        //按旺龙线下物联平台系统实际需求设置GridView风格
        public static void SetITLGridViewStyle(GridView gridView, bool aIsShowRowNo = false, Function<string> GetEmptyDisplayInfo = null, bool aExtendedLastColumn = true, bool aIsSetMouseWheel = true)
        {
            gridView.OptionsBehavior.Editable = false; //不允许对单元格进行编辑

            gridView.OptionsCustomization.AllowFilter = false; //不允许允许用户对数据进行过滤操作
            gridView.OptionsCustomization.AllowSort = false; //不允许用户对数据进行排序操作
            gridView.OptionsCustomization.AllowQuickHideColumns = false; //不允许用户快速隐藏数据列
            gridView.OptionsCustomization.AllowColumnMoving = false;  //不允许移动列位置
            gridView.OptionsCustomization.AllowColumnResizing = false; //不允许改版列宽度

            gridView.OptionsMenu.EnableColumnMenu = false; //禁用许列头上的菜单
            gridView.OptionsMenu.EnableFooterMenu = false;//禁用允许页脚上的菜单
            gridView.OptionsMenu.EnableGroupPanelMenu = false; //禁用分组面板上的菜单

            gridView.OptionsSelection.EnableAppearanceFocusedCell = false; //禁止获得焦点的单格使用外观
            gridView.OptionsSelection.EnableAppearanceHideSelection = true; //允许在控件失去焦点时，外观设置应用到选择的行上
            gridView.OptionsSelection.InvertSelection = false; //设置焦点的风格应用到获得焦点的单元格，还是获得焦点的那一行的所有单元格
            gridView.OptionsSelection.MultiSelect = false; //允许多选行

            gridView.OptionsView.ColumnAutoWidth = false; //自动调整列宽，使所有列的宽度和视图的宽度匹配
            gridView.OptionsView.ColumnHeaderAutoHeight = DefaultBoolean.False; //列标题自适应高度，设置后无行指示器
            gridView.OptionsView.EnableAppearanceEvenRow = true; //允许偶数行应用界面设置 可配合aGridView.Appearance.EvenRow使用
            gridView.OptionsView.EnableAppearanceOddRow = true; //允许奇数行应用界面设置 可配合aGridView.Appearance.OddRow使用
            gridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;  //底部区域
            gridView.OptionsView.HeaderFilterButtonShowMode = FilterButtonShowMode.Button;  //列标题排序按钮样式

            gridView.Appearance.Empty.BackColor = ControlUtilityTool.PubBackColorNormal;  //空白处背景色
            gridView.Appearance.EvenRow.BackColor = ControlUtilityTool.PubTableBackColorBlue;
            gridView.Appearance.OddRow.BackColor = ControlUtilityTool.PubTableBackColorWhite;
          
            gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;  //设置焦点风格

            gridView.ScrollStyle = ScrollStyleFlags.None;
            gridView.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;  //自动显示垂直滚动条                        
            gridView.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;

            gridView.OptionsView.ShowGroupPanel = false;//禁止显示分组面板
            gridView.IndicatorWidth = -1; //左侧指示器列的宽度
            gridView.OptionsView.ShowIndicator = true;

            if (aIsShowRowNo)
            {
                gridView.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler((sender, e) =>
                {
                    if ((e.Info.IsRowIndicator) && (e.RowHandle >= 0))
                    {
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    }
                });
            }

            if (aIsSetMouseWheel)
            {
                gridView.MouseWheel += new MouseEventHandler((sender, e) =>
                {
                    //定义 MouseToMove 来判断鼠标的滚轮是向前滚动，还是向后滚动（当MouseToMove大于0的时候是向前滚动，小于0的时候向后滚动）
                    //int MouseToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
                    if (e.Delta > 0)
                    {
                        gridView.MovePrev(); //向前滚动
                    }
                    else
                    {
                        gridView.MoveNext(); //向后滚动
                    }
                });
            }

            if (GetEmptyDisplayInfo != null)
            {
                //gridView.ViewCaption = emptyDisplayInfo.Trim(); //使用视图标题（配合OptionsView.ShowViewCaption使用）作为中间存储变量，在GridView无数据时进行显示的内容
                gridView.CustomDrawEmptyForeground += new DevExpress.XtraGrid.Views.Base.CustomDrawEventHandler((sender, e) =>
                {
                    if (gridView.RowCount == 0)
                    {
                        string strHint = GetEmptyDisplayInfo();
                        Font font = new Font("微软雅黑", 30, FontStyle.Regular);
                        Graphics g = gridView.GridControl.CreateGraphics();
                        SizeF sizeF = g.MeasureString(strHint, font);
                        Rectangle rectangle = new Rectangle(
                            (int)((e.Bounds.Width - sizeF.Width) / 2), (int)((e.Bounds.Height - sizeF.Height) / 2) + 20,
                            e.Bounds.Width - 5, e.Bounds.Height - 5);
                        e.Graphics.DrawString(strHint, font, Brushes.Black, rectangle);
                        g.Dispose();
                    }
                });
            }

            if (aExtendedLastColumn)
            {
                if (gridView.GridControl != null)
                {
                    gridView.GridControl.Resize += new EventHandler((sender, e) =>
                    {
                        if (!(sender is GridControl))
                        {
                            return;
                        }

                        GridControl grdCtrl = sender as GridControl;

                        if (grdCtrl.MainView is GridView)
                        {
                            MakeLastColumnClientInGridView(grdCtrl.MainView as GridView);
                        }
                    });
                }

            }

        }

        /// <summary>
        /// 设置DateEdit风格
        /// </summary>
        /// <param name="dateEdit"></param>
        public static void SetITLDateEditStyle(DateEdit dateEdit)
        {
            dateEdit.Properties.AllowFocused = false;  //屏蔽焦点
            dateEdit.Properties.AppearanceDisabled.ForeColor = Color.Black;
            dateEdit.Properties.ShowClear = false;
            dateEdit.Properties.ContextMenu = new ContextMenu();  //屏蔽右键菜单
        }

        /// <summary>
        /// 设置DateEdit显示日期和时间，均可在弹出框中编辑
        /// </summary>
        /// <param name="dt">DateEdit控件</param>
        /// <param name="dateFormat">日期显示格式</param>
        /// <param name="timeFormat">时间显示与编辑格式</param>
        public static void SetDateEditShowTime(DateEdit dt, string dateFormat = "yyyy-MM-dd", string timeFormat = "HH:mm:ss")
        {
            var displayFormat = dateFormat.Trim() + " " + timeFormat.Trim();
            //设置DateEdit显示日期和时间
            dt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dt.Properties.DisplayFormat.FormatString = displayFormat;
            dt.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dt.Properties.EditFormat.FormatString = displayFormat;
            dt.Properties.Mask.EditMask = displayFormat;
            //显示时钟图标
            //dateValid.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            //dateValid.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
            dt.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
            //设置时钟格式
            dt.Properties.VistaTimeProperties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dt.Properties.VistaTimeProperties.DisplayFormat.FormatString = timeFormat;
            dt.Properties.VistaTimeProperties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            dt.Properties.VistaTimeProperties.EditFormat.FormatString = timeFormat;
            dt.Properties.VistaTimeProperties.Mask.EditMask = timeFormat;

            dt.Properties.MinValue = (DateTime)(System.Data.SqlTypes.SqlDateTime.MinValue);
            dt.Properties.NullDate = (DateTime)(System.Data.SqlTypes.SqlDateTime.MinValue);
            dt.Properties.AllowNullInput = DefaultBoolean.False;
        }

        public static void SetITLXtraTabControlStyle(XtraTabControl tabCtrl)
        {
            //tabCtrl.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPageHeaders;
            tabCtrl.AppearancePage.HeaderActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(163)))), ((int)(((byte)(220)))));
        }

        public static void SetITLCheckedComboBoxEditStyle(CheckedComboBoxEdit chkcbb)
        {
            chkcbb.Properties.AllowFocused = false;  //不允许有焦点
            chkcbb.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            chkcbb.Properties.AutoHeight = false;
            chkcbb.Properties.ItemAutoHeight = false;
        }

        public static void SetITLTreeListStyle(TreeList treeList, Function<string> GetEmptyDisplayInfo = null)
        {
            treeList.OptionsMenu.EnableColumnMenu = false; //禁用许列头上的菜单
            treeList.OptionsMenu.EnableFooterMenu = false;//禁用允许页脚上的菜单

            treeList.OptionsBehavior.AllowRecursiveNodeChecking = true; //父节点的选中状态影响其子节点的选中状态
            treeList.OptionsBehavior.Editable = false;  //设置不可编辑
            treeList.OptionsSelection.EnableAppearanceFocusedCell = false;  //焦点单元格不受外观控制
            treeList.OptionsSelection.InvertSelection = false; //设置焦点的风格应用到获得焦点的单元格，还是获得焦点的那一行的所有单元格\
            treeList.OptionsView.ShowColumns = true;   //显示列标题
            treeList.OptionsView.ShowFilterPanelMode = DevExpress.XtraTreeList.ShowFilterPanelMode.Never;  //底部区域
            treeList.OptionsView.ShowIndicator = false; //不显示行指示器
                                                        //treeList.OptionsView.ShowRoot = false;  //不显示根节点


            treeList.VertScrollVisibility = DevExpress.XtraTreeList.ScrollVisibility.Auto;  //自动显示垂直滚动条
            treeList.OptionsView.EnableAppearanceEvenRow = true;  //奇数行使用外观
            treeList.OptionsView.EnableAppearanceOddRow = true;  //偶数行使用外观
            treeList.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None; //焦点样式
            treeList.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;  //列标题排序按钮样式
            treeList.OptionsView.ShowHorzLines = true;  //显示水平线
            treeList.OptionsView.ShowVertLines = true;  //显示垂直线

            treeList.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;  //列标题自适应高度，设置后无行指示器
            treeList.OptionsView.AutoWidth = true;  //允许自动调整列宽
            //treeList.Appearance.Empty.BackColor = ControlUtilityTool.PubBackColorNormal;  //空白处背景色
            treeList.Appearance.OddRow.BackColor = ControlUtilityTool.PubTableBackColorBlue;  //偶数行背景色
            treeList.Appearance.EvenRow.BackColor = ControlUtilityTool.PubTableBackColorWhite; //奇数行背景色
            treeList.CustomDrawNodeIndicator += (sender, e) =>  //绘制行指示器（treeList.OptionsView.ShowIndicator = true时有效）
            {
                TreeList tmpTree = sender as TreeList;
                DevExpress.Utils.Drawing.IndicatorObjectInfoArgs args = e.ObjectArgs as DevExpress.Utils.Drawing.IndicatorObjectInfoArgs;
                if (args != null)
                {
                    int rowNum = tmpTree.GetVisibleIndexByNode(e.Node) + 1;
                    args.DisplayText = rowNum.ToString();
                }
            };
            treeList.IndicatorWidth = (treeList.Nodes.Count + 1).ToString().Length * 9 + 23; //行指示器宽度（treeList.OptionsView.ShowIndicator = true时有效）
            treeList.RowHeight = 22; //行高
            treeList.OptionsCustomization.AllowColumnMoving = false;//不允许拖动列
            foreach (TreeListColumn treeListColumn in treeList.Columns)
            {
                treeListColumn.OptionsColumn.AllowFocus = false;  //不允许列头有焦点
                treeListColumn.OptionsColumn.AllowMove = false;  //列头不允许拖动
                treeListColumn.OptionsColumn.AllowSort = false; //不允许排序
                treeListColumn.OptionsColumn.AllowSize = false;  //不允许拖动改变列宽
                treeListColumn.OptionsColumn.FixedWidth = false; //固定宽度,仅在treeList.OptionsView.AutoWidth = true时有效
            }
            if (GetEmptyDisplayInfo != null)
            {
                //无数据时，显示文本
                treeList.CustomDrawEmptyArea += (sender, e) =>
                {
                    if (treeList.VisibleNodesCount <= 0)
                    {
                        string strHint = GetEmptyDisplayInfo();
                        Font font = new Font(PubFontFamily, 30, FontStyle.Regular);
                        Graphics g = treeList.CreateGraphics();
                        SizeF sizeF = g.MeasureString(strHint, font);
                        Rectangle rectangle = new Rectangle(
                           (int)((e.Bounds.Width - sizeF.Width) / 2), (int)((e.Bounds.Height - sizeF.Height) / 2) + 20,
                           e.Bounds.Width - 5, e.Bounds.Height - 5);
                        e.Graphics.FillRegion(Brushes.White, e.EmptyAreaRegion);
                        e.Graphics.DrawString(strHint, font, Brushes.Black, rectangle);
                        g.Dispose();
                        e.Handled = true;
                    }
                };
            }
        }

        public static TreeListColumn AddTreeListColumn(TreeList treeList, string aFieldName, string aColCapiton = "")
        {
            TreeListColumn oneCol = treeList.Columns.Add();
            if (aColCapiton == "")
            {
                oneCol.Caption = aFieldName;
            }
            else
            {
                oneCol.Caption = aColCapiton;
            }
            oneCol.FieldName = aFieldName;
            oneCol.Visible = true;
            oneCol.AppearanceCell.Options.UseTextOptions = true;
            oneCol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            oneCol.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            oneCol.OptionsColumn.AllowSort = false;
            oneCol.AppearanceHeader.Options.UseTextOptions = true;
            oneCol.OptionsColumn.AllowEdit = false;
            oneCol.OptionsColumn.AllowMove = false;
            oneCol.VisibleIndex = treeList.Columns.Count - 1;
            return oneCol;
        }

        /// <summary>
        /// 设置MemoEdit风格
        /// </summary>
        /// <param name="memoEdit"></param>
        public static void SetITLMemoEditStyle(MemoEdit memoEdit)
        {
            memoEdit.Properties.AllowFocused = false;  //屏蔽焦点
            memoEdit.Properties.ContextMenu = new ContextMenu();  //屏蔽右键菜单
        }

        /// <summary>
        /// 设置LayoutControl风格
        /// </summary>
        /// <param name="layoutControl"></param>
        public static void SetITLLayOutControlStyle(LayoutControl layoutControl)
        {
            layoutControl.AllowCustomization = false;  //屏蔽右键菜单
        }

        /// <summary>
        /// 设置IP输入正则
        /// </summary>
        /// <param name="edt"></param>
        public static void SetTextEditIPRegEx(TextEdit edt)
        {
            string byteMask = @"(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(25[0-5]))";
            string ipMask = byteMask + @"\." + byteMask + @"\." + byteMask + @"\." + byteMask;
            edt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            edt.Properties.Mask.EditMask = ipMask;
            edt.Properties.Mask.ShowPlaceHolders = true;
            edt.InvalidValue += (sender, e) =>
            {
                e.ExceptionMode = ExceptionMode.Ignore;
            };
        }

        /// <summary>
        /// 设置edit的输入值为整数，且限定大小，如果不加限制则默认9999
        /// </summary>
        /// <param name="edt"></param>
        /// <param name="maxNum"></param>
        public static void etTextEditNumRegEx(TextEdit edt, int maxNum = 9999)
        {
            string numMask = $"[0-{maxNum}]";
            edt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            edt.Properties.Mask.EditMask = numMask;
            edt.Properties.Mask.ShowPlaceHolders = true;
            edt.InvalidValue += (sender, e) =>
            {
                e.ExceptionMode = ExceptionMode.Ignore;
            };
        }

        /// <summary>
        /// 设置子网掩码输入正则
        /// </summary>
        /// <param name="edt"></param>
        public static void SetTextEditSubnetMaskRegEx(TextEdit edt)
        {
            edt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            edt.Properties.Mask.EditMask = @"((128|192)|2(24|4[08]|5[245]))(\.(0|(128|192)|2((24)|(4[08])|(5[245])))){3}";
            edt.Properties.Mask.ShowPlaceHolders = true;
            edt.InvalidValue += (sender, e) =>
            {
                e.ExceptionMode = ExceptionMode.Ignore;
            };
        }
        /// <summary>
        /// 设置端口号输入正则（1~65535）
        /// </summary>
        /// <param name="edt"></param>
        public static void SetTextEditPortRegEx(TextEdit edt)
        {
            edt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            edt.Properties.Mask.EditMask = @"([1-9]|[1-9]\d{1,3}|[1-5]\d{4}|6[0-5]{2}[0-3][0-5])";
            edt.Properties.Mask.ShowPlaceHolders = true;
            edt.InvalidValue += (sender, e) =>
            {
                e.ExceptionMode = ExceptionMode.Ignore;
            };
        }

        /// <summary>
        /// 设置名称输入正则
        /// </summary>
        /// <param name="edt"></param>
        public static void SetNameInputEditRegEx(TextEdit edt, string mask = NameEditMask)
        {
            edt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            edt.Properties.Mask.EditMask = mask;
            edt.Properties.Mask.ShowPlaceHolders = false;
        }
        /// <summary>
        /// 设置名称输入正则
        /// </summary>
        /// <param name="edt"></param>
        public static void SetNameInputEditRegEx(RepositoryItemTextEdit edt)
        {
            edt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            edt.Mask.EditMask = NameEditMask;
            edt.Mask.ShowPlaceHolders = false;
        }
    }
}
