///<summary>
///模块编号：云电梯参数设置模块
///作用：对云电梯进行搜索、参数设置
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
///5、云电梯设备类型增加新基点IP和端口、是否具有新基点属性HasNewBenchmark  CloudEntranceInfo 
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

///<summary>
///模块编号：云电梯参数设置模块
///Log编号：2
///修改描述：云电梯楼层对应表样式与栏位修改
///1、楼层对应表栏位设置为单表头
///2、楼层对应表栏位修改
///3、楼增对应表代码优化
///4、端子号、第一操纵盘端子号、第二操纵盘端子号，行列都不能重复
///修改日期：2020-04-27（注释日期）
///</summary>
///
///<summary>
///模块编号：楼层对应表
///Log编号：3
///修改描述：新增批量设置楼层对应表、导出楼层对应表
///修改日期：2020-08-14（修改日期）
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
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using ITL.ParamsSettingTool.SettingCenter;
using System.Reflection;
using ITL.ParamsSettingTool.ParamsSettingTool.Devices.CloudElevator;

namespace ITL.ParamsSettingTool
{
    public delegate void GetCloudFloorTable(FloorTableInfo data);
    public partial class CloudElevatorUserControl : GeneralDeviceUserControl
    {
        #region 字段常量定义
        private const string DNS_SERVER_IP = "dns_server_ip";  //DNS服务器IP
        private const string DHCP_FUNCTION = "dhcp_function"; //DHCP功能
        private const string EI_SERVER_IP = "ei_server_ip";  //线下物联平台服务器IP
        private const string EI_SERVER_PORT = "ei_server_port";  //线下物联平台服务器端口
        private const string LINKAGE_PORT = "linkage_port"; //联动控制器端口
        private const string PROJECT_NO = "Project_No";//项目编号
        private const string VERSION = "VerSion";//版本号

        private const string STATUS = "dev_status";//110009

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
        private const string LINKAGE_PORT_ALIAS = "协议控制器端口"; //联动控制器端口
        private const string PROJECT_NO_ALIAS = "项目编号";
        private const string VERSION_ALIAS = "版本号";

        private const string DEV_STATUS = "设备状态";//110009
        /// <summary>
        /// 新基点IP
        /// </summary>
        private const string NEWBENCHMARK_IP_ALIAS = "新基点IP";
        /// <summary>
        /// 新基点端口
        /// </summary>
        private const string NEWBENCHMARK_PORT_ALIAS = "新基点端口";

        #endregion

        private CloudElevatorInfo f_CurrentElevatorInfo = null;  //当前操作的设备信息
        private LocalDBOperate f_LocalDBOperate = null; //数据库操作类
        private bool f_IsRefresh = false;

        private string c_AuthFlag = string.Empty; //权限标识
        private string c_KeyName = string.Empty;//按键名称
        private string c_ActualFloor = string.Empty; //实际楼层
        private string c_TerminalNum = string.Empty; //端子号
        private string c_DevCheckFloor = string.Empty; //检查楼层
        private string c_TerminalNumSlave1 = string.Empty; //第一副操纵盘
        private string c_TerminalNumSlave2 = string.Empty; //第二副操纵盘
        private string c_TerminalNumIntercom = string.Empty; //对讲端子号

        private bool R_Btn_Click;
        private bool W_Btn_Click;

        private string Select_AuthFlag = string.Empty; //选中的权限标识
        private string Select_KeyName = string.Empty; //选中的按键名称
        private string Select_ActualFloor = string.Empty; //选中的实际楼层
        private string Select_DevNo = string.Empty; //选中的端子号
        private string Select_DevCheckFloor = string.Empty; //选中的检测楼层

        public ResponseInfo UserControl_FloorTable = null;

        private string f_StratAuthFlag = string.Empty;
        private string f_EndAuthFlag = string.Empty;
        private string f_StartDevNo = string.Empty;
        private bool IsCloudDownFloor = false;


        bool IsDevAs = false;

        string[] _StrDevNoLength;

        private string f_QuickForm_edt_KeyNameNo = string.Empty; //楼--单位
        private string f_QuickForm_cbbE_Start = string.Empty;//类别，快速设置 开始位置

        private int IsReadSuceess = 0; //是否读取成功    0--- 失败      1---成功

        //是否具有新基点属性，下载参数时候拼接命令用。
        private bool f_HasNewbenchmark = false;
        //是否具有第一副操纵盘
        private bool f_HasTerminalNumSlave1 = false;
        //是否具有第二副操纵盘
        private bool f_HasTerminalNumSlave2 = false;

        List<FloorRelationUIObject> floorRelationUIObjectList = new List<FloorRelationUIObject>();

        public enum ModifyType
        {
            /// <summary>
            /// 删除
            /// </summary>
            Delete,
            /// <summary>
            /// 新增
            /// </summary>
            Add
        }

        public bool f_QuickSetInfo_FloorTerminalNo = false;
        public bool f_QuickSetInfo_CheckFloor = false;
        public bool f_QuickSetInfo_FloorName = false;
        public bool f_QuickSetInfo_TerminalNumSlave1 = false;
        public bool f_QuickSetInfo_TerminalNumSlave2 = false;

        public ResponseInfo UserControl_FloorTableChangeNotify
        {
            get;
            set;
        }

        public CloudElevatorUserControl()
        {
            InitializeComponent();
        }

        protected override void InitUIOnLoad()
        {
            base.InitUIOnLoad();

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

            this.DevType = DeviceType.CloudElevator;
            this.edtLinkagePort.Text = "60202"; //默认值
            f_IsRefresh = true;
            this.rgpDHCP.SelectedIndex = 1;
            f_IsRefresh = false;

            //加载楼层对应表
            this.InitGrid();
            this.InitColumnEdit();
            FindCount = new Dictionary<string, hintInfo>();
            f_LocalDBOperate = new LocalDBOperate();
            this.ShowNewBenchmarkUI(false);
            HintProvider.WaitingDone(Application.ProductName);

            //初始时第一第二副操纵盘不可见
            this.chkTerminalNumSlave1.Checked = false;
            this.chkTerminalNumSlave2.Checked = false;

            grdFloorTable.Bands[grdFloorTable.Columns.IndexOf(grdFloorTable.Columns[nameof(FloorRelationUIObject.TerminalNumSlave1)])].Visible = false;
            grdFloorTable.Bands[grdFloorTable.Columns.IndexOf(grdFloorTable.Columns[nameof(FloorRelationUIObject.TerminalNumSlave2)])].Visible = false;
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

            this.rgpIsLink.SelectedIndexChanged += this.rgpIsLink_SelectedIndexChanged;

            this.edtNewBenchMarkIP.Leave += this.edtDevIp_Leave;
            this.edtNewBenchMarkPort.KeyUp += this.edtNewBenchMarkPort_KeyUp;

            this.Btn_BatchSet.Click += this.Btn_BatchSet_Click;

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
            f_DevicesDataTable.Columns.Add(STATUS, typeof(string));//110009
            f_DevicesDataTable.Columns.Add("linkAgeIP", typeof(string));

            f_DevicesDataTable.Columns.Add(NEWBENCHMARK_IP, typeof(string));
            f_DevicesDataTable.Columns.Add(NEWBENCHMARK_PORT, typeof(string));
            f_DevicesDataTable.Columns.Add(HAS_NEWBENCHMARK, typeof(bool));
            f_DevicesDataTable.Columns.Add(IS_SELECT, typeof(bool));


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
            //联动控制器端口
            this.AddOneGridViewColumn(grdView, LINKAGE_PORT, LINKAGE_PORT_ALIAS, 120);
            //项目编号
            this.AddOneGridViewColumn(grdView, PROJECT_NO, PROJECT_NO_ALIAS, 100);
            //设备状态  110009
            this.AddOneGridViewColumn(grdView, STATUS, DEV_STATUS, 100);
            //版本号
            this.AddOneGridViewColumn(grdView, VERSION, VERSION_ALIAS, 150);
            //线下物联平台服务器Ip
            this.AddOneGridViewColumn(grdView, "linkAgeIP", "云群控器IP", 150);
            //新基点IP
            this.AddOneGridViewColumn(grdView, NEWBENCHMARK_IP, NEWBENCHMARK_IP_ALIAS, 140);
            //新基点端口
            this.AddOneGridViewColumn(grdView, NEWBENCHMARK_PORT, NEWBENCHMARK_PORT_ALIAS, 100);

            this.gcDevices.DataSource = f_DevicesDataTable;
            ControlUtilityTool.AdjustIndicatorWidth(grdView);
        }

        //初始化表格
        public void InitGrid()
        {
            // bandedGridView1，注意这里声明的是：BandedGridView
            BandedGridView view = grdFloorTable as BandedGridView;

            ControlUtilityTool.SetITLGridViewStyle(this.grdFloorTable, true, GetEmptyDisplayInfo: () => { return "无数据"; }, aExtendedLastColumn: false);
            //允许自动列宽，需在band之前执行否则无效
            view.OptionsBehavior.Editable = true; //可编辑
            view.OptionsView.ColumnAutoWidth = false;
            view.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;  //设置焦点风格
            view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;  //底部区域
            view.OptionsView.HeaderFilterButtonShowMode = FilterButtonShowMode.Button;  //列标题排序按钮样式
            view.OptionsView.ColumnHeaderAutoHeight = DefaultBoolean.True;  //列标题自适应高度，设置后无行指示器
            view.OptionsSelection.EnableAppearanceFocusedCell = false;
            view.OptionsCustomization.AllowColumnMoving = false;
            view.OptionsCustomization.AllowColumnResizing = false;
            view.OptionsCustomization.AllowBandMoving = false;
            view.OptionsCustomization.AllowBandResizing = false;
            gridControl_FloorTable.ShowOnlyPredefinedDetails = true;//消除[+]号
            //因为有band列了，隐藏列标题  
            view.OptionsView.ShowColumnHeaders = false;
            view.OptionsView.ShowIndicator = false;
            view.BandPanelRowHeight = 25;
            view.RowHeight = 25;

            view.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;

            view.Columns.Clear();
            view.Bands.Clear();
            //开始视图的编辑  
            view.BeginUpdate(); //开始视图的编辑，防止触发其他事件
            view.BeginDataUpdate(); //开始数据的编辑

            view.Bands.Clear();
            view.Columns.Clear();

            view.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click; //单击整行选择

            GridBand bandFloor = grdFloorTable.Bands.AddBand("权限标识");
            GridBand bandCtrlFloorName = grdFloorTable.Bands.AddBand("按键名称");
            GridBand bandCtrlFloorReal = grdFloorTable.Bands.AddBand("实际楼层");
            GridBand bandCtrlCheckFloor = grdFloorTable.Bands.AddBand("检测楼层");
            GridBand bandCtrlTerminalNo = grdFloorTable.Bands.AddBand("端子号");
            GridBand bandTerminalNumSlave1 = grdFloorTable.Bands.AddBand("第一副操纵盘");
            GridBand bandTerminalNumSlave2 = grdFloorTable.Bands.AddBand("第二副操纵盘");
            GridBand bandIntercomTerminalNo = grdFloorTable.Bands.AddBand("对讲端子号");

            //添加Column
            //权限标识
            var colFlag = this.AddBandedGridColumn(grdFloorTable, nameof(FloorRelationUIObject.FloorNo), bandFloor, false);
            colFlag.Width = 120;
            colFlag.OptionsColumn.FixedWidth = true;

            //楼层名称
            var colFloor = this.AddBandedGridColumn(grdFloorTable, nameof(FloorRelationUIObject.FloorName), bandCtrlFloorName, true);
            //端子号
            var colTerminalNo = this.AddBandedGridColumn(grdFloorTable, nameof(FloorRelationUIObject.FloorTerminalNo), bandCtrlTerminalNo, true);
            //真实楼层
            var colFloorReal = this.AddBandedGridColumn(grdFloorTable, nameof(FloorRelationUIObject.FloorReal), bandCtrlFloorReal, true);
            var colCheckFloor = this.AddBandedGridColumn(grdFloorTable, nameof(FloorRelationUIObject.CheckFloor), bandCtrlCheckFloor, true);
            //副操纵盘端子号
            var colTerminalNumSlave1 = this.AddBandedGridColumn(grdFloorTable, nameof(FloorRelationUIObject.TerminalNumSlave1), bandTerminalNumSlave1, true);
            colTerminalNumSlave1.Width = 120;
            //第二副操纵盘
            var colTerminalNumSlave2 = this.AddBandedGridColumn(grdFloorTable, nameof(FloorRelationUIObject.TerminalNumSlave2), bandTerminalNumSlave2, true);
            colTerminalNumSlave2.Width = 140;
            //对讲楼层端子号
            var colIntercomTerminal = this.AddBandedGridColumn(grdFloorTable, nameof(FloorRelationUIObject.TerminalNumIntercom), bandIntercomTerminalNo, true);
            colIntercomTerminal.Width = 120;
            colIntercomTerminal.OptionsColumn.FixedWidth = true;

            foreach (GridBand band in view.Bands)
            {
                band.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                if (band.HasChildren)
                {
                    foreach (GridBand child in band.Children)
                    {
                        child.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    }
                }
            }

            foreach (BandedGridColumn item in view.Columns)
            {
                item.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }

            view.EndDataUpdate();//结束数据的编辑
            view.EndUpdate();   //结束视图的编辑

            //绑定数据源并显示，初始化值
            gridControl_FloorTable.DataSource = GetFloorRelationUIObjectListByDevice();

        }

        public static explicit operator CloudElevatorUserControl(Form v)
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
        private void AddOneDeviceToUI(CloudElevatorInfo deviceInfo)
        {

            DataRow[] rows = f_DevicesDataTable.Select(string.Format("{0}={1}", DEV_ID, deviceInfo.DevId));
            FindCount.AddToUpdate("" + deviceInfo.DevId);
            FindCount.AddToUpdate("" + deviceInfo.DevIp);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                try
                {
                    int FindSumCount = AppEnv.Singleton.UdpCount * AppEnv.Singleton.UdpCount;

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
                    rows[0][STATUS] = deviceInfo.DevStatus;//110009
                    rows[0]["linkAgeIP"] = deviceInfo.linkAgeIP;
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
                    deviceInfo.DevStatus,//110009
                    deviceInfo.linkAgeIP,
                    deviceInfo.NewBenchmarkIP,
                    deviceInfo.NewBenchmarkPort,
                    deviceInfo.HasNewBenchmark,
                    false
                );

            }
            //Repeat_Dev_Id.Add(deviceInfo.DevId);

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
            DeviceType devType = CommandProcesserHelper.GetDevTypeByCmdInfo(StrUtils.CopySubStr(strCmdStr, 6, 2));
            if (devType != DeviceType.CloudElevator)
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
        }
        /// <summary>
        /// 解析UDP搜索返回的数据
        /// </summary>
        /// <param name="strCmdStr"></param>
        private void AnalysisSearchDevicesRecvData(string strCmdStr)
        {
            //取返回的数据部分
            string strCmdReport = StrUtils.CopySubStr(strCmdStr, 18, strCmdStr.Length - 22);
            CloudElevatorInfo deviceInfo = new CloudElevatorInfo();

            //设备ID
            deviceInfo.DevId = CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4));
            //Mac地址
            deviceInfo.DevMac = CommonUtils.GetMacByHex(StrUtils.CopySubStr(strCmdReport, 2, 12));
            //DHCP功能
            deviceInfo.DHCPEnable = StrUtils.CopySubStr(strCmdReport, 14, 2) == "01";
            //设备IP
            deviceInfo.DevIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 16, 8));
            //子网掩码
            deviceInfo.SubnetMask = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 24, 8));
            //网关
            deviceInfo.GateWay = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 32, 8));
            //DNS服务器
            deviceInfo.DNSServerIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 40, 8));
            //线下物联平台服务器
            deviceInfo.EIServerIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 48, 8));
            //线下物联平台端口号
            deviceInfo.EIServerPort = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 56, 4), 0, 16);
            //联动器端口号
            deviceInfo.LinkagePort = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 60, 4), 0, 16);
            //项目编号 以十进制传输
            deviceInfo.ProjectNo = StrUtils.ComplementedStr(StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 64, 8), 0, 10).ToString(), 8, "0");


            //设备状态  110009
            deviceInfo.DevStatus = StrUtils.CopySubStr(strCmdReport, 72, 8);

            if (strCmdReport.Length > 98)
            {
                //版本号
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
                //82+26 长度IP
                if (strCmdReport.Length > 82 + f_VersionLength)
                {
                    int indexLinkAgeIP = 82 + f_VersionLength;
                    deviceInfo.linkAgeIP = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, indexLinkAgeIP, 8));
                    string strIp = StrUtils.CopySubStr(strCmdReport, 82 + f_VersionLength, 8);

                    //韦将杰 2020-04-26 增加新基点IP和接口解析

                    //华为新基点属性报文起始位置为主群控器IP起始位+8
                    int indexNewBenchmar = indexLinkAgeIP + 8;
                    //有华为新基点属性的报文长度
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

            try
            {

                //判断设备Id是否正确
                int devId = CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4));
                if (devId != f_CurrentElevatorInfo.DevId)
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                RunLog.Log("AnalysisDownParams Error :" + strCmdStr);
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

        /// <summary>
        /// 下载参数
        /// </summary>
        protected override void DownParams()
        {
            CloudElevatorInfo elevatorInfo = GetDeviceInfoBeforeDown();
            if (elevatorInfo == null)
            {
                return;
            }
            switch (this.tcDownParams.SelectedTabPageIndex)
            {
                case 0:
                    {
                        if (!R_Btn_Click)
                        {
                            this.DownBasicParams(elevatorInfo);
                        }
                    }
                    break;
                default:
                    break;
            }

            if (W_Btn_Click == true)
            {
                this.DownFloorTables(elevatorInfo); //下载楼层对应表
                W_Btn_Click = false;
            }
            else if (R_Btn_Click == true)
            {
                this.ReadFloorTables(elevatorInfo);//读取楼层对应表
                R_Btn_Click = false;
            }

            if ((!IsCloudDownFloor) && (W_Btn_Click == false) && (R_Btn_Click == false))
            {
                if ((!string.IsNullOrEmpty(f_StratAuthFlag)) && (!string.IsNullOrEmpty(f_EndAuthFlag)) && (!string.IsNullOrEmpty(f_StartDevNo)))
                {
                    this.QuickSetInLocal(elevatorInfo);
                    f_StratAuthFlag = string.Empty;
                    f_EndAuthFlag = string.Empty;
                    f_StartDevNo = string.Empty;
                }
            }

            //发送报文成功开始计时
            this.BeginTick = Environment.TickCount;
            this.CoolingTick = 600;
            this.IsBusy = true;
            this.tmrCommunication.Start();
        }

        protected void DownParamsV2(CloudElevatorInfo cloudElevatorInfo)
        {
            CloudElevatorInfo elevatorInfo = GetDeviceInfoBeforeDownV2(cloudElevatorInfo);
            if (elevatorInfo == null)
            {
                return;
            }
            switch (this.tcDownParams.SelectedTabPageIndex)
            {
                case 0:
                    {
                        if (!R_Btn_Click)
                        {
                            this.DownBasicParams(elevatorInfo);
                        }
                    }
                    break;
                default:
                    break;
            }

            if (W_Btn_Click == true)
            {
                this.DownFloorTables(elevatorInfo); //下载楼层对应表
                W_Btn_Click = false;
            }
            else if (R_Btn_Click == true)
            {
                this.ReadFloorTables(elevatorInfo);//读取楼层对应表
                R_Btn_Click = false;
            }

            if ((!IsCloudDownFloor) && (W_Btn_Click == false) && (R_Btn_Click == false))
            {
                if ((!string.IsNullOrEmpty(f_StratAuthFlag)) && (!string.IsNullOrEmpty(f_EndAuthFlag)) && (!string.IsNullOrEmpty(f_StartDevNo)))
                {
                    this.QuickSetInLocal(elevatorInfo);
                    f_StratAuthFlag = string.Empty;
                    f_EndAuthFlag = string.Empty;
                    f_StartDevNo = string.Empty;
                }
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
        private void DownBasicParams(CloudElevatorInfo elevatorInfo)
        {
            string strWriteCmd = this.GetBasicParamsWriteCmdStr(elevatorInfo);
            //判断长度
            if (strWriteCmd.Length < 52 + 8) //26字节 +4字节
            {
                HintProvider.ShowAutoCloseDialog(null, "生成的报文长度错误，请检查设置的参数是否正确");
                return;
            }
            string writeReport = this.GetWriteReport(elevatorInfo.DevId, AppConst.CMD_WORD_SET_CLOUD_ELEVATOR_PARAMS, strWriteCmd);
            string errMsg = string.Empty;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
            if (!this.UdpListener.SendData(endpoint, writeReport, ref errMsg))
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("设置参数失败，错误：{0}", errMsg));
            }
            f_CurrentElevatorInfo = elevatorInfo;
        }

        /// <summary>
        /// 获取写指令，返回要向设备写入数据的指令 
        /// 韦将杰
        /// 2020.02.25注释
        /// </summary>
        /// <param name="CloudLinkageInfo"></param>
        /// <returns></returns>
        private string GetBasicParamsWriteCmdStr(CloudElevatorInfo deviceInfo)
        {
            //协议版本，目前固定为02
            string strCmdStr = "02";
            //DHCP标志
            strCmdStr += deviceInfo.DHCPEnable ? "01" : "00";
            //设备Ip
            strCmdStr += deviceInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(deviceInfo.DevIp);
            //子网掩码
            strCmdStr += deviceInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(deviceInfo.SubnetMask);
            //网关
            strCmdStr += deviceInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(deviceInfo.GateWay);
            //DNS服务器
            strCmdStr += deviceInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(deviceInfo.DNSServerIp);
            //线下物联平台服务器地址
            strCmdStr += CommonUtils.GetHexByIP(deviceInfo.EIServerIp);
            //线下物联平台服务器端口
            strCmdStr += StrUtils.IntToHex(deviceInfo.EIServerPort, 4);
            //联动器端口
            strCmdStr += StrUtils.IntToHex(deviceInfo.LinkagePort, 4);
            //项目编号           
            strCmdStr += StrUtils.ComplementedStr(deviceInfo.ProjectNo.ToString(), 8, "0");

            //         strCmdStr += StrUtils.ComplementedStr("YDT10110V1060", 8, "0");

            if (deviceInfo.linkAgeIP.Length > 0)
            {
                //群控制器IP            
                strCmdStr += CommonUtils.GetHexByIP(deviceInfo.linkAgeIP);
            }
            else
            {
                strCmdStr += CommonUtils.GetHexByIP("0.0.0.0");
            }

            //如果具有新基点属性，则拼接数据进入命令
            if (f_HasNewbenchmark)
            {
                strCmdStr += CommonUtils.GetHexByIP(deviceInfo.NewBenchmarkIP);
                strCmdStr += StrUtils.IntToHex(StrUtils.StrToIntDef(deviceInfo.NewBenchmarkPort.Trim(), 0), 4);
            }
            string key3 = KeyMacOperate.GetMacEx(strCmdStr);
            //获取mac值
            strCmdStr += KeyMacOperate.GetMacEx(strCmdStr);

            return strCmdStr;
        }


        private string GetHexStrByNo(int tableNo)
        {
            string strTemp = StrUtils.IntToHex(tableNo, 2);
            strTemp = StrUtils.CopySubStr(strTemp, strTemp.Length - 2, 2);
            return strTemp;
        }

        private CloudElevatorInfo GetDeviceInfoBeforeDown()
        {
            if (!this.CheckUIValid())
            {
                return null;
            }
            CloudElevatorInfo deviceInfo = this.GetDeviceInfoByFocusRow();
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
            deviceInfo.linkAgeIP = this.txtLinkageIp.Text.Trim();

            if (f_HasNewbenchmark)
            {
                deviceInfo.NewBenchmarkIP = this.edtNewBenchMarkIP.Text.Trim();
                deviceInfo.NewBenchmarkPort = this.edtNewBenchMarkPort.Text.Trim();
            }
            return deviceInfo;
        }

        private CloudElevatorInfo GetDeviceInfoBeforeDownV2(CloudElevatorInfo deviceInfo)
        {
            if (!this.CheckUIValid())
            {
                return null;
            }
            //CloudElevatorInfo deviceInfo = this.GetDeviceInfoByFocusRow();
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
            deviceInfo.linkAgeIP = this.txtLinkageIp.Text.Trim();

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

            if (this.rgpIsLink.SelectedIndex == 0)
            {

                if (string.IsNullOrWhiteSpace(this.txtLinkageIp.Text.Trim()))
                {
                    HintProvider.ShowAutoCloseDialog(null, "云群控器IP不能为空");
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
        /// 双击事件
        /// </summary>
        protected override void ExcuteDoubleClick()
        {
            f_IsRefresh = true;
            try
            {
                //加载基础信息到界面
                CloudElevatorInfo deviceInfo = this.GetDeviceInfoByFocusRow();

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



                this.txtLinkageIp.Text = deviceInfo.linkAgeIP;

                this.txtLinkageIp.Tag = deviceInfo.linkAgeIP;

                if (deviceInfo.linkAgeIP != "" && deviceInfo.linkAgeIP != "0.0.0.0")
                {
                    rgpIsLink.SelectedIndex = 0;
                    txtLinkageIp.Enabled = true;
                }
                else
                {
                    rgpIsLink.SelectedIndex = 1;
                    txtLinkageIp.Enabled = false;
                }

                this.edtDevIp.Enabled = !deviceInfo.DHCPEnable;
                this.edtSubnetMark.Enabled = !deviceInfo.DHCPEnable;
                this.edtGateWay.Enabled = !deviceInfo.DHCPEnable;
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

                this.rgpDHCP.SelectedIndex = deviceInfo.DHCPEnable ? 0 : 1;

                //this.edtProjectNo.Text = deviceInfo.ProjectNo.ToString();
                if (deviceInfo.DHCPEnable)
                {
                    this.edtDevIp.Tag = deviceInfo.DevIp;
                    this.edtSubnetMark.Tag = deviceInfo.SubnetMask;
                    this.edtGateWay.Tag = deviceInfo.GateWay;
                    this.edtDNSServerIp.Tag = deviceInfo.DNSServerIp;
                    //2021-01-07 韦将杰 少这行代码会导致dhcp选择将项目编号清空为0
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
                //此处写将DeviceTableInfo中对应表信息加载到界面

            }
            finally
            {
                f_IsRefresh = false;
                //读取选中的设备楼层对应表
                R_Btn_Click = true;
                DownParams();
                R_Btn_Click = false;
            }
        }

        private CloudElevatorInfo GetDeviceInfoByFocusRow()
        {
            if (this.gvDevices.FocusedRowHandle < 0)
            {
                return null;
            }
            CloudElevatorInfo deviceInfo = new CloudElevatorInfo()
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
                VerSion = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, VERSION).ToString(),
                linkAgeIP = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, "linkAgeIP").ToString(),
                HasNewBenchmark = (bool)this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, HAS_NEWBENCHMARK)
            };

            this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, HAS_NEWBENCHMARK);
            if (deviceInfo.HasNewBenchmark)
            {
                deviceInfo.NewBenchmarkIP = gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, NEWBENCHMARK_IP).ToString();
                deviceInfo.NewBenchmarkPort = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, NEWBENCHMARK_PORT).ToString();
            }
            return deviceInfo;
        }

        private void rgpIsLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (f_IsRefresh)
            {
                return;
            }
            if (this.rgpIsLink.SelectedIndex == 1)
            {

                this.txtLinkageIp.Tag = this.txtLinkageIp.Text.Trim();
                this.txtLinkageIp.Text = string.Empty;

                this.txtLinkageIp.Enabled = false;

            }
            else
            {

                if (this.txtLinkageIp.Tag is string)
                {
                    this.txtLinkageIp.Text = this.txtLinkageIp.Tag.ToString();
                }

                this.txtLinkageIp.Enabled = true;

            }
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

        private void edtProjectNo_Leave(object sender, EventArgs e)
        {
            edtProjectNo.Text = StrUtils.ComplementedStr(edtProjectNo.Text.Trim(), 8, "0");
        }

        private void edtProjectNo_KeyUp(object sender, KeyEventArgs e)
        {

        }

        //切换TabControl
        private void tcDownParams_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {

            //string TabName = "楼层对应表";
            //string TabBaseInfo = "基本参数";
            foreach (XtraTabPage page in tcDownParams.TabPages)
            {
                if (tcDownParams.SelectedTabPageIndex == 0)
                {
                    //加载按钮 ---- 基本参数
                    Btn_DownLoadParm.Visible = true;
                    Btn_ReadLocalFloorTable.Visible = false;
                    Btn_ReadCloudFloorTable.Visible = false;
                    Btn_DownLoadDev.Visible = false;
                    Btn_SaveSet.Visible = false;
                    Btn_QuickSet.Visible = false;
                    Btn_BatchSet.Visible = false;
                    Btn_Export.Visible = false;
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
                    Btn_BatchSet.Visible = true;
                    Btn_Export.Visible = true;
                }
            }
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
            f_QuickSetInfo_FloorTerminalNo = false;
            f_QuickSetInfo_CheckFloor = false;
            f_QuickSetInfo_FloorName = false;
            f_QuickSetInfo_TerminalNumSlave1 = false;
            f_QuickSetInfo_TerminalNumSlave2 = false;

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

        }

        //事件：保存设置
        private void Btn_SaveSet_Click(object sender, EventArgs e)
        {
            W_Btn_Click = true;
            DownParams();
            W_Btn_Click = false;
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
                f_QuickSetInfo_FloorTerminalNo = Form_CloudFloorTableQuickSet.f_QuickSetInfo_FloorTerminalNo;
                f_QuickSetInfo_CheckFloor = Form_CloudFloorTableQuickSet.f_QuickSetInfo_CheckFloor;
                f_QuickSetInfo_FloorName = Form_CloudFloorTableQuickSet.f_QuickSetInfo_FloorName;
                f_QuickSetInfo_TerminalNumSlave1 = Form_CloudFloorTableQuickSet.f_QuickSetInfo_TerminalNumSlave1;
                f_QuickSetInfo_TerminalNumSlave2 = Form_CloudFloorTableQuickSet.f_QuickSetInfo_TerminalNumSlave2;

                f_QuickForm_edt_KeyNameNo = Form_CloudFloorTableQuickSet.f_edt_KeyNameNo;
                f_QuickForm_cbbE_Start = Form_CloudFloorTableQuickSet.f_QuickSetInfo_StartDevNo;

                this.QuickSetFloorTable();

                if ((!string.IsNullOrEmpty(f_StratAuthFlag)) && (!string.IsNullOrEmpty(f_EndAuthFlag)) && (!string.IsNullOrEmpty(f_StartDevNo)))
                {
                    IsCloudDownFloor = false;
                }
            }
        }

        /// <summary>
        /// 快速设置
        /// </summary>
        private void QuickSetInLocal(CloudElevatorInfo elevatorInfo)
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
                this.FloorTableWriteCmdStr(elevatorInfo, ref strWriteCmd, ref strCmdWord);

            }
            catch (Exception ex)
            {
                RunLog.Log(ex);
            }
        }

        /// <summary>
        /// 下载楼层对应表
        /// </summary>
        /// <param name="elevatorInfo"></param>
        private void DownFloorTables(CloudElevatorInfo elevatorInfo)
        {
            try
            {
                string strCmdWord = string.Empty;
                string strWriteCmd = string.Empty;
                this.FloorTableWriteCmdStr(elevatorInfo, ref strWriteCmd, ref strCmdWord);
                strWriteCmd = strWriteCmd.ToUpper();
                string WriteReport = this.GetWriteReport(elevatorInfo.DevId, strCmdWord, strWriteCmd);//报文

                if (string.IsNullOrEmpty(strWriteCmd))
                {
                    return;
                }

                RunLog.Log("下载楼层对应表报文：" + WriteReport);

                IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
                f_CurrentElevatorInfo = elevatorInfo;
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
                    string IsVersion = StrUtils.CopySubStr(elevatorInfo.VerSion, 9, 4);
                    //低于1060版本 下载不能超过1KB的字节。
                }
            }
            catch (Exception ex)
            {
                RunLog.Log("下载失败" + ex);
                W_Btn_Click = false;
            }
        }


        /// <summary>
        /// 组装下载 写入楼层对应表
        /// </summary>
        /// <param name="elevatorInfo"></param>
        /// <param name="WriteCmd"></param>
        /// <param name="cmdWord"></param>
        private void FloorTableWriteCmdStr(CloudElevatorInfo elevatorInfo, ref string WriteCmd, ref string cmdWord)
        {
            DownTableInfo downTableInfo = this.GetTablesBeforeDownInfo(elevatorInfo.DevId);
            if (downTableInfo == null)
            {
                return;
            }

            string temWriteCmd = this.GetFloorCmdStr();
            if (!string.IsNullOrWhiteSpace(temWriteCmd))
            {
                WriteCmd = temWriteCmd;
            }


            cmdWord = AppConst.CMD_WORD_SET_DOWN_DOWNLOAD_FLOOR_TABLE;
        }

        /// <summary>
        /// 获取楼层对应表下载的命令
        /// 2020-07-27 韦将杰
        /// </summary>
        /// <returns></returns>
        private string GetFloorCmdStr()
        {
            //传出的楼层对应表字符串  
            string strWriteCmd = string.Empty;

            string strAuthFlag = string.Empty;
            string strKeyName = string.Empty;
            string strActualFloor = string.Empty;
            string strDevCheckFloor = string.Empty;
            string strTerminalNum = string.Empty;

            string strTerminalNumSlave1 = string.Empty;
            string strTerminalNumSlave2 = string.Empty;
            string strTerminalNumIntercom = string.Empty;
            string StratAuthFlag = string.Empty;
            string EndAuthFlag = string.Empty;

            for (int c_CompareDevNo = 0; c_CompareDevNo < grdFloorTable.DataRowCount; c_CompareDevNo++)
            {
                //权限标识
                strAuthFlag += SelectView.GetRowCellValue(c_CompareDevNo, nameof(FloorRelationUIObject.FloorNo)).ToString();
                //按键名称
                strKeyName += SelectView.GetRowCellValue(c_CompareDevNo, nameof(FloorRelationUIObject.FloorName)).ToString();
                //实际楼层
                strActualFloor += SelectView.GetRowCellValue(c_CompareDevNo, nameof(FloorRelationUIObject.FloorReal)).ToString();
                //检测楼层
                strDevCheckFloor += SelectView.GetRowCellValue(c_CompareDevNo, nameof(FloorRelationUIObject.CheckFloor)).ToString();
                //端子号
                strTerminalNum += SelectView.GetRowCellValue(c_CompareDevNo, nameof(FloorRelationUIObject.FloorTerminalNo)).ToString();
                //第一副操纵盘
                strTerminalNumSlave1 += SelectView.GetRowCellValue(c_CompareDevNo, nameof(FloorRelationUIObject.TerminalNumSlave1)).ToString();
                //第二副操纵盘
                strTerminalNumSlave2 += SelectView.GetRowCellValue(c_CompareDevNo, nameof(FloorRelationUIObject.TerminalNumSlave2)).ToString();
                //对讲端子号
                strTerminalNumIntercom += SelectView.GetRowCellValue(c_CompareDevNo, nameof(FloorRelationUIObject.TerminalNumIntercom)).ToString();

                strAuthFlag += ",";
                strKeyName += ",";
                strActualFloor += ",";
                strDevCheckFloor += ",";
                strTerminalNum += ",";
                strTerminalNumSlave1 += ",";
                strTerminalNumSlave2 += ",";
                strTerminalNumIntercom += ",";
            }

            //删掉每个结尾的逗号
            strAuthFlag = strAuthFlag.Substring(0, strAuthFlag.Length - 1);
            strKeyName = strKeyName.Substring(0, strKeyName.Length - 1);
            strActualFloor = strActualFloor.Substring(0, strActualFloor.Length - 1);
            strDevCheckFloor = strDevCheckFloor.Substring(0, strDevCheckFloor.Length - 1);
            strTerminalNum = strTerminalNum.Substring(0, strTerminalNum.Length - 1);
            strTerminalNumSlave1 = strTerminalNumSlave1.Substring(0, strTerminalNumSlave1.Length - 1);
            strTerminalNumSlave2 = strTerminalNumSlave2.Substring(0, strTerminalNumSlave2.Length - 1);
            strTerminalNumIntercom = strTerminalNumIntercom.Substring(0, strTerminalNumIntercom.Length - 1);

            object obj = null;
            if (!f_HasTerminalNumSlave1 && !f_HasTerminalNumSlave2)
            {
                obj = new
                {
                    id = strAuthFlag,
                    floorName = strKeyName,
                    actualFloorNum = strActualFloor,
                    detectedFloorNum = strDevCheckFloor,
                    terminalNum = strTerminalNum,
                    terminalNumIntercom = strTerminalNumIntercom,
                };
            }
            else
            if (f_HasTerminalNumSlave1 && !f_HasTerminalNumSlave2)
            {
                obj = new
                {
                    id = strAuthFlag,
                    floorName = strKeyName,
                    actualFloorNum = strActualFloor,
                    detectedFloorNum = strDevCheckFloor,
                    terminalNum = strTerminalNum,
                    terminalNumIntercom = strTerminalNumIntercom,
                    terminalNumSlave1 = strTerminalNumSlave1,
                };
            }
            else
            if (f_HasTerminalNumSlave2)
            {
                obj = new
                {
                    id = strAuthFlag,
                    floorName = strKeyName,
                    actualFloorNum = strActualFloor,
                    detectedFloorNum = strDevCheckFloor,
                    terminalNum = strTerminalNum,
                    terminalNumIntercom = strTerminalNumIntercom,
                    terminalNumSlave1 = strTerminalNumSlave1,
                    terminalNumSlave2 = strTerminalNumSlave2,
                };
            }

            string jsondata = JsonConvert.SerializeObject(obj);
            byte[] bytes = System.Text.Encoding.GetEncoding("utf-8").GetBytes(jsondata);
            string hex = StrUtils.BytesToHexStr(bytes);

            strWriteCmd = hex;

            RunLog.Log("jsondata：   " + jsondata);
            RunLog.Log("WriteCmd：   " + strWriteCmd);

            strWriteCmd = strWriteCmd.Replace(",", " ");

            strWriteCmd = strWriteCmd.ToUpper();
            //删除空格
            strWriteCmd = strWriteCmd.Replace(" ", "");
            //获取mac值
            strWriteCmd += KeyMacOperate.GetMacEx(strWriteCmd);

            return strWriteCmd;
        }

        /// <summary>
        /// 读取楼层对应表
        /// </summary>
        /// <param name="elevatorInfo"></param>
        private void ReadFloorTables(CloudElevatorInfo elevatorInfo)
        {
            try
            {
                string strCmdWord = string.Empty;
                string strReadCmd = string.Empty;
                this.GetFloorTableReadCmdStr(elevatorInfo, ref strReadCmd, ref strCmdWord);
                string ReadReport = this.GetWriteReport(elevatorInfo.DevId, "5A31", "");//报文

                IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
                f_CurrentElevatorInfo = elevatorInfo;
                string errMsg = string.Empty;
                if (!this.UdpListener.SendData(endpoint, ReadReport, ref errMsg))
                {
                    HintProvider.ShowAutoCloseDialog(null, string.Format("读取本地对应表失败，错误：{0}", errMsg));
                }
                else


                {
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

                        if (IsReadSuceess != 1)
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
        /// <param name="elevatorInfo"></param>
        /// <param name="readCmd"></param>
        /// <param name="cmdWord"></param>
        private void GetFloorTableReadCmdStr(CloudElevatorInfo elevatorInfo, ref string readCmd, ref string cmdWord)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        private DownTableInfo GetTablesBeforeDownInfo(int devId)
        {
            DeviceTableInfo deviceTableInfo = new DeviceTableInfo()
            {
                DevId = devId
            };
            deviceTableInfo.InitDeviceTableInfoList();
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
                string strDevID = CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4)).ToString();

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
                //json序列化
                FloorRelationInfo FloorTableSerializer = JsonConvert.DeserializeObject<FloorRelationInfo>(Rcv_FloorTableInfo);

                BandedGridView view = grdFloorTable as BandedGridView;

                RepositoryItemTextEdit SpinEdit_AuthFlag = new RepositoryItemTextEdit();//权限标识

                #region 读取数据到string数组

                //权限标识
                string[] Is_AuthFlag = string.IsNullOrWhiteSpace(FloorTableSerializer.id)
                    ? new string[0] : FloorTableSerializer.id.Trim().Split(',');
                //按键名称
                string[] Is_KeyName = string.IsNullOrWhiteSpace(FloorTableSerializer.floorName)
                    ? new string[0] : FloorTableSerializer.floorName.Trim().Split(',');
                //实际楼层
                string[] Is_ActualFloor = string.IsNullOrWhiteSpace(FloorTableSerializer.actualFloorNum)
                    ? new string[0] : FloorTableSerializer.actualFloorNum.Trim().Split(',');
                //检测楼层
                string[] Is_DevCheckFloor = string.IsNullOrWhiteSpace(FloorTableSerializer.detectedFloorNum)
                    ? new string[0] : FloorTableSerializer.detectedFloorNum.Trim().Split(',');
                //端子号
                string[] Is_TerminalNum = string.IsNullOrWhiteSpace(FloorTableSerializer.terminalNum.ToString())
                    ? new string[0] : FloorTableSerializer.terminalNum.ToString().Trim().Split(',');
                //第一副操纵盘
                string[] Is_TerminalNumSlave1 = string.IsNullOrWhiteSpace(FloorTableSerializer.terminalNumSlave1)
                    ? new string[0] : FloorTableSerializer.terminalNumSlave1.Trim().Split(',');
                //第二副操纵盘
                string[] Is_TerminalNumSlave2 = string.IsNullOrWhiteSpace(FloorTableSerializer.terminalNumSlave2)
                    ? new string[0] : FloorTableSerializer.terminalNumSlave2.Trim().Split(',');
                //对讲端子号
                string[] Is_TerminalNumIntercom = string.IsNullOrWhiteSpace(FloorTableSerializer.terminalNumIntercom)
                    ? new string[0] : FloorTableSerializer.terminalNumIntercom.Trim().Split(',');
                #endregion

                #region 将数据加载到数据表，并重新绑定到gridControl_FloorTable

                List<FloorRelationUIObject> uiObjectList = new List<FloorRelationUIObject>();

                FloorRelationUIObject uiObject = null;

                try
                {
                    for (int i = 0; i < Is_AuthFlag.Length; i++)
                    {
                        uiObject = new FloorRelationUIObject();
                        //设备号
                        uiObject.DeviceId = int.Parse(strDevID);
                        //权限标识
                        uiObject.FloorNo = Convert.ToInt32(Is_AuthFlag[i]);
                        //按键名称
                        uiObject.FloorName = Is_KeyName[i].ToString();
                        //实际楼层
                        uiObject.FloorReal = Is_ActualFloor[i].ToString();
                        //检测楼层
                        uiObject.CheckFloor = Is_DevCheckFloor[i].ToString();
                        //端子号
                        uiObject.FloorTerminalNo = Is_TerminalNum.Length > i ? Is_TerminalNum[i].ToString() : FloorRelationUIObject.UNDEFINE_FLAG;
                        //第一副操纵盘
                        uiObject.TerminalNumSlave1 = Is_TerminalNumSlave1.Length > i ? Is_TerminalNumSlave1[i].ToString() : FloorRelationUIObject.UNDEFINE_FLAG;
                        //第二副操纵盘
                        uiObject.TerminalNumSlave2 = Is_TerminalNumSlave2.Length > i ? Is_TerminalNumSlave2[i].ToString() : FloorRelationUIObject.UNDEFINE_FLAG;
                        //对讲端子号
                        uiObject.TerminalNumIntercom = Is_TerminalNumIntercom.Length > i ? Is_TerminalNumIntercom[i].ToString() : FloorRelationUIObject.UNDEFINE_FLAG;

                        uiObjectList.Add(uiObject);
                        IsReadSucess = Is_ActualFloor[i].ToString();
                    }

                    this.gridControl_FloorTable.DataSource = uiObjectList;
                    if(!floorRelationUIObjectList.Exists(t=>t.DeviceId == uiObjectList.FirstOrDefault()?.DeviceId))
                    {
                        floorRelationUIObjectList.AddRange(uiObjectList);
                    }

                    this.f_HasTerminalNumSlave1 = Is_TerminalNumSlave1.Length == 0 ? false : true;
                    this.f_HasTerminalNumSlave2 = Is_TerminalNumSlave2.Length == 0 ? false : true;

                    this.ShowTerminalNumSlave();

                }
                catch (Exception ex)
                {
                    RunLog.Log(ex);
                    HintProvider.ShowAutoCloseDialog(null, string.Format("读取失败"), HintIconType.Err);
                }

                #endregion

                SpinEdit_AuthFlag.ReadOnly = true;  //设置只读

                SelectView = view;
                //return;
                if (!string.IsNullOrEmpty(IsReadSucess))
                {
                    IsCloudDownFloor = false;
                    IsReadSuceess = 1; //读取成功
                    HintProvider.ShowAutoCloseDialog(null, string.Format("读取成功"));
                    //Todo 禁用按键 
                }
                else
                {
                    IsReadSuceess = 2;
                    HintProvider.ShowAutoCloseDialog(null, string.Format("读取失败"), HintIconType.Err);
                }

                if (IsCloudDownFloor)
                {
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

        public void GetCloudFloorTable(FloorTableInfo data)
        {
            try
            {
                BandedGridView view = grdFloorTable as BandedGridView;

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

                c_TerminalNum = data.terminalFloor.ToString();
                string[] Is_DevNo = c_TerminalNum.Trim().Split(',');
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

                _StrDevNoLength = c_TerminalNum.Trim().Split(','); //获取云端读取数据长度
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

        private void bandedGridView1_Click(object sender, EventArgs e)
        {
            grdFloorTable.ShowEditor();
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

            }
            else
            {
                this.liNewBenchMarkPort.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.liNewBenchMarkIp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void edtNewBenchMarkPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            edtNewBenchMarkPort.ForeColor = Color.FromArgb(32, 31, 53);
            Btn_DownLoadDev.Enabled = true;
            Btn_DownLoadParm.Enabled = true;

            int NewBenchMarkPort = StrUtils.StrToIntDef(this.edtNewBenchMarkPort.Text.Trim(), 0);

            if (NewBenchMarkPort >= 1024)
            {
                edtNewBenchMarkPort.ForeColor = Color.FromArgb(32, 31, 53);
                Btn_DownLoadDev.Enabled = true;
                Btn_DownLoadParm.Enabled = true;
                return;
            }

            if ((NewBenchMarkPort >= 0) && (NewBenchMarkPort < 1024))
            {
                edtNewBenchMarkPort.ForeColor = Color.Red;
                edtNewBenchMarkPort.ForeColor = Color.Red;
                Btn_DownLoadDev.Enabled = false;
                Btn_DownLoadParm.Enabled = false;
            }

            if ((NewBenchMarkPort >= 1000) && (NewBenchMarkPort < 1024))
            {
                HintProvider.ShowAutoCloseDialog(null, "端口号不能小于1024", HintIconType.Warning, 1000);
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
        /// <summary>
        /// 初始化楼层对应表数据表 字段绑定事件
        /// </summary>
        private void InitColumnEdit()
        {

            //端子号更新绑定
            grdFloorTable.Columns[nameof(FloorRelationUIObject.FloorTerminalNo)].ColumnEdit = this.GetRepositoryItemComboBox();
            grdFloorTable.Columns[nameof(FloorRelationUIObject.TerminalNumIntercom)].ColumnEdit = this.GetRepositoryItemComboBox();
            grdFloorTable.Columns[nameof(FloorRelationUIObject.FloorReal)].ColumnEdit = this.GetRepositoryItemComboBox();
            grdFloorTable.Columns[nameof(FloorRelationUIObject.CheckFloor)].ColumnEdit = this.GetRepositoryItemComboBox();
            //副操纵盘端子号
            grdFloorTable.Columns[nameof(FloorRelationUIObject.TerminalNumSlave1)].ColumnEdit = this.GetRepositoryItemComboBox();
            //残疾人操纵盘端子号
            grdFloorTable.Columns[nameof(FloorRelationUIObject.TerminalNumSlave2)].ColumnEdit = this.GetRepositoryItemComboBox();

            grdFloorTable.Columns[nameof(FloorRelationUIObject.FloorName)].ColumnEdit = GetRepositoryItemTextEdit();
            grdFloorTable.ValidatingEditor += GrdFloorTable_ValidateTerminalNoEditor;
            grdFloorTable.ValidatingEditor += GrdFloorTable_ValidateFloorEditor;
        }

        /// <summary>
        ///绑定下拉框事件并在下拉框里面增加数据
        /// </summary>
        /// <returns></returns>
        private RepositoryItemComboBox GetRepositoryItemComboBox()
        {
            RepositoryItemComboBox cbx = new RepositoryItemComboBox();
            cbx.AllowFocused = false;
            cbx.TextEditStyle = TextEditStyles.DisableTextEditor;
            cbx.Items.Clear();
            cbx.Items.Add(FloorRelationUIObject.UNDEFINE_FLAG); //端子特殊处理
            for (int i = 1; i <= 112; i++)
            {
                cbx.Items.Add(i);
            }

            cbx.EditValueChanged += (sender, e) =>
            {
                grdFloorTable.PostEditor();
            };
            cbx.QueryPopUp += (sender, e) =>
            {
                cbx.Items.Clear();
                cbx.Items.Add(FloorRelationUIObject.UNDEFINE_FLAG); //端子特殊处理
                for (int i = 1; i <= 112; i++)
                {
                    cbx.Items.Add(i.ToString());
                }
            };

            return cbx;
        }

        private RepositoryItemTextEdit GetRepositoryItemTextEdit()
        {
            RepositoryItemTextEdit edt = new RepositoryItemTextEdit();
            edt.ContextMenu = new ContextMenu();
            edt.MaxLength = 3;
            ControlUtilityTool.SetNameInputEditRegEx(edt);
            edt.Validating += (sender, e) =>
            {
                TextEdit txt = sender as TextEdit;
                if (string.IsNullOrWhiteSpace(txt?.Text))
                {
                    txt.Undo();
                }
                else
                {
                    txt.Text = txt.Text.Trim();
                }
            };
            edt.Enter += (sender, e) =>
            {
                (sender as TextEdit)?.SelectAll();
            };
            return edt;
        }

        /// <summary>
        /// 添加BandedGridView的列
        /// </summary>
        /// <param name="view"></param>
        /// <param name="field"></param>
        /// <param name="ownerBand"></param>
        /// <param name="editable"></param>
        /// <returns></returns>
        private BandedGridColumn AddBandedGridColumn(BandedGridView view, string field, GridBand ownerBand, bool editable)
        {
            BandedGridColumn col = view.Columns.AddField(field);
            col.OwnerBand = ownerBand;
            col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;//不可排序
            col.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            col.OptionsColumn.AllowEdit = editable;
            col.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowOnlyInEditor;
            col.Visible = true;
            col.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            //视图中添加此列  
            view.Columns.Add(col);
            return col;
        }

        /// <summary>
        /// 获取某个电梯的FloorRelationUIObjectList以绑定到Grid
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private List<FloorRelationUIObject> GetFloorRelationUIObjectListByDevice()
        {
            List<FloorRelationUIObject> list = new List<FloorRelationUIObject>();

            for (int i = 1; i <= 112; i++)
            {
                FloorRelationUIObject rel = new FloorRelationUIObject() //20190318
                {
                    FloorNo = i,
                    FloorName = i.ToString(),
                    IntercomFloorName = i.ToString(),
                    FloorReal = FloorRelationUIObject.convertFloorParamToUI(i),
                    CheckFloor = FloorRelationUIObject.convertFloorParamToUI(i),

                    FloorTerminalNo = FloorRelationUIObject.UNDEFINE_FLAG,
                    TerminalNumSlave1 = FloorRelationUIObject.UNDEFINE_FLAG,
                    TerminalNumSlave2 = FloorRelationUIObject.UNDEFINE_FLAG,
                    TerminalNumIntercom = FloorRelationUIObject.UNDEFINE_FLAG,
                };
                list.Add(rel);
            }
            return list;

        }

        /// <summary>
        /// 楼层有效性检测，真实楼层、检测楼层，不能重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdFloorTable_ValidateFloorEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            var col = grdFloorTable.FocusedColumn;

            string[] listFiledName = {
                nameof(FloorRelationUIObject.FloorReal),
                nameof(FloorRelationUIObject.CheckFloor),
                nameof(FloorRelationUIObject.TerminalNumIntercom)
            };

            if (!(listFiledName.Contains(col.FieldName)))
            {
                return;
            }

            var comb = grdFloorTable.ActiveEditor as ComboBoxEdit;
            var oldValue = comb.OldEditValue;
            var value = FloorRelationUIObject.convertUIToFloorParam(e.Value.ToString());

            if (value.ToString() == FloorRelationUIObject.UNDEFINE_FLAG)
            {
                //“-”字符串不做校验
                return;
            }

            var list = gridControl_FloorTable.DataSource as List<FloorRelationUIObject>;
            FloorRelationUIObject relUIExchange = null;

            if (col.FieldName == nameof(FloorRelationUIObject.FloorReal))
            {
                relUIExchange = list.Where(p => compareFloorValue(p.FloorReal, e.Value)).FirstOrDefault();
            }
            else if (col.FieldName == nameof(FloorRelationUIObject.CheckFloor))
            {
                relUIExchange = list.Where(p => compareFloorValue(p.CheckFloor, e.Value)).FirstOrDefault();
            }
            else if (col.FieldName == nameof(FloorRelationUIObject.TerminalNumIntercom))
            {
                relUIExchange = list.Where(p => compareFloorValue(p.TerminalNumIntercom, e.Value)).FirstOrDefault();
            }

            if (relUIExchange == null)
            {
                e.Valid = true;
                return;
            }

            var tmp = string.Format("与权限标识{0}的数据相同，是否交换？", relUIExchange.FloorNo);
            if (HintProvider.ShowConfirmDialog(null, tmp, buttons: ConfirmFormButtons.OKCancel, defaultButton: ConfirmFormDefaultButton.Cancel) == DialogResult.OK)
            {
                int rowHExchange = list.IndexOf(relUIExchange);
                grdFloorTable.SetRowCellValue(grdFloorTable.GetRowHandle(rowHExchange), col, oldValue);
                e.Valid = true;
            }
            else
            {
                comb.EditValue = oldValue;
                comb.IsModified = false; //只有IsModified=true时，才会触发ValidatingEditor事件
                e.Valid = false;
            }
        }

        /// <summary>
        /// 端口号有效性校验，一个端口号只能绑定一个控制器，且
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdFloorTable_ValidateTerminalNoEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            var col = grdFloorTable.FocusedColumn;

            string[] listFiledName = {nameof(FloorRelationUIObject.FloorTerminalNo),
                //nameof(FloorRelationUIObject.TerminalNumIntercom),
                nameof(FloorRelationUIObject.TerminalNumSlave1),
                nameof(FloorRelationUIObject.TerminalNumSlave2) };

            if (!(listFiledName.Contains(col.FieldName)))
            {
                return;
            }

            var comb = grdFloorTable.ActiveEditor as ComboBoxEdit;
            var oldValue = comb.OldEditValue;
            var value = FloorRelationUIObject.convertUIToFloorParam(e.Value.ToString());

            var list = gridControl_FloorTable.DataSource as List<FloorRelationUIObject>;

            //端子号都不能重复，行列都不重复
            var relUIExchange = list.Where(p => compareFloorValue(p.FloorTerminalNo, e.Value));
            //if (relUIExchange.Count() == 0)
            //{
            //    relUIExchange = list.Where(p => compareFloorValue(p.TerminalNumIntercom, e.Value));
            //};

            if (relUIExchange.Count() == 0)
            {
                relUIExchange = list.Where(p => compareFloorValue(p.TerminalNumSlave1, e.Value));
            };

            if (relUIExchange.Count() == 0)
            {
                relUIExchange = list.Where(p => compareFloorValue(p.TerminalNumSlave2, e.Value));
            };

            if (relUIExchange.Count() == 0)
            {
                e.Valid = true;
                return;
            }

            var rowData = (FloorRelationUIObject)relUIExchange.FirstOrDefault();
            string filedName = GetColNameByRow(rowData, listFiledName, e.Value.ToString());
            int rowHExchange = list.IndexOf(relUIExchange.FirstOrDefault());
            var tmp = string.Format("与权限标识{0}的端口号有重复，是否交换？", relUIExchange.FirstOrDefault().FloorNo);

            if (HintProvider.ShowConfirmDialog(null, tmp, buttons: ConfirmFormButtons.OKCancel, defaultButton: ConfirmFormDefaultButton.Cancel) == DialogResult.OK)
            {
                grdFloorTable.SetRowCellValue(grdFloorTable.GetRowHandle(rowHExchange), filedName, oldValue);
                e.Valid = true;
            }
            else
            {
                comb.EditValue = oldValue;
                comb.IsModified = false; //只有IsModified=true时，才会触发ValidatingEditor事件
                e.Valid = false;
            }
        }

        /// <summary>
        /// 通过某行rowData数据，经过listFiledName过滤，获取与与之有相等值的数据，返回列名
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="listFiledName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetColNameByRow(FloorRelationUIObject rowData, string[] listFiledName, string value)
        {
            PropertyInfo[] ps = rowData.GetType().GetProperties();
            string filedName = string.Empty;
            foreach (PropertyInfo info in ps)
            {
                var valueCompar = info.GetValue(rowData, null);
                if ((valueCompar != null && valueCompar.ToString() == value) &&
                    listFiledName.Contains(info.Name)
                    )
                {
                    filedName = info.Name;
                    return filedName;
                }
            }
            return filedName;
        }

        /// <summary>
        /// 比较楼层相关信息与界面单元格的取值
        /// </summary>
        /// <param name="floorNo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool compareFloorValue(string floorNo, object value)
        {
            if (null == value)
            {
                return false;
            }
            if (FloorRelationUIObject.UNDEFINE_FLAG.Equals(value.ToString()))
            {
                return false;
            }
            return value.ToString().Equals(floorNo.ToString());
        }

        /// <summary>
        ///快速设置楼层对应表，数据加载到数据表 
        /// </summary>
        private void QuickSetFloorTable()
        {

            //快速设置
            string[] Range_DevNo = new string[0];
            string[] Range_DevNoCompare = new string[0];
            string[] QuickSet_KeyName = new string[0];
            List<object> listQuickSet_No = new List<object>();

            //给快速设置的下拉框增加数值
            for (int a = 1; a <= 112; a++)
            {
                listQuickSet_No.Add(a);
            }

            if (!string.IsNullOrEmpty(f_StratAuthFlag))
            {
                int Dev_start = int.Parse(f_StratAuthFlag);
                int Dev_End = int.Parse(f_EndAuthFlag);
                int QuickSetStartNo = int.Parse(f_QuickForm_cbbE_Start); //快速设置开始位置
                int TmpSet = QuickSetStartNo;

                for (; Dev_start <= Dev_End; Dev_start++)
                {
                    string TmpQSetStartNo = TmpSet.ToString();

                    listQuickSet_No.Insert(Dev_start - 1, TmpQSetStartNo);//获取UI端子号

                    if (f_QuickSetInfo_FloorTerminalNo)
                    {
                        //端子号
                        grdFloorTable.SetRowCellValue(Dev_start - 1, nameof(FloorRelationUIObject.FloorTerminalNo), listQuickSet_No[Dev_start - 1]);
                    }
                    if (f_QuickSetInfo_CheckFloor)
                    {
                        //端子号
                        grdFloorTable.SetRowCellValue(Dev_start - 1, nameof(FloorRelationUIObject.CheckFloor), listQuickSet_No[Dev_start - 1]);
                    }
                    if (f_QuickSetInfo_FloorName)
                    {
                        //端子号
                        grdFloorTable.SetRowCellValue(Dev_start - 1, nameof(FloorRelationUIObject.FloorName), listQuickSet_No[Dev_start - 1]);
                    }
                    if (f_QuickSetInfo_TerminalNumSlave1)
                    {
                        //端子号
                        grdFloorTable.SetRowCellValue(Dev_start - 1, nameof(FloorRelationUIObject.TerminalNumSlave1), listQuickSet_No[Dev_start - 1]);
                    }
                    if (f_QuickSetInfo_TerminalNumSlave2)
                    {
                        //端子号
                        grdFloorTable.SetRowCellValue(Dev_start - 1, nameof(FloorRelationUIObject.TerminalNumSlave2), listQuickSet_No[Dev_start - 1]);
                    }

                    TmpSet++;
                }
                if (Dev_End == 112)
                {
                    grdFloorTable.SetRowCellValue(Dev_start, nameof(FloorRelationUIObject.FloorName), listQuickSet_No[Dev_start - 1]);
                    grdFloorTable.SetRowCellValue(Dev_start, nameof(FloorRelationUIObject.CheckFloor), listQuickSet_No[Dev_start - 1]);
                    grdFloorTable.SetRowCellValue(Dev_start, nameof(FloorRelationUIObject.FloorTerminalNo), listQuickSet_No[Dev_start - 1]);
                }

                f_QuickSetInfo_FloorTerminalNo = false;
                f_QuickSetInfo_CheckFloor = false;
                f_QuickSetInfo_FloorName = false;
                f_QuickSetInfo_TerminalNumSlave1 = false;
                f_QuickSetInfo_TerminalNumSlave2 = false;
            }
        }

        private void chkTerminalNumSlave1_CheckedChanged(object sender, EventArgs e)
        {
            var index = grdFloorTable.Columns.IndexOf(grdFloorTable.Columns[nameof(FloorRelationUIObject.TerminalNumSlave1)]);
            if (chkTerminalNumSlave1.Checked)
            {
                grdFloorTable.Bands[index].Visible = true;
                f_HasTerminalNumSlave1 = true;
                gridControl_FloorTable.Width = 690;
            }
            else
            {
                chkTerminalNumSlave2.Checked = false;
                grdFloorTable.Bands[index].Visible = false;
                f_HasTerminalNumSlave1 = false;
                gridControl_FloorTable.Width = 570;
            }
        }

        private void chkTerminalNumSlave2_CheckedChanged(object sender, EventArgs e)
        {
            var index = grdFloorTable.Columns.IndexOf(grdFloorTable.Columns[nameof(FloorRelationUIObject.TerminalNumSlave2)]);
            if (chkTerminalNumSlave2.Checked)
            {
                //第二副操纵盘勾起时必须勾第一副操纵盘
                chkTerminalNumSlave1.Checked = true;
                grdFloorTable.Bands[index].Visible = true;
                f_HasTerminalNumSlave1 = true;
                f_HasTerminalNumSlave2 = true;
                gridControl_FloorTable.Width = 825;
            }
            else
            {
                grdFloorTable.Bands[index].Visible = false;
                f_HasTerminalNumSlave2 = false;
                gridControl_FloorTable.Width = 690;
            }
        }

        /// <summary>
        /// 判断是否展示副操纵盘
        /// </summary>
        private void ShowTerminalNumSlave()
        {
            this.chkTerminalNumSlave1.Checked = this.f_HasTerminalNumSlave1;
            grdFloorTable.Bands[grdFloorTable.Columns.IndexOf(
                grdFloorTable.Columns[nameof(FloorRelationUIObject.TerminalNumSlave1)])].Visible = this.f_HasTerminalNumSlave1;

            this.chkTerminalNumSlave2.Checked = this.f_HasTerminalNumSlave2;
            grdFloorTable.Bands[grdFloorTable.Columns.IndexOf(
                grdFloorTable.Columns[nameof(FloorRelationUIObject.TerminalNumSlave2)])].Visible = this.f_HasTerminalNumSlave2;
        }

        /// <summary>
        /// 新增一行
        /// </summary>
        /// <param name="item"></param>
        private void ModifyDataSource(ModifyType modifyType)
        {
            //获取当前数据源
            var dataSource = (List<FloorRelationUIObject>)gridControl_FloorTable.DataSource;
            //获取当前数据源的行数，+1为新增的行
            int dataCount = dataSource.Count + 1;

            FloorRelationUIObject addItem = new FloorRelationUIObject() //20190318
            {
                FloorNo = dataCount,
                FloorName = dataCount.ToString(),

                FloorReal = FloorRelationUIObject.convertFloorParamToUI(dataCount),
                CheckFloor = FloorRelationUIObject.convertFloorParamToUI(dataCount),

                FloorTerminalNo = FloorRelationUIObject.UNDEFINE_FLAG,
                TerminalNumSlave1 = FloorRelationUIObject.UNDEFINE_FLAG,
                TerminalNumSlave2 = FloorRelationUIObject.UNDEFINE_FLAG,
                TerminalNumIntercom = FloorRelationUIObject.UNDEFINE_FLAG,
            };

            if (dataSource == null)
            {
                dataSource = new List<FloorRelationUIObject>();
            }
            //data.Remove(item);

            switch (modifyType)
            {
                case ModifyType.Add:
                    {
                        dataSource.Add(addItem);
                        gridControl_FloorTable.DataSource = dataSource;
                        break;
                    }
                case ModifyType.Delete:
                    {
                        //移除行，注意参数是从0开始
                        grdFloorTable.DeleteRow(grdFloorTable.RowCount - 1);
                        break;
                    }
            }

            gridControl_FloorTable.Refresh();
            this.grdFloorTable.FocusedRowHandle = this.grdFloorTable.DataRowCount - 1;
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            this.ModifyDataSource(ModifyType.Add);
        }

        private void btnDelRow_Click(object sender, EventArgs e)
        {
            this.ModifyDataSource(ModifyType.Delete);
        }

        private void Btn_Export_Click(object sender, EventArgs e)
        {
            //HintProvider.ShowAutoCloseDialog(null, "正在导出楼层对应表！");
            //读取楼层对应表
            DownFloorTableBySelectDev();
            Thread.Sleep(1000);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Microsoft Excel files(*.xls)|*.xls;*.xlsx|所有文件|*.*";//设置文件类型
            sfd.FileName = "楼层对应表";//设置默认文件名
            sfd.DefaultExt = "xls";//设置默认格式（可以不设）
            sfd.AddExtension = true;//设置自动在文件名中添加扩展名
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if(this.floorRelationUIObjectList.Count == 0)
                {
                    Thread.Sleep(2000);
                }
                ExportHelperV2.OutFileToDisk(this.floorRelationUIObjectList, sfd.FileName);
            }
        }

        private void Btn_BatchSet_Click(object sender, EventArgs e)
        {
            DownParm = true;
            BatchSetFloorTable();
        }

        /// <summary>
        /// 批量设置楼层对应表
        /// </summary>
        private void BatchSetFloorTable()
        {

            List<int> devIdList = f_DevicesDataTable.AsEnumerable().
                Where(t => t.Field<bool>(IS_SELECT)).
                Select(t => t.Field<int>(DEV_ID)).ToList();
            //如果选中行数为0，直接退出
            if (devIdList.Count < 1)
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("请勾选设备！"));
                return;
            }

            string strWriteCmd = string.Empty;
            strWriteCmd = this.GetFloorCmdStr().ToUpper();
            if (string.IsNullOrWhiteSpace(strWriteCmd))
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("读取楼层对应表数据失败！"));
                return;
            }

            foreach (var devID in devIdList)
            {
                try
                {
                    string WriteReport = this.GetWriteReport(devID, "5A30", strWriteCmd);//报文
                    if (string.IsNullOrEmpty(strWriteCmd))
                    {
                        return;
                    }

                    RunLog.Log("批量下载楼层对应表报文：" + WriteReport);

                    IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));

                    string errMsg = string.Empty;
                    if (!this.UdpListener.SendData(endpoint, WriteReport, ref errMsg))
                    {
                        HintProvider.ShowAutoCloseDialog(null, string.Format("下载对应表失败，错误：{0}", errMsg));
                    };

                    //发送报文成功开始计时
                    this.BeginTick = Environment.TickCount;
                    this.CoolingTick = 600;
                    this.IsBusy = true;
                    this.tmrCommunication.Start();
                }

                catch (Exception ex)
                {
                    RunLog.Log("下载失败" + ex);
                }
            }
        }

        private List<CloudElevatorInfo> GetDeviceInfoBySelectData()
        {
            List<CloudElevatorInfo> cloudElevatorInfoList = new List<CloudElevatorInfo>();
            var selectData = f_DevicesDataTable.AsEnumerable().Where(t => t.Field<bool>(IS_SELECT));
            foreach (var item in selectData)
            {
                CloudElevatorInfo deviceInfo = new CloudElevatorInfo()
                {
                    DevId = StrUtils.StrToIntDef(item[DEV_ID].ToString(), 0),
                    DevMac = item[DEV_MAC].ToString(),
                    DevIp = item[DEV_IP].ToString(),
                    SubnetMask = item[SUBNET_MARK].ToString(),
                    GateWay = item[GATE_WAY].ToString(),
                    DHCPEnable = item[DHCP_FUNCTION].ToString() == "启用",
                    DNSServerIp = item[DNS_SERVER_IP].ToString(),
                    EIServerIp = item[EI_SERVER_IP].ToString(),
                    EIServerPort = StrUtils.StrToIntDef(item[EI_SERVER_PORT].ToString(), 0),
                    LinkagePort = StrUtils.StrToIntDef(item[LINKAGE_PORT].ToString(), 0),
                    ProjectNo = item[PROJECT_NO].ToString(),
                    VerSion = item[VERSION].ToString(),
                    linkAgeIP = item["linkAgeIP"].ToString(),
                    HasNewBenchmark = (bool)item[HAS_NEWBENCHMARK]
                };
                if (deviceInfo.HasNewBenchmark)
                {
                    deviceInfo.NewBenchmarkIP = item[NEWBENCHMARK_IP].ToString();
                    deviceInfo.NewBenchmarkPort = item[NEWBENCHMARK_PORT].ToString();
                }
                cloudElevatorInfoList.Add(deviceInfo);
            }
            return cloudElevatorInfoList;
        }
        private void DownFloorTableBySelectDev()
        {
            floorRelationUIObjectList.Clear();
            f_IsRefresh = true;
            //加载基础信息到界面
            var cloudElevatorInfoList = this.GetDeviceInfoBySelectData();

            if (cloudElevatorInfoList == null || cloudElevatorInfoList.Count == 0)
            {
                return;
            }
            foreach (var deviceInfo in cloudElevatorInfoList)
            {
                try
                {
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


                    this.txtLinkageIp.Text = deviceInfo.linkAgeIP;

                    this.txtLinkageIp.Tag = deviceInfo.linkAgeIP;

                    if (deviceInfo.linkAgeIP != "" && deviceInfo.linkAgeIP != "0.0.0.0")
                    {
                        rgpIsLink.SelectedIndex = 0;
                        txtLinkageIp.Enabled = true;
                    }
                    else
                    {
                        rgpIsLink.SelectedIndex = 1;
                        txtLinkageIp.Enabled = false;
                    }

                    this.edtDevIp.Enabled = !deviceInfo.DHCPEnable;
                    this.edtSubnetMark.Enabled = !deviceInfo.DHCPEnable;
                    this.edtGateWay.Enabled = !deviceInfo.DHCPEnable;
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
                }
                finally
                {
                    f_IsRefresh = false;
                    //读取选中的设备楼层对应表
                    R_Btn_Click = true;
                    DownParamsV2(deviceInfo);
                    R_Btn_Click = false;
                }
                //此处写将DeviceTableInfo中对应表信息加载到界面
            }
        }
    }
}
