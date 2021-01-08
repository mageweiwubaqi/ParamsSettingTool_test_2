///<summary>
///模块编号：云门禁参数设置模块
///作用：对云门禁进行搜索、参数设置
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
///5、云门禁设备类型增加新基点IP和端口、是否具有新基点属性HasNewBenchmark  CloudEntranceInfo 
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
namespace ITL.ParamsSettingTool.ParamsSettingTool.Devices.CloudEntrance
{
  //  public delegate void GetCloudFloorTable(FloorTableInfo data);

    public partial class CloudEntrancelUserControl : GeneralDeviceUserControl
    {
        public CloudEntrancelUserControl()
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
        private const string DEVTYPE = "DevType";//版本号        
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

        private const string DNS_SERVER_IP_ALIAS = "DNS服务器IP";  //DNS服务器IP
        private const string DHCP_FUNCTION_ALIAS = "DHCP功能"; //DHCP功能
        private const string EI_SERVER_IP_ALIAS = "线下物联平台服务器IP地址";  //线下物联平台服务器IP
        private const string EI_SERVER_PORT_ALIAS = "线下物联平台服务器端口";  //线下物联平台服务器端口
        private const string LINKAGE_PORT_ALIAS = "协议控制器端口"; //联动控制器端口
        private const string PROJECT_NO_ALIAS = "项目编号";
        private const string VERSION_ALIAS = "版本号";
        private const string DEV_STATUS = "设备状态";
        /// <summary>
        /// 新基点IP
        /// </summary>
        private const string NEWBENCHMARK_IP_ALIAS = "新基点IP";
        /// <summary>
        /// 新基点端口
        /// </summary>
        private const string NEWBENCHMARK_PORT_ALIAS = "新基点端口";

        #endregion

        private CloudEntranceInfo f_CurrentEntranceInfo = null;  //当前操作的设备信息
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
            this.tbDownTables.Hide();

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

            this.DevType = DeviceType.GroupEntrance;

            this.edtLinkagePort.Text = "60202"; //默认值
            f_IsRefresh = true;
            this.rgpDHCP.SelectedIndex = 1;
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

            this.edtDevIp.Leave += this.edtDevIp_Leave;
            this.edtSubnetMark.Leave += this.edtDevIp_Leave;
            this.edtGateWay.Leave += this.edtDevIp_Leave;
            this.edtDNSServerIp.Leave += this.edtDevIp_Leave;
            this.edtEIServerIp.Leave += this.edtDevIp_Leave;
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
            f_DevicesDataTable.Columns.Add(DEVTYPE, typeof(string));
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
            this.AddOneGridViewColumn(grdView, VERSION, VERSION_ALIAS, 260);
            this.AddOneGridViewColumn(grdView, STATUS, DEV_STATUS, 100);
            //新基点IP
            this.AddOneGridViewColumn(grdView, NEWBENCHMARK_IP, NEWBENCHMARK_IP_ALIAS, 140);
            //新基点端口
            this.AddOneGridViewColumn(grdView, NEWBENCHMARK_PORT, NEWBENCHMARK_PORT_ALIAS, 100);

            //门参数
            // this.AddOneGridViewColumn(grdView, LINKAGE_PRAM, MJ_LINKAGE_PRAM, 200);

            //设备状态


            this.gcDevices.DataSource = f_DevicesDataTable;
            ControlUtilityTool.AdjustIndicatorWidth(grdView);
        }

        public static explicit operator CloudEntrancelUserControl(Form v)
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
        private void AddOneDeviceToUI(CloudEntranceInfo deviceInfo)
        {

            DataRow[] rows = f_DevicesDataTable.Select(string.Format("{0}={1}", DEV_ID, deviceInfo.DevId));

            FindCount.AddToUpdate("" + deviceInfo.DevId);
            FindCount.AddToUpdate("" + deviceInfo.DevIp);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                try
                {
                    int FindSumCount = AppEnv.Singleton.UdpCount * AppEnv.Singleton.UdpCount ;

                    if (FindCount["" + deviceInfo.DevId].FontCount > FindSumCount && !FindCount["" + deviceInfo.DevId].IsHint)
                    {
                        FindCount["" + deviceInfo.DevId].IsHint = true;
                        HintProvider.ShowAutoCloseDialog(null, "设备ID冲突,ID:" + deviceInfo.DevId+ ",mac :"+ deviceInfo.DevMac);
                    }
                    if (FindCount[deviceInfo.DevIp].FontCount > FindSumCount && !FindCount["" + deviceInfo.DevIp].IsHint)
                    {
                        FindCount["" + deviceInfo.DevIp].IsHint = true;
                        HintProvider.ShowAutoCloseDialog(null, string.Format("设备IP冲突,IP:{0},ID:{1},Mac:{2}", deviceInfo.DevIp, deviceInfo.DevId, deviceInfo.DevMac));
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
                    rows[0][DEVTYPE] = deviceInfo.DevType;
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
                     deviceInfo.DevType,
                     deviceInfo.DevStatus,
                     deviceInfo.NewBenchmarkIP,
                     deviceInfo.NewBenchmarkPort,
                     deviceInfo.HasNewBenchmark
                );

            }
            //Repeat_Dev_Id.Add(elevatorInfo.DevId);

            //排序
            f_DevicesDataTable.DefaultView.Sort = string.Format("{0} {1}", DEV_ID, "ASC");
            ControlUtilityTool.AdjustIndicatorWidth(this.gvDevices);
        }

        /// <summary>
        /// 重写父类的解析函数，包括搜索数据和设置参数的解析。
        /// </summary>
        /// <param name="strCmdStr"></param>
        protected override void AnalysisRecvDataEx(string strCmdStr)
        {
            //报文格式F2 XX XX ... XX XX F3
            //判断设备类型是否合法
            //云联动器的设备类型为CloudLinkageInfoCtrl  0x16
            RunLog.Log(string.Format("云门禁报文：{0}", strCmdStr));
            DeviceType devType = CommandProcesserHelper.GetDevTypeByCmdInfo(StrUtils.CopySubStr(strCmdStr, 6, 2));
            if (devType != DeviceType.GroupEntrance)
            {
                return;
            }
            //根据命令字，解析对应命令
            string strCmdWord = StrUtils.CopySubStr(strCmdStr, 14, 4);
            if (strCmdWord == AppConst.CMD_WORD_SEARCH_DEVIDES)
            {
                //判断报文长度
                //      if (strCmdStr.Length < 86 + 8) //43字节 +项目编号4字节
                if (strCmdStr.Length < 74) //分体门禁
                {
                    RunLog.Log(string.Format("报文长度错误，报文：{0}", strCmdStr));
                    return;
                }
                this.AnalysisSearchDevicesRecvData(strCmdStr);
            }
            else if (strCmdWord == AppConst.CMD_WORD_SET_CLOUD_ELEVATOR_PARAMS)
            {
                //if (strCmdStr.Length > 22)
                //    return;
                //判断报文长度
                if (strCmdStr.Length < 22) //12字节
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


        //F2 01 25 11 02 00 00 10 00 04 40 00 02 A8 F0 76 DD 04 C0 A8 7B DA F0 7F F0 7F F0 7F 00 C0 A8 7B F0 7E C0 A8 7B 6F 31 25 00 00 D3 A2 4E 01 00 00 01 24 1E 04 00 00 1D 59 4C 44 30 31 31 30 41 20 56 31 30 30 30 2F 59 4D 4A 30 31 32 30 41 20 56 31 30 30 30 6A F3 
        /// <summary>
        /// 解析UDP搜索返回的数据
        /// </summary>
        /// <param name="strCmdStr"></param>
        private void AnalysisSearchDevicesRecvData(string strCmdStr)
        {

            //取返回的数据部分
            string strCmdReport = StrUtils.CopySubStr(strCmdStr, 18, strCmdStr.Length - 22);

            CloudEntranceInfo deviceInfo = new CloudEntranceInfo();

            //设备类型
            //  deviceInfo.DevType = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 1, 2), 0, 16);

            //设备类型以长度区分  长的为云门禁，短的为分体门禁

            int strCmdStrLenth = strCmdStr.Length;
            if (strCmdStrLenth <= 74)
            {
                deviceInfo.DevType = 1; //分体门禁
            }
            else
            {
                deviceInfo.DevType = 2;//云门禁
            }
         
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
            //CloudLinkageInfo.EIServerIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 48, 8));
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
            deviceInfo.ProjectNo = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 64, 8), 0, 10).ToString();
            if (deviceInfo.ProjectNo.Length < 2)
            {
                deviceInfo.ProjectNo = "";
            }
            else
            {
                deviceInfo.ProjectNo= StrUtils.ComplementedStr(StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 64, 8), 0, 10).ToString(), 8, "0");
            }
            //设备状态
            deviceInfo.DevStatus = StrUtils.CopySubStr(strCmdReport, 72, 8);

            string Length = StrUtils.CopySubStr(strCmdReport, 80, 2);

            //程序版本号长度
            string VersionLength = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 80, 2), 0, 16).ToString();
            //RunLog.Log("设备ID为：" + CloudLinkageInfo.DevId + "设备IP为：" + CloudLinkageInfo.DevIp + "搜索报文版本长度为：" + VersionLength);
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

            //2020-04-28 韦将杰 云门禁不需要显示云联动器的设备
            //设备类型是版本号开头三位
            string tmpDeviceType = StrUtils.CopySubStr(deviceInfo.VerSion, 0, 3);
            if (StrUtils.CopySubStr(deviceInfo.VerSion,0,3) == "YLD")
            {
                return;
            }

            //韦将杰 2020-04-26 增加新基点IP和接口解析
            //华为新基点属性报文起始位置为82+版本号长度
            int indexNewBenchmar = 82 + f_VersionLength;
            //华为新基点IP，位置为82+版本号长度，ip四字节端口号4字节
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
  

           this.AddOneDeviceToUI(deviceInfo);
        }

        /// <summary>
        /// 解析下载后UDP返回的数据
        /// </summary>
        /// <param name="strCmdStr"></param>
        private void AnalysisDownParams(string strCmdStr)
        {
            //取返回的数据部分
            string strCmdReport = StrUtils.CopySubStr(strCmdStr, 18, strCmdStr.Length - 22);

            //判断设备Id是否正确
            int devId = CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4));
            if (devId != f_CurrentEntranceInfo.DevId)
            {
                return;
            }

            if (strCmdStr.Length == 22)
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
                        HintProvider.ShowAutoCloseDialog(null, "参数设置失败，需初始化后才能设置网络参数");
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


       /// <summary>
       /// 下载参数
       /// </summary>
        protected override void DownParams()
        {
            CloudEntranceInfo deviceInfo = GetDeviceInfoBeforeDown();
            if (deviceInfo == null)
            {
                return;
            }
            switch (this.tcDownParams.SelectedTabPageIndex)
            {
                case 0: //下载设备参数.
                    {
                        if (!R_Btn_Click)
                        {
                            this.DownBasicParams(deviceInfo);
                        }
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
        /// 下载设备参数.
        /// </summary>
        /// <param name="elevatorInfo">云门禁.</param>
        private void DownBasicParams(CloudEntranceInfo deviceInfo)
        {
            string strWriteCmd = this.GetBasicParamsWriteCmdStr(deviceInfo);
            string writeReport = this.GetWriteReport(deviceInfo.DevId, AppConst.CMD_WORD_SET_CLOUD_GROUPLIKE_PARAMS, strWriteCmd);
            string errMsg = string.Empty;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
            if (!this.UdpListener.SendData(endpoint, writeReport, ref errMsg))
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("设置参数失败，错误：{0}", errMsg));
            }
            f_CurrentEntranceInfo = deviceInfo;
        }

        /// <summary>
        /// 获取写指令，返回要向设备写入数据的指令 
        /// 韦将杰
        /// 2020.02.25注释
        /// </summary>
        /// <param name="CloudLinkageInfo"></param>
        /// <returns></returns>
        private string GetBasicParamsWriteCmdStr(CloudEntranceInfo deviceInfo)
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
            //协议版本目前固定为00
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
            

            //云门禁增加MAC校验 韦将杰 2020-02-25
            string strMac = KeyMacOperate.GetMacEx(strCmdStr);

            strCmdStr += strMac;

            return strCmdStr;
         }

        //private void DownTables(GroupCloudLinkage elevatorInfo)
        //{
        //    string strCmdWord = string.Empty;
        //    string strWriteCmd = string.Empty;
        //    this.GetFloorTableWriteCmdStr(elevatorInfo, ref strWriteCmd, ref strCmdWord);
        //    string writeReport = this.GetWriteReport(elevatorInfo.DevId, strCmdWord, strWriteCmd);
        //    string errMsg = string.Empty;
        //    IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
        //    if (!this.UdpListener.SendData(endpoint, writeReport, ref errMsg))
        //    {
        //        HintProvider.ShowAutoCloseDialog(null, string.Format("设置对应表失败，错误：{0}", errMsg));
        //    }
        //    f_CurrentElevatorInfo = elevatorInfo;
        //}
   
        private string GetHexStrByNo(int tableNo)
        {
            string strTemp = StrUtils.IntToHex(tableNo, 2);
            strTemp = StrUtils.CopySubStr(strTemp, strTemp.Length - 2, 2);
            return strTemp;
        }

        private CloudEntranceInfo GetDeviceInfoBeforeDown()
        {
            if (!this.CheckUIValid())
            {
                return null;
            }
            CloudEntranceInfo deviceInfo = this.GetDeviceInfoByFocusRow();
            if (deviceInfo == null)
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

            if (this.edtProjectNo.Enabled)
            {
                if (string.IsNullOrWhiteSpace(this.edtProjectNo.Text.Trim()))
                {
                    HintProvider.ShowAutoCloseDialog(null, "项目编号不能为空");
                    return false;
                }
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
        /// 双击行
        /// </summary>
        protected override void ExcuteDoubleClick()
        {
            f_IsRefresh = true;
            try
            {
                //加载基础信息到界面
                CloudEntranceInfo deviceInfo = this.GetDeviceInfoByFocusRow();
                if (deviceInfo == null)
                {
                    return;
                }

                edtNewBenchMarkPort.ForeColor = Color.FromArgb(32, 31, 53);
                Btn_DownLoadDev.Enabled = true;
                Btn_DownLoadParm.Enabled = true;

                this.ShowNewBenchmarkUI(deviceInfo.HasNewBenchmark);
                this.edtDevIp.Text = deviceInfo.DHCPEnable ? string.Empty : deviceInfo.DevIp;
                this.edtSubnetMark.Text = deviceInfo.DHCPEnable ? string.Empty : deviceInfo.SubnetMask;
                this.edtGateWay.Text = deviceInfo.DHCPEnable ? string.Empty : deviceInfo.GateWay;
                this.edtDNSServerIp.Text = deviceInfo.DHCPEnable ? string.Empty : deviceInfo.DNSServerIp;
                this.edtEIServerIp.Text = deviceInfo.EIServerIp;
                this.edtEIServerPort.Text = deviceInfo.EIServerPort.ToString();
                this.edtLinkagePort.Text = deviceInfo.LinkagePort.ToString();
                this.rgpDHCP.SelectedIndex = deviceInfo.DHCPEnable ? 0 : 1;

                if (deviceInfo.DevType == 1)//分体门禁
                {
                   // edtDNSServerIp.Text = "";
                    edtProjectNo.Text = "";
                    this.edtDNSServerIp.Enabled = false;
                    this.rgpDHCP.Enabled = false;
                    this.edtProjectNo.Enabled = false;

                    return;

                }
                else
                {
                    this.edtDNSServerIp.Enabled = true;
                    this.rgpDHCP.Enabled = true;
                    this.edtProjectNo.Enabled = true;
                }

                this.edtDevIp.Enabled = !deviceInfo.DHCPEnable;
                this.edtSubnetMark.Enabled = !deviceInfo.DHCPEnable;
                this.edtGateWay.Enabled = !deviceInfo.DHCPEnable;
                this.edtDNSServerIp.Enabled = !deviceInfo.DHCPEnable;
                this.edtProjectNo.Text = StrUtils.ComplementedStr(deviceInfo.ProjectNo.ToString(), 8, "0");

                if (deviceInfo.DHCPEnable)
                {
                    this.edtDevIp.Tag = deviceInfo.DevIp;
                    this.edtSubnetMark.Tag = deviceInfo.SubnetMask;
                    this.edtGateWay.Tag = deviceInfo.GateWay;
                    this.edtDNSServerIp.Tag = deviceInfo.DNSServerIp;
                    this.edtProjectNo.Tag = deviceInfo.ProjectNo;
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
              
            }
            finally
            {
                f_IsRefresh = false;
                //读取选中的设备楼层对应表
                R_Btn_Click = true;
               //  DownParams();  ///zhang 2019-4-27
                R_Btn_Click = false;
            }
        }

        private CloudEntranceInfo GetDeviceInfoByFocusRow()
        {
            if (this.gvDevices.FocusedRowHandle < 0)
            {
                return null;
            }
            CloudEntranceInfo deviceInfo = new CloudEntranceInfo()
            {
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
                DevType = StrUtils.StrToIntDef( this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, DEVTYPE).ToString(),0),
                VerSion = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, VERSION).ToString(),
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

                this.edtDevIp.Text = string.Empty;
                this.edtSubnetMark.Text = string.Empty;
                this.edtGateWay.Text = string.Empty;
                this.edtDNSServerIp.Text = string.Empty;

                this.edtDevIp.Enabled = false;
                this.edtSubnetMark.Enabled = false;
                this.edtGateWay.Enabled = false;
                this.edtDNSServerIp.Enabled = false;
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
        private void QuickSetInLocal(GroupCloudLinkage elevatorInfo)
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
               

            }
            catch (Exception ex)
            {
                RunLog.Log(ex);
            }
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
        /// 组装下载读取控制器信息
        /// </summary>
        /// <param name="elevatorInfo"></param>
        /// <param name="readCmd"></param>
        /// <param name="cmdWord"></param>
        private void GetFloorTableReadCmdStr(GroupCloudLinkage elevatorInfo, ref string readCmd, ref string cmdWord)
        {

            DownTableInfo downTableInfo = this.GetTablesBeforeDownInfo(elevatorInfo.DevId);
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

                    HintProvider.ShowAutoCloseDialog(null, string.Format("读取成功"));
                    //HintProvider.ShowAutoCloseDialog(null, string.Format("读取成功"), HintIconType.OK, 3000);

                    //Todo 禁用按键 
                }
                else
                {
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
            edtNewBenchMarkPort.ForeColor = Color.FromArgb(32, 31, 53);
            Btn_DownLoadDev.Enabled = true;
            Btn_DownLoadParm.Enabled = true;

            int NewBenchMarkPort = StrUtils.StrToIntDef(this.edtNewBenchMarkPort.Text.Trim(), 0);

            if (NewBenchMarkPort >= 1024)
            {
                return;
            }

            if ((NewBenchMarkPort > 0) && (NewBenchMarkPort < 1024))
            {
                edtNewBenchMarkPort.ForeColor = Color.Red;
                Btn_DownLoadDev.Enabled = false;
                Btn_DownLoadParm.Enabled = false;
            }

            if ((NewBenchMarkPort >= 1000) && (NewBenchMarkPort < 1024))
            {
                //HintProvider.ShowAutoCloseDialog(null, "端口号不能小于1024", HintIconType.Warning, 1000);
            }
        }


    }
}
