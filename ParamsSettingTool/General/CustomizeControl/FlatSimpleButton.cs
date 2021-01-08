using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using ITL.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ITL.General
{
    public partial class FlatSimpleButton : SimpleButton
    {
        private Color f_HotBackColor = Color.FromArgb(60, 195, 245);

        public FlatSimpleButton()
        {

            InitializeComponent();
            bool designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            if (!designMode)
            {
                SetMyUltraFlatPainter(f_HotBackColor);
            }
        }

        public FlatSimpleButton(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            bool designMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            if (!designMode)
            {
                SetMyUltraFlatPainter(f_HotBackColor);
            }
        }

        public FlatSimpleButton(Color color)
        {
            InitializeComponent();

            SetMyUltraFlatPainter(color);
        }

        public Color HotBackColor
        {
            get
            {
                return f_HotBackColor;
            }
            set
            {
                f_HotBackColor = value;
                if (painter != null)
                {
                    painter.HotBackColor = value;
                }

            }

        }

        private MyUltraFlatLookAndFeelPainters painter;
        void SetMyUltraFlatPainter(Color hotBackColor)
        {
            Type type = typeof(DevExpress.LookAndFeel.LookAndFeelPainterHelper);
            FieldInfo fi = type.GetField("painters", BindingFlags.Static | BindingFlags.NonPublic);
            BaseLookAndFeelPainters[] painters = (BaseLookAndFeelPainters[])fi.GetValue(null);
            painter = new MyUltraFlatLookAndFeelPainters(null);
            painter.HotBackColor = hotBackColor;
            painters[(int)ActiveLookAndFeelStyle.UltraFlat] = painter;

            //this.Appearance.BackColor = Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(148)))), ((int)(((byte)(248)))));
            this.Appearance.BackColor = Color.FromArgb(9, 163, 220);  //QQ风格
            this.Appearance.Font = new Font(ControlUtilityTool.PubFontFamily, 10.5F, FontStyle.Bold);
            this.Appearance.ForeColor = Color.White;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            this.AllowFocus = false;

        }

        private string f_FullText = "";

        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                base.Text = GetTextToDisplay(value);
            }
        }

        string GetTextToDisplay(string fullText)
        {
            //记住原文本值
            f_FullText = fullText;
            string displayText = fullText;

            int maxW = this.MaximumSize.Width;
            if (maxW > 0)
            {
                Graphics graphics = this.CreateGraphics();
                int? imgW = this.Image?.Width;
                int fulltxtW = graphics.MeasureString(fullText, this.Font).ToSize().Width;

                if (fulltxtW + imgW > maxW)
                {
                    for (int i = 0; i < fullText.Length; i++)
                    {
                        string txt = fullText.Substring(0, i + 1);
                        int txtW = graphics.MeasureString(txt, this.Font).ToSize().Width;
                        if (txtW + imgW > maxW - 20)
                        {
                            displayText = txt + "..";
                            SuperToolTip superTip = new SuperToolTip();
                            superTip.Items.Add(fullText);
                            this.SuperTip = superTip;
                            break;
                        }
                    }
                }
            }
            return displayText;
        }

    }

    class MyUltraFlatLookAndFeelPainters : UltraFlatLookAndFeelPainters
    {
        private Color f_HotBackColor = Color.Empty;
        public Color HotBackColor
        {
            get
            {
                return f_HotBackColor;
            }
            set
            {
                f_HotBackColor = value;
                if (painter != null)
                {
                    painter.HotBackColor = value;
                }
            }
        }

        public MyUltraFlatLookAndFeelPainters(UserLookAndFeel owner) : base(owner)
        {
        }

        private MyUltraFlatButtonObjectPainter painter;
        protected override ObjectPainter CreateButtonPainter()
        {
            if (painter == null)
            {
                painter = new MyUltraFlatButtonObjectPainter();
                painter.HotBackColor = HotBackColor;
            }
            return painter;
        }
    }

    class MyUltraFlatButtonObjectPainter : UltraFlatButtonObjectPainter
    {
        private Color f_HotBackColor = Color.Empty;
        public Color HotBackColor
        {
            get
            {
                if (brush != null)
                {
                    return brush.Color;
                }
                return f_HotBackColor;
            }
            set
            {
                f_HotBackColor = value;
                if (brush != null)
                {
                    brush.Color = value;
                }
            }
        }

        private SolidBrush brush;
        protected override Brush GetHotBackBrush(ObjectInfoArgs e, bool pressed)
        {
            if (brush == null)
            {
                brush = new SolidBrush(HotBackColor);
            }
            return brush;
        }
        protected override Color GetHotBorderColor(ObjectInfoArgs e, bool pressed)
        {
            return HotBackColor;
        }
    }
}
