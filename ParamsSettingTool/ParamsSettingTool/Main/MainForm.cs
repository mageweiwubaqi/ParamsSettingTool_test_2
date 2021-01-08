using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ITL.General;
using ITL.Framework;
using DevExpress.XtraEditors;
using ITL.Public;
using DevExpress.LookAndFeel;
using System.IO;
using ITL.DataDefine;
using ITL.ParamsSettingTool.ParamsSettingTool.Devices.GroupLinkageCtrl;
using ITL.ParamsSettingTool.ParamsSettingTool.Devices.CloudEntrance;
using ITL.ParamsSettingTool.ParamsSettingTool.Devices.CloudGroupLinkageCtrl;

namespace ITL.ParamsSettingTool
{
    public partial class MainForm : GeneralForm
    {       
        private Dictionary<string, GeneralUserControl> f_UserControls = null;
        private UdpListener f_UdpListener = null;
        string title = "";
        public UdpListener f_UdpRevListener = null;
        public bool IsClickRead { get; set; }

        public MainForm()
        {

            InitializeComponent();

          
        }

        /// <summary>
        /// 解决界面大小变化时闪烁的问题
        /// </summary>
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;////用双缓冲从下到上绘制窗口的所有子孙
        //        return cp;
        //    }
        //}

        protected override void InitUIOnLoad()
        {
            base.InitUIOnLoad();
            if (!UtilityTool.IsDesignMode())
            {
                //仅执行一次，设置程序默认字体
                WindowsFormsSettings.DefaultFont = new Font(ControlUtilityTool.PubFontFamily, 10.5F, FontStyle.Regular);
                //设置皮肤
                UserLookAndFeel.Default.SetSkinStyle(ControlUtilityTool.DEFAULT_SKIN_NAME);
                //解决滚轮无法控制滚动条的问题
                WindowsFormsSettings.SmartMouseWheelProcessing = false;
                //设置背景图
                this.LoadBackgroundImg();
                //加载dll
                this.LoadDllFiles();
                //加载楼层表文件
                this.CreateFloorTableFiles();
            }

            if (!this.ShowInputPsdForm())
            {
                this.Close();
                return;
            }

        }

        private void CreateFloorTableFiles()
        {
            #region 楼层映射表参数属性
            iniFileControl ini = new iniFileControl(Application.StartupPath + @"\CloudAdd.ini");
            if (ini.ExistINIFile())//验证是否存在文件，存在就读取
            {

            }
            else
            {
                //ini.IniWriteValue("权限标识", "colAuthFlag", "");
                //ini.IniWriteValue("按键名称", "colKeyName", "");
                //ini.IniWriteValue("实际楼层", "colActualFloor", "");
                //ini.IniWriteValue("端子号", "colDevNo", "");
                //ini.IniWriteValue("检测楼层", "colDevCheckFloor", "");
                ini.IniWriteValue("云服务配置", "地址", "http://smartcard.jia-r.com/smartCard/syncdata/device/elevator/getCloudElevatorList");
                ini.IniWriteValue("云楼层对应表配置", "地址", "http://smartcard.jia-r.com/smartCard/syncdata/device/elevator/getElevatorFloorConfig");

            }
            #endregion
        }

        protected override void InitUIOnShown()
        {
            HintProvider.WaitingDone(Application.ProductName);
            base.InitUIOnShown();

            this.pnlButtom.Visible = false;
            this.btnMin.Visible = true;
            ControlUtilityTool.SetControlDefaultFont(this.lblCaption, 15, FontStyle.Bold);

            //this.lblCaption.Text = "网络设备参数设置工具";
            string strVersion = string.Format("V{0}", StrUtils.GetFileVersion(Path.Combine(Application.StartupPath, "ParamsSettingTool.exe")));
            title = "网络设备参数设置工具" + "(" + strVersion + ")";
            this.lblCaption.Text = title;

            f_UserControls = new Dictionary<string, GeneralUserControl>();

            //启动Udp监听
            int udpPort = StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_SOURCE_PORT].ToString(), AppConst.UDP_SOURCE_PORT);
            f_UdpListener = new UdpListener(udpPort);
            string errMsg = string.Empty;
            if(!f_UdpListener.StartUdp(ref errMsg))
            {
                HintProvider.ShowAutoCloseDialog(this, string.Format("启动Udp监听失败，错误：{0}", errMsg));
            }
            AppXmlConfig.Singleton.Save();


            //暂时无云对讲设备
            this.nbItemCloudIntercom.Visible = false;
        }

        protected override void InitUIEvents()
        {
            base.InitUIEvents();
            this.nbItemCloudElevator.LinkClicked += this.nbItemCloudElevator_LinkClicked;
            this.nbItemCloudIntercom.LinkClicked += this.nbItemCloudIntercom_LinkClicked;
            this.nbItemLinkageCtrl.LinkClicked += this.nbItemLinkageCtrl_LinkClicked;
            this.nbItemCloudGroupLinkage.LinkClicked += this.nbItemCloudGroupLinkage_LinkClicked;
            this.nbItemGroupLinkControl.LinkClicked += this.nbItemGroupLinkControl_LinkClicked;
            this.nbItemCouldEntrance.LinkClicked += this.nbItemCouldEntrance_LinkClicked;
            this.FormClosing += this.MainForm_FormClosing;

            //新增群控器，原群控器改，名为云群控器
            this.nbItemGroupLinkage.LinkClicked += this.nbItemGroupLinkage_LinkClicked;
        }

        /// <summary>
        /// 加载dll依赖
        /// </summary>
        private void LoadDllFiles()
        {
            string strPath = Path.Combine(Application.StartupPath, @"DependentFiles\Delphi");
            if (!Directory.Exists(strPath))
            {
                string strMsg = string.Format("{0} not exists!", strPath);
                RunLog.Log(strMsg, LogType.ltError);
                HintProvider.ShowConfirmDialog(null, strMsg, buttons: ConfirmFormButtons.OK);
                return;
            }
            UtilityTool.LoadDllFile(strPath, TripleDESIntf.TRIDES_DLL);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            f_UdpListener.StopUdp();
        }

        /// <summary>
        /// 显示背景图片
        /// </summary>
        private void LoadBackgroundImg()
        {
            string imgPath = Path.Combine(Environment.CurrentDirectory, @"DependentFiles\Image\home_page.jpg");
            if (File.Exists(imgPath))
            {
                try
                {
                    this.pnlReportContainer.ContentImage = Image.FromFile(imgPath);
                    this.pnlReportContainer.ContentImageAlignment = ContentAlignment.MiddleCenter;
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// 显示输入密码的界面
        /// </summary>
        /// <returns></returns>
        private bool ShowInputPsdForm()
        {
            //判断是否设置过密码
            AppEnv.Singleton.SystemPsd = AppXmlConfig.Singleton[AppXmlConfig.SYSTEM_PSD].ToString();
            if ((AppEnv.Singleton.SystemPsd.Length == 16) && (AppEnv.Singleton.SystemPsd != KeyMacOperate.DEFAULT_SYSTEM_ENCRY_PSD))
            {
                return true;
            }
            InputPsdForm psdForm = new InputPsdForm();
            psdForm.Owner = this;
            if (psdForm.ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 动态加载窗体到pnlReportContainer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ucName"></param>
        private void ShowUserControl<T>(string ucName) where T : GeneralDeviceUserControl
        {
            foreach(GeneralDeviceUserControl userControl in f_UserControls.Values)
            {
                userControl.Visible = false;
            }
            if (f_UserControls.ContainsKey(ucName))
            {
                GeneralUserControl uc = f_UserControls[ucName];
                uc.Visible = true;
                uc.BringToFront();
                return;
            }
            HintProvider.StartWaiting(null, "正在加载", "", Application.ProductName, showDelay: 0, showCloseButtonDelay: int.MaxValue);
            T t = UtilityTool.ShowUserControl<T>(this.pnlReportContainer);
            //绑定udp监听器到pnlReportContainer
            t.UdpListener = f_UdpListener;
            //绑定接收数据的函数
            t.UdpListener.RecvCallback += t.RecvCallBack;
            f_UserControls.Add(ucName, t);
        }

        private void nbItemCloudElevator_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.lblCaption.Text = title+"—云电梯";
            this.SetCaptionPosition();
            this.ShowUserControl<CloudElevatorUserControl>(nameof(CloudElevatorUserControl));
        }

        private void nbItemCloudIntercom_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.lblCaption.Text = title + "—云对讲";
            this.SetCaptionPosition();
            this.ShowUserControl<CloudIntercomUserControl>(nameof(CloudIntercomUserControl));
        }

        private void nbItemLinkageCtrl_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.lblCaption.Text = title + "—协议控制器";
            this.SetCaptionPosition();
            this.ShowUserControl<LinkageCtrlUserControl>(nameof(LinkageCtrlUserControl));
        }
        private void nbItemCloudGroupLinkage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.lblCaption.Text = title + "—云群控器";
            this.SetCaptionPosition();
            this.ShowUserControl<CloudGroupLinkageCtrlUserControl>(nameof(CloudGroupLinkageCtrlUserControl));
        }
        //nbItemGroupLinkControl_LinkClicked
        private void nbItemGroupLinkControl_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.lblCaption.Text = title + "—云联动器";
            this.SetCaptionPosition();
            this.ShowUserControl<CloudLinkCtrlUserControl>(nameof(CloudLinkCtrlUserControl));
        }
        
        private void nbItemCouldEntrance_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.lblCaption.Text = title + "—云门禁";
            this.SetCaptionPosition();
            this.ShowUserControl<CloudEntrancelUserControl>(nameof(CloudEntrancelUserControl));
        }

        private void nbItemGroupLinkage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            this.lblCaption.Text = title + "—群控器";
            this.SetCaptionPosition();
            this.ShowUserControl<GroupLinkageCtrlUserControl>(nameof(GroupLinkageCtrlUserControl));
        }
    }
}
