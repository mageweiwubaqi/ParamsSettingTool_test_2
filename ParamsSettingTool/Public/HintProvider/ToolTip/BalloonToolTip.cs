using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using DevExpress.Utils;

namespace ITL.Public
{


   /// <summary>
   /// 气泡提示类
   /// </summary>
    public class BalloonToolTip
    {
        private static ToolTipController f_ToolTipControler;
        public BalloonToolTip()
        {
            if (f_ToolTipControler == null)
            {
                f_ToolTipControler = new ToolTipController();
                f_ToolTipControler.ShowBeak = true;
                f_ToolTipControler.ShowShadow = true;
                f_ToolTipControler.Rounded = true;
                f_ToolTipControler.CloseOnClick = DefaultBoolean.True;
            }
      
        }

        private void Show(string toolTip, Control control, ToolTipLocation toolTipLocation, int duration)
        {
            f_ToolTipControler.AutoPopDelay = 2000;
            f_ToolTipControler.ShowHint(toolTip, control, toolTipLocation);
            
        }

        public static void ShowBalloon(string toolTip, Control control, ToolTipLocation toolTipLocation,
            int duration)
        {
            BalloonToolTip tip = new BalloonToolTip();
            tip.Show(toolTip, control, toolTipLocation, duration);
        }

    }


}
