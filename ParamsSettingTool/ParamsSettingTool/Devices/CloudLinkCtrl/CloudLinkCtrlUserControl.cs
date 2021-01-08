///<summary>
///模块编号：云联动器参数设置模块
///作用：对云联动器进行搜索、参数设置
///作者：韦将杰
///编写日期：2020-04-27（注释日期）
///</summary>

///<summary>
///Log编号：1
///修改描述：新增新基点 NewBenchmark IP和地址
///1、增加对应字段常量
///2、增加对应字段名称常量
///3、新增对应UI，设置风格，绑定事件
///4、数据表增加列（InitDevicesDataTable），列名称InitDevicesGridView
///5、云联动器设备类型增加新基点IP和端口、是否具有新基点属性HasNewBenchmark  CloudEntranceInfo 
///6、增加设备到UI （AddOneDeviceToUI） 
///7、解析命令返回信息，判断是否有新基点属性，有则解析（AnalysisSearchDevicesRecvData）
///8、双击行，将数据表字段加载到对应UI控件 （ExcuteDoubleClick）
///9、将数据行的数据加载到界面(GetEntranceInfoByFocusRow)
///10、下载参数（DownParams）
///11、UI数据加载到参数（GetDeviceInfoBeforeDown）
///12、参数下载（DownBasicParams）
///13、组装数据（GetBasicParamsWriteCmdStr）
///作者：韦将杰
///修改日期：2020-04-27
///</summary>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using ITL.Public;
using DevExpress.XtraGrid.Columns;
using ITL.DataDefine;
using ITL.Framework;
using System.Net;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid;

using DevExpress.XtraTab;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Repository;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Threading;

using System.Threading.Tasks;
using DevExpress.Data.Browsing.Design;
using Newtonsoft.Json.Linq;

//namespace ITL.ParamsSettingTool.ParamsSettingTool.Devices.CloudLinkCtrl
namespace ITL.ParamsSettingTool
{
    public partial class CloudLinkCtrlUserControl : GeneralDeviceUserControl
    {
        public CloudLinkCtrlUserControl()
        {
            InitializeComponent();
        }

        #region 字段常量定义
        private const string DNS_SERVER_IP = "dns_server_ip";  //DNS服务器IP
        private const string DHCP_FUNCTION = "dhcp_function"; //DHCP功能
        private const string EI_SERVER_IP = "ei_server_ip";  //线下物联平台服务器IP
        private const string EI_SERVER_PORT = "ei_server_port";  //线下物联平台服务器端口
        private const string LINKAGE_PORT = "linkage_port"; //联动控制器端口

        private const string PROJECT_NO = "Project_No";//项目编号
        private const string VERSION = "VerSion";//版本号
        private const string LINKAGE_PRAM = "mj_linkage_pram";
        private const string STATUS = "dev_status";

        /// <summary>
        /// 是否具有新基点属性
        /// </summary>
        private const string HAS_NEWBENCHMARK = "has_newbenchmark";
        /// <summary>
        /// 新基点IP
        /// </summary>
        private const string NEWBENCHMARK_IP = "newbenchmark_ip";
        /// <summary>
        /// 新基点端口
        /// </summary>
        private const string NEWBENCHMARK_PORT = "newbenchmark_port";
        #endregion

        #region 字段名称常量定义
        private const string DNS_SERVER_IP_ALIAS = "DNS服务器IP地址";  //DNS服务器IP
        private const string DHCP_FUNCTION_ALIAS = "DHCP功能"; //DHCP功能
        private const string EI_SERVER_IP_ALIAS = "线下物联平台服务器IP地址";  //线下物联平台服务器IP

        private const string EI_SERVER_PORT_ALIAS = "线下物联平台服务器端口";  //线下物联平台服务器端口
        private const string PROJECT_NO_ALIAS = "项目编号";
        private const string VERSION_ALIAS = "版本号";
        private const string MJ_LINKAGE_PRAM = "门禁联动参数";
        private const string DEV_STATUS= "设备状态";

        /// <summary>
        /// 新基点IP
        /// </summary>
        private const string NEWBENCHMARK_IP_ALIAS = "新基点IP";
        /// <summary>
        /// 新基点端口
        /// </summary>
        private const string NEWBENCHMARK_PORT_ALIAS = "新基点端口";
        #endregion

        private CloudLinkageInfoCtrlInfo f_CurrentCloudLinkageInfo = null;  //当前操作的设备信息

        private LocalDBOperate f_LocalDBOperate = null; //数据库操作类
        private bool f_IsRefresh = false;
        private string c_AuthFlag = string.Empty; //权限标识
        private string c_KeyName = string.Empty;//按键名称
        private string c_ActualFloor = string.Empty; //实际楼层

        private string c_DevNo = string.Empty; //端子号
        private string c_DevCheckFloor = string.Empty; //检查楼层
        private bool R_Btn_Click;
        private bool W_Btn_Click;
        private string Select_AuthFlag = string.Empty; //选中的权限标识

        private string Select_KeyName = string.Empty; //选中的按键名称
        private string Select_ActualFloor = string.Empty; //选中的实际楼层
        private string Select_DevNo = string.Empty; //选中的端子号
        private string Select_DevCheckFloor = string.Empty; //选中的检测楼层
        public ResponseInfo UserControl_FloorTable = null;

        private Action f_UserControl_FloorTable;
        public string testStr = string.Empty;
        private string f_StratAuthFlag = string.Empty;
        private string f_EndAuthFlag = string.Empty;
        private string f_StartDevNo = string.Empty;

        private bool IsCloudDownFloor = false;
        private bool IsBathProcess = false; //批处理,快速设置
        private bool IsAllProcess = false;//全部处理.

        List<object> listDevNoA = new List<object>();
        List<object> listDevNoB = new List<object>();
        List<object> listDevNoCompare = new List<object>();
        List<object> listDeActullyFloor = new List<object>();
        List<object> listDevCheckFloor = new List<object>();

        List<object> listCompareCheckFloor = new List<object>();
        List<object> listCompareActullyFloor = new List<object>();
        //List<object> listDevNoComa = new List<object>();
        List<object> listDevNoTmp = new List<object>();

        bool IsDevAs = false;
        string[] _StrDevNoLength;

        private bool f_QuickForm_RadCheckFloor = false;
        private bool f_QuickForm_RadDevNo = false;
        private bool f_QuickForm_RadKeyName = false;
        private string f_QuickForm_edt_KeyNameNo = string.Empty; //楼--单位
        private string f_QuickForm_cbbE_Start = string.Empty;//类别，快速设置 开始位置

        private string IsReadNull = string.Empty; //接收楼层对应表数据是否为空
        private int IsReadSuceess = 0; //是否读取成功    0--- 失败      1---成功

        List<data> LinkCtrlInfo = new List<data>(); //联动器信息

        string testTmp = string.Empty; //测试
        
        //是否具有新基点属性，下载参数时候拼接命令用。
        private bool f_HasNewbenchmark = false;
        public ResponseInfo UserControl_FloorTableChangeNotify
        {
            get;
            set;
        }

        protected override void InitUIOnLoad()
        {
            base.InitUIOnLoad();

            this.tbDownTables.PageVisible = false;

            ControlUtilityTool.SetITLLayOutControlStyle(this.lcParams);
            ControlUtilityTool.SetITLTextEditStyle(this.edtDevIp);
            ControlUtilityTool.SetITLTextEditStyle(this.edtSubnetMark);
            ControlUtilityTool.SetITLTextEditStyle(this.edtGateWay);
            ControlUtilityTool.SetITLTextEditStyle(this.edtDNSServerIp);

            ControlUtilityTool.SetITLTextEditStyle(this.edtEIServerIp);
            ControlUtilityTool.SetTextEditPortRegEx(this.edtEIServerPort);
            ControlUtilityTool.SetTextEditPortRegEx(this.edtLinkagePort);
            ControlUtilityTool.SetITLTextEditStyle(this.edtProjectNo);
            ControlUtilityTool.SetITLXtraTabControlStyle(this.tcDownParams);

            ControlUtilityTool.SetControlDefaultFont(this.lblDownTablesType, 12);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtDevIp);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtSubnetMark);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtGateWay);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtDNSServerIp);

            ControlUtilityTool.SetTextEditIPRegEx(this.edtEIServerIp);
            ControlUtilityTool.SetTextEditIPRegEx(this.txtLinkageIp);

            ControlUtilityTool.SetTextEditIPRegEx(this.edtNewBenchMarkIP);
            ControlUtilityTool.SetTextEditPortRegEx(this.edtNewBenchMarkPort);

            edtProjectNo.Properties.AutoHeight = true;

            this.edtDevIp.Properties.MaxLength = 15;
            this.edtSubnetMark.Properties.MaxLength = 15;
            this.edtGateWay.Properties.MaxLength = 15;
            this.edtDNSServerIp.Properties.MaxLength = 15;
            this.edtEIServerIp.Properties.MaxLength = 15;

            this.edtEIServerPort.Properties.MaxLength = 5;
            this.edtLinkagePort.Properties.MaxLength = 5;
            this.edtProjectNo.Properties.MaxLength = 8;

            this.edtNewBenchMarkPort.Properties.MaxLength = 5;
            
            this.DevType = DeviceType.CloudLinkageInfoCtrl;
            this.edtLinkagePort.Text = "60202"; //默认值
            f_IsRefresh = true;
            this.rgpDHCP.SelectedIndex = 1;

            #region 云联动器
            //云联动器
            this.FrontYqkIp1.KeyPress += CommonUtils.edtIp_KeyPress;
            this.BackYqkIp1.KeyPress += CommonUtils.edtIp_KeyPress;
            ControlUtilityTool.SetTextEditIPRegEx(this.FrontYqkIp1);
            ControlUtilityTool.SetTextEditIPRegEx(this.BackYqkIp1);

            this.FrontYqkIp2.KeyPress += CommonUtils.edtIp_KeyPress;
            this.BackYqkIp2.KeyPress += CommonUtils.edtIp_KeyPress;
            ControlUtilityTool.SetTextEditIPRegEx(this.FrontYqkIp2);
            ControlUtilityTool.SetTextEditIPRegEx(this.BackYqkIp2);

            this.FrontYqkIp3.KeyPress += CommonUtils.edtIp_KeyPress;
            this.BackYqkIp3.KeyPress += CommonUtils.edtIp_KeyPress;
            ControlUtilityTool.SetTextEditIPRegEx(this.FrontYqkIp3);
            ControlUtilityTool.SetTextEditIPRegEx(this.BackYqkIp3);

            this.FrontYqkIp4.KeyPress += CommonUtils.edtIp_KeyPress;
            this.BackYqkIp4.KeyPress += CommonUtils.edtIp_KeyPress;
            ControlUtilityTool.SetTextEditIPRegEx(this.FrontYqkIp4);
            ControlUtilityTool.SetTextEditIPRegEx(this.BackYqkIp4);

            groudControl_Config1_DoorNum3.Visible = true;
            groudControl_Config1_DoorNum4.Visible = true;
            ItemConfigDoor3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            ItemConfigDoor4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            
            this.edt_GateWay.KeyPress += CommonUtils.edtIp_KeyPress;
            ControlUtilityTool.SetTextEditIPRegEx(this.edt_GateWay);

            InstallFloorName1.Properties.MaxLength = 3;
            InstallFloorName2.Properties.MaxLength = 3;
            InstallFloorName3.Properties.MaxLength = 3;
            InstallFloorName4.Properties.MaxLength = 3;


            #endregion

            f_IsRefresh = false;
            FindCount = new Dictionary<string, hintInfo>();
            f_LocalDBOperate = new LocalDBOperate();

            this.ShowNewBenchmarkUI(false);

            HintProvider.WaitingDone(Application.ProductName);
        }

        /// <summary>
        /// UI上面绑定的事件，集中在这里管理
        /// </summary>
        protected override void InitUIEvents()
        {
            base.InitUIEvents();

            this.edtDevIp.Leave += this.edtDevIp_Leave;
            this.rgpDHCP.SelectedIndexChanged += this.rgpDHCP_SelectedIndexChanged;
            this.edtDevIp.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtSubnetMark.KeyPress += CommonUtils.edtIp_KeyPress;

            this.edtGateWay.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtDNSServerIp.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtEIServerIp.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtEIServerPort.KeyPress += CommonUtils.edtPort_KeyPress;
            this.edtLinkagePort.KeyPress += CommonUtils.edtPort_KeyPress;

            this.edtProjectNo.KeyPress += CommonUtils.edtPort_KeyPress;
            this.edtNewBenchMarkPort.KeyPress += CommonUtils.edtPort_KeyPress;
            this.edtNewBenchMarkIP.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtNewBenchMarkIP.Leave += this.edtDevIp_Leave;
            this.edtNewBenchMarkPort.KeyUp += this.edtNewBenchMarkPort_KeyUp;
        }
        /// <summary>
        /// 数据列表增加字段
        /// </summary>
        protected override void InitDevicesDataTable()
        {
            base.InitDevicesDataTable();

            f_DevicesDataTable.Columns.Add(DNS_SERVER_IP, typeof(string));
            f_DevicesDataTable.Columns.Add(DHCP_FUNCTION, typeof(string));
            f_DevicesDataTable.Columns.Add(EI_SERVER_IP, typeof(string));
            f_DevicesDataTable.Columns.Add(EI_SERVER_PORT, typeof(string));
            f_DevicesDataTable.Columns.Add(LINKAGE_PORT, typeof(string));

            f_DevicesDataTable.Columns.Add(PROJECT_NO, typeof(string));
            f_DevicesDataTable.Columns.Add(VERSION, typeof(string));
            f_DevicesDataTable.Columns.Add(LINKAGE_PRAM, typeof(string));
            f_DevicesDataTable.Columns.Add(STATUS, typeof(string));
            f_DevicesDataTable.Columns.Add(NEWBENCHMARK_IP, typeof(string));

            f_DevicesDataTable.Columns.Add(NEWBENCHMARK_PORT, typeof(string));
            f_DevicesDataTable.Columns.Add(HAS_NEWBENCHMARK, typeof(bool));

        }

        /// <summary>
        /// 增加字段对应的字段名称
        /// </summary>
        /// <param name="grdView"></param>
        protected override void InitDevicesGridView(GridView grdView)
        {
            base.InitDevicesGridView(grdView);

            //DNS服务器IP
            this.AddOneGridViewColumn(grdView, DNS_SERVER_IP, DNS_SERVER_IP_ALIAS, 130);

            //DHCP功能
            this.AddOneGridViewColumn(grdView, DHCP_FUNCTION, DHCP_FUNCTION_ALIAS, 100);

            //线下物联平台服务器Ip
            this.AddOneGridViewColumn(grdView, EI_SERVER_IP, EI_SERVER_IP_ALIAS, 140);

            //线下物联平台服务器端口
            this.AddOneGridViewColumn(grdView, EI_SERVER_PORT, EI_SERVER_PORT_ALIAS, 130);
          

            //项目编号
            this.AddOneGridViewColumn(grdView, PROJECT_NO, PROJECT_NO_ALIAS, 100);

            //版本号
            this.AddOneGridViewColumn(grdView, VERSION, VERSION_ALIAS, 150);

            //门参数
            // this.AddOneGridViewColumn(grdView, LINKAGE_PRAM, MJ_LINKAGE_PRAM, 200);

            //设备状态
            this.AddOneGridViewColumn(grdView, STATUS, DEV_STATUS, 100);

            //新基点IP
            this.AddOneGridViewColumn(grdView, NEWBENCHMARK_IP, NEWBENCHMARK_IP_ALIAS, 140);
            //新基点端口
            this.AddOneGridViewColumn(grdView, NEWBENCHMARK_PORT, NEWBENCHMARK_PORT_ALIAS, 100);


            this.gcDevices.DataSource = f_DevicesDataTable;
            ControlUtilityTool.AdjustIndicatorWidth(grdView);
        }

        public static explicit operator CloudLinkCtrlUserControl(Form v)
        {
            throw new NotImplementedException();
        }

        protected override void GeneralDeviceUserControl_SizeChanged(object sender, EventArgs e)
        {
            this.gvDevices.Columns[this.gvDevices.Columns.Count - 1].Width = 120;
            ControlUtilityTool.AdjustIndicatorWidth(this.gvDevices);
        }


        private void edtDevIp_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(edtSubnetMark.EditValue?.ToString()))
            {
                return;
            }
            //A类ip（1.0.0.0—127.255.255.255）默认掩码 255.0.0.0
            //B类ip（128.0.0.0—191.255.255.255）默认掩码 255.255.0.0
            //C类ip（192.0.0.0—223.255.255.255）默认掩码 255.255.255.0
            List<string> ips = this.edtDevIp.EditValue?.ToString()?.Split(".".ToCharArray())?.ToList();
            if (ips != null && ips.Count > 0)
            {
                int ip1 = StrUtils.StrToIntDef(ips.First());
                if (ip1 < 128)
                {
                    this.edtSubnetMark.Text = "255.0.0.0";
                }
                else if (ip1 < 192)
                {
                    this.edtSubnetMark.Text = "255.255.0.0";
                }
                else
                {
                    this.edtSubnetMark.Text = "255.255.255.0";
                }
            }
        }
        /// <summary>
        /// 添加一个设备到界面
        /// </summary>
        /// <param name="__cloudEntranceInfo"></param>
        private void AddOneDeviceToUI(CloudLinkageInfoCtrlInfo deviceInfo)
        {
            DataRow[] rows = f_DevicesDataTable.Select(string.Format("{0}={1}", DEV_ID, deviceInfo.DevId));
            FindCount.AddToUpdate("" + deviceInfo.DevId);
            FindCount.AddToUpdate("" + deviceInfo.DevIp);

     

            //如果行数大于一行，则追加在后面
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                try
                {
                    int FindSumCount = AppEnv.Singleton.UdpCount * AppEnv.Singleton.UdpCount ;

                    if (FindCount["" + deviceInfo.DevId].FontCount > FindSumCount && !FindCount["" + deviceInfo.DevId].IsHint)
                    {
                        FindCount["" + deviceInfo.DevId].IsHint = true;
                        HintProvider.ShowAutoCloseDialog(null, "设备ID冲突,ID:" + deviceInfo.DevId);

                    }
                    if (FindCount[deviceInfo.DevIp].FontCount > FindSumCount && !FindCount["" + deviceInfo.DevIp].IsHint)
                    {
                        FindCount["" + deviceInfo.DevIp].IsHint = true;
                        HintProvider.ShowAutoCloseDialog(null, string.Format("设备IP冲突,IP:{0},ID:{1}", deviceInfo.DevIp, deviceInfo.DevId));
                    }
                    rows[0][DEV_MAC] = deviceInfo.DevMac;
                    rows[0][DEV_IP] = deviceInfo.DevIp;
                    rows[0][SUBNET_MARK] = deviceInfo.SubnetMask;
                    rows[0][GATE_WAY] = deviceInfo.GateWay;
                    rows[0][DNS_SERVER_IP] = deviceInfo.DNSServerIp;

                    rows[0][DHCP_FUNCTION] = deviceInfo.DHCPEnable ? "启用" : "禁用";
                    rows[0][EI_SERVER_IP] = deviceInfo.EIServerIp;
                    rows[0][EI_SERVER_PORT] = deviceInfo.EIServerPort;
                    rows[0][LINKAGE_PORT] = deviceInfo.LinkagePort;
                    rows[0][PROJECT_NO] = deviceInfo.ProjectNo;

                    rows[0][VERSION] = deviceInfo.VerSion;
                    rows[0][LINKAGE_PRAM] = deviceInfo.MJLinkagePram;
                    rows[0][STATUS] = deviceInfo.DevStatus;
                    rows[0][NEWBENCHMARK_IP] = deviceInfo.NewBenchmarkIP;
                    rows[0][NEWBENCHMARK_PORT] = deviceInfo.NewBenchmarkPort;
                    rows[0][HAS_NEWBENCHMARK] = deviceInfo.HasNewBenchmark;
                }
                finally
                {
                    rows[0].EndEdit();
                }
            }
            else
            {
                f_DevicesDataTable.Rows.Add(
                    deviceInfo.DevId, 
                    deviceInfo.DevMac, 
                    deviceInfo.DevIp,
                    deviceInfo.SubnetMask, 
                    deviceInfo.GateWay,
                    deviceInfo.DNSServerIp,
                    deviceInfo.DHCPEnable ? "启用" : "禁用", 
                    deviceInfo.EIServerIp,
                    deviceInfo.EIServerPort, 
                    deviceInfo.LinkagePort, 
                    deviceInfo.ProjectNo, 
                    deviceInfo.VerSion, 
                    deviceInfo.MJLinkagePram,
                    deviceInfo.DevStatus,
                    deviceInfo.NewBenchmarkIP,
                    deviceInfo.NewBenchmarkPort,
                    deviceInfo.HasNewBenchmark);
            }
            //Repeat_Dev_Id.Add(deviceInfo.DevId);

            //排序
            f_DevicesDataTable.DefaultView.Sort = string.Format("{0} {1}", DEV_ID, "ASC");
            ControlUtilityTool.AdjustIndicatorWidth(this.gvDevices);
        }

        protected override void AnalysisRecvDataEx(string strCmdStr)
        {

            RunLog.Log(string.Format("云联动器报文：{0}", strCmdStr));
            //报文格式F2 XX XX ... XX XX F3
            //判断设备类型是否合法
            //云联动器的设备类型为CloudLinkageInfoCtrl  0x16
            DeviceType devType = CommandProcesserHelper.GetDevTypeByCmdInfo(StrUtils.CopySubStr(strCmdStr, 6, 2));
            if (devType != DeviceType.CloudLinkageInfoCtrl)
            {
                return;
            }
            //根据命令字，解析对应命令
            string strCmdWord = StrUtils.CopySubStr(strCmdStr, 14, 4);
            if (strCmdWord == AppConst.CMD_WORD_SEARCH_DEVIDES)
            {
                //判断报文长度
                if (strCmdStr.Length < 86 + 8) //43字节 +项目编号4字节
                {
                    RunLog.Log(string.Format("报文长度错误，报文：{0}", strCmdStr));
                    return;
                }
                this.AnalysisSearchDevicesRecvData(strCmdStr);
            }
            else if (strCmdWord == AppConst.CMD_WORD_SET_CLOUD_ELEVATOR_PARAMS)
            {
                //判断报文长度
                if (strCmdStr.Length < 24) //12字节
                {
                    RunLog.Log(string.Format("报文长度错误，报文：{0}", strCmdStr));
                    return;
                }
                this.AnalysisDownParams(strCmdStr);
            }
            else if (strCmdWord == AppConst.CMD_WORD_SET_DOWN_READ_FLOOR_TABLE)
            {
                //判断报文长度

                this.AnalysisReadFloorInfoData(strCmdStr);
            }
            //this.UpdateHintInfo(strCmdStr);
        }

        /// <summary>
        /// 处理返回的数据，strCmdReport变量用于存储返回报文的数据块
        /// 写入data控件，展现到列表
        /// </summary>
        /// <param name="strCmdStr"></param>
        private void AnalysisSearchDevicesRecvData(string strCmdStr)
        {
            #region 初始化界面  ---- 清空

            this.edtDevIp.Text = string.Empty;
            this.edtSubnetMark.Text = string.Empty;
            this.edtGateWay.Text = string.Empty;
            this.edt_GateWay.Text = string.Empty;
            this.edtDNSServerIp.Text = string.Empty;
            this.edtProjectNo.Text = string.Empty;
            this.edtEIServerIp.Text = string.Empty;
            this.edtEIServerPort.Text = string.Empty;

            InstallFloorName1.Text = string.Empty;
            DelayOutCall1.Text = string.Empty;
            FrontYqkIp1.Text = string.Empty;
            FrontDtNum1.Text = string.Empty;
            BackYqkIp1.Text = string.Empty;
            BackDtNum1.Text = string.Empty;

            InstallFloorName2.Text = string.Empty;
            DelayOutCall2.Text = string.Empty;
            FrontYqkIp2.Text = string.Empty;
            FrontDtNum2.Text = string.Empty;
            BackYqkIp2.Text = string.Empty;
            BackDtNum2.Text = string.Empty;

            InstallFloorName3.Text = string.Empty;
            DelayOutCall3.Text = string.Empty;
            FrontYqkIp3.Text = string.Empty;
            FrontDtNum3.Text = string.Empty;
            BackYqkIp3.Text = string.Empty;
            BackDtNum3.Text = string.Empty;

            InstallFloorName4.Text = string.Empty;
            DelayOutCall4.Text = string.Empty;
            FrontYqkIp4.Text = string.Empty;
            FrontDtNum4.Text = string.Empty;
            BackYqkIp4.Text = string.Empty;
            BackDtNum4.Text = string.Empty;
            edtNewBenchMarkIP.Text = string.Empty;
            edtNewBenchMarkPort.Text = string.Empty;
            #endregion

            //取返回的数据部分
            string strCmdReport = StrUtils.CopySubStr(strCmdStr, 18, strCmdStr.Length - 22);

            CloudLinkageInfoCtrlInfo deviceInfo = new CloudLinkageInfoCtrlInfo();

            //设备类型
            deviceInfo.DevType = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 1, 2), 0, 16);

            //设备ID
            deviceInfo.DevId = CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4));
            //Mac地址
            deviceInfo.DevMac = CommonUtils.GetMacByHex(StrUtils.CopySubStr(strCmdReport, 2, 12));
            
            //设备IP
            deviceInfo.DevIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 16, 8));
            //子网掩码
            deviceInfo.SubnetMask = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 24, 8));
            //网关
            deviceInfo.GateWay = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 32, 8));

            //线下物联平台服务器
            //deviceInfo.EIServerIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 48, 8));
            deviceInfo.EIServerIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 40, 8));

            //线下物联平台端口号
            deviceInfo.EIServerPort = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 48, 4), 0, 16);

            //协议版本号
            deviceInfo.ConnVersion = StrUtils.CopySubStr(strCmdReport, 52, 2);

            //DHCP功能
            deviceInfo.DHCPEnable = StrUtils.CopySubStr(strCmdReport, 54, 2) == "01";

            //DNS服务器
            deviceInfo.DNSServerIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 56, 8));

            //项目编号 以十进制传输
            deviceInfo.ProjectNo = StrUtils.ComplementedStr(
                StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 64, 8), 0, 10).ToString(), 8, "0");

            //设备状态
            deviceInfo.DevStatus = StrUtils.CopySubStr(strCmdReport, 72, 8);

            //程序版本号长度
            string VersionLength = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 80, 2), 0, 16).ToString();
            //RunLog.Log("设备ID为：" + deviceInfo.DevId + "设备IP为：" + deviceInfo.DevIp + "搜索报文版本长度为：" + VersionLength);
            int f_VersionLength = int.Parse(VersionLength) * 2;
            string UtilsVersionReport = StrUtils.CopySubStr(strCmdReport, 82, f_VersionLength);
            string UtilsVersion = string.Empty;
            for (int i = 0; i <= UtilsVersionReport.Length; i++)
            {
                UtilsVersion += StrUtils.CopySubStr(UtilsVersionReport, i * 2, 2);
                UtilsVersion += " ";
            }
            //处理多余空格
            UtilsVersion = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(UtilsVersion, " ");
            deviceInfo.VerSion = CommonUtils.UnHex(UtilsVersion, "utf-8");

            /* 电梯联动参数解析
                 * 旧版本：程序版本号后面紧跟以"7B22"开头的电梯联动参数，直接截取到末尾
                 * 新版本：程序版本后面跟着电梯联动参数Json报文长度，2Byte，其后才是Json报文，Json后是华为新基点属性。
                 *          Json报文长度,2Byte，仅V2020版本及之后有，参数工具等软件需做兼容。
                 *          若不为0x7B，则是V2020版本，若为0x7B，则是V2010及之前版本。
                 * 韦将杰
                 * 2020-04-28
	        */

            //版本号后面报文的起始位置
            int indexAfterVersion = 82 + f_VersionLength;
            string strAfterVersion = StrUtils.CopySubStr(strCmdReport, indexAfterVersion, 4);

            //如果紧跟版本号后面是7B22，则是就版本的云联动器。
            if (strAfterVersion.Equals("7B22"))
            {
                //电梯联动参数的长度等于总长度减前面所有数据的长度
                int lenghtMJLPram = strCmdReport.Length - (82 + f_VersionLength);

                //门禁联动参数
                deviceInfo.MJLinkagePram = StrUtils.CopySubStr(strCmdReport, strCmdReport.Length - lenghtMJLPram, lenghtMJLPram);
                //将十六进制的数据转换成字符串，这里是一个json串。
                deviceInfo.MJLinkagePram = CommonUtils.UnHex(deviceInfo.MJLinkagePram, "utf-8");
            }
            else
            {
                //电梯联动参数起始位
                int indexMJLPram = indexAfterVersion + 4;
                //电梯联动参数的长度等于strAfterVersion的十进制值字节，*2得字符串长度
                int lenghtMJLPram = StrUtils.StrToIntDef(strAfterVersion , 0, 16) * 2;
                //门禁联动参数
                deviceInfo.MJLinkagePram = StrUtils.CopySubStr(strCmdReport, indexMJLPram, lenghtMJLPram);
                //将十六进制的数据转换成字符串，这里是一个json串。
                deviceInfo.MJLinkagePram = CommonUtils.UnHex(deviceInfo.MJLinkagePram, "utf-8");
               
                //华为新基点属性报文起始位置为82+版本号长度
                int indexNewBenchmar = indexMJLPram + lenghtMJLPram;          
                //华为新基点IP，位置为82+版本号长度，ip四字节端口号二字节
                int hasNewBenchmarkLenght = indexNewBenchmar + 8 + 4;
                if (strCmdReport.Length >= hasNewBenchmarkLenght)
                {
                    //华为新基点IP
                    deviceInfo.NewBenchmarkIP = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, indexNewBenchmar, 8));
                    //华为新基点端口          
                    deviceInfo.NewBenchmarkPort = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, indexNewBenchmar + 8, 4), 0, 16).ToString();
                    deviceInfo.HasNewBenchmark = true;
                }
                else
                {
                    deviceInfo.HasNewBenchmark = false;
                }
            }

            testTmp = deviceInfo.MJLinkagePram;

            this.AddOneDeviceToUI(deviceInfo);
        }

        private void AnalysisDownParams(string strCmdStr)
        {
            //F209051608000010100012F3
            //取返回的数据部分
            string strCmdReport = StrUtils.CopySubStr(strCmdStr, 18, strCmdStr.Length - 22);

            //判断设备Id是否正确
            int devId = CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4));
            if (devId != f_CurrentCloudLinkageInfo.DevId)
            {
                return;
            }
            //获取返回的状态
            int intStatus = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 0, 2), -1, 16);
            switch (intStatus)
            {
                case 0x00:
                    {
                        this.IsBusy = false;
                        this.tmrCommunication.Stop();
                        this.tmrCommunication.Interval = 100;
                        if (!this.ExcuteSearchDevices())
                        {
                            HintProvider.ShowAutoCloseDialog(null, "参数设置成功，更新设备信息失败!");
                            return;
                        }
                        HintProvider.StartWaiting(null, "参数设置成功，正在搜索设备", "", Application.ProductName, showDelay: 0, showCloseButtonDelay: int.MaxValue);
                    }
                    return;
                case 0x01:
                    {
                        HintProvider.ShowAutoCloseDialog(null, "参数设置失败");
                    }
                    break;
                case 0x02:
                    {
                        HintProvider.ShowAutoCloseDialog(null, "参数设置失败，需初始化云电梯，才能设置网络参数");
                    }
                    break;
                default:
                    {
                        HintProvider.ShowAutoCloseDialog(null, "参数设置失败，未知错误");
                    }
                    break;
            }

            this.IsBusy = false;
        }



        //下载参数
        protected override void DownParams()
        {
            CloudLinkageInfoCtrlInfo CloudLinkageInfo = GetDeviceInfoBeforeDown();
            if (CloudLinkageInfo == null)
            {
                return;
            }
            switch (this.tcDownParams.SelectedTabPageIndex)
            {
                case 0:
                    {
                        if (!R_Btn_Click)
                        {
                            this.DownBasicParams(CloudLinkageInfo);
                        }
                    }
                    break;
                //case 2:
                //    {
                //        this.DownTables(CloudLinkageInfo);
                //    }
                //    break;
                default:
                    break;
            }


            //if (W_Btn_Click == true)
            //{
            //    this.DownFloorTables(CloudLinkageInfo); //下载楼层对应表
            //    W_Btn_Click = false;
            //}
            //else if (R_Btn_Click == true)
            //{
            //    this.ReadFloorTables(CloudLinkageInfo);//读取楼层对应表
            //    R_Btn_Click = false;
            //}

            //if ((!IsCloudDownFloor) && (W_Btn_Click == false) && (R_Btn_Click == false))
            //{
            //    if ((!string.IsNullOrEmpty(f_StratAuthFlag)) && (!string.IsNullOrEmpty(f_EndAuthFlag)) && (!string.IsNullOrEmpty(f_StartDevNo)))
            //    {
            //        this.QuickSetInLocal(CloudLinkageInfo);
            //        f_StratAuthFlag = string.Empty;
            //        f_EndAuthFlag = string.Empty;
            //        f_StartDevNo = string.Empty;
            //    }

            //}

            //发送报文成功开始计时
            this.BeginTick = Environment.TickCount;
            this.CoolingTick = 600;
            this.IsBusy = true;
            this.tmrCommunication.Start();
        }

        private void DownBasicParams(CloudLinkageInfoCtrlInfo CloudLinkageInfo)
        {
            //获取写指令
            string strWriteCmd = this.GetBasicParamsWriteCmdStr(CloudLinkageInfo);
            RunLog.Log("DownBasicParams.strWriteCmd ----> " + strWriteCmd);
            //判断长度
            //if (strWriteCmd.Length < 49 + 8) //26字节 +4字节
            //{
            //    HintProvider.ShowAutoCloseDialog(null, "生成的报文长度错误，请检查设置的参数是否正确");
            //    return;
            //}
            string writeReport = this.GetWriteReport(CloudLinkageInfo.DevId, AppConst.CMD_WORD_SET_CLOUD_ELEVATOR_PARAMS, strWriteCmd);
            string errMsg = string.Empty;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
            if (!this.UdpListener.SendData(endpoint, writeReport, ref errMsg))
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("设置参数失败，错误：{0}", errMsg));
            }
            f_CurrentCloudLinkageInfo = CloudLinkageInfo;
        }

        /// <summary>
        /// 获取写指令，返回要向设备写入数据的指令 
        /// 韦将杰
        /// 2020.02.25注释
        /// </summary>
        /// <param name="CloudLinkageInfo"></param>
        /// <returns></returns>
        private string GetBasicParamsWriteCmdStr(CloudLinkageInfoCtrlInfo deviceInfo)
        {

            //IP协议类型 固定设置为4
            string strCmdStr = "04";
            //string strCmdStr = string.Empty;
            //设备Ip
            strCmdStr += deviceInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(deviceInfo.DevIp);

            //子网掩码
            strCmdStr += deviceInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(deviceInfo.SubnetMask);

            //网关
            strCmdStr += deviceInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(deviceInfo.GateWay);

            //线下物联平台服务器地址
            strCmdStr += CommonUtils.GetHexByIP(deviceInfo.EIServerIp);

            //线下物联平台服务器端口
            strCmdStr += StrUtils.IntToHex(deviceInfo.EIServerPort, 4);

            //协议版本，云联动器目前固定为00
            strCmdStr += "00";

            //DNS服务器
            strCmdStr += deviceInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(deviceInfo.DNSServerIp);

            //DHCP标志
            strCmdStr += deviceInfo.DHCPEnable ? "01" : "00";
            
            //项目编号           
            strCmdStr += StrUtils.ComplementedStr(deviceInfo.ProjectNo.ToString(), 8, "0");

            //如果具有新基点属性，则拼接数据进入命令
            if (f_HasNewbenchmark)
            {
                strCmdStr += CommonUtils.GetHexByIP(deviceInfo.NewBenchmarkIP);
                strCmdStr += StrUtils.IntToHex(StrUtils.StrToIntDef(deviceInfo.NewBenchmarkPort.Trim(), 0), 4);
            }
            //云联动器增加MAC校验 韦将杰 2020-02-25
            string strMac = KeyMacOperate.GetMacEx(strCmdStr);
            strCmdStr += strMac;

            return strCmdStr;
        }

        private void DownTables(CloudLinkageInfoCtrlInfo CloudLinkageInfo)
        {
            string strCmdWord = string.Empty;
            string strWriteCmd = string.Empty;
            this.GetFloorTableWriteCmdStr(CloudLinkageInfo, ref strWriteCmd, ref strCmdWord);
            string writeReport = this.GetWriteReport(CloudLinkageInfo.DevId, strCmdWord, strWriteCmd);
            string errMsg = string.Empty;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
            if (!this.UdpListener.SendData(endpoint, writeReport, ref errMsg))
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("设置对应表失败，错误：{0}", errMsg));
            }
            f_CurrentCloudLinkageInfo = CloudLinkageInfo;
        }

        private void GetFloorTableWriteCmdStr(CloudLinkageInfoCtrlInfo CloudLinkageInfo, ref string writeCmd, ref string cmdWord)
        {
            DownTableInfo downTableInfo = this.GetTablesBeforeDown(CloudLinkageInfo.DevId);
            if (downTableInfo == null)
            {
                return;
            }
            switch (this.rgpDownTablesType.SelectedIndex)
            {
                case 0:
                    {
                        writeCmd = downTableInfo.FloorTable;
                        cmdWord = AppConst.CMD_WORD_SET_DOWN_FLOOR_TABLE;
                    }
                    break;
                case 1:
                    {
                        writeCmd = downTableInfo.IntercomFloorTable;
                        cmdWord = AppConst.CMD_WORD_SET_DOWN_INTERCOM_FLOOR_TABLE;
                    }
                    break;
                case 2:
                    {
                        writeCmd = downTableInfo.RealFloorTable;
                        cmdWord = AppConst.CMD_WORD_SET_DOWN_REAL_FLOOR_TABLE;
                    }
                    break;
                case 3:
                    {
                        writeCmd = downTableInfo.StatusFloorTable;
                        cmdWord = AppConst.CMD_WORD_SET_DOWN_STATUS_FLOOR_TABLE;
                    }
                    break;
                default:
                    break;
            }
            //获取mac值
            writeCmd += KeyMacOperate.GetMacEx(writeCmd);
        }

        private DownTableInfo GetTablesBeforeDown(int devId)
        {
            DeviceTableInfo deviceTableInfo = new DeviceTableInfo()
            {
                DevId = devId
            };
            //此处写从界面获取对应表信息的逻辑，保存为DeviceTableInfo 

            if (!f_LocalDBOperate.SaveTableToDB(deviceTableInfo))
            {
                HintProvider.ShowAutoCloseDialog(null, "保存对应表到数据库失败！");
                return null;
            }

            //将DeviceTableInfo对应表信息转换为DownTableInfo对应表信息
            DownTableInfo downTableInfo = new DownTableInfo();
            for (int i = 0; i < deviceTableInfo.TableList.Count; i++)
            {
                TableInfo tableInfo = deviceTableInfo.TableList.ElementAt(i).Value;
                downTableInfo.FloorTable += this.GetHexStrByNo(tableInfo.TerminalNo);
                downTableInfo.IntercomFloorTable += this.GetHexStrByNo(tableInfo.IntercomTerminalNo);
                downTableInfo.RealFloorTable += this.GetHexStrByNo(tableInfo.RealFloorNo);
                downTableInfo.StatusFloorTable += this.GetHexStrByNo(tableInfo.StatusFloorNo);
            }

            return downTableInfo;
        }

        private string GetHexStrByNo(int tableNo)
        {
            string strTemp = StrUtils.IntToHex(tableNo, 2);
            strTemp = StrUtils.CopySubStr(strTemp, strTemp.Length - 2, 2);
            return strTemp;
        }

        /// <summary>
        /// 获取光标所在位置的设备信息
        /// </summary>
        /// <returns></returns>
        private CloudLinkageInfoCtrlInfo GetDeviceInfoBeforeDown()
        {
            CloudLinkageInfoCtrlInfo deviceInfo = GetDeviceInfoByFocusRow();
            //如果取得的返回值为空，即所选行的信息为空，则退出
            if (deviceInfo == null)
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("未选中设备或所选行信息为空，请重选！"));
                return null;
            }

            if (!this.CheckUIValid())
            {
                return null;
            }
        
            deviceInfo.DHCPEnable = this.rgpDHCP.SelectedIndex == 0;
            deviceInfo.DevIp = deviceInfo.DHCPEnable ? string.Empty : this.edtDevIp.Text.Trim();
            deviceInfo.SubnetMask = deviceInfo.DHCPEnable ? string.Empty : this.edtSubnetMark.Text.Trim();
            deviceInfo.GateWay = deviceInfo.DHCPEnable ? string.Empty : this.edtGateWay.Text.Trim();
            deviceInfo.DNSServerIp = deviceInfo.DHCPEnable ? string.Empty : this.edtDNSServerIp.Text.Trim();
            deviceInfo.EIServerIp = this.edtEIServerIp.Text.Trim();
            deviceInfo.EIServerPort = StrUtils.StrToIntDef(this.edtEIServerPort.Text.Trim(), 0);
            deviceInfo.LinkagePort = StrUtils.StrToIntDef(this.edtLinkagePort.Text.Trim(), 0);
            deviceInfo.ProjectNo = this.edtProjectNo.Text.Trim();
            
            deviceInfo.GateWay = deviceInfo.DHCPEnable ? string.Empty : this.edt_GateWay.Text.Trim();

            if (f_HasNewbenchmark)
            {
                deviceInfo.NewBenchmarkIP = this.edtNewBenchMarkIP.Text.Trim();
                deviceInfo.NewBenchmarkPort = this.edtNewBenchMarkPort.Text.Trim();
            }

            return deviceInfo;
        }

        private bool CheckUIValid()
        {
            //当DHCP启用时IP、掩码、网关、DNS均不作判断
            if (this.rgpDHCP.SelectedIndex != 0)
            {
                if (string.IsNullOrWhiteSpace(this.edtDevIp.Text.Trim()))
                {
                    HintProvider.ShowAutoCloseDialog(null, "设备IP不能为空");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(this.edtSubnetMark.Text.Trim()))
                {
                    HintProvider.ShowAutoCloseDialog(null, "子网掩码不能为空");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(this.edtGateWay.Text.Trim()))
                {
                    HintProvider.ShowAutoCloseDialog(null, "网关不能为空");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(this.edtDNSServerIp.Text.Trim()))
                {
                    HintProvider.ShowAutoCloseDialog(null, "DNS服务器地址不能为空");
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(this.edtEIServerIp.Text.Trim()))
            {
                HintProvider.ShowAutoCloseDialog(null, "线下物联平台服务器地址不能为空");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.edtEIServerPort.Text.Trim()))
            {
                HintProvider.ShowAutoCloseDialog(null, "线下物联平台服务器端口不能为空");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.edtLinkagePort.Text.Trim()))
            {
                HintProvider.ShowAutoCloseDialog(null, "联动器端口不能为空");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.edtProjectNo.Text.Trim()))
            {
                HintProvider.ShowAutoCloseDialog(null, "项目编号不能为空");
                return false;
            }

            if (!Regex.IsMatch(edtProjectNo.Text.Trim(), "[0-9]"))
            {
                HintProvider.ShowAutoCloseDialog(null, "项目编号只允许输入数字");
                return false;
            }

            if (f_HasNewbenchmark)
            {
                if (string.IsNullOrWhiteSpace(this.edtNewBenchMarkPort.Text.Trim()))
                {
                    HintProvider.ShowAutoCloseDialog(null, "新基点端口号不能为空");
                    return false;
                }
                int NewBenchMarkPort = StrUtils.StrToIntDef(this.edtNewBenchMarkPort.Text.Trim(), 0);
                if (NewBenchMarkPort < 1024)
                {
                    HintProvider.ShowAutoCloseDialog(null, "端口号不能小于1024", HintIconType.Warning, 1000);
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 云联动器列表里面的双击事件
        /// </summary>
        protected override void ExcuteDoubleClick()
        {
            f_IsRefresh = true;
            try
            {
                //加载基础信息到界面
                CloudLinkageInfoCtrlInfo deviceInfo = GetDeviceInfoByFocusRow();
                if (deviceInfo == null)
                {
                    return;
                }

                edtNewBenchMarkPort.ForeColor = Color.FromArgb(32, 31, 53);
                DownDevList.Enabled = true;

                this.ShowNewBenchmarkUI(deviceInfo.HasNewBenchmark);

                this.edtDevIp.Text = deviceInfo.DHCPEnable ? string.Empty : deviceInfo.DevIp;
                this.edtSubnetMark.Text = deviceInfo.DHCPEnable ? string.Empty : deviceInfo.SubnetMask;
                this.edtGateWay.Text = deviceInfo.DHCPEnable ? string.Empty : deviceInfo.GateWay;
                this.edt_GateWay.Text = this.edtGateWay.Text; //网关赋值
                this.edtDNSServerIp.Text = deviceInfo.DHCPEnable ? string.Empty : deviceInfo.DNSServerIp;

                this.edtEIServerIp.Text = deviceInfo.EIServerIp;
                this.edtEIServerPort.Text = deviceInfo.EIServerPort.ToString();
                this.edtLinkagePort.Text = deviceInfo.LinkagePort.ToString();
                this.rgpDHCP.SelectedIndex = deviceInfo.DHCPEnable ? 0 : 1;

                this.edtDevIp.Enabled = !deviceInfo.DHCPEnable;
                this.edtSubnetMark.Enabled = !deviceInfo.DHCPEnable;
                this.edtGateWay.Enabled = !deviceInfo.DHCPEnable;
                this.edt_GateWay.Enabled = this.edtGateWay.Enabled;
                this.edtDNSServerIp.Enabled = !deviceInfo.DHCPEnable;

                this.edtProjectNo.Text = StrUtils.ComplementedStr(deviceInfo.ProjectNo.ToString(), 8, "0");
                if (!edtProjectNo.Text.Trim().Equals("00000000"))
                {
                    edtProjectNo.Enabled = false;
                }
                else
                {
                    edtProjectNo.Enabled = true;
                }
                //this.edtProjectNo.Text = deviceInfo.ProjectNo.ToString();
                if (deviceInfo.DHCPEnable)
                {
                    this.edtDevIp.Tag = deviceInfo.DevIp;
                    this.edtSubnetMark.Tag = deviceInfo.SubnetMask;
                    this.edtGateWay.Tag = deviceInfo.GateWay;
                    this.edtDNSServerIp.Tag = deviceInfo.DNSServerIp;
                }

                //加载对应表信息到界面
                DeviceTableInfo deviceTableInfo = new DeviceTableInfo()
                {
                    DevId = deviceInfo.DevId
                };
                if (!f_LocalDBOperate.GetTableFromDB(deviceTableInfo))
                {
                    HintProvider.ShowAutoCloseDialog(null, "获取设备对应表信息失败！");
                    return;
                }
              
                int iCount = 0;
                if (deviceInfo.MJLinkagePram.Length >= 477)
                {
                    iCount = 4;
                }
                else
                {
                    iCount = 2;
                }
                LinkCtrlInfo.Clear();
                for (int i = 0; i < iCount; i++)
                {
                    string JsonData = cutJson(deviceInfo.MJLinkagePram, "data", i);
                    data LinkData = JsonConvert.DeserializeObject<data>(JsonData);
                    LinkCtrlInfo.Add(LinkData);
                }

                //如果具有新基点的属性，则赋值给UI，私有变量f_HasNewbenchmark为true。
                if (deviceInfo.HasNewBenchmark)
                {
                    this.edtNewBenchMarkIP.Text = deviceInfo.NewBenchmarkIP;
                    this.edtNewBenchMarkPort.Text = deviceInfo.NewBenchmarkPort;
                    f_HasNewbenchmark = true;
                }
                else
                {
                    f_HasNewbenchmark = false;
                }

                //将信息加载到界面
                if (LinkCtrlInfo.Count == 2)
                {

                    #region 1号门配置
                    InstallFloorName1.Text = LinkCtrlInfo[0].InstallFloorName.Trim();
                    DelayOutCall1.Text = LinkCtrlInfo[0].DelayOutCall.ToString();
                    FrontYqkIp1.Text = LinkCtrlInfo[0].FrontYqkIp;
                    FrontDtNum1.Text = LinkCtrlInfo[0].FrontDtNum.ToString();
                    BackYqkIp1.Text = LinkCtrlInfo[0].BackYqkIp;
                    BackDtNum1.Text = LinkCtrlInfo[0].BackDtNum.ToString();
                    groudControl_Config1_DoorNum1.Text = LinkCtrlInfo[0].DoorNum.ToString();

                    FrontLinkSta1.Text = "●成功";
                    FrontLinkSta1.ForeColor = Color.Green;
                    BackLinkSta1.Text = "●成功";
                    BackLinkSta1.ForeColor = Color.Green;


                    if (LinkCtrlInfo[0].FrontLinkSta == "-1")
                    {
                        FrontLinkSta1.Text = "●失败";
                        FrontLinkSta1.ForeColor = Color.Red;
                    }

                    if (LinkCtrlInfo[0].BackLinkSta == "-1")
                    {
                        BackLinkSta1.Text = "●失败";
                        BackLinkSta1.ForeColor = Color.Red;
                    }
        

                    #endregion

                    #region 2号门配置
                    InstallFloorName2.Text = LinkCtrlInfo[1].InstallFloorName.Trim();
                    DelayOutCall2.Text = LinkCtrlInfo[1].DelayOutCall.ToString();
                    FrontYqkIp2.Text = LinkCtrlInfo[1].FrontYqkIp;
                    FrontDtNum2.Text = LinkCtrlInfo[1].FrontDtNum.ToString();
                    BackYqkIp2.Text = LinkCtrlInfo[1].BackYqkIp;
                    BackDtNum2.Text = LinkCtrlInfo[1].BackDtNum.ToString();
                    groudControl_Config1_DoorNum2.Text = LinkCtrlInfo[1].DoorNum.ToString();

                    FrontLinkSta2.Text = "●成功";
                    FrontLinkSta2.ForeColor = Color.Green;
                    BackLinkSta2.Text = "●成功";
                    BackLinkSta2.ForeColor = Color.Green;


                    if (LinkCtrlInfo[1].FrontLinkSta == "-1")
                    {
                        FrontLinkSta2.Text = "●失败";
                        FrontLinkSta2.ForeColor = Color.Red;
                    }

                    if (LinkCtrlInfo[1].BackLinkSta == "-1")
                    {
                        BackLinkSta2.Text = "●失败";
                        BackLinkSta2.ForeColor = Color.Red;
                    }
                    #endregion

                    groudControl_Config1_DoorNum3.Visible = false;
                    ItemConfigDoor3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    groudControl_Config1_DoorNum4.Visible = false;
                    ItemConfigDoor4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
                else  if (LinkCtrlInfo.Count == 4)
                {

                    #region 1号门配置
                    InstallFloorName1.Text = LinkCtrlInfo[0].InstallFloorName.Trim();
                    DelayOutCall1.Text = LinkCtrlInfo[0].DelayOutCall.ToString();
                    FrontYqkIp1.Text = LinkCtrlInfo[0].FrontYqkIp;
                    FrontDtNum1.Text = LinkCtrlInfo[0].FrontDtNum.ToString();
                    BackYqkIp1.Text = LinkCtrlInfo[0].BackYqkIp;
                    BackDtNum1.Text = LinkCtrlInfo[0].BackDtNum.ToString();
                    groudControl_Config1_DoorNum1.Text = LinkCtrlInfo[0].DoorNum.ToString();

                    FrontLinkSta1.Text = "●成功";
                    FrontLinkSta1.ForeColor = Color.Green;
                    BackLinkSta1.Text = "●成功";
                    BackLinkSta1.ForeColor = Color.Green;


                    if (LinkCtrlInfo[0].FrontLinkSta == "-1")
                    {
                        FrontLinkSta1.Text = "●失败";
                        FrontLinkSta1.ForeColor = Color.Red;
                    }

                    if (LinkCtrlInfo[0].BackLinkSta == "-1")
                    {
                        BackLinkSta1.Text = "●失败";
                        BackLinkSta1.ForeColor = Color.Red;
                    }
                    #endregion

                    #region 2号门配置
                    InstallFloorName2.Text = LinkCtrlInfo[1].InstallFloorName.Trim();
                    DelayOutCall2.Text = LinkCtrlInfo[1].DelayOutCall.ToString();
                    FrontYqkIp2.Text = LinkCtrlInfo[1].FrontYqkIp;
                    FrontDtNum2.Text = LinkCtrlInfo[1].FrontDtNum.ToString();
                    BackYqkIp2.Text = LinkCtrlInfo[1].BackYqkIp;
                    BackDtNum2.Text = LinkCtrlInfo[1].BackDtNum.ToString();
                    groudControl_Config1_DoorNum2.Text = LinkCtrlInfo[1].DoorNum.ToString();

                    FrontLinkSta2.Text = "●成功";
                    FrontLinkSta2.ForeColor = Color.Green;
                    BackLinkSta2.Text = "●成功";
                    BackLinkSta2.ForeColor = Color.Green;


                    if (LinkCtrlInfo[1].FrontLinkSta == "-1")
                    {
                        FrontLinkSta2.Text = "●失败";
                        FrontLinkSta2.ForeColor = Color.Red;
                    }

                    if (LinkCtrlInfo[1].BackLinkSta == "-1")
                    {
                        BackLinkSta2.Text = "●失败";
                        BackLinkSta2.ForeColor = Color.Red;
                    }
                    #endregion

                    #region 3号门配置
                    InstallFloorName3.Text = LinkCtrlInfo[2].InstallFloorName.Trim();
                    DelayOutCall3.Text = LinkCtrlInfo[2].DelayOutCall.ToString();
                    FrontYqkIp3.Text = LinkCtrlInfo[2].FrontYqkIp;
                    FrontDtNum3.Text = LinkCtrlInfo[2].FrontDtNum.ToString();
                    BackYqkIp3.Text = LinkCtrlInfo[2].BackYqkIp;
                    BackDtNum3.Text = LinkCtrlInfo[2].BackDtNum.ToString();
                    groudControl_Config1_DoorNum3.Text = LinkCtrlInfo[2].DoorNum.ToString();

                    FrontLinkSta3.Text = "●成功";
                    FrontLinkSta3.ForeColor = Color.Green;
                    BackLinkSta3.Text = "●成功";
                    BackLinkSta3.ForeColor = Color.Green;


                    if (LinkCtrlInfo[2].FrontLinkSta == "-1")
                    {
                        FrontLinkSta3.Text = "●失败";
                        FrontLinkSta3.ForeColor = Color.Red;
                    }

                    if (LinkCtrlInfo[2].BackLinkSta == "-1")
                    {
                        BackLinkSta3.Text = "●失败";
                        BackLinkSta3.ForeColor = Color.Red;
                    }
                    #endregion

                    #region 4号门配置
                    InstallFloorName4.Text = LinkCtrlInfo[3].InstallFloorName.Trim();
                    DelayOutCall4.Text = LinkCtrlInfo[3].DelayOutCall.ToString();
                    FrontYqkIp4.Text = LinkCtrlInfo[3].FrontYqkIp;
                    FrontDtNum4.Text = LinkCtrlInfo[3].FrontDtNum.ToString();
                    BackYqkIp4.Text = LinkCtrlInfo[3].BackYqkIp;
                    BackDtNum4.Text = LinkCtrlInfo[3].BackDtNum.ToString();
                    groudControl_Config1_DoorNum4.Text = LinkCtrlInfo[3].DoorNum.ToString();

                    FrontLinkSta4.Text = "●成功";
                    FrontLinkSta4.ForeColor = Color.Green;
                    BackLinkSta4.Text = "●成功";
                    BackLinkSta4.ForeColor = Color.Green;


                    if (LinkCtrlInfo[3].FrontLinkSta == "-1")
                    {
                        FrontLinkSta4.Text = "●失败";
                        FrontLinkSta4.ForeColor = Color.Red;
                    }

                    if (LinkCtrlInfo[3].BackLinkSta == "-1")
                    {
                        BackLinkSta4.Text = "●失败";
                        BackLinkSta4.ForeColor = Color.Red;
                    }
                    #endregion

                    groudControl_Config1_DoorNum3.Visible = true;
                    groudControl_Config1_DoorNum4.Visible = true;
                    ItemConfigDoor3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    ItemConfigDoor4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }

            }
            finally
            {
                f_IsRefresh = false;
                //读取选中的设备楼层对应表
                R_Btn_Click = true;
                //DownParams();
                R_Btn_Click = false;
            }
        }

        /// <summary>
        /// Json处理
        /// </summary>
        /// <param name="json"></param>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string cutJson(string json, string item, int index)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            string value = jo[item][index].ToString();
            return value;
        }

        private CloudLinkageInfoCtrlInfo GetDeviceInfoByFocusRow()
        {
            //如果没有焦点则退出。
            if (this.gvDevices.FocusedRowHandle < 0)
            {
                return null;
            }
            CloudLinkageInfoCtrlInfo deviceInfo = new CloudLinkageInfoCtrlInfo()
            {
                //GetRowCellValue取得当某行某个单元格的值，第一个参数为行，第二个参数为字段
                DevId = StrUtils.StrToIntDef(this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, DEV_ID).ToString(), 0),
                DevMac = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, DEV_MAC).ToString(),
                DevIp = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, DEV_IP).ToString(),
                SubnetMask = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, SUBNET_MARK).ToString(),
                GateWay = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, GATE_WAY).ToString(),

                DHCPEnable = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, DHCP_FUNCTION).ToString() == "启用",
                DNSServerIp = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, DNS_SERVER_IP).ToString(),
                EIServerIp = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, EI_SERVER_IP).ToString(),
                EIServerPort = StrUtils.StrToIntDef(this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, EI_SERVER_PORT).ToString(), 0),
                LinkagePort = StrUtils.StrToIntDef(this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, LINKAGE_PORT).ToString(), 0),

                ProjectNo = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, PROJECT_NO).ToString(),
                VerSion = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, VERSION).ToString(),
                MJLinkagePram = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, LINKAGE_PRAM).ToString(),
                DevStatus = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, STATUS).ToString(),
                HasNewBenchmark = (bool)this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, HAS_NEWBENCHMARK),
            };
            this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, HAS_NEWBENCHMARK);
            if (deviceInfo.HasNewBenchmark)
            {
                deviceInfo.NewBenchmarkIP = gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, NEWBENCHMARK_IP).ToString();
                deviceInfo.NewBenchmarkPort = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, NEWBENCHMARK_PORT).ToString();
            }
            return deviceInfo;
        }


        private void rgpDHCP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (f_IsRefresh)
            {
                return;
            }
            if (this.rgpDHCP.SelectedIndex == 0)
            {
                this.edtDevIp.Tag = this.edtDevIp.Text.Trim();
                this.edtSubnetMark.Tag = this.edtSubnetMark.Text.Trim();
                this.edtGateWay.Tag = this.edtGateWay.Text.Trim();
                this.edtDNSServerIp.Tag = this.edtDNSServerIp.Text.Trim();
                this.edtProjectNo.Tag = this.edtProjectNo.Text.Trim();
                this.edt_GateWay.Tag = this.edt_GateWay.Text.Trim();
                

                this.edtDevIp.Text = string.Empty;
                this.edtSubnetMark.Text = string.Empty;
                this.edtGateWay.Text = string.Empty;
                this.edtDNSServerIp.Text = string.Empty;
                this.edt_GateWay.Text = string.Empty;
                //网关
                
                this.edtDevIp.Enabled = false;
                this.edtSubnetMark.Enabled = false;
                this.edtGateWay.Enabled = false;
                this.edtDNSServerIp.Enabled = false;
                this.edt_GateWay.Enabled = false;

            }
            else
            {
                if (this.edtDevIp.Tag is string)
                {
                    this.edtDevIp.Text = this.edtDevIp.Tag.ToString();
                }
                if (this.edtSubnetMark.Tag is string)
                {
                    this.edtSubnetMark.Text = this.edtSubnetMark.Tag.ToString();
                }
                if (this.edtGateWay.Tag is string)
                {
                    this.edtGateWay.Text = this.edtGateWay.Tag.ToString();
                    this.edt_GateWay.Text = this.edtGateWay.Text;
                    //this.edtGateWay.Text = this.edt_GateWay.Text;

                }
                if (this.edtDNSServerIp.Tag is string)
                {
                    this.edtDNSServerIp.Text = this.edtDNSServerIp.Tag.ToString();
                }

     
                if (this.edtProjectNo.Tag is string)
                {
                    this.edtProjectNo.Text = StrUtils.ComplementedStr(this.edtProjectNo.Tag.ToString(), 8, "0");
                }

                this.edtDevIp.Enabled = true;
                this.edtSubnetMark.Enabled = true;
                this.edtGateWay.Enabled = true;
                this.edtDNSServerIp.Enabled = true;
                this.edt_GateWay.Enabled = true;
            }
        }

        private void edtProjectNo_Leave(object sender, EventArgs e)
        {
            edtProjectNo.Text = StrUtils.ComplementedStr(edtProjectNo.Text.Trim(), 8, "0");
        }

        private void edtProjectNo_KeyUp(object sender, KeyEventArgs e)
        {

            //int no = StrUtils.StrToIntDef(edtProjectNo.Text.Trim(), 0);
            //edtProjectNo.Text = StrUtils.ComplementedStr(no.ToString(), 8, "0");
        }

        //切换TabControl
        private void tcDownParams_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {

            //string TabName = "楼层对应表";
            //string TabBaseInfo = "基本参数";
            foreach (XtraTabPage page in tcDownParams.TabPages)
            {

                //if (Is_LoaclDownTable == false)
                //{
                //    DialogResult dr = MessageBox.Show("未进行参数下载,切换会清空已下载的表", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                //    if (dr == DialogResult.OK)
                //    {
                //        InitGrid();//加载楼层对应表
                //        Is_LoaclDownTable = true;
                //        tcDownParams.SelectedTabPageIndex = 1;
                //    }
                //    else
                //    {
                //        tcDownParams.SelectedTabPageIndex = 0;
                //    }
                //}
                if (tcDownParams.SelectedTabPageIndex == 0)
                {
                    //加载按钮 ---- 基本参数
                    Btn_DownLoadParm.Visible = true;
                    Btn_ReadLocalFloorTable.Visible = false;
                    Btn_ReadCloudFloorTable.Visible = false;
                    Btn_DownLoadDev.Visible = false;
                    Btn_SaveSet.Visible = false;
                    Btn_QuickSet.Visible = false;

                    //if(Is_LoaclDownTable == false)
                    //{
                    //    DialogResult dr = MessageBox.Show("未进行参数下载,切换会清空已下载的表", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //    if (dr == DialogResult.OK)
                    //    {
                    //        InitGrid();//加载楼层对应表
                    //        Is_LoaclDownTable = true;
                    //    }
                    //}


                }

                else if (tcDownParams.SelectedTabPageIndex == 1)
                {
                    // 加载按钮---- 楼层对应表
                    Btn_DownLoadParm.Visible = false;
                    Btn_ReadLocalFloorTable.Visible = true;
                    Btn_ReadCloudFloorTable.Visible = true;
                    Btn_DownLoadDev.Visible = true;
                    //Btn_SaveSet.Visible = true;
                    Btn_QuickSet.Visible = true;

                    //Is_LoaclDownTable = false;
                    InitGrid();//加载楼层对应表
                }
            }

        }
        //初始化表格
        public void InitGrid()
        {
            // bandedGridView1，注意这里声明的是：BandedGridView
            BandedGridView view = bandedGridView1 as BandedGridView;

            view.BeginUpdate(); //开始视图的编辑，防止触发其他事件
            view.BeginDataUpdate(); //开始数据的编辑

            //if(Is_LoaclDownTable)
            //{
            //    view.Bands.Clear();
            //}
            view.Bands.Clear();

            //修改附加选项
            view.OptionsView.ShowColumnHeaders = false;                         //因为有Band列了，所以把ColumnHeader隐藏
            view.OptionsView.ShowGroupPanel = false;                            //如果没必要分组，就把它去掉
            view.OptionsView.EnableAppearanceEvenRow = false;                   //是否启用偶数行外观
            view.OptionsView.EnableAppearanceOddRow = true;                     //是否启用奇数行外观
            view.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;   //是否显示过滤面板
            view.OptionsCustomization.AllowColumnMoving = false;                //是否允许移动列
            view.OptionsCustomization.AllowColumnResizing = false;              //是否允许调整列宽
            view.OptionsCustomization.AllowGroup = false;                       //是否允许分组
            view.OptionsCustomization.AllowFilter = false;                      //是否允许过滤
            view.OptionsCustomization.AllowSort = true;                         //是否允许排序
            view.OptionsSelection.EnableAppearanceFocusedCell = true;           //光标显示被选单元格
            view.OptionsBehavior.Editable = true;                               //是否允许用户编辑单元格

            view.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click; //单击整行选择
            //view.OptionsView.EnableAppearanceEvenRow = true;//是否启用偶数行外观
            //view.OptionsView.EnableAppearanceOddRow = true;//是否启用奇数行外观

            //添加列标题
            GridBand bandID = view.Bands.AddBand("ID");
            bandID.Visible = false; //隐藏ID列
            GridBand bandcolAuthFlag = view.Bands.AddBand("权限标识\n(映射楼层)");
            GridBand bandcolFloor = view.Bands.AddBand("楼层");
            GridBand bandcolFloor_keyName = bandcolFloor.Children.AddBand("按键名称");
            GridBand bandcolFloor_ActualFloor = bandcolFloor.Children.AddBand("实际楼层");
            GridBand bandcolDevNo = view.Bands.AddBand("端子号");
            GridBand bandcolDevCheckFloor = view.Bands.AddBand("检测楼层");
            //GridBand bandRemark = view.Bands.AddBand("备注");

            //列大小对齐
            bandcolAuthFlag.MinWidth = 180;
            bandcolFloor.MinWidth = 400;
            bandcolDevNo.MinWidth = 150;
            bandcolDevCheckFloor.MinWidth = 150;

            //列标题对齐方式
            bandcolAuthFlag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            bandcolFloor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            bandcolFloor_keyName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            bandcolFloor_ActualFloor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            bandcolDevNo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            bandcolDevCheckFloor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            ////bandRemark.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            //获取云端数据
            List<IlistColumnInterface> listDataSource = new List<IlistColumnInterface>();
            //AutoIndex、权限标识、按键名称、实际楼层、端子号、检测楼层
            //listDataSource.Add(new IlistColumnInterface(1, "11","大堂","1","-1", "1")); //测试
            listDataSource.Add(new IlistColumnInterface(1, "", "", "", "", ""));

            //绑定数据源并显示
            gridControl_FloorTable.DataSource = listDataSource;
            gridControl_FloorTable.MainView.PopulateColumns();

            //[权限标识]列绑定ComboBox
            //RepositoryItemTextEdit AuthFlag_riCombo = new RepositoryItemTextEdit();
            //AuthFlag_riCombo.Items.AddRange(new string[] { "-1", "0" });
            //gridControl_FloorTable.RepositoryItems.Add(AuthFlag_riCombo);
            //view.Columns["AuthFlag"].ColumnEdit = AuthFlag_riCombo;

            //[按键名称]列绑定ComboBox
            //RepositoryItemComboBox KeyName_riCombo = new RepositoryItemComboBox();
            //KeyName_riCombo.Items.AddRange(new string[] { "-1", "0" });
            //gridControl_FloorTable.RepositoryItems.Add(KeyName_riCombo);
            //view.Columns["KeyName"].ColumnEdit = KeyName_riCombo;

            //[实际楼层]列绑定ComboBox
            RepositoryItemComboBox ActuallFloor_riCombo = new RepositoryItemComboBox();
            ActuallFloor_riCombo.Items.AddRange(new string[] { "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8",
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
            "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
            "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
            "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
            "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
            "51", "52", "53", "54", "55", "56", "57", "58", "59", "60",
            "61", "62", "63", "64", "65", "66", "67", "68", "69", "70",
            "71", "72", "73", "74", "75", "76", "77", "8", "79", "80",
            "81", "82", "83", "84", "85", "86", "87", "88", "89", "90",
            "91", "92", "93", "94", "95", "96", "97", "98", "99", "100",
            "101", "102","103","104","105","106","107","108","109","110","111","112"});
            gridControl_FloorTable.RepositoryItems.Add(ActuallFloor_riCombo);
            view.Columns["ActualFloor"].ColumnEdit = ActuallFloor_riCombo;
            //view.Columns["ActualFloor"].SummaryItem.FieldName = "ActualFloor";
            ActuallFloor_riCombo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor; //设置只读

            //[端子号]列绑定ComboBox
            RepositoryItemComboBox riCombo_colDevNo = new RepositoryItemComboBox();

            //String StrcolDevNo = string.Empty;
            //for (int i=1; i < 113;i++)
            //{
            //    string StrLeft = @"""";
            //    StrLeft = StrLeft.Remove(1, StrLeft.Length-1);
            //    string StrRight = "\"" + ",";
            //    //StrRight = StrRight.Remove(1, StrRight.Length);
            //    StrcolDevNo += StrLeft + i.ToString() + StrRight;
            //}
            //riCombo_colDevNo.Items.AddRange(new string[] { StrcolDevNo });


            riCombo_colDevNo.Items.AddRange(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
            "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
            "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
            "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
            "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
            "51", "52", "53", "54", "55", "56", "57", "58", "59", "60",
            "61", "62", "63", "64", "65", "66", "67", "68", "69", "70",
            "71", "72", "73", "74", "75", "76", "77", "8", "79", "80",
            "81", "82", "83", "84", "85", "86", "87", "88", "89", "90",
            "91", "92", "93", "94", "95", "96", "97", "98", "99", "100",
            "101", "102","103","104","105","106","107","108","109","110","111","112"});
            gridControl_FloorTable.RepositoryItems.Add(riCombo_colDevNo);
            view.Columns["DevNo"].ColumnEdit = riCombo_colDevNo;
            riCombo_colDevNo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor; //设置只读

            //[检测楼层]列绑定ComboBox
            RepositoryItemComboBox riCombo_colDevCheckFloor = new RepositoryItemComboBox();
            riCombo_colDevCheckFloor.Items.AddRange(new string[] { "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8",
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
            "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
            "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
            "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
            "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
            "51", "52", "53", "54", "55", "56", "57", "58", "59", "60",
            "61", "62", "63", "64", "65", "66", "67", "68", "69", "70",
            "71", "72", "73", "74", "75", "76", "77", "8", "79", "80",
            "81", "82", "83", "84", "85", "86", "87", "88", "89", "90",
            "91", "92", "93", "94", "95", "96", "97", "98", "99", "100",
            "101", "102","103","104","105","106","107","108","109","110","111","112" });
            gridControl_FloorTable.RepositoryItems.Add(riCombo_colDevCheckFloor);
            view.Columns["DevCheckFloor"].ColumnEdit = riCombo_colDevCheckFloor;
            riCombo_colDevCheckFloor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor; //设置只读

            ////[楼层]列绑定SpinEdit
            //RepositoryItemSpinEdit riSpin = new RepositoryItemSpinEdit();
            //gridControl_FloorTable.RepositoryItems.Add(riSpin);
            //view.Columns["keyName"].ColumnEdit = riSpin;
            ////view.Columns["Floor_ActualFloor"].ColumnEdit = riSpin;


            //将标题列和数据列对应
            view.Columns["ID"].OwnerBand = bandID;
            view.Columns["AuthFlag"].OwnerBand = bandcolAuthFlag;
            view.Columns["KeyName"].OwnerBand = bandcolFloor_keyName;
            view.Columns["ActualFloor"].OwnerBand = bandcolFloor_ActualFloor;
            view.Columns["DevNo"].OwnerBand = bandcolDevNo;
            view.Columns["DevCheckFloor"].OwnerBand = bandcolDevCheckFloor;

            view.Columns["ActualFloor"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
            view.Columns["AuthFlag"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
            view.Columns["KeyName"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
            view.Columns["DevNo"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
            view.Columns["DevCheckFloor"].OptionsColumn.AllowEdit = false;//设置列不可以编辑

            view.EndDataUpdate();//结束数据的编辑
            view.EndUpdate();   //结束视图的编辑
        }

        //事件:云端楼层对应表Form
        private void Btn_ReadCloudFloorTable_Click(object sender, EventArgs e)
        {
            ReadCloudFloorTableForm Form_ReadCloudFloorTable = new ReadCloudFloorTableForm();
            Form_ReadCloudFloorTable.StartPosition = FormStartPosition.CenterScreen;

            Form_ReadCloudFloorTable.SendCloudFloorHex = UserControl_FloorTable;

            if (Form_ReadCloudFloorTable.ShowDialog(this) == DialogResult.OK)
            {

                UserControl_FloorTable = Form_ReadCloudFloorTable.SendCloudFloorHex;
                GetCloudFloorTable(UserControl_FloorTable.data);

                //RunLog.Log("****** testStr" + UserControl_FloorTable.data);



                IsCloudDownFloor = true; //标识为云端读取的楼层对应表

                //读取本地成功，下载与保存不可用
                Btn_SaveSet.Enabled = false;
                //Btn_DownLoadDev.Enabled = false;

                return;
            }

        }


        //事件：下载楼层对应表 
        private void Btn_DownLoadDev_Click(object sender, EventArgs e)
        {
            //快速设置
            f_QuickForm_RadCheckFloor = false;
            f_QuickForm_RadDevNo = false;
            f_QuickForm_RadKeyName = false;

            W_Btn_Click = true;
            DownParams();
            W_Btn_Click = false;
        }

        //事件：读取本地楼层对应表
        private void Btn_ReadLocalFloorTable_Click(object sender, EventArgs e)
        {
            R_Btn_Click = true;
            DownParams();
            R_Btn_Click = false;

            //ToDo 正在加载数据...

            //WaitForm tbBox = new WaitForm();
            //tbBox.StartPosition = FormStartPosition.CenterScreen;

            //if (tbBox.ShowDialog(this) == DialogResult.OK)
            //{

            //}
        }


        //事件：保存设置
        private void Btn_SaveSet_Click(object sender, EventArgs e)
        {
            W_Btn_Click = true;
            DownParams();
            W_Btn_Click = false;
            IsAllProcess = true;
        }

        //事件：快速设置
        private void Btn_QuickSet_Click(object sender, EventArgs e)
        {
            CloudFloorTableQuickSetForm Form_CloudFloorTableQuickSet = new CloudFloorTableQuickSetForm();
            Form_CloudFloorTableQuickSet.StartPosition = FormStartPosition.CenterScreen;
            //Form_CloudFloorTableQuickSet.Show();

            Form_CloudFloorTableQuickSet.f_QuickSetInfo_StratAuthFlag = f_StratAuthFlag;
            Form_CloudFloorTableQuickSet.f_QuickSetInfo_EndAuthFlag = f_EndAuthFlag;
            Form_CloudFloorTableQuickSet.f_QuickSetInfo_StartDevNo = f_StartDevNo;

            if (Form_CloudFloorTableQuickSet.ShowDialog(this) == DialogResult.OK)
            {
                f_StratAuthFlag = Form_CloudFloorTableQuickSet.f_QuickSetInfo_StratAuthFlag;
                f_EndAuthFlag = Form_CloudFloorTableQuickSet.f_QuickSetInfo_EndAuthFlag;
                f_StartDevNo = Form_CloudFloorTableQuickSet.f_QuickSetInfo_StartDevNo;
                //获取选中事件
                f_QuickForm_RadDevNo = Form_CloudFloorTableQuickSet.f_QuickSetInfo_FloorTerminalNo;
                f_QuickForm_RadCheckFloor = Form_CloudFloorTableQuickSet.f_QuickSetInfo_CheckFloor;
                f_QuickForm_RadKeyName = Form_CloudFloorTableQuickSet.f_QuickSetInfo_FloorName;
                f_QuickForm_edt_KeyNameNo = Form_CloudFloorTableQuickSet.f_edt_KeyNameNo;
                f_QuickForm_cbbE_Start = Form_CloudFloorTableQuickSet.f_QuickSetInfo_StartDevNo;

                if ((!string.IsNullOrEmpty(f_StratAuthFlag)) && (!string.IsNullOrEmpty(f_EndAuthFlag)) && (!string.IsNullOrEmpty(f_StartDevNo)))
                {
                    //QuickSetInLocal();
                    //IsBathProcess = true;
                    //DownParams();
                    //取消保存设置
                    //R_Btn_Click = true;
                    DownParams();
                    //R_Btn_Click = false;
                    IsAllProcess = true;
                    IsCloudDownFloor = false;

                }
            }
        }

        /// <summary>
        /// 快速设置
        /// </summary>
        private void QuickSetInLocal(CloudLinkageInfoCtrlInfo CloudLinkageInfo)
        {
            //如果为云端下载的数据不执行快速设置
            if (IsCloudDownFloor)
            {
                return;
            }

            try
            {
                string strCmdWord = string.Empty;
                string strWriteCmd = string.Empty;
                this.FloorTableWriteCmdStr(CloudLinkageInfo, ref strWriteCmd, ref strCmdWord);

            }
            catch (Exception ex)
            {
                RunLog.Log(ex);
            }
        }

        /// <summary>
        /// 下载楼层对应表
        /// </summary>
        /// <param name="CloudLinkageInfo"></param>
        private void DownFloorTables(CloudLinkageInfoCtrlInfo CloudLinkageInfo)
        {
            try
            {
                string strCmdWord = string.Empty;
                string strWriteCmd = string.Empty;
                this.FloorTableWriteCmdStr(CloudLinkageInfo, ref strWriteCmd, ref strCmdWord);
                //string WriteReport = this.GetWriteReport(CloudLinkageInfo.DevId, "5A30", "");//报文
                strWriteCmd = strWriteCmd.ToUpper();
                string WriteReport = this.GetWriteReport(CloudLinkageInfo.DevId, "5A30", strWriteCmd);//报文

                if (string.IsNullOrEmpty(strWriteCmd))
                {
                    return;
                }
                ///快速设置不写入
                if (f_QuickForm_RadCheckFloor || f_QuickForm_RadDevNo || f_QuickForm_RadKeyName)
                {
                    f_QuickForm_RadCheckFloor = false;
                    f_QuickForm_RadDevNo = false;
                    f_QuickForm_RadKeyName = false;
                    return;
                }

                //获取mac值
                //ReadReport += KeyMacOperate.GetMacEx(ReadReport);

                IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
                f_CurrentCloudLinkageInfo = CloudLinkageInfo;
                string errMsg = string.Empty;
                if (!this.UdpListener.SendData(endpoint, WriteReport, ref errMsg))
                {
                    HintProvider.ShowAutoCloseDialog(null, string.Format("下载对应表失败，错误：{0}", errMsg));
                }
                else
                {
                    if (IsDevAs)
                    {
                        IsDevAs = false;
                        return;
                    }
                    string IsVersion = StrUtils.CopySubStr(CloudLinkageInfo.VerSion, 9, 4);
                    //低于1060版本 下载不能超过1KB的字节。

                }
            }
            catch (Exception ex)
            {
                RunLog.Log("下载失败" + ex);
                ///快速设置不写入
                if (f_QuickForm_RadCheckFloor || f_QuickForm_RadDevNo || f_QuickForm_RadKeyName)
                {
                    f_QuickForm_RadCheckFloor = false;
                    f_QuickForm_RadDevNo = false;
                    f_QuickForm_RadKeyName = false;
                    return;
                }
                else
                {
                    //HintProvider.ShowAutoCloseDialog(null, string.Format("下载失败"));
                    W_Btn_Click = false;

                }

            }

        }


        /// <summary>
        /// 组装下载 写入楼层对应表
        /// </summary>
        /// <param name="CloudLinkageInfo"></param>
        /// <param name="WriteCmd"></param>
        /// <param name="cmdWord"></param>
        private void FloorTableWriteCmdStr(CloudLinkageInfoCtrlInfo CloudLinkageInfo, ref string WriteCmd, ref string cmdWord)
        {

            DownTableInfo downTableInfo = this.GetTablesBeforeDownInfo(CloudLinkageInfo.DevId);
            string strAuthFlagReport = string.Empty;
            string strKeyNameReport = string.Empty;
            string strActualFloorReport = string.Empty;
            string strDevNoReport = string.Empty;
            string strDevCheckFloorReport = string.Empty;

            if (downTableInfo == null)
            {
                return;
            }
            string StrCompareDevNoA = string.Empty;
            string StrCompareDevNoB = string.Empty;
            string StrKeyName = string.Empty;
            string StrAuthFlag = string.Empty;
            string StrActualFloor = string.Empty;
            string StrDevCheckFloor = string.Empty;
            string[] ArrKeyName = new string[112];

            int[] pRows = SelectView.GetSelectedRows();

            string StratAuthFlag = string.Empty;
            string EndAuthFlag = string.Empty;

            int EndFlag = 2;
            //if(_StrDevNoLength[_StrDevNoLength.Length - 1] == "")
            //{
            RunLog.Log("1050 测试用：1 取值长度:" + _StrDevNoLength.Length);

            _StrDevNoLength = _StrDevNoLength.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            //}
            string IsVersion = StrUtils.CopySubStr(CloudLinkageInfo.VerSion, 9, 4);
            if (IsVersion == "1050")
            {
                //_StrDevNoLength = 112;
            }

            for (int c_CompareDevNo = 0; c_CompareDevNo < _StrDevNoLength.Length; c_CompareDevNo++)
            {
                //StrCompareDevNo += SelectView.GetRowCellValue(pRows[c_CompareDevNo], "DevNo").ToString();
                StrCompareDevNoA += SelectView.GetRowCellValue(c_CompareDevNo, "DevNo").ToString();
                StrCompareDevNoA += ",";
                StrCompareDevNoB = c_DevNo.Replace(",", " ");

                StrKeyName += SelectView.GetRowCellValue(c_CompareDevNo, "KeyName").ToString();
                ArrKeyName[c_CompareDevNo] = SelectView.GetRowCellValue(c_CompareDevNo, "KeyName").ToString();
                //ArrKeyName[c_CompareDevNo] = ArrKeyName[c_CompareDevNo].Trim().ToString();20190325

                //StrKeyName += ArrKeyName[c_CompareDevNo];
                StrKeyName += ",";

                StrAuthFlag += SelectView.GetRowCellValue(c_CompareDevNo, "AuthFlag").ToString();
                StrAuthFlag += ",";

                StrActualFloor += SelectView.GetRowCellValue(c_CompareDevNo, "ActualFloor").ToString();
                StrActualFloor += ",";

                StrDevCheckFloor += SelectView.GetRowCellValue(c_CompareDevNo, "DevCheckFloor").ToString();
                StrDevCheckFloor += ",";

                StratAuthFlag = _StrDevNoLength[0];
                EndAuthFlag = _StrDevNoLength[_StrDevNoLength.Length - 1];

            }


            RunLog.Log("1050 测试用：2 取值长度:" + _StrDevNoLength.Length);

            RunLog.Log("1050 测试用 --- 1 下载:  权限标识---" + StrAuthFlag + " \n按键名称--" + StrKeyName + " \n实际楼层---" + StrActualFloor + " \n端子号" + StrCompareDevNoA + " \n检测楼层" + StrDevCheckFloor);

            #region 快速设置
            //快速设置
            string[] Range_DevNo = new string[0];
            string[] Range_DevNoCompare = new string[0];
            string[] QuickSet_KeyName = new string[0];
            List<object> listQuickSet_KeyName = new List<object>();
            List<object> listQuickSet_CheckFloor = new List<object>();
            List<object> listQuickSet_DevNo = new List<object>();

            for (int a = 0; a <= 112; a++)
            {
                listQuickSet_KeyName.Add(a);
                listQuickSet_CheckFloor.Add(a);
                listQuickSet_DevNo.Add(a);
            }

            //////////////////////////////////////////////////////
            if (!string.IsNullOrEmpty(f_StratAuthFlag))
            {
                int Dev_start = int.Parse(f_StratAuthFlag);
                int Dev_End = int.Parse(f_EndAuthFlag);
                int QuickSetStartNo = int.Parse(f_QuickForm_cbbE_Start); //快速设置开始位置
                int TmpSet = QuickSetStartNo;

                listDevNoA.Clear(); //使用前清空

                for (; Dev_start <= Dev_End; Dev_start++)
                {
                    //if(QuickSetStartNo == 1)
                    //{
                    string TmpQSetStartKeyName = TmpSet.ToString() + f_QuickForm_edt_KeyNameNo.ToString();
                    string TmpQSetStartNo = TmpSet.ToString();

                    listQuickSet_KeyName.Insert(Dev_start - 1, TmpQSetStartKeyName);//获取UI按键名称
                    listQuickSet_CheckFloor.Insert(Dev_start - 1, TmpQSetStartNo);//获取UI检测楼层
                    listQuickSet_DevNo.Insert(Dev_start - 1, TmpQSetStartNo);//获取UI端子号

                    if (f_QuickForm_RadDevNo)
                    {
                        SelectView.SetRowCellValue(Dev_start - 1, "DevNo", listQuickSet_DevNo[Dev_start - 1]);
                    }
                    if (f_QuickForm_RadCheckFloor)
                    {
                        SelectView.SetRowCellValue(Dev_start - 1, "DevCheckFloor", listQuickSet_CheckFloor[Dev_start - 1]);
                    }
                    if (f_QuickForm_RadKeyName)
                    {
                        SelectView.SetRowCellValue(Dev_start - 1, "KeyName", listQuickSet_KeyName[Dev_start - 1]);
                    }
                    TmpSet++;


                }
                if (Dev_End == 112)
                {
                    SelectView.SetRowCellValue(Dev_start, "KeyName", listQuickSet_KeyName[Dev_start - 1]);
                    SelectView.SetRowCellValue(Dev_start, "DevCheckFloor", listQuickSet_CheckFloor[Dev_start - 1]);
                    SelectView.SetRowCellValue(Dev_start, "DevNo", listQuickSet_DevNo[Dev_start - 1]);
                }
                f_QuickForm_RadDevNo = false;
                f_QuickForm_RadCheckFloor = false;
                f_QuickForm_RadKeyName = false;
            }

            #endregion

            try
            {
                ///相同端子判断
                //int Devi_start = int.Parse(StratAuthFlag);_StrDevNoLength
                int Devi_End = 0;
                if (EndAuthFlag == "-")
                {
                    Devi_End = _StrDevNoLength.Length;
                }
                else
                {
                    Devi_End = int.Parse(EndAuthFlag);
                }

                listDevNoA.Clear(); //使用前清空
                listDevNoCompare.Clear();
                string[] Range_ActualFloor = new string[0];
                string[] Range_DevCheckFloor = new string[0];

                listDeActullyFloor.Clear();
                listDevCheckFloor.Clear();
                listCompareActullyFloor.Clear();
                listCompareCheckFloor.Clear();

                //for (int Devi_start = 0; Devi_start <= Devi_End - 1; Devi_start++)
                for (int Devi_start = 0; Devi_start <= Devi_End - 1; Devi_start++)
                {
                    Range_DevNo = StrCompareDevNoA.Trim().Split(','); //读取的端子号
                    listDevNoA.Add(Range_DevNo[Devi_start]); //下载的端子号
                    listDevNoCompare.Add(Range_DevNo[Devi_start]);

                    Range_ActualFloor = StrActualFloor.Trim().Split(','); //读取实际楼层
                    listDeActullyFloor.Add(Range_ActualFloor[Devi_start]);
                    listCompareActullyFloor.Add(Range_ActualFloor[Devi_start]);

                    Range_DevCheckFloor = StrDevCheckFloor.Trim().Split(','); //读取检测楼层
                    listDevCheckFloor.Add(Range_DevCheckFloor[Devi_start]);
                    listCompareCheckFloor.Add(Range_DevCheckFloor[Devi_start]);

                    //if (Range_DevNo[Devi_start] != "-")
                    //{
                    //    listDevNoA = listDevNoA.Distinct().ToList();
                    //}
                    //if (Range_ActualFloor[Devi_start] != "-")
                    //{
                    //    listDeActullyFloor = listDeActullyFloor.Distinct().ToList();
                    //}
                }
            }
            catch (Exception error)
            {
                RunLog.Log(error);
            }

            int Devi_EndB = 0;
            if (EndAuthFlag == "-")
            {
                Devi_EndB = _StrDevNoLength.Length;
            }
            else
            {
                Devi_EndB = int.Parse(EndAuthFlag);
            }
            string strIsContinue = string.Empty;//是否有 -
            string Value = string.Empty;
            //int Devi_EndB = int.Parse(EndAuthFlag);

            List<object> DevListB = new List<object>();
            List<object> DevTmp = new List<object>();

            List<object> CheckListB = new List<object>();
            List<object> CheckTmp = new List<object>();

            List<object> ActullyListB = new List<object>();
            List<object> ActullyTmp = new List<object>();

            //if (listDevNoCompare.Count != listDevNoA.Count)
            //{
            listDevNoTmp.Clear();
            foreach (string strTmp in listDevNoCompare)
            {
                if (listDevNoTmp.Contains(strTmp))
                {
                    if (strTmp != "-")
                    {
                        HintProvider.ShowAutoCloseDialog(null, string.Format("端子号相同请重新设置"));
                        return;
                    }
                    //else
                    //{
                    //    DevListB.Add(strTmp);
                    //}
                }
                else
                {
                    listDevNoTmp.Add(strTmp);

                }
            }
            //}


            ActullyTmp.Clear();
            //if (listCompareActullyFloor.Count != listDeActullyFloor.Count)
            //{
            foreach (string strTmp in listDeActullyFloor)
            {
                if (ActullyTmp.Contains(strTmp))
                {
                    if (strTmp != "-")
                    {
                        HintProvider.ShowAutoCloseDialog(null, string.Format("实际楼层相同请重新设置"));
                        return;
                    }
                    //else
                    //{
                    //    DevListB.Add(strTmp);
                    //}
                }
                else
                {
                    ActullyTmp.Add(strTmp);

                }
            }
            //}
            //if (listCompareCheckFloor.Count != listDevCheckFloor.Count)
            //{
            CheckTmp.Clear();

            foreach (string strTmp in listDevCheckFloor)
            {
                if (CheckTmp.Contains(strTmp))
                {
                    if (strTmp != "-")
                    {
                        HintProvider.ShowAutoCloseDialog(null, string.Format("检测楼层相同请重新设置"));
                        return;
                    }
                    //else
                    //{
                    //    DevListB.Add(strTmp);
                    //}
                }
                else
                {
                    CheckTmp.Add(strTmp);

                }
            }
            //}


            ////// 下载到设备
            strAuthFlagReport = CommonUtils.ToHex(StrAuthFlag, "utf-8", true);
            strKeyNameReport = CommonUtils.ToHex(StrKeyName, "utf-8", true);
            strActualFloorReport = CommonUtils.ToHex(StrActualFloor, "utf-8", true);
            strDevNoReport = CommonUtils.ToHex(StrCompareDevNoA, "utf-8", true);
            strDevCheckFloorReport = CommonUtils.ToHex(StrDevCheckFloor, "utf-8", true);
            IsAllProcess = false;


            RunLog.Log("******** StrCompareDevNo :" + StrCompareDevNoA);

            //批处理
            if (IsBathProcess)
            {
                int i_StartAuthFlag = 0;
                int i_EndAuthFlag = 0;
                if (string.IsNullOrEmpty(f_StratAuthFlag))
                {
                    i_StartAuthFlag = int.Parse(f_StratAuthFlag);
                }
                if (string.IsNullOrEmpty(f_EndAuthFlag))
                {
                    i_EndAuthFlag = int.Parse(f_EndAuthFlag);
                }
                int f_StartDevNo = c_DevNo.Length;
                for (; i_StartAuthFlag < i_EndAuthFlag; i_StartAuthFlag++)
                {
                    c_AuthFlag += i_StartAuthFlag.ToString();
                }

                strAuthFlagReport = CommonUtils.ToHex(c_AuthFlag, "utf-8", true);
                strKeyNameReport = CommonUtils.ToHex(c_KeyName, "utf-8", true);
                strActualFloorReport = CommonUtils.ToHex(c_ActualFloor, "utf-8", true);
                strDevNoReport = CommonUtils.ToHex(c_DevNo, "utf-8", true);
                strDevCheckFloorReport = CommonUtils.ToHex(c_DevCheckFloor, "utf-8", true);

                IsBathProcess = false;
            }

            //处理转换后出现的空格字符
            strAuthFlagReport = strAuthFlagReport.Substring(0, strAuthFlagReport.Length - 2);
            strKeyNameReport = strKeyNameReport.Substring(0, strKeyNameReport.Length - 2);
            strActualFloorReport = strActualFloorReport.Substring(0, strActualFloorReport.Length - 2);
            strDevCheckFloorReport = strDevCheckFloorReport.Substring(0, strDevCheckFloorReport.Length - 2);
            strDevNoReport = strDevNoReport.Substring(0, strDevNoReport.Length - 2);

            WriteCmd += "7B 22 69 64 22 3A 22" + strAuthFlagReport + "22 2C";
            WriteCmd += "22 66 6C 6F 6F 72 4E 61 6D 65 22 3A 22" + strKeyNameReport + "22 2C";
            WriteCmd += "22 61 63 74 75 61 6C 46 6C 6F 6F 72 4E 75 6D 22 3A 22" + strActualFloorReport + "22 2C";
            WriteCmd += "22 64 65 74 65 63 74 65 64 46 6C 6F 6F 72 4E 75 6D 22 3A 22" + strDevCheckFloorReport + "22 2C";
            //WriteCmd += "22 74 65 72 6D 69 6E 61 6C 4E 75 6D 22 3A 22" + strDevNoReport + "22 7D";

            WriteCmd += "22 74 65 72 6D 69 6E 61 6C 4E 75 6D 22 3A 22" + strDevNoReport + "22 2C";
            WriteCmd += "22 74 65 72 6D 69 6E 61 6C 4E 75 6D 49 6E 74 65 72 63 6F 6D 22 3A 22" + strDevNoReport + "22 7D";


            RunLog.Log("****************WriteCmd对讲端子同步  :" + "22 74 65 72 6D 69 6E 61 6C 4E 75 6D 49 6E 74 65 72 63 6F 6D 22 3A 22" + strDevNoReport + "22 7D");
            RunLog.Log("****************WriteCmd对讲端子同步全部  :" + WriteCmd);

            RunLog.Log("1050 测试用 ---下载:  权限标识---" + strAuthFlagReport + " \n按键名称--" + strKeyNameReport + " \n实际楼层---" + strActualFloorReport + " \n端子号" + strDevNoReport + " \n检测楼层" + strDevCheckFloorReport);

            cmdWord = AppConst.CMD_WORD_SET_DOWN_DOWNLOAD_FLOOR_TABLE;
            WriteCmd = WriteCmd.Replace(",", " ");

            WriteCmd = WriteCmd.ToUpper();
            //RunLog.Log("****************WriteCmd前  :" + WriteCmd);
            WriteCmd = WriteCmd.Replace(" ", "");
            //RunLog.Log("****************WriteCmd后  :" + WriteCmd);


            //获取mac值
            WriteCmd += KeyMacOperate.GetMacEx(WriteCmd);

            //RunLog.Log("****************WriteCmd获取mac值后   :" + WriteCmd);


        }

        /// <summary>
        /// RepeatInfo用来描述重复项
        /// </summary>
        class RepeatInfo
        {
            // 值
            public int Value { get; set; }
            // 重复次数
            public int RepeatNum { get; set; }
        }

        /// <summary>
        /// 读取楼层对应表
        /// </summary>
        /// <param name="CloudLinkageInfo"></param>
        private void ReadFloorTables(CloudLinkageInfoCtrlInfo CloudLinkageInfo)
        {
            try
            {
                string strCmdWord = string.Empty;
                string strReadCmd = string.Empty;
                this.GetFloorTableReadCmdStr(CloudLinkageInfo, ref strReadCmd, ref strCmdWord);
                //string ReadReport = this.GetWriteReport(CloudLinkageInfo.DevId, "5A31", strReadCmd);//报文
                string ReadReport = this.GetWriteReport(CloudLinkageInfo.DevId, "5A31", "");//报文

                //获取mac值
                //ReadReport += KeyMacOperate.GetMacEx(ReadReport);

                IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
                f_CurrentCloudLinkageInfo = CloudLinkageInfo;
                string errMsg = string.Empty;
                if (!this.UdpListener.SendData(endpoint, ReadReport, ref errMsg))
                {
                    HintProvider.ShowAutoCloseDialog(null, string.Format("读取本地对应表失败，错误：{0}", errMsg));
                }
                else
                {
                    //正在加载
                    //WaitForm tbBox = new WaitForm();
                    //tbBox.StartPosition = FormStartPosition.CenterScreen;

                    //if (tbBox.ShowDialog(this) == DialogResult.OK)
                    //{

                    //}

                    //IsCloudDownFloor = false;
                    HintProvider.StartWaiting(null, "正在读取", "", "A1", showDelay: 0, showCloseButtonDelay: 5000);



                    Task t = new Task(() =>
                    {
                        System.DateTime sTime = System.DateTime.Now;
                        System.DateTime eTime = sTime.AddSeconds(5);
                        while (true)
                        {
                            if (sTime > eTime || IsReadSuceess == 1)
                            {


                                break;

                            }
                            sTime = System.DateTime.Now;


                        }

                        HintProvider.WaitingDone("A1");

                        if (IsReadSuceess == 0)
                        {
                            HintProvider.ShowAutoCloseDialog(null, string.Format("读取超时"), HintIconType.Err);
                        }
                        else
                        {
                            IsReadSuceess = 0;
                        }


                        RunLog.Log(this.UdpListener.SendData(endpoint, ReadReport, ref errMsg));

                    });
                    t.Start();
                    //t.ContinueWith((task) =>
                    //{
                    //    HintProvider.WaitingDone(this);
                    //});










                    //读取本地成功，下载与保存可用
                    Btn_SaveSet.Enabled = true;
                    Btn_DownLoadDev.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("读取失败"), HintIconType.Err);
                RunLog.Log(ex);
            }

        }

        /// <summary>
        /// 组装下载读取控制器信息
        /// </summary>
        /// <param name="CloudLinkageInfo"></param>
        /// <param name="readCmd"></param>
        /// <param name="cmdWord"></param>
        private void GetFloorTableReadCmdStr(CloudLinkageInfoCtrlInfo CloudLinkageInfo, ref string readCmd, ref string cmdWord)
        {

            DownTableInfo downTableInfo = this.GetTablesBeforeDownInfo(CloudLinkageInfo.DevId);
            if (downTableInfo == null)
            {
                return;
            }
            readCmd = downTableInfo.FloorTable;
            cmdWord = AppConst.CMD_WORD_SET_DOWN_READ_FLOOR_TABLE;

            //获取mac值
            readCmd += KeyMacOperate.GetMacEx(readCmd);
        }

        private DownTableInfo GetTablesBeforeDownInfo(int devId)
        {
            DeviceTableInfo deviceTableInfo = new DeviceTableInfo()
            {
                DevId = devId
            };
            //将DeviceTableInfo对应表信息转换为DownTableInfo对应表信息
            DownTableInfo downTableInfo = new DownTableInfo();
            for (int i = 0; i < deviceTableInfo.TableList.Count; i++)
            {
                TableInfo tableInfo = deviceTableInfo.TableList.ElementAt(i).Value;
                downTableInfo.FloorTable += this.GetHexStrByNo(tableInfo.TerminalNo);
                downTableInfo.IntercomFloorTable += this.GetHexStrByNo(tableInfo.IntercomTerminalNo);
                downTableInfo.RealFloorTable += this.GetHexStrByNo(tableInfo.RealFloorNo);
                downTableInfo.StatusFloorTable += this.GetHexStrByNo(tableInfo.StatusFloorNo);
            }


            return downTableInfo;
        }

        /// <summary>
        /// 读取本地楼层对应表信息
        /// </summary>
        /// <param name="strCmdStr"></param>
        private void AnalysisReadFloorInfoData(string strCmdStr)
        {
            try
            {
                //取返回的数据部分
                string strCmdReport = StrUtils.CopySubStr(strCmdStr, 18, strCmdStr.Length - 22);
                FloorRelationInfo Rcv_FloorRelationInfo = new FloorRelationInfo();

                string strResultReport = string.Empty;
                //string result = Regex.Replace(strCmdReport, @"(\d{2}(?!$))", "$1 ");
                string IsReadSucess = string.Empty;

                for (int i = 0; i <= strCmdReport.Length; i++)
                {
                    strResultReport += StrUtils.CopySubStr(strCmdReport, i * 2, 2);
                    strResultReport += " ";
                }
                //处理多余空格
                strResultReport = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(strResultReport, " ");

                //编码转换
                string Rcv_FloorTableInfo = CommonUtils.UnHex(strResultReport, "utf-8");

                FloorRelationInfo FloorTableSerializer = JsonConvert.DeserializeObject<FloorRelationInfo>(Rcv_FloorTableInfo);

                BandedGridView view = bandedGridView1 as BandedGridView;
                RepositoryItemComboBox ActuallFloor_riCombo = new RepositoryItemComboBox();//实际楼层
                RepositoryItemComboBox riCombo_colDevNo = new RepositoryItemComboBox(); //端子号
                RepositoryItemComboBox riCombo_colDevCheckFloor = new RepositoryItemComboBox();//检测楼层
                RepositoryItemTextEdit SpinEdit_AuthFlag = new RepositoryItemTextEdit();//权限标识
                RepositoryItemTextEdit TextEdit_KeyName = new RepositoryItemTextEdit();//按键名称

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ActualFloor"));
                dt.Columns.Add(new DataColumn("DevNo"));
                dt.Columns.Add(new DataColumn("DevCheckFloor"));
                dt.Columns.Add(new DataColumn("AuthFlag"));
                dt.Columns.Add(new DataColumn("KeyName"));

                TextEdit_KeyName.MaxLength = 10;
                //TextEdit_KeyName.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;//使用正则表达式
                TextEdit_KeyName.Mask.EditMask = (@"[a-zA-Z0-9\u4e00-\u9fa5][a-zA-Z\u4e00-\u9fa5]");//正则表达式的结构

                List<FloorRelationBaseInfo> listDataSource = new List<FloorRelationBaseInfo>();
                //按键名称
                c_KeyName = FloorTableSerializer.floorName.ToString();
                string[] Is_KeyName = c_KeyName.Trim().Split(',');
                string[] buff_KeyName = new string[Is_KeyName.Length];
                //实际楼层
                c_ActualFloor = FloorTableSerializer.actualFloorNum.ToString();
                string[] Is_ActualFloor = c_ActualFloor.Trim().Split(',');
                //byte[] buff_ActualFloor = new byte[Is_ActualFloor.Length];
                string[] buff_ActualFloor = new string[Is_ActualFloor.Length];

                //端子号
                c_DevNo = FloorTableSerializer.terminalNum.ToString();
                string[] Is_DevNo = c_DevNo.Trim().Split(',');
                //byte[] buff_DevNo = new byte[Is_DevNo.Length];
                string[] buff_DevNo = new string[Is_DevNo.Length];

                _StrDevNoLength = c_DevNo.Trim().Split(','); //获取本地读取数据长度
                RunLog.Log("比较********** _StrDevNo Array ---- " + _StrDevNoLength.Length);
                _StrDevNoLength = _StrDevNoLength.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                //检测楼层
                c_DevCheckFloor = FloorTableSerializer.detectedFloorNum.ToString();
                string[] Is_DevCheckFloor = c_DevCheckFloor.Trim().Split(',');
                string[] buff_DevCheckFloor = new string[Is_DevCheckFloor.Length];
                ///权限标识
                c_AuthFlag = FloorTableSerializer.id.ToString();
                string[] Is_AuthFlag = c_AuthFlag.Trim().Split(',');
                string[] buff_AuthFlag = new string[Is_AuthFlag.Length];
                //byte[] buff_AuthFlag = new byte[Is_AuthFlag.Length];

                listDevNoB.Clear(); //清空比对的数据
                //for (int i = 0; i < buff_ActualFloor.Length; i++)
                for (int i = 0; i < _StrDevNoLength.Length; i++)
                {
                    //buff_ActualFloor[i] = Convert.ToByte(Is_ActualFloor[i], 16);
                    buff_ActualFloor[i] = Is_ActualFloor[i];
                    //buff_DevNo[i] = Convert.ToByte(Is_DevNo[i], 16);
                    buff_DevNo[i] = Is_DevNo[i];
                    buff_DevCheckFloor[i] = Is_DevCheckFloor[i];
                    buff_AuthFlag[i] = Is_AuthFlag[i];
                    //buff_AuthFlag[i] = Convert.ToByte(Is_AuthFlag[i], 16);

                    buff_KeyName[i] = Is_KeyName[i];
                    listDevNoB.Add(buff_DevNo[i]);//读取时的listB
                    IsReadSucess = buff_ActualFloor[i].ToString();
                    IsReadNull = buff_ActualFloor[i].ToString();


                    DataRow row = dt.NewRow();
                    row["ActualFloor"] = buff_ActualFloor[i].ToString();
                    row["DevNo"] = buff_DevNo[i].ToString();
                    row["DevCheckFloor"] = buff_DevCheckFloor[i];
                    row["AuthFlag"] = buff_AuthFlag[i];
                    row["KeyName"] = buff_KeyName[i];
                    dt.Rows.Add(row);
                    this.gridControl_FloorTable.DataSource = dt;

                }

                //SpinEdit_AuthFlag.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor; //设置只读
                SpinEdit_AuthFlag.ReadOnly = true;  //设置只读

                IsReadSucess = buff_ActualFloor[0].ToString();
                IsReadNull = buff_ActualFloor[0].ToString();

                ///*****下拉框
                ///端子号
                riCombo_colDevNo.Items.AddRange(new string[] { "-","1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
                "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
                "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
                "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
                "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
                "51", "52", "53", "54", "55", "56", "57", "58", "59", "60",
                "61", "62", "63", "64", "65", "66", "67", "68", "69", "70",
                "71", "72", "73", "74", "75", "76", "77", "8", "79", "80",
                "81", "82", "83", "84", "85", "86", "87", "88", "89", "90",
                "91", "92", "93", "94", "95", "96", "97", "98", "99", "100",
                "101", "102","103","104","105","106","107","108","109","110","111","112"});
                gridControl_FloorTable.RepositoryItems.Add(riCombo_colDevNo);
                view.Columns["DevNo"].ColumnEdit = riCombo_colDevNo;
                riCombo_colDevNo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor; //设置只读

                //检测楼层
                riCombo_colDevCheckFloor.Items.AddRange(new string[] { "-", "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8",
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
                "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
                "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
                "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
                "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
                "51", "52", "53", "54", "55", "56", "57", "58", "59", "60",
                "61", "62", "63", "64", "65", "66", "67", "68", "69", "70",
                "71", "72", "73", "74", "75", "76", "77", "8", "79", "80",
                "81", "82", "83", "84", "85", "86", "87", "88", "89", "90",
                "91", "92", "93", "94", "95", "96", "97", "98", "99", "100",
                "101", "102","103","104","105","106","107","108","109","110","111","112" });
                gridControl_FloorTable.RepositoryItems.Add(riCombo_colDevCheckFloor);
                view.Columns["DevCheckFloor"].ColumnEdit = riCombo_colDevCheckFloor;
                riCombo_colDevCheckFloor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor; //设置只读

                //实际楼层
                ActuallFloor_riCombo.Items.AddRange(new string[] { "-", "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8",
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
                "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
                "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
                "31", "32", "33", "34", "35", "36", "37", "38", "39", "40",
                "41", "42", "43", "44", "45", "46", "47", "48", "49", "50",
                "51", "52", "53", "54", "55", "56", "57", "58", "59", "60",
                "61", "62", "63", "64", "65", "66", "67", "68", "69", "70",
                "71", "72", "73", "74", "75", "76", "77", "78", "79", "80",
                "81", "82", "83", "84", "85", "86", "87", "88", "89", "90",
                "91", "92", "93", "94", "95", "96", "97", "98", "99", "100",
                "101", "102","103","104","105","106","107","108","109","110","111","112"});
                gridControl_FloorTable.RepositoryItems.Add(ActuallFloor_riCombo);
                view.Columns["ActualFloor"].ColumnEdit = ActuallFloor_riCombo;
                ActuallFloor_riCombo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor; //设置只读

                view.Columns["ActualFloor"].ColumnEdit = ActuallFloor_riCombo;
                view.Columns["AuthFlag"].ColumnEdit = SpinEdit_AuthFlag;
                view.Columns["KeyName"].ColumnEdit = TextEdit_KeyName;
                view.Columns["DevNo"].ColumnEdit = riCombo_colDevNo;
                view.Columns["DevCheckFloor"].ColumnEdit = riCombo_colDevCheckFloor;

                SelectView = view;
                //return;
                if (!string.IsNullOrEmpty(IsReadSucess))
                {
                    IsCloudDownFloor = false;
                    IsReadSuceess = 1; //读取成功

                    HintProvider.ShowAutoCloseDialog(null, string.Format("读取成功"));
                    //HintProvider.ShowAutoCloseDialog(null, string.Format("读取成功"), HintIconType.OK, 3000);

                    //Todo 禁用按键 
                }
                else
                {
                    IsReadSuceess = 2;
                    HintProvider.ShowAutoCloseDialog(null, string.Format("读取失败"), HintIconType.Err);
                }

                view.Columns["ActualFloor"].OptionsColumn.AllowEdit = true;//设置列不可以编辑
                view.Columns["AuthFlag"].OptionsColumn.AllowEdit = true;//设置列不可以编辑
                view.Columns["KeyName"].OptionsColumn.AllowEdit = true;//设置列不可以编辑
                view.Columns["DevNo"].OptionsColumn.AllowEdit = true;//设置列不可以编辑
                view.Columns["DevCheckFloor"].OptionsColumn.AllowEdit = true;//设置列不可以编辑

                if (IsCloudDownFloor)
                {
                    view.Columns["ActualFloor"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                    view.Columns["AuthFlag"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                    view.Columns["KeyName"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                    view.Columns["DevNo"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                    view.Columns["DevCheckFloor"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                    IsCloudDownFloor = false;

                }


            }
            catch (Exception ex)
            {
                RunLog.Log(ex);
                HintProvider.ShowAutoCloseDialog(null, string.Format("读取失败"), HintIconType.Err);
                InitGrid();
            }

        }

        /// <summary>
        /// 获取选中行的值
        /// </summary>
        BandedGridView SelectView = null;
        private void gridControl_FloorTable_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (SelectView.GetSelectedRows() == null)
                {
                    return;
                }
                int[] pRows = SelectView.GetSelectedRows();
                if (pRows.GetLength(0) > 0)
                {
                    Select_AuthFlag = SelectView.GetRowCellValue(pRows[0], "AuthFlag").ToString();
                    Select_KeyName = SelectView.GetRowCellValue(pRows[0], "KeyName").ToString();
                    Select_ActualFloor = SelectView.GetRowCellValue(pRows[0], "ActualFloor").ToString();
                    Select_DevNo = SelectView.GetRowCellValue(pRows[0], "DevNo").ToString();
                    Select_DevCheckFloor = SelectView.GetRowCellValue(pRows[0], "DevCheckFloor").ToString();
                }

            }
            catch (Exception ex)
            {
                RunLog.Log(string.Format("选中行报文错误，报文：{0}", ex));
            }

        }

        public void GetCloudFloorTable(FloorTableInfo data)
        {
            try
            {

                RunLog.Log("************** data :" + data);

                BandedGridView view = bandedGridView1 as BandedGridView;

                RepositoryItemComboBox ActuallFloor_riCombo = new RepositoryItemComboBox();//实际楼层
                RepositoryItemComboBox riCombo_colDevNo = new RepositoryItemComboBox(); //端子号
                RepositoryItemComboBox riCombo_colDevCheckFloor = new RepositoryItemComboBox();//检测楼层
                RepositoryItemTextEdit SpinEdit_AuthFlag = new RepositoryItemTextEdit();//权限标识
                RepositoryItemTextEdit TextEdit_KeyName = new RepositoryItemTextEdit();//按键名称

                view.OptionsSelection.EnableAppearanceFocusedCell = false;

                DataTable Clouddt = new DataTable();
                Clouddt.Columns.Add(new DataColumn("ActualFloor"));
                Clouddt.Columns.Add(new DataColumn("DevNo"));
                Clouddt.Columns.Add(new DataColumn("DevCheckFloor"));
                Clouddt.Columns.Add(new DataColumn("AuthFlag"));
                Clouddt.Columns.Add(new DataColumn("KeyName"));

                ///20190326
                view.Columns["ActualFloor"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                view.Columns["AuthFlag"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                view.Columns["KeyName"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                view.Columns["DevNo"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                view.Columns["DevCheckFloor"].OptionsColumn.AllowEdit = false;//设置列不可以编辑

                //按键名称
                c_KeyName = data.logicalFloor.ToString();
                string[] Is_KeyName = c_KeyName.Trim().Split(',');
                string[] buff_KeyName = new string[Is_KeyName.Length];
                //实际楼层
                if (string.IsNullOrWhiteSpace(data.logicFloor))
                {
                    data.logicFloor = "未设置";
                }

                c_ActualFloor = data.floorNum.ToString();


                string[] Is_ActualFloor = c_ActualFloor.Trim().Split(',');
                string[] buff_ActualFloor = new string[Is_ActualFloor.Length];

                //端子号
                if (string.IsNullOrWhiteSpace(data.terminalFloor))
                {
                    data.terminalFloor = "未设置";
                }

                c_DevNo = data.terminalFloor.ToString();
                string[] Is_DevNo = c_DevNo.Trim().Split(',');
                string[] buff_DevNo = new string[Is_DevNo.Length];

                //检测楼层
                if (string.IsNullOrWhiteSpace(data.detectFloor))
                {
                    data.detectFloor = "未设置";
                }
                c_DevCheckFloor = data.detectFloor.ToString();
                string[] Is_DevCheckFloor = c_DevCheckFloor.Trim().Split(',');
                string[] buff_DevCheckFloor = new string[Is_DevCheckFloor.Length];

                ///权限标识
                c_AuthFlag = data.naturalFloor.ToString();
                string[] Is_AuthFlag = c_AuthFlag.Trim().Split(',');
                string[] buff_AuthFlag = new string[Is_AuthFlag.Length];

                RunLog.Log("1050 测试用 ---读取:  权限标识---" + c_AuthFlag + " \n按键名称--" + c_KeyName + " \n实际楼层---" + c_ActualFloor + " \n端子号" + c_DevNo + " \n检测楼层" + c_DevCheckFloor);

                _StrDevNoLength = c_DevNo.Trim().Split(','); //获取云端读取数据长度
                _StrDevNoLength = _StrDevNoLength.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                for (int i = 0; i < _StrDevNoLength.Length; i++)
                {
                    DataRow Cloudrow = Clouddt.NewRow();

                    if (buff_ActualFloor.Length > 1)
                    {
                        buff_ActualFloor[i] = Is_ActualFloor[i];
                        Cloudrow["ActualFloor"] = buff_ActualFloor[i].ToString();
                    }
                    if (buff_DevNo.Length > 1)
                    {
                        buff_DevNo[i] = Is_DevNo[i];
                        Cloudrow["DevNo"] = buff_DevNo[i].ToString();
                    }
                    if (buff_DevCheckFloor.Length > 1)
                    {
                        buff_DevCheckFloor[i] = Is_DevCheckFloor[i];
                        Cloudrow["DevCheckFloor"] = buff_DevCheckFloor[i];
                    }
                    if (buff_AuthFlag.Length > 1)
                    {
                        buff_AuthFlag[i] = Is_AuthFlag[i];
                        Cloudrow["AuthFlag"] = buff_AuthFlag[i];
                    }
                    buff_KeyName[i] = Is_KeyName[i];
                    Cloudrow["KeyName"] = buff_KeyName[i];

                    Clouddt.Rows.Add(Cloudrow);
                    this.gridControl_FloorTable.DataSource = Clouddt;

                }

                view.Columns["ActualFloor"].ColumnEdit = ActuallFloor_riCombo;
                view.Columns["AuthFlag"].ColumnEdit = SpinEdit_AuthFlag;
                view.Columns["KeyName"].ColumnEdit = TextEdit_KeyName;
                view.Columns["DevNo"].ColumnEdit = riCombo_colDevNo;
                view.Columns["DevCheckFloor"].ColumnEdit = riCombo_colDevCheckFloor;


                HintProvider.ShowAutoCloseDialog(null, string.Format("读取云端楼层对应表成功"));
                //不可编辑
                //if (IsCloudDownFloor)
                //{
                view.Columns["ActualFloor"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                view.Columns["AuthFlag"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                view.Columns["KeyName"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                view.Columns["DevNo"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                view.Columns["DevCheckFloor"].OptionsColumn.AllowEdit = false;//设置列不可以编辑
                //}

            }
            catch (Exception ex)
            {

                RunLog.Log(ex);

            }

        }

        public interface IMdiParent
        {
            void TableInfoData(FloorTableInfo data);
        }

        /// <summary>
        /// 下载基础参数
        /// 云联动器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownDevList_Click(object sender, EventArgs e)
        {
            //获取相应参数
            CloudLinkageInfoCtrlInfo CloudLinkageInfo = GetDeviceInfoBeforeDown();

            //如果取值为空则跳出
            if (CloudLinkageInfo == null)
            {
                return;
            }

            //判断如果选中的是
            switch (this.tcDownParams.SelectedTabPageIndex)
            {
                case 0:
                {
                    //这里是真正下载的函数
                    this.DownBasicParams(CloudLinkageInfo);
                }
                    break;
                //case 2:
                //    {
                //        this.DownTables(CloudLinkageInfo);
                //    }
                //    break;
                default:
                    break;
            }

            //发送报文成功开始计时
            this.BeginTick = Environment.TickCount;
            this.CoolingTick = 600;
            this.IsBusy = true;
            this.tmrCommunication.Start();
        }

        /// <summary>
        /// 下载云联动器配置参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownConfigList_Click_1(object sender, EventArgs e)
        {
            //获取相应参数
            CloudLinkageInfoCtrlInfo CloudLinkageInfo = GetDeviceInfoBeforeDown();

            //如果取值为空则跳出
            if (CloudLinkageInfo == null)
            {
                return;
            }

            //判断如果选中的是
            switch (this.tcDownParams.SelectedTabPageIndex)
            {
                case 0:
                    {
                        #region 云联动器
                        //云联动器
                        if (LinkCtrlInfo.Count == 2)
                        {
                            #region 1号门界面配置
                            if (string.IsNullOrWhiteSpace(this.InstallFloorName1.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "安装楼层不能为空");
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(this.DelayOutCall1.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "延迟呼梯设置不能为空");
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontYqkIp1.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器IP不能为空");
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontDtNum1.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器机号不能为空");
                                return;
                            }
                            //if (string.IsNullOrWhiteSpace(this.BackYqkIp1.Text.Trim()))
                            //{
                            //    HintProvider.ShowAutoCloseDialog(null, "背门群控器IP不能为空");
                            //    return;
                            //}
                            //if (string.IsNullOrWhiteSpace(this.BackDtNum1.Text.Trim()))
                            //{
                            //    HintProvider.ShowAutoCloseDialog(null, "背门群控器机号不能为空");
                            //    return;
                            //}
                            #endregion

                            #region 2号门界面配置
                            if (string.IsNullOrWhiteSpace(this.InstallFloorName2.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "安装楼层不能为空");
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(this.DelayOutCall2.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "延迟呼梯设置不能为空");
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontYqkIp2.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器IP不能为空");
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontDtNum2.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器机号不能为空");
                                return;
                            }

                            //if (string.IsNullOrWhiteSpace(this.BackYqkIp2.Text.Trim()))
                            //{
                            //    HintProvider.ShowAutoCloseDialog(null, "背门群控器IP不能为空");
                            //    return;
                            //}
                            //if (string.IsNullOrWhiteSpace(this.BackDtNum2.Text.Trim()))
                            //{
                            //    HintProvider.ShowAutoCloseDialog(null, "背门群控器机号不能为空");
                            //    return;
                            //}
                            #endregion
                        }

                        if (LinkCtrlInfo.Count == 4)
                        {
                            #region 1号门界面配置
                            if (string.IsNullOrWhiteSpace(this.InstallFloorName1.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "安装楼层不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.DelayOutCall1.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "延迟呼梯设置不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontYqkIp1.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器IP不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontDtNum1.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器机号不能为空");
                                return ;
                            }

                            #endregion

                            #region 2号门界面配置
                            if (string.IsNullOrWhiteSpace(this.InstallFloorName2.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "安装楼层不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.DelayOutCall2.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "延迟呼梯设置不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontYqkIp2.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器IP不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontDtNum2.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器机号不能为空");
                                return ;
                            }

                            #endregion

                            #region 3号门界面配置
                            if (string.IsNullOrWhiteSpace(this.InstallFloorName3.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "安装楼层不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.DelayOutCall3.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "延迟呼梯设置不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontYqkIp3.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器IP不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontDtNum3.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器机号不能为空");
                                return ;
                            }

                            #endregion

                            #region 4号门界面配置
                            if (string.IsNullOrWhiteSpace(this.InstallFloorName4.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "安装楼层不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.DelayOutCall4.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "延迟呼梯设置不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontYqkIp4.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器IP不能为空");
                                return ;
                            }
                            if (string.IsNullOrWhiteSpace(this.FrontDtNum4.Text.Trim()))
                            {
                                HintProvider.ShowAutoCloseDialog(null, "正门群控器机号不能为空");
                                return ;
                            }
                            //if (string.IsNullOrWhiteSpace(this.BackYqkIp4.Text.Trim()))
                            //{
                            //    HintProvider.ShowAutoCloseDialog(null, "背门群控器IP不能为空");
                            //    return ;
                            //}
                            //if (string.IsNullOrWhiteSpace(this.BackDtNum4.Text.Trim()))
                            //{
                            //    HintProvider.ShowAutoCloseDialog(null, "背门群控器机号不能为空");
                            //    return ;
                            //}
                            #endregion
                        }
                        #endregion
                        //这里是真正下载的函数
                        AppConst.IsDownParmCloudLinkage = true;

                        this.DownLinkCtrlBasicParams(CloudLinkageInfo);
                    }
                    break;

                default:
                    break;
            }

            //发送报文成功开始计时
            this.BeginTick = Environment.TickCount;
            this.CoolingTick = 600;
            this.IsBusy = true;
            this.tmrCommunication.Start();

         
        }

        /// <summary>
        /// 下载云联动器基础参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownDevList_Click_1(object sender, EventArgs e)
        {
            //获取相应参数
            CloudLinkageInfoCtrlInfo CloudLinkageInfo = GetDeviceInfoBeforeDown();

            //如果取值为空则跳出
            if (CloudLinkageInfo == null)
            {
                return;
            }

            //判断如果选中的是
            switch (this.tcDownParams.SelectedTabPageIndex)
            {
                case 0:
                    {
                        this.IsBusy = true;

                        //这里是真正下载的函数
                        this.DownBasicParams(CloudLinkageInfo);
                    }
                    break;

                default:
                    break;
            }

            //发送报文成功开始计时
            this.BeginTick = Environment.TickCount;
            this.CoolingTick = 600;
            this.IsBusy = true;
            this.tmrCommunication.Start();
        }

        /// <summary>
        /// 组装云联动器的联动电梯参数
        /// LinkCtrlInfo 是联动器联动电梯数
        /// 韦将杰 2020-02-25 注释
        /// </summary>
        /// <param name="CloudLinkageInfo"></param>
        /// <returns></returns>
        private string GetLinkCtrlBasicParamsWriteCmdStr(CloudLinkageInfoCtrlInfo CloudLinkageInfo)
        {
            string strCmdStr = string.Empty;

            //CommonUtils.ToHex(c_AuthFlag, "utf-8", true);
            strCmdStr = "7B,22,64,61,74,61,22,3A,5B,7B,22,"; //数据头

            if (LinkCtrlInfo.Count == 2)
            {
                #region 获取1号门界面配置数据
                string _InstallFloorName1 = CommonUtils.ToHex(InstallFloorName1.Text.Trim(), "utf-8", true);
                string _DelayOutCall1 = CommonUtils.ToHex(DelayOutCall1.Text, "utf-8", true);
                string _FrontYqkIp1 = CommonUtils.ToHex(FrontYqkIp1.Text, "utf-8", true);
                string _FrontDtNum1 = CommonUtils.ToHex(FrontDtNum1.Text, "utf-8", true);
                string _BackYqkIp1 = CommonUtils.ToHex(BackYqkIp1.Text, "utf-8", true);
                string _BackDtNum1 = CommonUtils.ToHex(BackDtNum1.Text, "utf-8", true);
                string _groudControl_Config1_DoorNum1 = CommonUtils.ToHex(groudControl_Config1_DoorNum1.Text, "utf-8", true);

                //背门电梯号
                strCmdStr += "42,61,63,6B,44,74,4E,75,6D,22,3A,22," + _BackDtNum1 + ",22,2C,22" + ",";
                //背门群控器IP
                strCmdStr += "42,61,63,6B,59,71,6B,49,70,22,3A,22" + _BackYqkIp1 + ",22,2C,22" + ",";
                //延迟呼梯
                strCmdStr += "44,65,6C,61,79,4F,75,74,43,61,6C,6C,22,3A," + _DelayOutCall1 + ",2C,22" + ",";
                //门号              
                strCmdStr += "44,6F,6F,72,4E,75,6D,22,3A," + _groudControl_Config1_DoorNum1 + ",2C,22" + ",";
                //正门电梯号
                strCmdStr += "46,72,6F,6E,74,44,74,4E,75,6D,22,3A,22," + _FrontDtNum1 + "22,,2C,22" + ",";
                //正门群控器IP
                strCmdStr += "46,72,6F,6E,74,59,71,6B,49,70,22,3A,22," + _FrontYqkIp1 + ",22,2C,22" + ",";
                //安装楼层
                strCmdStr += "49,6E,73,74,61,6C,6C,46,6C,6F,6F,72,4E,61,6D,65,22,3A,22," + _InstallFloorName1 + "22,7D,2C,7B,22";
                #endregion

                #region 获取2号门界面配置数据
                string _InstallFloorName2 = CommonUtils.ToHex(InstallFloorName2.Text.Trim(), "utf-8", true);
                string _DelayOutCall2 = CommonUtils.ToHex(DelayOutCall2.Text, "utf-8", true);
                string _FrontYqkIp2 = CommonUtils.ToHex(FrontYqkIp2.Text, "utf-8", true);
                string _FrontDtNum2 = CommonUtils.ToHex(FrontDtNum2.Text, "utf-8", true);
                BackYqkIp2.Text = BackYqkIp2.Text.Trim();
                string _BackYqkIp2 = CommonUtils.ToHex(BackYqkIp2.Text, "utf-8", true);
                string _BackDtNum2 = CommonUtils.ToHex(BackDtNum2.Text, "utf-8", true);
                string _groudControl_Config1_DoorNum2 = CommonUtils.ToHex(groudControl_Config1_DoorNum2.Text, "utf-8", true);
                strCmdStr += "42,61,63,6B,44,74,4E,75,6D,22,3A,22," + _BackDtNum2 + ",22,2C,22" + ",";
                strCmdStr += "42,61,63,6B,59,71,6B,49,70,22,3A,22," + _BackYqkIp2 + ",22,2C,22" + ",";
                strCmdStr += "44,65,6C,61,79,4F,75,74,43,61,6C,6C,22,3A," + _DelayOutCall2 + ",2C,22" + ",";
                strCmdStr += "44,6F,6F,72,4E,75,6D,22,3A," + _groudControl_Config1_DoorNum2 + ",2C,22" + ",";
                strCmdStr += "46,72,6F,6E,74,44,74,4E,75,6D,22,3A,22," + _FrontDtNum2 + ",22,2C,22" + ",";
                strCmdStr += "46,72,6F,6E,74,59,71,6B,49,70,22,3A,22," + _FrontYqkIp2 + ",22,2C,22" + ",";
                strCmdStr += "49,6E,73,74,61,6C,6C,46,6C,6F,6F,72,4E,61,6D,65,22,3A,22," + _InstallFloorName2
                    + "22,7D,5D,2C,22,76,65,72,73,69,6F,6E,22,3A,22,30,22,7D";
                #endregion
            }
            else if (LinkCtrlInfo.Count == 4)
            {
                #region 获取1号门界面配置数据
                string _InstallFloorName1 = CommonUtils.ToHex(InstallFloorName1.Text.Trim(), "utf-8", true);
                string _DelayOutCall1 = CommonUtils.ToHex(DelayOutCall1.Text, "utf-8", true);
                string _FrontYqkIp1 = CommonUtils.ToHex(FrontYqkIp1.Text, "utf-8", true);
                string _FrontDtNum1 = CommonUtils.ToHex(FrontDtNum1.Text, "utf-8", true);
                string _BackYqkIp1 = CommonUtils.ToHex(BackYqkIp1.Text, "utf-8", true);
                string _BackDtNum1 = CommonUtils.ToHex(BackDtNum1.Text, "utf-8", true);
                string _groudControl_Config1_DoorNum1 = CommonUtils.ToHex(groudControl_Config1_DoorNum1.Text, "utf-8", true);
                strCmdStr += "42,61,63,6B,44,74,4E,75,6D,22,3A,22," + _BackDtNum1 + ",22,2C,22" + ",";
                strCmdStr += "42,61,63,6B,59,71,6B,49,70,22,3A,22" + _BackYqkIp1 + ",22,2C,22" + ",";
                strCmdStr += "44,65,6C,61,79,4F,75,74,43,61,6C,6C,22,3A," + _DelayOutCall1 + ",2C,22" + ",";
                strCmdStr += "44,6F,6F,72,4E,75,6D,22,3A," + _groudControl_Config1_DoorNum1 + ",2C,22" + ",";
                strCmdStr += "46,72,6F,6E,74,44,74,4E,75,6D,22,3A,22," + _FrontDtNum1 + ",22,2C,22" + ",";
                strCmdStr += "46,72,6F,6E,74,59,71,6B,49,70,22,3A,22," + _FrontYqkIp1 + ",22,2C,22" + ",";
                strCmdStr += "49,6E,73,74,61,6C,6C,46,6C,6F,6F,72,4E,61,6D,65,22,3A,22," + _InstallFloorName1 + "22,7D,2C,7B,22";
                #endregion

                #region 获取2号门界面配置数据
                string _InstallFloorName2 = CommonUtils.ToHex(InstallFloorName2.Text.Trim(), "utf-8", true);
                string _DelayOutCall2 = CommonUtils.ToHex(DelayOutCall2.Text, "utf-8", true);
                string _FrontYqkIp2 = CommonUtils.ToHex(FrontYqkIp2.Text, "utf-8", true);
                string _FrontDtNum2 = CommonUtils.ToHex(FrontDtNum2.Text, "utf-8", true);
                string _BackYqkIp2 = CommonUtils.ToHex(BackYqkIp2.Text, "utf-8", true);
                string _BackDtNum2 = CommonUtils.ToHex(BackDtNum2.Text, "utf-8", true);
                string _groudControl_Config1_DoorNum2 = CommonUtils.ToHex(groudControl_Config1_DoorNum2.Text, "utf-8", true);
                strCmdStr += "42,61,63,6B,44,74,4E,75,6D,22,3A,22," + _BackDtNum2 + ",22,2C,22" + ",";
                strCmdStr += "42,61,63,6B,59,71,6B,49,70,22,3A,22," + _BackYqkIp2 + ",22,2C,22" + ",";
                strCmdStr += "44,65,6C,61,79,4F,75,74,43,61,6C,6C,22,3A," + _DelayOutCall2 + ",2C,22" + ",";
                strCmdStr += "44,6F,6F,72,4E,75,6D,22,3A," + _groudControl_Config1_DoorNum2 + ",2C,22" + ",";
                strCmdStr += "46,72,6F,6E,74,44,74,4E,75,6D,22,3A,22," + _FrontDtNum2 + ",22,2C,22" + ",";
                strCmdStr += "46,72,6F,6E,74,59,71,6B,49,70,22,3A,22," + _FrontYqkIp2 + ",22,2C,22" + ",";
                strCmdStr += "49,6E,73,74,61,6C,6C,46,6C,6F,6F,72,4E,61,6D,65,22,3A,22," + _InstallFloorName2 + "22,7D,2C,7B,22";
                #endregion

                #region 获取3号门界面配置数据
                string _InstallFloorName3 = CommonUtils.ToHex(InstallFloorName3.Text.Trim(), "utf-8", true);
                string _DelayOutCall3 = CommonUtils.ToHex(DelayOutCall3.Text, "utf-8", true);
                string _FrontYqkIp3 = CommonUtils.ToHex(FrontYqkIp3.Text, "utf-8", true);
                string _FrontDtNum3 = CommonUtils.ToHex(FrontDtNum3.Text, "utf-8", true);
                string _BackYqkIp3 = CommonUtils.ToHex(BackYqkIp3.Text, "utf-8", true);
                string _BackDtNum3 = CommonUtils.ToHex(BackDtNum3.Text, "utf-8", true);
                string _groudControl_Config1_DoorNum3 = CommonUtils.ToHex(groudControl_Config1_DoorNum3.Text, "utf-8", true);
                strCmdStr += "42,61,63,6B,44,74,4E,75,6D,22,3A,22," + _BackDtNum3 + ",22,2C,22" + ",";
                strCmdStr += "42,61,63,6B,59,71,6B,49,70,22,3A,22," + _BackYqkIp3 + ",22,2C,22" + ",";
                strCmdStr += "44,65,6C,61,79,4F,75,74,43,61,6C,6C,22,3A," + _DelayOutCall3 + ",2C,22" + ",";
                strCmdStr += "44,6F,6F,72,4E,75,6D,22,3A," + _groudControl_Config1_DoorNum3 + ",2C,22" + ",";
                strCmdStr += "46,72,6F,6E,74,44,74,4E,75,6D,22,3A,22," + _FrontDtNum3 + "22,,2C,22" + ",";
                strCmdStr += "46,72,6F,6E,74,59,71,6B,49,70,22,3A,22," + _FrontYqkIp3 + ",22,2C,22" + ",";
                strCmdStr += "49,6E,73,74,61,6C,6C,46,6C,6F,6F,72,4E,61,6D,65,22,3A,22," + _InstallFloorName3 + "22,7D,2C,7B,22";
                #endregion

                #region 获取4号门界面配置数据
                string _InstallFloorName4 = CommonUtils.ToHex(InstallFloorName4.Text.Trim(), "utf-8", true);
                string _DelayOutCall4 = CommonUtils.ToHex(DelayOutCall4.Text, "utf-8", true);
                string _FrontYqkIp4 = CommonUtils.ToHex(FrontYqkIp4.Text, "utf-8", true);
                string _FrontDtNum4 = CommonUtils.ToHex(FrontDtNum4.Text, "utf-8", true);
                BackYqkIp4.Text = BackYqkIp4.Text.Trim();
                string _BackYqkIp4 = CommonUtils.ToHex(BackYqkIp4.Text, "utf-8", true);
                string _BackDtNum4 = CommonUtils.ToHex(BackDtNum4.Text, "utf-8", true);
                string _groudControl_Config1_DoorNum4 = CommonUtils.ToHex(groudControl_Config1_DoorNum4.Text, "utf-8", true);
                strCmdStr += "42,61,63,6B,44,74,4E,75,6D,22,3A,22," + _BackDtNum4 + ",22,2C,22" + ",";
                strCmdStr += "42,61,63,6B,59,71,6B,49,70,22,3A,22," + _BackYqkIp4 + ",22,2C,22" + ",";
                strCmdStr += "44,65,6C,61,79,4F,75,74,43,61,6C,6C,22,3A," + _DelayOutCall4 + ",2C,22" + ",";
                strCmdStr += "44,6F,6F,72,4E,75,6D,22,3A," + _groudControl_Config1_DoorNum4 + ",2C,22" + ",";
                strCmdStr += "46,72,6F,6E,74,44,74,4E,75,6D,22,3A,22," + _FrontDtNum4 + ",22,2C,22" + ",";
                strCmdStr += "46,72,6F,6E,74,59,71,6B,49,70,22,3A,22," + _FrontYqkIp4 + ",22,2C,22" + ",";
                strCmdStr += "49,6E,73,74,61,6C,6C,46,6C,6F,6F,72,4E,61,6D,65,22,3A,22," + _InstallFloorName4
                    + "22,7D,5D,2C,22,76,65,72,73,69,6F,6E,22,3A,22,30,22,7D";
                #endregion
            }

            strCmdStr = strCmdStr.Replace(",", "");
            //云联动器联动电梯参数下载增加MAC校验 韦将杰 2020-02-25
            //先取出逗号再加密
            string strMac = KeyMacOperate.GetMacEx(strCmdStr);
            strCmdStr += strMac;

            RunLog.Log("isLinkCtrl ----> " + strCmdStr);
            
            return strCmdStr;
        }

        /// <summary>
        /// 云联动器报文
        /// </summary>
        /// <param name="CloudLinkageInfo"></param>
        private void DownLinkCtrlBasicParams(CloudLinkageInfoCtrlInfo CloudLinkageInfo)
        {
            //获取写指令
            string strWriteCmd = this.GetLinkCtrlBasicParamsWriteCmdStr(CloudLinkageInfo);

            RunLog.Log("DownLinkCtrlBasicParams.strWriteCmd ----> " + strWriteCmd);
            //判断长度
            //if (strWriteCmd.Length < 49 + 8) //26字节 +4字节
            //{
            //    HintProvider.ShowAutoCloseDialog(null, "生成的报文长度错误，请检查设置的参数是否正确");
            //    return;
            //}
            string writeReport = this.GetWriteReport(CloudLinkageInfo.DevId, AppConst.CMD_WORD_SET_CLOUD_LINKCTRL_PARAMS, strWriteCmd);

            RunLog.Log("组装的数据:" + strWriteCmd + " 下发报文" + writeReport);
            string errMsg = string.Empty;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
            if (!this.UdpListener.SendData(endpoint, writeReport, ref errMsg))
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("设置参数失败，错误：{0}", errMsg));
            }
            f_CurrentCloudLinkageInfo = CloudLinkageInfo;
        }

        //1号门延迟呼梯
        private void DelayOutCall1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (DelayOutCall1.Text != "")
                {
                    int EdtPortValue = int.Parse(DelayOutCall1.Text);
                    if (EdtPortValue > 600)
                    {
                        HintProvider.ShowAutoCloseDialog(null, "延迟呼梯超出设置范围,请重新输入0~600范围的值");

                        DelayOutCall1.Text = string.Empty;
                        DelayOutCall1.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //2号门延迟呼梯
        private void DelayOutCall2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (DelayOutCall2.Text != "")
                {
                    int EdtPortValue = int.Parse(DelayOutCall2.Text);
                    if (EdtPortValue > 600)
                    {
                        HintProvider.ShowAutoCloseDialog(null,"延迟呼梯超出设置范围,请重新输入0~600范围的值");
                        DelayOutCall2.Text = string.Empty;
                        DelayOutCall2.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //3号门延迟呼梯
        private void DelayOutCall3_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (DelayOutCall3.Text != "")
                {
                    int EdtPortValue = int.Parse(DelayOutCall3.Text);
                    if (EdtPortValue > 600)
                    {
                        HintProvider.ShowAutoCloseDialog(null, "延迟呼梯超出设置范围,请重新输入0~600范围的值");

                        DelayOutCall3.Text = string.Empty;
                        DelayOutCall3.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //4号门延迟呼梯
        private void DelayOutCall4_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (DelayOutCall4.Text != "")
                {
                    int EdtPortValue = int.Parse(DelayOutCall4.Text);
                    if (EdtPortValue > 600)
                    {
                        HintProvider.ShowAutoCloseDialog(null, "延迟呼梯超出设置范围,请重新输入0~600范围的值");

                        DelayOutCall4.Text = string.Empty;
                        DelayOutCall4.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //1号门正门机号
        private void FrontDtNum1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (FrontDtNum1.Text != "")
                {
                    int EdtPortValue = int.Parse(FrontDtNum1.Text);
                    if (EdtPortValue > 384)
                    {
                        HintProvider.ShowAutoCloseDialog(null, "正门电梯机号超出设置范围,请重新输入0~384范围的值");

                        FrontDtNum1.Text = string.Empty;
                        FrontDtNum1.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //1号门背门机号
        private void BackDtNum1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (BackDtNum1.Text != "")
                {
                    int EdtPortValue = int.Parse(BackDtNum1.Text);
                    if (EdtPortValue > 384)
                    {
                        HintProvider.ShowAutoCloseDialog(null, "背门电梯机号超出设置范围,请重新输入0~384范围的值");

                        BackDtNum1.Text = string.Empty;
                        BackDtNum1.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //2号门正门机号
        private void FrontDtNum2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (FrontDtNum2.Text != "")
                {
                    int EdtPortValue = int.Parse(FrontDtNum2.Text);
                    if (EdtPortValue > 384)
                    {
                        HintProvider.ShowAutoCloseDialog(null,"正门电梯机号超出设置范围,请重新输入0~384范围的值");
                        FrontDtNum2.Text = string.Empty;
                        FrontDtNum2.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //2号门背门机号
        private void BackDtNum2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (BackDtNum2.Text != "")
                {
                    int EdtPortValue = int.Parse(BackDtNum2.Text);
                    if (EdtPortValue > 384)
                    {
                        HintProvider.ShowAutoCloseDialog(null,"背门电梯机号超出设置范围,请重新输入0~384范围的值");
          
                        BackDtNum2.Text = string.Empty;
                        BackDtNum2.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //3号门正门机号
        private void FrontDtNum3_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (FrontDtNum3.Text != "")
                {
                    int EdtPortValue = int.Parse(FrontDtNum3.Text);
                    if (EdtPortValue > 384)
                    {
                        HintProvider.ShowAutoCloseDialog(null,"正门电梯机号超出设置范围,请重新输入0~384范围的值");
                        FrontDtNum3.Text = string.Empty;
                        FrontDtNum3.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //3号门背门机号
        private void BackDtNum3_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (BackDtNum3.Text != "")
                {
                    int EdtPortValue = int.Parse(BackDtNum3.Text);
                    if (EdtPortValue > 384)
                    {
                        HintProvider.ShowAutoCloseDialog(null, "背门电梯机号超出设置范围,请重新输入0~384范围的值");
                        BackDtNum3.Text = string.Empty;
                        BackDtNum3.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //4号门正门机号
        private void FrontDtNum4_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (FrontDtNum4.Text != "")
                {
                    int EdtPortValue = int.Parse(FrontDtNum4.Text);
                    if (EdtPortValue > 384)
                    {
                        HintProvider.ShowAutoCloseDialog(null, "正门电梯机号超出设置范围,请重新输入0~384范围的值");

                        FrontDtNum4.Text = string.Empty;
                        FrontDtNum4.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        //4号门背门机号
        private void BackDtNum4_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (BackDtNum4.Text != "")
                {
                    int EdtPortValue = int.Parse(BackDtNum4.Text);
                    if (EdtPortValue > 384)
                    {
                        HintProvider.ShowAutoCloseDialog(null, "背门电梯机号超出设置范围,请重新输入0~384范围的值");
                        BackDtNum4.Text = string.Empty;
                        BackDtNum4.Text = "";
                    }
                }
            }
            catch
            {

            }
        }

        private void labelControl13_Click(object sender, EventArgs e)
        {

        }

        private void labelControl33_Click(object sender, EventArgs e)
        {

        }

        private void InstallFloorName1_EditValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 判断是否显示新基点的属性
        /// </summary>
        /// <param name="hasNewBenchmark"></param>
        private void ShowNewBenchmarkUI(bool hasNewBenchmark)
        {
            if (hasNewBenchmark)
            {
                
                this.liNewBenchMarkPort.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.liNewBenchMarkIp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            } else
            {
                this.liNewBenchMarkPort.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.liNewBenchMarkIp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void edtNewBenchMarkPort_KeyUp(object sender, KeyEventArgs e)
        {
            
            int NewBenchMarkPort = StrUtils.StrToIntDef(this.edtNewBenchMarkPort.Text.Trim(), 0);

            if (NewBenchMarkPort >= 1024)
            {
                edtNewBenchMarkPort.ForeColor = Color.FromArgb(32, 31, 53);
                DownDevList.Enabled = true;
                return;
            }

            if ((NewBenchMarkPort > 0) && (NewBenchMarkPort < 1024))
            {
                edtNewBenchMarkPort.ForeColor = Color.Red;
                DownDevList.Enabled = false;
            }

            if ((NewBenchMarkPort >= 1000) && (NewBenchMarkPort < 1024))
            {
              //  HintProvider.ShowAutoCloseDialog(null, "端口号不能小于1024", HintIconType.Warning, 1000);
            }
        }

    }
}
