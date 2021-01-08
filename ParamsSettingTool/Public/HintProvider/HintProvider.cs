using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ITL.Public
{

    public class HintProvider
    {
        ///// <summary>
        ///// 启动百分比进度条
        ///// </summary>
        ///// <param name="total">进度总值</param>
        ///// <param name="message">提示信息</param>
        ///// <param name="callback">Hint回调返回参数</param>
        //public static void StartProgress(SynchronizationContext uiContext, int total, string message, Action<HintArgs> callback = null)
        //{
        //    ProgressFormManage.Singleton.StartProgress(uiContext, total, message, callback);
        //}
        ///// <summary>
        ///// 百分比进度条
        ///// </summary>
        ///// <param name="current">当前进度值</param>
        ///// <param name="message">提示消息</param>
        //public static void ShowProcess(int current, string message)
        //{
        //    ProgressFormManage.Singleton.ShowProgress(current, message);
        //}
        ///// <summary>
        ///// 关闭百分比进度条
        ///// </summary>
        //public static void ProcessDone()
        //{
        //    ProgressFormManage.Singleton.ShowProgress(int.MaxValue, "");
        //}

        /// <summary>
        /// 自动隐藏提示框
        /// </summary>
        /// <param name="parentForm">所有者，影响显示位置，为null时屏幕居中</param>
        /// <param name="text"></param>
        /// <param name="iconType"></param>
        /// <param name="atLeastDuration"></param>
        /// <param name="atMostDuration"></param>
        /// <param name="waitForClose"></param>
        public static void ShowAutoCloseDialog(Form parentForm, string text, HintIconType iconType = HintIconType.OK,int atLeastDuration = 1500, int atMostDuration = 5000, bool waitForClose = false)
        {
            AutoCloseDialog.ShowHint(text, iconType, atLeastDuration, atMostDuration, parentForm, waitForClose);
        }

        public static void ShowBallonTip(Control control, string text,
            ToolTipLocation toolTipLocation = ToolTipLocation.TopRight, int duration = 3000)
        {
            BalloonToolTip.ShowBalloon(text, control, toolTipLocation, duration);
        }

        /// <summary>
        /// 启动无进度等待条，多次启动仍是同一个
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="description"></param>
        /// <param name="identity">等待方标识</param>
        /// <param name="handleCloseCallback"></param>
        /// <param name="delay">延迟启动毫秒数</param>
        /// <param name="owner">所有者，影响显示位置，为null时屏幕居中</param>
        public static void StartWaiting(Form owner, string caption, string description, object identity,
            Action handleCloseCallback = null, int showDelay = 100, int showCloseButtonDelay = 5000)
        {
            WaitFormManager.Singleton.StartWaiting(owner, caption, description, identity, handleCloseCallback, showDelay, showCloseButtonDelay);
        }
        /// <summary>
        /// 更新无进度等待条文本，caption/description为null时不更新界面文本
        /// 若当前没有已经启动的等待条，则无效
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="description"></param>
        /// <param name="identity"></param>
        public static void Waiting(string caption, string description, object identity)
        {
            WaitFormManager.Singleton.Waiting(caption, description, identity);
        }
        /// <summary>
        /// 完成当前等待方下的等待更新
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="isForceClose">强制关闭等待条，忽略其他Control</param>
        public static void WaitingDone(object identity, bool isForceClose = false)
        {
            WaitFormManager.Singleton.Done(identity, isForceClose);
        }

        /// <summary>
        /// 显示确认框，showMSeconds = 0时表示不进行倒计时
        /// </summary>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <param name="buttons"></param>
        /// <param name="defaultButton"></param>
        /// <param name="showMSeconds"></param>
        /// <returns></returns>
        public static DialogResult ShowConfirmDialog(Form owner, string text, ConfirmFormIcons icon = ConfirmFormIcons.Hint, 
            ConfirmFormButtons buttons = ConfirmFormButtons.OK, 
            ConfirmFormDefaultButton defaultButton = ConfirmFormDefaultButton.OK, int showMSeconds = 0)
        {
            return ConfirmForm.ShowConfirmForm(text, icon, buttons, defaultButton, showMSeconds, owner);
        }
    }
}
