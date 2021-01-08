using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using ITL.Public;
using ITL.DataDefine;
using ITL.Framework;
using System.Net;

using DevExpress.XtraTab;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Repository;
using Newtonsoft.Json;

using System.Threading.Tasks;
using ITL.General;

namespace ITL.ParamsSettingTool.ParamsSettingTool.Devices.GroupLinkageCtrl
{
    public delegate void GetCloudFloorTable(FloorTableInfo data);

    public partial class GroupLinkageCtrlUserControl : GeneralDeviceUserControl
    {
        public GroupLinkageCtrlUserControl()
        {
            InitializeComponent();
        }

        private const string DNS_SERVER_IP = "dns_server_ip";  //DNS服务器IP
        private const string DHCP_FUNCTION = "dhcp_function"; //DHCP功能
        private const string EI_SERVER_IP = "ei_server_ip";  //线下物联平台服务器IP
        private const string EI_SERVER_PORT = "ei_server_port";  //线下物联平台服务器端口
        private const string LINKAGE_PORT = "linkage_port"; //联动控制器端口
        private const string PROJECT_NO = "Project_No";//项目编号
        private const string VERSION = "VerSion";//版本号

        //韦将杰 2019.09.24 新增 群控器设备类型，分为普通群控器与云群控器两种
        /// <summary>
        /// 群控器设备类型，分为普通群控器与云群控器两种
        /// </summary>
        private const string GROUP_DEV_TYPE = "GroupDevType";//群控器设备类型
        //韦将杰 2019.09.25 新增 是否为普通群控器
        /// <summary>
        /// 是否为普通群控器
        /// </summary>
        private const string IS_COMM_DEV = "IsCommGroupDev";
        /// <summary>
        ///  主群控器IP
        /// </summary>
        private const string MAIN_GROUP_DEV_IP = "MainGroupDevIP";//群控器设备类型
        /// <summary>
        /// 云电梯数量
        /// </summary>
        private const string CLOUD_ELEVATOR_COUNT = "cloud_elevator_count";  
        /// <summary>
        /// 云电梯ItemIP
        /// </summary>
        private const string CLOUD_ELEVATOR_ITEM_IP = "cloud_elevator_item_ip_{0}";
        /// <summary>
        /// 云电梯Item连接状态
        /// </summary>
        private const string CLOUD_ELEVATOR_ITEM_CON_STATUES = "cloud_elevator_item_con_statues_{0}";
        /// <summary>
        /// 云电梯ItemProp
        /// </summary>
        private const string CLOUD_ELEVATOR_ITEM_PROP = "cloud_elevator_item_prop_{0}";
        /// <summary>
        /// 云群控器数量
        /// </summary>
        private const string CLOUD_GROUP_LINKAGE_COUNT = "cloud_group_linkage_count";
        /// <summary>
        /// 云群控器Item连接状态
        /// </summary>
        private const string CLOUD_GROUP_LINKAGE_ITEM_CON_STATUES = "cloud_group_linkage_item_con_statues_{0}";
        /// <summary>
        /// 云群控器ItemIP
        /// </summary>
        private const string CLOUD_GROUP_LINKAGE_ITEM_IP = "cloud_group_linkage_item_ip_{0}";
        /// <summary>
        /// 云群控器ItemProp
        /// </summary>
        private const string CLOUD_GROUP_LINKAGE_ITEM_PROP = "cloud_group_linkage_item_prop_{0}";


        private const string DNS_SERVER_IP_ALIAS = "DNS服务器IP";  //DNS服务器IP
        private const string DHCP_FUNCTION_ALIAS = "DHCP功能"; //DHCP功能
        private const string EI_SERVER_IP_ALIAS = "线下物联平台服务器IP地址";  //线下物联平台服务器IP
        private const string EI_SERVER_PORT_ALIAS = "线下物联平台服务器端口";  //线下物联平台服务器端口
        private const string LINKAGE_PORT_ALIAS = "协议控制器端口"; //联动控制器端口
        private const string PROJECT_NO_ALIAS = "项目编号";
        private const string VERSION_ALIAS = "版本号";
        private const string GROUP_DEV_TYPE_ALIAS = "群控器设备类型";//群控器设备类型

        //韦将杰 2020-04-09 新增
        private const string CLOUD_ELEVATOR_COUNT_ALIAS = "云电梯数量";  //云电梯数量
        private const string CLOUD_ELEVATOR_ITEM_IP_ALIAS = "{0}号云电梯IP地址";  //云电梯ItemIP
        private const string CLOUD_ELEVATOR_ITEM_PROP_ALIAS = "{0}号云电梯门属性"; //云电梯ItemProp

        private const string CLOUD_GROUP_LINKAGE_COUNT_ALIAS = "云群控器数量";  //云电梯数量
        private const string CLOUD_GROUP_LINKAGE_ITEM_IP_ALIAS = "{0}号云群控器IP地址";  //云电梯ItemIP
        private const string CLOUD_GROUP_LINKAGE_ITEM_PROP_ALIAS = "{0}号云群控器门属性"; //云电梯ItemProp

        private const string COMMON_GROUP_DEV = "DT0310F";   //普通群控器的标志，通过判断版本号的前7个字符判断，如有变动改这里。

        private GroupCloudLinkage f_CurrentDevInfo = null;  //当前操作的设备信息
        private LocalDBOperate f_LocalDBOperate = null; //数据库操作类
        private bool f_IsRefresh = false;

        private string c_AuthFlag = string.Empty; //权限标识
        private string c_KeyName = string.Empty;//按键名称
        private string c_ActualFloor = string.Empty; //实际楼层
        private string c_DevNo = string.Empty; //端子号
        private string c_DevCheckFloor = string.Empty; //检查楼层

        private bool R_Btn_Click;//读楼层对应表
        private bool W_Btn_Click;//写楼层对应表

        private string Select_AuthFlag = string.Empty; //选中的权限标识
        private string Select_KeyName = string.Empty; //选中的按键名称
        private string Select_ActualFloor = string.Empty; //选中的实际楼层
        private string Select_DevNo = string.Empty; //选中的端子号
        private string Select_DevCheckFloor = string.Empty; //选中的检测楼层

        public ResponseInfo UserControl_FloorTable = null;
        
        public string testStr = string.Empty;

        private string f_StratAuthFlag = string.Empty;
        private string f_EndAuthFlag = string.Empty;
        private string f_StartDevNo = string.Empty;
        private bool IsCloudDownFloor = false;
        private bool IsBathProcess = false; //批处理,快速设置

        List<object> listDevNoA = new List<object>();
        List<object> listDevNoB = new List<object>();
        List<object> listDevNoCompare = new List<object>();
        List<object> listDeActullyFloor = new List<object>();
        List<object> listDevCheckFloor = new List<object>();
        List<object> listCompareCheckFloor = new List<object>();
        List<object> listCompareActullyFloor = new List<object>();

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

        /// <summary>
        /// 初始化的设备类型
        /// </summary>
        enum IniDevType
        {
            /// <summary>
            /// 云电梯
            /// </summary>
            CloudElevator,
            /// <summary>
            /// 云群控器
            /// </summary>
            CloudGroup
        }

        private Dictionary<int, CloudElevatorItemUserControl> f_UCloudElevatorItems = null;

        private Dictionary<int, CloudGroupLinkageItemUserControl> f_UCloudGroupLinkageItems = null;

        protected override void InitUIOnLoad()
        {
            base.InitUIOnLoad();
            //这样是隐藏不了页面的
            //this.tbDownTables.Hide();
            this.tbDownTables.PageVisible = false;

            ControlUtilityTool.SetITLLayOutControlStyle(this.lcParams);
            ControlUtilityTool.SetITLTextEditStyle(this.edtDevIp);
            ControlUtilityTool.SetITLTextEditStyle(this.edtSubnetMark);
            ControlUtilityTool.SetITLTextEditStyle(this.edtGateWay);
            ControlUtilityTool.SetITLTextEditStyle(this.edtDNSServerIp);
            ControlUtilityTool.SetITLTextEditStyle(this.edtEIServerIp);
            ControlUtilityTool.SetITLTextEditStyle(this.edtEIServerPort);
            ControlUtilityTool.SetITLTextEditStyle(this.edtLinkagePort);
            ControlUtilityTool.SetITLTextEditStyle(this.edtProjectNo);
            ControlUtilityTool.SetITLXtraTabControlStyle(this.tcDownParams);
            ControlUtilityTool.SetITLTextEditStyle(this.edtMainGroupDevIP);

            ControlUtilityTool.SetITLComboBoxEditStyle(this.cmbedtCloudElevatorCount);
            ControlUtilityTool.SetITLComboBoxEditStyle(this.cmbedtCloudGroupCount);

            ControlUtilityTool.SetTextEditIPRegEx(this.edtDevIp);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtSubnetMark);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtGateWay);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtDNSServerIp);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtEIServerIp);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtMainGroupDevIP);     

            edtProjectNo.Properties.AutoHeight = true;

            this.edtDevIp.Properties.MaxLength = 15;
            this.edtSubnetMark.Properties.MaxLength = 15;
            this.edtGateWay.Properties.MaxLength = 15;
            this.edtDNSServerIp.Properties.MaxLength = 15;
            this.edtEIServerIp.Properties.MaxLength = 15;
            this.edtEIServerPort.Properties.MaxLength = 5;
            this.edtLinkagePort.Properties.MaxLength = 5;
            this.edtProjectNo.Properties.MaxLength = 8;

            this.edtDevIp.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtSubnetMark.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtGateWay.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtDNSServerIp.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtEIServerIp.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtEIServerPort.KeyPress += CommonUtils.edtPort_KeyPress;
            this.edtLinkagePort.KeyPress += CommonUtils.edtPort_KeyPress;
            this.edtProjectNo.KeyPress += CommonUtils.edtPort_KeyPress;

            this.DevType = DeviceType.GroupCloudLink;
            this.edtLinkagePort.Text = "60202"; //默认值
            f_IsRefresh = true;
            this.rgpDHCP.SelectedIndex = 1;
            this.rgpMainGroupDev.SelectedIndex = 0;
            f_IsRefresh = false;

            //加载楼层对应表
            InitGrid();

            FindCount = new Dictionary<string, hintInfo>();
            f_LocalDBOperate = new LocalDBOperate();

            HintProvider.WaitingDone(Application.ProductName);

            layoutControlItemMainGroupDev.ContentVisible = false;
            liMainGroupDevIP.ContentVisible = false;
            
            f_UCloudGroupLinkageItems = new Dictionary<int, CloudGroupLinkageItemUserControl>();
            f_UCloudElevatorItems = new Dictionary<int, CloudElevatorItemUserControl>();

            this.InitDevicesCountCmdEdit();
            this.AddComboBoxEditClickEvent();
            //this.InitCloudElevatorItemsUI(AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT);
            //this.InitCloudGroupLinkageItemsUI(AppConst.LINKAGE_CTRL_CLOUD_GROUPLINKAGE_MAX_COUNT);
        }

        protected override void InitUIEvents()
        {
            base.InitUIEvents();
            this.edtDevIp.Leave += this.edtDevIp_Leave;
            this.rgpDHCP.SelectedIndexChanged += this.rgpDHCP_SelectedIndexChanged;
            this.rgpMainGroupDev.SelectedIndexChanged += this.radioGroupMainGroupDev__SelectedIndexChanged;
        }
        /// <summary>
        /// 数据源增加数据列
        /// </summary>
        protected override void InitDevicesDataTable()
        {
            base.InitDevicesDataTable();
       
            f_DevicesDataTable.Columns.Add(DNS_SERVER_IP, typeof(string));
            f_DevicesDataTable.Columns.Add(DHCP_FUNCTION, typeof(string));
            f_DevicesDataTable.Columns.Add(EI_SERVER_IP, typeof(string));
            f_DevicesDataTable.Columns.Add(EI_SERVER_PORT, typeof(string));
            f_DevicesDataTable.Columns.Add(LINKAGE_PORT, typeof(string));
          
            f_DevicesDataTable.Columns.Add(VERSION, typeof(string));
            f_DevicesDataTable.Columns.Add(PROJECT_NO, typeof(string));
            //群控器设备类型
            f_DevicesDataTable.Columns.Add(GROUP_DEV_TYPE, typeof(string));
            //是否普通群控器
            f_DevicesDataTable.Columns.Add(IS_COMM_DEV, typeof(bool));
            //主群控器ip
            f_DevicesDataTable.Columns.Add(MAIN_GROUP_DEV_IP, typeof(string));

            //云电梯数量列
            f_DevicesDataTable.Columns.Add(CLOUD_ELEVATOR_COUNT, typeof(string));
            for (int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
            {
                //ip和属性
                f_DevicesDataTable.Columns.Add(string.Format(CLOUD_ELEVATOR_ITEM_CON_STATUES, i), typeof(string));
                f_DevicesDataTable.Columns.Add(string.Format(CLOUD_ELEVATOR_ITEM_IP, i), typeof(string));
                f_DevicesDataTable.Columns.Add(string.Format(CLOUD_ELEVATOR_ITEM_PROP, i), typeof(string));
            }
            //云群控器数量
            f_DevicesDataTable.Columns.Add(CLOUD_GROUP_LINKAGE_COUNT, typeof(string));
            for (int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_GROUPLINKAGE_MAX_COUNT; i++)
            {
                //ip和属性
                f_DevicesDataTable.Columns.Add(string.Format(CLOUD_GROUP_LINKAGE_ITEM_CON_STATUES, i), typeof(string));
                f_DevicesDataTable.Columns.Add(string.Format(CLOUD_GROUP_LINKAGE_ITEM_IP, i), typeof(string));
                f_DevicesDataTable.Columns.Add(string.Format(CLOUD_GROUP_LINKAGE_ITEM_PROP, i), typeof(string));
            }

        }
        /// <summary>
        /// 数据表增加显示列
        /// </summary>
        /// <param name="grdView"></param>
        protected override void InitDevicesGridView(GridView grdView)
        {
            base.InitDevicesGridView(grdView);
            
            //在基类的基础上增加特殊数据列
            
            //群控器设备类型
            this.AddOneGridViewColumn(grdView, GROUP_DEV_TYPE, GROUP_DEV_TYPE_ALIAS, 120);

            //DNS服务器IP
            this.AddOneGridViewColumn(grdView, DNS_SERVER_IP, DNS_SERVER_IP_ALIAS, 120);

            //DHCP功能
            this.AddOneGridViewColumn(grdView, DHCP_FUNCTION, DHCP_FUNCTION_ALIAS, 100);

            //版本号
            this.AddOneGridViewColumn(grdView, VERSION, VERSION_ALIAS, 180);

            //增加一个没用的列，不加这个上面版本号长度设置不了。暂时处理 韦将杰 2020-05-22
            this.AddOneGridViewColumn(grdView, "  ", "", 1);

            #region 云群控器、云电梯IP和门属性等都不显示
            /*
                //云电梯数量
                this.AddOneGridViewColumn(grdView, CLOUD_ELEVATOR_COUNT, CLOUD_ELEVATOR_COUNT_ALIAS, 100);



                //云电梯IP和门属性
                for (int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
                {
                    this.AddOneGridViewColumn(grdView, string.Format(CLOUD_ELEVATOR_ITEM_IP, i), string.Format(CLOUD_ELEVATOR_ITEM_IP_ALIAS, i), 120);
                    this.AddOneGridViewColumn(grdView, string.Format(CLOUD_ELEVATOR_ITEM_PROP, i), string.Format(CLOUD_ELEVATOR_ITEM_PROP_ALIAS, i), 120);
                }

                //云群控器数量
                this.AddOneGridViewColumn(grdView, CLOUD_GROUP_LINKAGE_COUNT, CLOUD_GROUP_LINKAGE_COUNT_ALIAS, 100);
                //云群控器IP和门属性
                for (int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_GROUPLINKAGE_MAX_COUNT; i++)
                {
                    this.AddOneGridViewColumn(grdView, string.Format(CLOUD_GROUP_LINKAGE_ITEM_IP, i), string.Format(CLOUD_GROUP_LINKAGE_ITEM_IP_ALIAS, i), 130);
                    this.AddOneGridViewColumn(grdView, string.Format(CLOUD_GROUP_LINKAGE_ITEM_PROP, i), string.Format(CLOUD_GROUP_LINKAGE_ITEM_PROP_ALIAS, i), 130);
                }

             */

            #endregion

            this.gcDevices.DataSource = f_DevicesDataTable;
            ControlUtilityTool.AdjustIndicatorWidth(grdView);
            


        }

        /// <summary>
        /// 云群控器、云电梯数量下拉框内容加载
        /// </summary>
        private void InitDevicesCountCmdEdit()
        {
            this.cmbedtCloudElevatorCount.Properties.Items.Clear();
            this.cmbedtCloudElevatorCount.Properties.Items.Add("无");

            this.cmbedtCloudGroupCount.Properties.Items.Clear();
            this.cmbedtCloudGroupCount.Properties.Items.Add("无");
            for (int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
            {
                this.cmbedtCloudElevatorCount.Properties.Items.Add(i);
            }

            for (int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_GROUPLINKAGE_MAX_COUNT; i++)
            {
                this.cmbedtCloudGroupCount.Properties.Items.Add(i);
            }
            this.cmbedtCloudElevatorCount.SelectedIndex = 0;
            this.cmbedtCloudGroupCount.SelectedIndex = 0;
        }
        /// <summary>
        /// 下拉框绑定事件统一管理
        /// </summary>
        private void AddComboBoxEditClickEvent()
        {
            cmbedtCloudElevatorCount.SelectedIndexChanged += cmbedtCloudElevatorCoun_SelectedIndexChanged;
            cmbedtCloudGroupCount.SelectedIndexChanged += cmbedtCloudGroupCount_SelectedIndexChanged;
        }

        private void cmbedtCloudElevatorCoun_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = (sender as DevExpress.XtraEditors.ComboBoxEdit).SelectedIndex;
            //this.InitCloudElevatorItemsUI(index);
            this.InitItemsUI(IniDevType.CloudElevator);
        }

        private void cmbedtCloudGroupCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = (sender as DevExpress.XtraEditors.ComboBoxEdit).SelectedIndex;
            //this.InitCloudGroupLinkageItemsUI(index);

            this.InitItemsUI(IniDevType.CloudGroup);
        }

        /// <summary>
        /// 加载云电梯和云群控器相关数据
        /// </summary>
        private void InitItemsUI(IniDevType iniDevType)
        {
            //电梯和群控器选择数量
            int elevatorSelect = cmbedtCloudElevatorCount.SelectedIndex;
            int groupSelect = cmbedtCloudGroupCount.SelectedIndex;

            //电梯和群控器已有数量
            int elevatorItemsCount = f_UCloudElevatorItems.Count;
            int groupItemsCount = f_UCloudGroupLinkageItems.Count;

            //如果选择的数量与原有的数量相同，直接退出;
            if ((elevatorSelect == elevatorItemsCount) && (groupSelect == groupItemsCount))
            {
                return;
            }

            layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //如果云电梯数量为0云群控器数量不为0，则云群控器加载在sclCtrlItems上面
            if (elevatorSelect == 0 && groupSelect > 0)
            {
                layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                f_UCloudElevatorItems.Clear();
                for (int i = 0; i < elevatorItemsCount; i++)
                {
                    sclCtrlItems.Controls[i].Visible = false;
                }

                this.ModifyGroupLinkageItems(groupSelect,groupItemsCount);
                return;
            }

            if (iniDevType == IniDevType.CloudElevator)
            {
                this.ModifyElevatorItems(elevatorSelect,elevatorItemsCount);
            }

            if (iniDevType == IniDevType.CloudGroup)
            {
                this.ModifyGroupLinkageItems(groupSelect, groupItemsCount);
            }

        }

        private void ModifyGroupLinkageItems(int SelectCount, int ItemsCount)
        {
            //如果选择的云群控器数量较原有的多，即新增
            if (SelectCount > ItemsCount)
            {
                for (int i = ItemsCount + 1; i <= SelectCount; i++)
                {
                    CloudGroupLinkageItemUserControl uc = UtilityTool.ShowUserControl<CloudGroupLinkageItemUserControl>(this.sclCloudGroupCtrlItems, DockStyle.Top);
                    uc.ItemId = i;
                    f_UCloudGroupLinkageItems.Add(i, uc);
                }
                return;
            }

            //减了群控器数量
            if (SelectCount < ItemsCount)
            {
                for (int i = ItemsCount; i > SelectCount; i--)
                {
                    foreach (var item in this.sclCloudGroupCtrlItems.Controls)
                    {
                        var control = item as CloudGroupLinkageItemUserControl;
                        if (control != null && control.ItemId == i)
                        {
                            this.sclCloudGroupCtrlItems.Controls.Remove(control);
                            break;
                        }
                    }
                    f_UCloudGroupLinkageItems.Remove(i);
                }
                return;
            }

            if (SelectCount == ItemsCount)
            {
                return;
            }
            return;
        }


        private void ModifyElevatorItems(int SelectCount, int ItemsCount)
        {
            //如果选择的云群控器数量较原有的多，即新增
            if (SelectCount > ItemsCount)
            {
                for (int i = 0; i <= ItemsCount; i++)
                {
                    //sclCtrlItems.Controls[i].Visible = true;
                }

                for (int i = ItemsCount + 1; i <= SelectCount; i++)
                {
                    CloudElevatorItemUserControl uc = UtilityTool.ShowUserControl<CloudElevatorItemUserControl>(this.sclCtrlItems, DockStyle.Top);
                    uc.ItemId = i;
                    f_UCloudElevatorItems.Add(i, uc);
                }
                return;
            }

            //减了数量
            if (SelectCount < ItemsCount)
            {
                for (int i = ItemsCount; i > SelectCount; i--)
                {
                    foreach (var item in this.sclCtrlItems.Controls)
                    {
                        var control = item as CloudElevatorItemUserControl;
                        if(control!=null && control.ItemId == i)
                        {
                            this.sclCtrlItems.Controls.Remove(control);
                            break;
                        }
                    }
                    f_UCloudElevatorItems.Remove(i);
                }
                return;
            }

            if (SelectCount == ItemsCount)
            {
                return;
            }
            return;
        }


        public static explicit operator GroupLinkageCtrlUserControl(Form v)
        {
            throw new NotImplementedException();
        }

        protected override void GeneralDeviceUserControl_SizeChanged(object sender, EventArgs e)
        {
            this.gvDevices.Columns[this.gvDevices.Columns.Count - 1].Width = 120;
            ControlUtilityTool.AdjustIndicatorWidth(this.gvDevices);
        }

        /// <summary>
        /// 设备IP编辑框离开事件，自动填充子网掩码
        /// 判断设备IP的第一段，小于128  小于192  小于255 的三种情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 解析返回的数据，重写自父类，父类的函数为虚函数
        /// </summary>
        /// <param name="strCmdStr"></param>
        protected override void AnalysisRecvDataEx(string strCmdStr)
        {
            //报文格式F2 XX XX ... XX XX F3
            //判断设备类型是否合法
            DeviceType devType = CommandProcesserHelper.GetDevTypeByCmdInfo(StrUtils.CopySubStr(strCmdStr, 6, 2));
            if (devType != DeviceType.GroupCloudLink)
            {
                return;
            }
            
            RunLog.Log("群控器搜索结果返回" + strCmdStr);
            //根据命令字，解析对应命令
            string strCmdWord = StrUtils.CopySubStr(strCmdStr, 14, 4);
            if (strCmdWord == AppConst.CMD_WORD_SEARCH_DEVIDES)
            {
                //判断报文长度（这里的判断我也看不懂 2020-04-13 韦将杰）
                if (strCmdStr.Length < 86 + 8) //43字节 +项目编号4字节
                {
                    RunLog.Log(string.Format("报文长度错误，报文：{0}", strCmdStr));
                    return;
                }
                this.AnalysisSearchDevicesRecvData(strCmdStr);
            }
            else if (strCmdWord == AppConst.CMD_WORD_SET_GROUPLIKE_PARAMS)
            {
                //判断报文长度，报文头尾总12字节
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
        /// 解析搜索返回的数据
        /// </summary>
        /// <param name="strCmdStr"></param>
        private void AnalysisSearchDevicesRecvData(string strCmdStr)
        {
            //取返回的数据部分
            string strCmdReport = StrUtils.CopySubStr(strCmdStr, 18, strCmdStr.Length - 22);
            GroupCloudLinkage DevInfo = new GroupCloudLinkage();

            //设备ID
            DevInfo.DevId = CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4));
            //Mac地址
            DevInfo.DevMac = CommonUtils.GetMacByHex(StrUtils.CopySubStr(strCmdReport, 2, 12));
            //DHCP功能
            DevInfo.DHCPEnable = StrUtils.CopySubStr(strCmdReport, 14, 2) == "01";
            //设备IP
            DevInfo.DevIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 16, 8));
            //子网掩码
            DevInfo.SubnetMask = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 24, 8));
            //网关
            DevInfo.GateWay = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 32, 8));
            //DNS服务器
            DevInfo.DNSServerIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 40, 8));

            //以下处理与云群控器有差异
            
            //设备状态（暂时没啥用）
            DevInfo.DevStatues = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdStr, 48, 8), 0, 16);

            #region 这部分也没啥用，需要注释掉
            /*
                //线下物联平台服务器
                DevInfo.EIServerIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 48, 8));
                //线下物联平台端口号
                DevInfo.EIServerPort = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 56, 4), 0, 16);
                //联动器端口号
                DevInfo.LinkagePort = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 60, 4), 0, 16);
                //项目编号 以十进制传输
                DevInfo.ProjectNo = StrUtils.ComplementedStr(StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 64, 8), 0, 10).ToString(), 8, "0");
            */
            #endregion

            //版本号 变长，长度1（Byte) + 版本号N (Byte)
            string VersionLength = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 56, 2), 0, 16).ToString();
            //RunLog.Log("设备ID为：" + DevInfo.DevId + "设备IP为：" + DevInfo.DevIp + "搜索报文版本长度为：" + VersionLength);
            int f_VersionLength = int.Parse(VersionLength) * 2;
            string UtilsVersionReport = StrUtils.CopySubStr(strCmdReport, 58, f_VersionLength);
            string UtilsVersion = string.Empty;
            for (int i = 0; i <= UtilsVersionReport.Length; i++)
            {
                UtilsVersion += StrUtils.CopySubStr(UtilsVersionReport, i * 2, 2);
                UtilsVersion += " ";
            }
            //处理多余空格
            UtilsVersion = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(UtilsVersion, " ");
            DevInfo.VerSion = CommonUtils.UnHex(UtilsVersion, "utf-8");

            //版本号取七位数，用于区分是普通群控器还是云群控器
            string SubVerSionStr = StrUtils.CopySubStr(DevInfo.VerSion, 0, 7);

            if (SubVerSionStr == COMMON_GROUP_DEV)
            {
                DevInfo.IsCommGroupDev = true;
                DevInfo.GroupDevType = "普通群控器";
            } else
            {
                DevInfo.IsCommGroupDev = false;
                DevInfo.GroupDevType = "云群控器";
            }


            //主群控器ip，程序版本号起始位置+程序版本号长度
            int MainIPStartIndex = 58 + f_VersionLength;
            DevInfo.MainGroupDevIP = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, MainIPStartIndex, 8));

            //云电梯控制器数量
            int CloudElevatorCountIndex = MainIPStartIndex + 8;
            DevInfo.CloudElevatorCount = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, CloudElevatorCountIndex, 2), 0, 16);

            //云电梯数据项起始位置
            int ElevatorItemsIndex = CloudElevatorCountIndex + 2;
            int ElevatorItemsLength = 0;
            for (int i = 1; i <= DevInfo.CloudElevatorCount; i++)
            {
                //云电梯数据项长度，每一项6个字节
                int ItemLeng = (i-1)  * 12;
                int DevIPIndex = ElevatorItemsIndex + ItemLeng;
                int ConStatuesIndex = DevIPIndex + 8;
                int CtrlProportiesIndex = ConStatuesIndex + 2;
             
                DevInfo.CloudElevatorItems[i].DevIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, DevIPIndex, 8));
                DevInfo.CloudElevatorItems[i].ConStatues = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, ConStatuesIndex, 2), 0, 16);
                DevInfo.CloudElevatorItems[i].CtrlProporties = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, CtrlProportiesIndex, 2), 0, 16);

                //云电梯数据项总长度
                ElevatorItemsLength = i * 12;
            }

            //云电梯控制器数量
            int CloudGroupLinkageCountIndex = ElevatorItemsIndex + ElevatorItemsLength;
            DevInfo.CloudGroupCtrlCount = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, CloudGroupLinkageCountIndex, 2), 0, 16);
            //云群控器数据项起始位置
            int GroupLinkageItemsIndex = CloudGroupLinkageCountIndex + 2;
            for (int i = 1; i <= DevInfo.CloudGroupCtrlCount; i++)
            {
                //云电梯数据项长度
                int ItemLeng = (i - 1) * 12;
                int DevIPIndex = GroupLinkageItemsIndex + ItemLeng;
                int ConStatuesIndex = DevIPIndex + 8;
                int CtrlProportiesIndex = ConStatuesIndex + 2;

                string devip = string.Empty;

                devip = StrUtils.CopySubStr(strCmdReport, DevIPIndex, 8);

                DevInfo.CloudGropCtrlItems[i].DevIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, DevIPIndex, 8));
                DevInfo.CloudGropCtrlItems[i].ConStatues = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, ConStatuesIndex, 2), 0, 16);
                DevInfo.CloudGropCtrlItems[i].CtrlProporties = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, CtrlProportiesIndex, 2), 0, 16);
            }



            this.AddOneDeviceToUI(DevInfo);
        }

        /// <summary>
        /// 将一个设备的各个参数加载到UI界面
        /// </summary>
        /// <param name="DevInfo"></param>
        private void AddOneDeviceToUI(GroupCloudLinkage DevInfo)
        {

            //   DataRow[] rows = f_DevicesDataTable.Select(string.Format("{0}={1}  and {2}={3}", DEV_ID, DevInfo.DevId, DEV_IP, DevInfo.DevIp));
            //   DataRow[] rows = f_DevicesDataTable.Select(""+ DEV_ID + "="+ DevInfo.DevId + " and "+ DEV_IP + "='" + DevInfo.DevIp + "' ");
            DataRow[] rows = f_DevicesDataTable.Select(string.Format("{0}={1}", DEV_ID, DevInfo.DevId));
            FindCount.AddToUpdate("" + DevInfo.DevId);
            FindCount.AddToUpdate("" + DevInfo.DevIp);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                try
                {
                    int FindSumCount = AppEnv.Singleton.UdpCount * AppEnv.Singleton.UdpCount;
                    if (FindCount["" + DevInfo.DevId].FontCount > FindSumCount && !FindCount["" + DevInfo.DevId].IsHint)
                    {

                        FindCount["" + DevInfo.DevId].IsHint = true;
                        HintProvider.ShowAutoCloseDialog(null, "设备ID冲突,ID:" + DevInfo.DevId);

                    }
                    if (FindCount[DevInfo.DevIp].FontCount > FindSumCount && !FindCount["" + DevInfo.DevIp].IsHint)
                    {
                        FindCount["" + DevInfo.DevIp].IsHint = true;
                        HintProvider.ShowAutoCloseDialog(null, string.Format("设备IP冲突,IP:{0},ID:{1}", DevInfo.DevIp, DevInfo.DevId));

                    }

                    rows[0][DEV_MAC] = DevInfo.DevMac;
                    rows[0][DEV_IP] = DevInfo.DevIp;
                    rows[0][SUBNET_MARK] = DevInfo.SubnetMask;
                    rows[0][GATE_WAY] = DevInfo.GateWay;
                    rows[0][DNS_SERVER_IP] = DevInfo.DNSServerIp;

                    rows[0][DHCP_FUNCTION] = DevInfo.DHCPEnable ? "启用" : "禁用";
                    rows[0][EI_SERVER_IP] = DevInfo.EIServerIp;
                    rows[0][EI_SERVER_PORT] = DevInfo.EIServerPort;
                    rows[0][LINKAGE_PORT] = DevInfo.LinkagePort;
                    rows[0][PROJECT_NO] = DevInfo.ProjectNo;

                    rows[0][VERSION] = DevInfo.VerSion;
                    rows[0][GROUP_DEV_TYPE] = DevInfo.GroupDevType;
                    rows[0][IS_COMM_DEV] = DevInfo.IsCommGroupDev;
                    rows[0][MAIN_GROUP_DEV_IP] = DevInfo.MainGroupDevIP;

                    rows[0][CLOUD_ELEVATOR_COUNT] = DevInfo.CloudElevatorCount;
                    for (int i = 1; i <= DevInfo.CloudElevatorCount; i++)
                    {
                        rows[0][string.Format(CLOUD_ELEVATOR_ITEM_IP, i)] = DevInfo.CloudElevatorItems[i].DevIp;
                        rows[0][string.Format(CLOUD_ELEVATOR_ITEM_PROP, i)] =
                            CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudElevatorItems[i].CtrlProporties);
                    }

                    rows[0][CLOUD_GROUP_LINKAGE_COUNT] = DevInfo.CloudGroupCtrlCount;
                    for (int i = 1; i <= DevInfo.CloudGroupCtrlCount; i++)
                    {
                        rows[0][string.Format(CLOUD_GROUP_LINKAGE_ITEM_IP, i)] = DevInfo.CloudGropCtrlItems[i].DevIp;
                        rows[0][string.Format(CLOUD_GROUP_LINKAGE_ITEM_PROP, i)] =
                            CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudGropCtrlItems[i].CtrlProporties);
                    }

                }
                finally
                {
                    rows[0].EndEdit();
                }
            }
            else
            {
             
                f_DevicesDataTable.Rows.Add(
                    DevInfo.DevId,
                    DevInfo.DevMac,
                    DevInfo.DevIp,
                    DevInfo.SubnetMask,
                    DevInfo.GateWay,
                    DevInfo.DNSServerIp,
                    DevInfo.DHCPEnable ? "启用" : "禁用",
                    DevInfo.EIServerIp,
                    DevInfo.EIServerPort,
                    DevInfo.LinkagePort,
                    DevInfo.VerSion,
                    DevInfo.ProjectNo,
                    DevInfo.GroupDevType,
                    DevInfo.IsCommGroupDev,
                    DevInfo.MainGroupDevIP,
                    DevInfo.CloudElevatorCount,
                    DevInfo.CloudElevatorItems[1].ConStatues,
                    DevInfo.CloudElevatorItems[1].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudElevatorItems[1].CtrlProporties),
                    DevInfo.CloudElevatorItems[2].ConStatues,
                    DevInfo.CloudElevatorItems[2].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudElevatorItems[2].CtrlProporties),
                    DevInfo.CloudElevatorItems[3].ConStatues,
                    DevInfo.CloudElevatorItems[3].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudElevatorItems[3].CtrlProporties),
                    DevInfo.CloudElevatorItems[4].ConStatues,
                    DevInfo.CloudElevatorItems[4].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudElevatorItems[4].CtrlProporties),
                    DevInfo.CloudElevatorItems[5].ConStatues,
                    DevInfo.CloudElevatorItems[5].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudElevatorItems[5].CtrlProporties),
                    DevInfo.CloudElevatorItems[6].ConStatues,
                    DevInfo.CloudElevatorItems[6].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudElevatorItems[6].CtrlProporties),
                    DevInfo.CloudElevatorItems[7].ConStatues,
                    DevInfo.CloudElevatorItems[7].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudElevatorItems[7].CtrlProporties),
                    DevInfo.CloudElevatorItems[8].ConStatues,
                    DevInfo.CloudElevatorItems[8].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudElevatorItems[8].CtrlProporties),
                    

                    DevInfo.CloudGroupCtrlCount,
                    DevInfo.CloudGropCtrlItems[1].ConStatues,
                    DevInfo.CloudGropCtrlItems[1].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudGropCtrlItems[1].CtrlProporties),
                    DevInfo.CloudGropCtrlItems[2].ConStatues,
                    DevInfo.CloudGropCtrlItems[2].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(DevInfo.CloudGropCtrlItems[2].CtrlProporties)
                    );


            }
            //排序
            f_DevicesDataTable.DefaultView.Sort = string.Format("{0} {1}", DEV_ID, "ASC");
            ControlUtilityTool.AdjustIndicatorWidth(this.gvDevices);
        }

        /// <summary>
        /// 下载参数
        /// </summary>
        /// <param name="strCmdStr"></param>
        private void AnalysisDownParams(string strCmdStr)
        {
            //F201018017000010100097F3
            //取返回的数据部分
            string strCmdReport = StrUtils.CopySubStr(strCmdStr, 18, strCmdStr.Length - 22);

            //判断设备Id是否正确
            int devId = CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4));
            if (devId != f_CurrentDevInfo.DevId)
            {
                return;
            }
            //获取返回的状态
            int intStatus = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 0, 2), -1, 16);
            switch (intStatus)
            {
                case 0x00:
                    {
                        //this.AddOneDeviceToUI(f_CurrentDevInfo);
                        //HintProvider.ShowAutoCloseDialog(null, "参数设置成功");

                        this.IsBusy = false;
                        this.tmrCommunication.Stop();
                        this.tmrCommunication.Interval = 100;

                        //韦将杰 2021-01-07 
                        //设置成功后，直接自动搜索，此时设备返回的数据还是原数据。
                        //修改参数工具代价小，改参数工具。
                        //将设置成功后自动搜索功能，进行延迟处理。
                        //如果哪天设备做了升级，修复这个bug，那这行代码可以注释掉，速度有飞跃。
                        System.Threading.Thread.Sleep(300);

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
            GroupCloudLinkage DevInfo = GetDevInfoBeforeDown();
            if (DevInfo == null)
            {
                return;
            }
            switch (this.tcDownParams.SelectedTabPageIndex)
            {
                case 0: //下载设备参数.
                    {
                        if (!R_Btn_Click)
                        {
                            this.DownBasicParams(DevInfo);
                        }
                    }
                    break;
                default:
                    break;
            }

            if (W_Btn_Click == true)
            {
                this.DownFloorTables(DevInfo); //下载楼层对应表
                W_Btn_Click = false;
            }
            else if (R_Btn_Click == true)
            {
                this.ReadFloorTables(DevInfo);//读取楼层对应表
                R_Btn_Click = false;
            }

            if ((!IsCloudDownFloor) && (W_Btn_Click == false) && (R_Btn_Click == false))
            {
                if ((!string.IsNullOrEmpty(f_StratAuthFlag)) && (!string.IsNullOrEmpty(f_EndAuthFlag)) && (!string.IsNullOrEmpty(f_StartDevNo)))
                {
                    this.QuickSetInLocal(DevInfo);
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
        /// <param name="DevInfo">云群控器.</param>
        private void DownBasicParams(GroupCloudLinkage DevInfo)
        {
            string strWriteCmd = this.GetBasicParamsWriteCmdStr(DevInfo);
            //判断长度
            if (strWriteCmd.Length < 48 + 8) //下载参数，最短字节数，其中云电梯云群控器数量都是0，总24字节 + (mac校验4字节)
            {
                HintProvider.ShowAutoCloseDialog(null, "生成的报文长度错误，请检查设置的参数是否正确");
                return;
            }
            string writeReport = this.GetWriteReport(DevInfo.DevId, AppConst.CMD_WORD_SET_GROUPLIKE_PARAMS, strWriteCmd);

            RunLog.Log("下载的命令报文：" + writeReport);
            string errMsg = string.Empty;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
            if (!this.UdpListener.SendData(endpoint, writeReport, ref errMsg))
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("设置参数失败，错误：{0}", errMsg));
            }
            f_CurrentDevInfo = DevInfo;
        }

        /// <summary>
        /// 拼接，获取下载基础参数的指令串
        /// </summary>
        /// <param name="DevInfo"></param>
        /// <returns></returns>
        private string GetBasicParamsWriteCmdStr(GroupCloudLinkage DevInfo)
        {
            //协议版本，目前固定为02
            string strCmdStr = "02";
            //DHCP标志
            strCmdStr += DevInfo.DHCPEnable ? "01" : "00";
            //设备Ip
            strCmdStr += DevInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(DevInfo.DevIp);
            //子网掩码
            strCmdStr += DevInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(DevInfo.SubnetMask);
            //网关
            strCmdStr += DevInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(DevInfo.GateWay);
            //DNS服务器
            strCmdStr += DevInfo.DHCPEnable ? "00000000" : CommonUtils.GetHexByIP(DevInfo.DNSServerIp);


            //线下物联平台服务器地址
            //strCmdStr += CommonUtils.GetHexByIP(DevInfo.EIServerIp);
            //线下物联平台服务器端口
            //strCmdStr += StrUtils.IntToHex(DevInfo.EIServerPort, 4);
            //联动器端口
            //strCmdStr += StrUtils.IntToHex(DevInfo.LinkagePort, 4);

            //预留4 2  2 字节
            //strCmdStr += "0000000000000000";

            //项目编号           
            //strCmdStr += StrUtils.ComplementedStr(DevInfo.ProjectNo.ToString(), 8, "0");
            //主群控器地址，如果是云群控器则为00000000
            strCmdStr += ! DevInfo.IsCommGroupDev ? "00000000" : CommonUtils.GetHexByIP(DevInfo.MainGroupDevIP);
            //云电梯数量
            strCmdStr += StrUtils.IntToHex(DevInfo.CloudElevatorCount, 2);
            //云电梯设备信息
            for (int i = 1; i <= DevInfo.CloudElevatorCount; i++)
            {
                strCmdStr += CommonUtils.GetHexByIP(DevInfo.CloudElevatorItems[i].DevIp);
                strCmdStr += StrUtils.IntToHex(DevInfo.CloudElevatorItems[i].CtrlProporties, 2);
            }

            //云群控器数量
            strCmdStr += StrUtils.IntToHex(DevInfo.CloudGroupCtrlCount, 2);

            //8个云电梯设备信息
            for (int i = 1; i <= DevInfo.CloudGroupCtrlCount; i++)
            {
                strCmdStr += CommonUtils.GetHexByIP(DevInfo.CloudGropCtrlItems[i].DevIp);
                strCmdStr += StrUtils.IntToHex(DevInfo.CloudGropCtrlItems[i].CtrlProporties, 2);
            }
            //mac校验
            string strMac = KeyMacOperate.GetMacEx(strCmdStr);

            strCmdStr += strMac;
            return strCmdStr;
        }
        /// <summary>
        /// 楼层对应表下载前的判断
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
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
        /// 下载参数前需要先获取对应数据
        /// </summary>
        /// <returns></returns>
        private GroupCloudLinkage GetDevInfoBeforeDown()
        {
            if (!this.CheckUIValid())
            {
                return null;
            }
            GroupCloudLinkage DevInfo = this.GetDeviceInfoByFocusRow();
            if (DevInfo == null)
            {
                return null;
            }
            DevInfo.DHCPEnable = this.rgpDHCP.SelectedIndex == 0;
            DevInfo.DevIp = DevInfo.DHCPEnable ? string.Empty : this.edtDevIp.Text.Trim();
            DevInfo.SubnetMask = DevInfo.DHCPEnable ? string.Empty : this.edtSubnetMark.Text.Trim();
            DevInfo.GateWay = DevInfo.DHCPEnable ? string.Empty : this.edtGateWay.Text.Trim();
            DevInfo.DNSServerIp = DevInfo.DHCPEnable ? string.Empty : this.edtDNSServerIp.Text.Trim();
            DevInfo.EIServerIp = this.edtEIServerIp.Text.Trim();
            DevInfo.EIServerPort = StrUtils.StrToIntDef(this.edtEIServerPort.Text.Trim(), 0);
            DevInfo.LinkagePort = StrUtils.StrToIntDef(this.edtLinkagePort.Text.Trim(), 0);
            DevInfo.ProjectNo = this.edtProjectNo.Text.Trim();
            DevInfo.MainGroupDevIP = this.edtMainGroupDevIP.Text.Trim();

            //云电梯参数项
            int ElevatorCount = this.cmbedtCloudElevatorCount.SelectedIndex;
            DevInfo.CloudElevatorCount = ElevatorCount;

            for (int i = 1; i <= ElevatorCount; i++)
            {
                DevInfo.CloudElevatorItems[i].DevIp = f_UCloudElevatorItems[i].DevIp;
                DevInfo.CloudElevatorItems[i].CtrlProporties = f_UCloudElevatorItems[i].DevPropIndex;

            }

            //云群控器参数项
            int GroupLinkageCount = this.cmbedtCloudGroupCount.SelectedIndex;
            DevInfo.CloudGroupCtrlCount = GroupLinkageCount;

            for (int i = 1; i <= GroupLinkageCount; i++)
            {
                DevInfo.CloudGropCtrlItems[i].DevIp = f_UCloudGroupLinkageItems[i].DevIp;
                DevInfo.CloudGropCtrlItems[i].CtrlProporties = f_UCloudGroupLinkageItems[i].DevPropIndex;
                
            }


            return DevInfo;
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


            #region 云电梯属性不能为空
            //也可以用这个来取值
            //int index = cmbedtCloudElevatorCount.SelectedIndex;

            int controlsCount = f_UCloudElevatorItems.Count;
            for (int i = 1; i <= controlsCount; i++)
            {
                if (f_UCloudElevatorItems[i].DevIp == string.Empty)
                {
                    HintProvider.ShowAutoCloseDialog(null, "云电梯IP地址不能为空");
                    return false;
                }

                if (f_UCloudElevatorItems[i].DevPropIndex < 0)
                {
                    HintProvider.ShowAutoCloseDialog(null, "云电梯控制器属性不能为空");
                    return false;
                }
            }

            #region 判断属性不为空，这里是通过遍历界面上的控件进行判断，暂不使用

            //int controlsCount = sclCtrlItems.Controls.Count;

            //for (int i = 0; i < controlsCount; i++)
            //{
            //    if ((sclCtrlItems.Controls[i] as CloudElevatorItemUserControl).DevIp == string.Empty)
            //    {
            //        HintProvider.ShowAutoCloseDialog(null, "云电梯IP地址不能为空");
            //        return false;
            //    }

            //    if ((sclCtrlItems.Controls[i] as CloudElevatorItemUserControl).DevPropIndex < 0)
            //    {
            //        HintProvider.ShowAutoCloseDialog(null, "云电梯控制器属性不能为空");
            //        return false;
            //    }

            //    //也可以通过这个来判断
            //    //f_UCloudElevatorItems[i].DevIp;
            //    //f_UCloudElevatorItems[i].DevPropIndex;
            //}

            #endregion


            #endregion

            #region 云群控器属性不能为空

            controlsCount = f_UCloudGroupLinkageItems.Count;
            for (int i = 1; i <= controlsCount; i++)
            {
                if (f_UCloudGroupLinkageItems[i].DevIp == string.Empty)
                {
                    HintProvider.ShowAutoCloseDialog(null, "云群控器IP地址不能为空");
                    return false;
                }

                if (f_UCloudGroupLinkageItems[i].DevPropIndex < 0)
                {
                    HintProvider.ShowAutoCloseDialog(null, "云群控器控制器属性不能为空");
                    return false;
                }
            }

            #region 判断属性不为空，这里是通过遍历界面上的控件进行判断，暂不使用
            //controlsCount = sclCloudGroupCtrlItems.Controls.Count;

            //for (int i = 0; i < controlsCount; i++)
            //{
            //    if ((sclCloudGroupCtrlItems.Controls[i] as CloudGroupLinkageItemUserControl).DevIp == string.Empty)
            //    {
            //        HintProvider.ShowAutoCloseDialog(null, "云群控器IP地址不能为空");
            //        return false;
            //    }

            //    if ((sclCloudGroupCtrlItems.Controls[i] as CloudGroupLinkageItemUserControl).DevPropIndex < 0)
            //    {
            //        HintProvider.ShowAutoCloseDialog(null, "云群控器控制器属性不能为空");
            //        return false;
            //    }
            //}
            #endregion
            #endregion

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
                GroupCloudLinkage DevInfo = this.GetDeviceInfoByFocusRow();
                if (DevInfo == null)
                {
                    return;
                }

                this.edtDevIp.Text = DevInfo.DHCPEnable ? string.Empty : DevInfo.DevIp;
                this.edtSubnetMark.Text = DevInfo.DHCPEnable ? string.Empty : DevInfo.SubnetMask;
                this.edtGateWay.Text = DevInfo.DHCPEnable ? string.Empty : DevInfo.GateWay;
                this.edtDNSServerIp.Text = DevInfo.DHCPEnable ? string.Empty : DevInfo.DNSServerIp;
                this.edtEIServerIp.Text = DevInfo.EIServerIp;
                this.edtEIServerPort.Text = DevInfo.EIServerPort.ToString();
                this.edtLinkagePort.Text = DevInfo.LinkagePort.ToString();
                this.rgpDHCP.SelectedIndex = DevInfo.DHCPEnable ? 0 : 1;

                this.edtDevIp.Enabled = !DevInfo.DHCPEnable;
                this.edtSubnetMark.Enabled = !DevInfo.DHCPEnable;
                this.edtGateWay.Enabled = !DevInfo.DHCPEnable;
                this.edtDNSServerIp.Enabled = !DevInfo.DHCPEnable;


                //this.edtProjectNo.Text = StrUtils.ComplementedStr(DevInfo.ProjectNo.ToString(), 8, "0");

                //如果项目编号为00000000
                //并且设
                //设备为云群控器，则项目编号可编辑
                //edtProjectNo.Enabled = edtProjectNo.Text.Trim() =="00000000" && (! DevInfo.IsCommGroupDev);

                //群控器为普通群控器时主群控器相关才可见
                layoutControlItemMainGroupDev.ContentVisible = DevInfo.IsCommGroupDev;
                liMainGroupDevIP.ContentVisible = DevInfo.IsCommGroupDev;

                //如果主群控器IP为0.0.0.0.则说明设备为主群控器
                if (DevInfo.MainGroupDevIP == "0.0.0.0")
                {
                    rgpMainGroupDev.SelectedIndex = 0;
                    edtMainGroupDevIP.Enabled = false;
                }
                else
                {
                    rgpMainGroupDev.SelectedIndex = 1;
                    edtMainGroupDevIP.Enabled = true;
                }
                
                //主群控器IP
                edtMainGroupDevIP.Text = DevInfo.MainGroupDevIP;

                //DevInfo.DHCPEnable ? "启用" : "禁用";
                //this.edtProjectNo.Text = DevInfo.ProjectNo.ToString();
                if (DevInfo.DHCPEnable)
                {
                    this.edtDevIp.Tag = DevInfo.DevIp;
                    this.edtSubnetMark.Tag = DevInfo.SubnetMask;
                    this.edtGateWay.Tag = DevInfo.GateWay;
                    this.edtDNSServerIp.Tag = DevInfo.DNSServerIp;
                    this.edtProjectNo.Tag = DevInfo.ProjectNo;
                }

                this.cmbedtCloudElevatorCount.SelectedIndex = DevInfo.CloudElevatorCount;
                this.cmbedtCloudGroupCount.SelectedIndex = DevInfo.CloudGroupCtrlCount;


                #region 云电梯云联动器数据项加载到界面
                    for (int i = 1; i <= DevInfo.CloudElevatorCount; i++)
                    {
                        f_UCloudElevatorItems[i].ConStatues = DevInfo.CloudElevatorItems[i].ConStatues;
                        f_UCloudElevatorItems[i].DevIp = DevInfo.CloudElevatorItems[i].DevIp;
                        f_UCloudElevatorItems[i].DevPropIndex = DevInfo.CloudElevatorItems[i].CtrlProporties;
                    }

                    for (int i = 1; i <= DevInfo.CloudGroupCtrlCount; i++)
                    {
                        f_UCloudGroupLinkageItems[i].ConStatues = DevInfo.CloudGropCtrlItems[i].ConStatues;
                        f_UCloudGroupLinkageItems[i].DevIp = DevInfo.CloudGropCtrlItems[i].DevIp;
                        f_UCloudGroupLinkageItems[i].DevPropIndex = DevInfo.CloudGropCtrlItems[i].CtrlProporties;

                    }
                #endregion

                //加载对应表信息到界面
                DeviceTableInfo deviceTableInfo = new DeviceTableInfo()
                {
                    DevId = DevInfo.DevId
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
               //  DownParams();  ///zhang 2019-4-27
                R_Btn_Click = false;
            }
        }

        /// <summary>
        /// 取出当前焦点行的数据，存储到结构体DevInfo中
        /// </summary>
        /// <returns></returns>
        private GroupCloudLinkage GetDeviceInfoByFocusRow()
        {
            if (this.gvDevices.FocusedRowHandle < 0)
            {
                return null;
            }
            GroupCloudLinkage DevInfo = new GroupCloudLinkage()
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
                GroupDevType = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, GROUP_DEV_TYPE).ToString(),
                IsCommGroupDev = StrUtils.StrToBoolDef( this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, IS_COMM_DEV).ToString(), false),
                MainGroupDevIP = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, MAIN_GROUP_DEV_IP).ToString(),
                CloudElevatorCount = StrUtils.StrToIntDef(this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, CLOUD_ELEVATOR_COUNT).ToString(), 0),
                CloudGroupCtrlCount = StrUtils.StrToIntDef(this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, CLOUD_GROUP_LINKAGE_COUNT).ToString(),
                0)

            };

            for (int i = 1; i <= DevInfo.CloudElevatorCount; i++)
            {
                DevInfo.CloudElevatorItems[i].ConStatues 
                    = StrUtils.StrToIntDef(
                        this.gvDevices.GetRowCellValue(
                            this.gvDevices.FocusedRowHandle, string.Format(CLOUD_ELEVATOR_ITEM_CON_STATUES, i)
                            ).ToString(),
                        0);

                DevInfo.CloudElevatorItems[i].DevIp 
                    = this.gvDevices.GetRowCellValue(
                        this.gvDevices.FocusedRowHandle, string.Format(CLOUD_ELEVATOR_ITEM_IP, i)
                        ).ToString();
                DevInfo.CloudElevatorItems[i].CtrlProporties 
                    = CommonUtils.GetCloudElevatorPropertiesByName(
                        this.gvDevices.GetRowCellValue(
                            this.gvDevices.FocusedRowHandle, string.Format(CLOUD_ELEVATOR_ITEM_PROP, i)
                            ).ToString());
            }

            for (int i = 1; i <= DevInfo.CloudGroupCtrlCount; i++)
            {
                DevInfo.CloudGropCtrlItems[i].ConStatues 
                    = StrUtils.StrToIntDef(
                        this.gvDevices.GetRowCellValue(
                            this.gvDevices.FocusedRowHandle, string.Format(CLOUD_GROUP_LINKAGE_ITEM_CON_STATUES, i)
                            ).ToString(),
                        0);
                 DevInfo.CloudGropCtrlItems[i].DevIp 
                    = this.gvDevices.GetRowCellValue(
                        this.gvDevices.FocusedRowHandle, string.Format(CLOUD_GROUP_LINKAGE_ITEM_IP, i) ).ToString();

                DevInfo.CloudGropCtrlItems[i].CtrlProporties 
                    = CommonUtils.GetCloudElevatorPropertiesByName(
                        this.gvDevices.GetRowCellValue(
                            this.gvDevices.FocusedRowHandle, string.Format(CLOUD_GROUP_LINKAGE_ITEM_PROP, i)).ToString());
            }

            return DevInfo;
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


        private void radioGroupMainGroupDev__SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.rgpMainGroupDev.SelectedIndex == 0)
            {
                //edtProjectNo.Enabled = false;
                edtMainGroupDevIP.Text = "0.0.0.0";
                edtMainGroupDevIP.Enabled = false;
                this.cmbedtCloudElevatorCount.Enabled = true;
                this.cmbedtCloudGroupCount.Enabled = true;
            }
            else
            {
                edtMainGroupDevIP.Enabled = true;
                this.cmbedtCloudElevatorCount.SelectedIndex = 0;
                this.cmbedtCloudGroupCount.SelectedIndex = 0;
                this.cmbedtCloudElevatorCount.Enabled = false;
                this.cmbedtCloudGroupCount.Enabled = false;
            }
            
        }
        /// <summary>
        /// 切换TabControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcDownParams_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {

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
                }

                else if (tcDownParams.SelectedTabPageIndex == 1)
                {
                    // 加载按钮---- 楼层对应表
                    Btn_DownLoadParm.Visible = false;
                    Btn_ReadLocalFloorTable.Visible = true;
                    //Btn_ReadCloudFloorTable.Visible = true;
                    Btn_DownLoadDev.Visible = true;
                    //Btn_SaveSet.Visible = true;
                    Btn_QuickSet.Visible = true;

                    //Is_LoaclDownTable = false;
                    //InitGrid();//加载楼层对应表
                }
            }

        }
       
        /// <summary>
        /// 初始化表格
        /// </summary>
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
                f_QuickForm_RadDevNo = Form_CloudFloorTableQuickSet.f_QuickSetInfo_FloorTerminalNo;
                f_QuickForm_RadCheckFloor = Form_CloudFloorTableQuickSet.f_QuickSetInfo_CheckFloor;
                f_QuickForm_RadKeyName = Form_CloudFloorTableQuickSet.f_QuickSetInfo_FloorName;
                f_QuickForm_edt_KeyNameNo = Form_CloudFloorTableQuickSet.f_edt_KeyNameNo;
                f_QuickForm_cbbE_Start = Form_CloudFloorTableQuickSet.f_QuickSetInfo_StartDevNo;

                if ((!string.IsNullOrEmpty(f_StratAuthFlag)) && (!string.IsNullOrEmpty(f_EndAuthFlag)) && (!string.IsNullOrEmpty(f_StartDevNo)))
                {
                    //取消保存设置
                    //R_Btn_Click = true;
                    DownParams();
                    //R_Btn_Click = false;
                    IsCloudDownFloor = false;

                }
            }
        }

        /// <summary>
        /// 快速设置
        /// </summary>
        private void QuickSetInLocal(GroupCloudLinkage DevInfo)
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
                this.FloorTableWriteCmdStr(DevInfo, ref strWriteCmd, ref strCmdWord);

            }
            catch (Exception ex)
            {
                RunLog.Log(ex);
            }
        }

        /// <summary>
        /// 下载楼层对应表
        /// </summary>
        /// <param name="DevInfo"></param>
        private void DownFloorTables(GroupCloudLinkage DevInfo)
        {
            try
            {
                string strCmdWord = string.Empty;
                string strWriteCmd = string.Empty;
                this.FloorTableWriteCmdStr(DevInfo, ref strWriteCmd, ref strCmdWord);
                //string WriteReport = this.GetWriteReport(DevInfo.DevId, "5A30", "");//报文
                strWriteCmd = strWriteCmd.ToUpper();
                string WriteReport = this.GetWriteReport(DevInfo.DevId, "5A30", strWriteCmd);//报文

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
                f_CurrentDevInfo = DevInfo;
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
                    string IsVersion = StrUtils.CopySubStr(DevInfo.VerSion, 9, 4);
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
        /// <param name="DevInfo"></param>
        /// <param name="WriteCmd"></param>
        /// <param name="cmdWord"></param>
        private void FloorTableWriteCmdStr(GroupCloudLinkage DevInfo, ref string WriteCmd, ref string cmdWord)
        {

            DownTableInfo downTableInfo = this.GetTablesBeforeDownInfo(DevInfo.DevId);
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
            string IsVersion = StrUtils.CopySubStr(DevInfo.VerSion, 9, 4);
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

                }
                else
                {
                    ActullyTmp.Add(strTmp);

                }
            }

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
        /// <param name="DevInfo"></param>
        private void ReadFloorTables(GroupCloudLinkage DevInfo)
        {
            try
            {
                string strCmdWord = string.Empty;
                string strReadCmd = string.Empty;
                this.GetFloorTableReadCmdStr(DevInfo, ref strReadCmd, ref strCmdWord);
                //string ReadReport = this.GetWriteReport(DevInfo.DevId, "5A31", strReadCmd);//报文
                string ReadReport = this.GetWriteReport(DevInfo.DevId, "5A31", "");//报文

                //获取mac值
                //ReadReport += KeyMacOperate.GetMacEx(ReadReport);

                IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
                f_CurrentDevInfo = DevInfo;
                string errMsg = string.Empty;
                if (!this.UdpListener.SendData(endpoint, ReadReport, ref errMsg))
                {
                    HintProvider.ShowAutoCloseDialog(null, string.Format("读取本地对应表失败，错误：{0}", errMsg));
                }
                else
                {

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
        /// <param name="DevInfo"></param>
        /// <param name="readCmd"></param>
        /// <param name="cmdWord"></param>
        private void GetFloorTableReadCmdStr(GroupCloudLinkage DevInfo, ref string readCmd, ref string cmdWord)
        {

            DownTableInfo downTableInfo = this.GetTablesBeforeDownInfo(DevInfo.DevId);
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

    }
}
