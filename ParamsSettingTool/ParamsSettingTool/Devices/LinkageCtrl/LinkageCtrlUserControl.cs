using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ITL.Public;
using ITL.DataDefine;
using DevExpress.XtraGrid.Views.Grid;
using ITL.Framework;
using DevExpress.XtraGrid.Columns;
using System.Net;

namespace ITL.ParamsSettingTool
{
    public partial class LinkageCtrlUserControl : GeneralDeviceUserControl
    {
        private const string CLOUD_ELEVATOR_UDP_PORT = "cloud_udp_port";  //云电梯UDP端口
        private const string THIRD_INTERCOM_UDP_PORT = "third_intercom_udp_port";  //第三方对讲机UDP端口
        private const string CLOUD_ELEVATOR_COUNT = "cloud_elevator_count";  //云电梯数量
        private const string CLOUD_ELEVATOR_ITEM_IP = "cloud_elevator_item_ip_{0}";  //云电梯ItemIP
        private const string CLOUD_ELEVATOR_ITEM_PROP = "cloud_elevator_item_prop_{0}"; //云电梯ItemProp

        private const string CLOUD_ELEVATOR_UDP_PORT_ALIAS = "云电梯UDP端口";  //云电梯UDP端口
        private const string THIRD_INTERCOM_UDP_PORT_ALIAS = "对讲UDP端口";  //第三方对讲机UDP端口
        private const string CLOUD_ELEVATOR_COUNT_ALIAS = "云电梯数量";  //云电梯数量
        private const string CLOUD_ELEVATOR_ITEM_IP_ALIAS = "{0}号云电梯IP地址";  //云电梯ItemIP
        private const string CLOUD_ELEVATOR_ITEM_PROP_ALIAS = "{0}号云电梯门属性"; //云电梯ItemProp

        private LinkageCtrlInfo f_CurrentLinkageCtrlInfo = null;  //当前操作的设备信息
        private Dictionary<int, CloudElevatorItemUserControl> f_UCloudElevatorItems = null;

        public LinkageCtrlUserControl()
        {
            InitializeComponent();
        }

        protected override void InitUIOnLoad()
        {
            base.InitUIOnLoad();

            ControlUtilityTool.SetITLLayOutControlStyle(this.lcLinkageCtrl);
            ControlUtilityTool.SetITLTextEditStyle(this.edtDevIp);
            ControlUtilityTool.SetITLTextEditStyle(this.edtSubnetMark);
            ControlUtilityTool.SetITLTextEditStyle(this.edtGateWay);
            ControlUtilityTool.SetITLTextEditStyle(this.edtCloudUdpPort);
            ControlUtilityTool.SetITLTextEditStyle(this.edtThirdUdpPort);
            ControlUtilityTool.SetITLComboBoxEditStyle(this.cmbCloudCtrlCount);
            ControlUtilityTool.SetControlDefaultColor(this.sclCtrlItems, ControlUtilityTool.PubBackColorNormal);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtDevIp);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtSubnetMark);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtGateWay);
            
            this.edtDevIp.Properties.MaxLength = 15;
            this.edtSubnetMark.Properties.MaxLength = 15;
            this.edtGateWay.Properties.MaxLength = 15;
            this.edtCloudUdpPort.Properties.MaxLength = 5;
            this.edtThirdUdpPort.Properties.MaxLength = 5;

            this.edtDevIp.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtSubnetMark.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtGateWay.KeyPress += CommonUtils.edtIp_KeyPress;
            this.edtCloudUdpPort.KeyPress += CommonUtils.edtPort_KeyPress;
            this.edtThirdUdpPort.KeyPress += CommonUtils.edtPort_KeyPress;

            this.InitDevicesCountCmdEdit();
            this.InitCloudElevatorItemsUI();

            this.DevType = DeviceType.LinkageCtrl;
            HintProvider.WaitingDone(Application.ProductName);

            FindCount = new Dictionary<string, hintInfo>();
        }

        protected override void InitUIEvents()
        {
            base.InitUIEvents();
            this.edtDevIp.Leave += this.edtDevIp_Leave;
        }

        protected override void InitDevicesDataTable()
        {
            base.InitDevicesDataTable();
            f_DevicesDataTable.Columns.Add(CLOUD_ELEVATOR_UDP_PORT, typeof(string));
            f_DevicesDataTable.Columns.Add(THIRD_INTERCOM_UDP_PORT, typeof(string));
            f_DevicesDataTable.Columns.Add(CLOUD_ELEVATOR_COUNT, typeof(string));
            for (int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
            {
                f_DevicesDataTable.Columns.Add(string.Format(CLOUD_ELEVATOR_ITEM_IP, i), typeof(string));
                f_DevicesDataTable.Columns.Add(string.Format(CLOUD_ELEVATOR_ITEM_PROP, i), typeof(string));
            }
        }

        protected override void InitDevicesGridView(GridView grdView)
        {
            base.InitDevicesGridView(grdView);

            //云电梯UDP端口
            this.AddOneGridViewColumn(grdView, CLOUD_ELEVATOR_UDP_PORT, CLOUD_ELEVATOR_UDP_PORT_ALIAS, 150);

            //第三方对讲机UDP端口
            this.AddOneGridViewColumn(grdView, THIRD_INTERCOM_UDP_PORT, THIRD_INTERCOM_UDP_PORT_ALIAS, 150);

            //云电梯数量
            this.AddOneGridViewColumn(grdView, CLOUD_ELEVATOR_COUNT, CLOUD_ELEVATOR_COUNT_ALIAS, 100);

            //云电梯IP和门属性
            for (int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
            {
                this.AddOneGridViewColumn(grdView, string.Format(CLOUD_ELEVATOR_ITEM_IP, i), string.Format(CLOUD_ELEVATOR_ITEM_IP_ALIAS, i), 120);
                this.AddOneGridViewColumn(grdView, string.Format(CLOUD_ELEVATOR_ITEM_PROP, i), string.Format(CLOUD_ELEVATOR_ITEM_PROP_ALIAS, i), 120);
            }

            this.gcDevices.DataSource = f_DevicesDataTable;
            ControlUtilityTool.AdjustIndicatorWidth(grdView);
        }

        private void InitDevicesCountCmdEdit()
        {
            this.cmbCloudCtrlCount.Properties.Items.Clear();
            this.cmbCloudCtrlCount.Properties.Items.Add("无");
            for(int i = 1; i <= 8; i++)
            {
                this.cmbCloudCtrlCount.Properties.Items.Add(i);
            }
            this.cmbCloudCtrlCount.SelectedIndex = 0;
        }

        private void InitCloudElevatorItemsUI()
        {
            f_UCloudElevatorItems = new Dictionary<int, CloudElevatorItemUserControl>();
            for(int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
            {
                CloudElevatorItemUserControl uc = UtilityTool.ShowUserControl<CloudElevatorItemUserControl>(this.sclCtrlItems, DockStyle.Top);
                uc.ItemId = i;
                f_UCloudElevatorItems.Add(i, uc);
            }
        }

        protected override void GeneralDeviceUserControl_SizeChanged(object sender, EventArgs e)
        {
            base.GeneralDeviceUserControl_SizeChanged(sender, e);
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

        private void AddOneDeviceToUI(LinkageCtrlInfo linkageCtrlInfo)
        {
            DataRow[] rows = f_DevicesDataTable.Select(string.Format("{0}={1}", DEV_ID, linkageCtrlInfo.DevId));
            FindCount.AddToUpdate("" + linkageCtrlInfo.DevId);
            FindCount.AddToUpdate("" + linkageCtrlInfo.DevIp);
            if (rows.Length > 0)
            {
                rows[0].BeginEdit();
                try
                {
                    int FindSumCount = AppEnv.Singleton.UdpCount * AppEnv.Singleton.UdpCount + 1;

                    if (FindCount["" + linkageCtrlInfo.DevId].FontCount > FindSumCount && !FindCount["" + linkageCtrlInfo.DevId].IsHint)
                    {
                       
                        FindCount["" + linkageCtrlInfo.DevId].IsHint = true;
                        HintProvider.ShowAutoCloseDialog(null, "设备ID冲突,ID:" + linkageCtrlInfo.DevId);

                    }
                    if (FindCount[linkageCtrlInfo.DevIp].FontCount > FindSumCount && !FindCount["" + linkageCtrlInfo.DevIp].IsHint)
                    {
                        FindCount["" + linkageCtrlInfo.DevIp].IsHint = true;
                        HintProvider.ShowAutoCloseDialog(null, string.Format("设备IP冲突,IP:{0},ID:{1}",linkageCtrlInfo.DevIp,linkageCtrlInfo.DevId));
                    }

                    rows[0][DEV_MAC] = linkageCtrlInfo.DevMac;
                    rows[0][DEV_IP] = linkageCtrlInfo.DevIp;
                    rows[0][SUBNET_MARK] = linkageCtrlInfo.SubnetMask;
                    rows[0][GATE_WAY] = linkageCtrlInfo.GateWay;
                    rows[0][CLOUD_ELEVATOR_UDP_PORT] = linkageCtrlInfo.CloudUdpPort;
                    rows[0][THIRD_INTERCOM_UDP_PORT] = linkageCtrlInfo.ThirdUdpPort;
                    rows[0][CLOUD_ELEVATOR_COUNT] = linkageCtrlInfo.CloudElevatorCount;
                    for(int i = 1; i <= 8; i++)
                    {
                        rows[0][string.Format(CLOUD_ELEVATOR_ITEM_IP, i)] = linkageCtrlInfo.CloudElevatorItems[i].DevIp;
                        rows[0][string.Format(CLOUD_ELEVATOR_ITEM_PROP, i)] = 
                            CommonUtils.GetNameByCloudElevatorProperties(linkageCtrlInfo.CloudElevatorItems[i].CtrlProporties);
                    }
                }
                finally
                {
                    rows[0].EndEdit();
                }
            }
            else
            {
                f_DevicesDataTable.Rows.Add(linkageCtrlInfo.DevId, linkageCtrlInfo.DevMac, linkageCtrlInfo.DevIp,
                    linkageCtrlInfo.SubnetMask, linkageCtrlInfo.GateWay, linkageCtrlInfo.CloudUdpPort,
                    linkageCtrlInfo.ThirdUdpPort, linkageCtrlInfo.CloudElevatorCount,
                    linkageCtrlInfo.CloudElevatorItems[1].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(linkageCtrlInfo.CloudElevatorItems[1].CtrlProporties),
                    linkageCtrlInfo.CloudElevatorItems[2].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(linkageCtrlInfo.CloudElevatorItems[2].CtrlProporties),
                    linkageCtrlInfo.CloudElevatorItems[3].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(linkageCtrlInfo.CloudElevatorItems[3].CtrlProporties),
                    linkageCtrlInfo.CloudElevatorItems[4].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(linkageCtrlInfo.CloudElevatorItems[4].CtrlProporties),
                    linkageCtrlInfo.CloudElevatorItems[5].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(linkageCtrlInfo.CloudElevatorItems[5].CtrlProporties),
                    linkageCtrlInfo.CloudElevatorItems[6].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(linkageCtrlInfo.CloudElevatorItems[6].CtrlProporties),
                    linkageCtrlInfo.CloudElevatorItems[7].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(linkageCtrlInfo.CloudElevatorItems[7].CtrlProporties),
                    linkageCtrlInfo.CloudElevatorItems[8].DevIp,
                    CommonUtils.GetNameByCloudElevatorProperties(linkageCtrlInfo.CloudElevatorItems[8].CtrlProporties)
                    );

            }
            //排序
            f_DevicesDataTable.DefaultView.Sort = string.Format("{0} {1}", DEV_ID, "ASC");
            ControlUtilityTool.AdjustIndicatorWidth(this.gvDevices);
        }

        protected override void AnalysisRecvDataEx(string strCmdStr)
        {
            //报文格式F2 XX XX ... XX XX F3
            //判断设备类型是否合法
            DeviceType devType = CommandProcesserHelper.GetDevTypeByCmdInfo(StrUtils.CopySubStr(strCmdStr, 6, 2));
            if (devType != DeviceType.LinkageCtrl)
            {
                return;
            }
            //根据命令字，解析对应命令
            string strCmdWord = StrUtils.CopySubStr(strCmdStr, 14, 4);
            if (strCmdWord == AppConst.CMD_WORD_SEARCH_DEVIDES)
            {
                //判断报文长度
                //取云电梯数量
                int cloudElevatorCount = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdStr, 64, 2), 0, 16);
                if (strCmdStr.Length < 70 + cloudElevatorCount * 10) //35 + cloudElevatorCount 字节
                {
                    HintProvider.ShowAutoCloseDialog(null, string.Format("报文长度错误，报文：{0}", strCmdStr));
                    return;
                }
                this.AnalysisSearchDevicesRecvData(strCmdStr);
            }
            else if (strCmdWord == AppConst.CMD_WORD_SET_CLOUD_ELEVATOR_PARAMS)
            {
                //判断报文长度
                if (strCmdStr.Length < 24) //12字节
                {
                    HintProvider.ShowAutoCloseDialog(null, string.Format("报文长度错误，报文：{0}", strCmdStr));
                    return;
                }
                this.AnalysisDownParams(strCmdStr);
            }

            //this.UpdateHintInfo(strCmdStr);
        }

        private void AnalysisSearchDevicesRecvData(string strCmdStr)
        {
            //取返回的数据部分
            string strCmdReport = StrUtils.CopySubStr(strCmdStr, 18, strCmdStr.Length - 22);
            LinkageCtrlInfo linkageCtrlInfo = new LinkageCtrlInfo();

            //设备ID
            linkageCtrlInfo.DevId = CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4));
            //Mac地址
            linkageCtrlInfo.DevMac = CommonUtils.GetMacByHex(StrUtils.CopySubStr(strCmdReport, 2, 12));
            //设备IP
            linkageCtrlInfo.DevIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 14, 8));
            //子网掩码
            linkageCtrlInfo.SubnetMask = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 22, 8));
            //网关
            linkageCtrlInfo.GateWay = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 30, 8));
            //云电梯UDP端口
            linkageCtrlInfo.CloudUdpPort = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 38, 4), 0, 16);
            //第三方对讲机UDP端口
            linkageCtrlInfo.ThirdUdpPort = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 42, 4), 0, 16);
            //云电梯数量
            linkageCtrlInfo.CloudElevatorCount = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 46, 2), 0, 16);
            //8个云电梯设备信息
            for(int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
            {
                if(i <= linkageCtrlInfo.CloudElevatorCount)
                {
                    linkageCtrlInfo.CloudElevatorItems[i].DevIp = CommonUtils.GetIPByHex(StrUtils.CopySubStr(strCmdReport, 48 + (i - 1) * 10, 8));
                    linkageCtrlInfo.CloudElevatorItems[i].CtrlProporties = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 48 + 8 + (i - 1) * 10, 2), 0, 16);
                }
                else
                {
                    linkageCtrlInfo.CloudElevatorItems[i].DevIp = string.Empty;
                    linkageCtrlInfo.CloudElevatorItems[i].CtrlProporties = -1;
                }
            }

            this.AddOneDeviceToUI(linkageCtrlInfo);
        }

        private void AnalysisDownParams(string strCmdStr)
        {
            //取返回的数据部分
            string strCmdReport = StrUtils.CopySubStr(strCmdStr, 18, strCmdStr.Length - 22);

            //判断设备Id是否正确
            int devId = CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4));
            if (devId != f_CurrentLinkageCtrlInfo.DevId)
            {
                return;
            }
            //获取返回的状态
            int intStatus = StrUtils.StrToIntDef(StrUtils.CopySubStr(strCmdReport, 0, 2), -1, 16);
            switch (intStatus)
            {
                case 0x00:
                    {
                        this.AddOneDeviceToUI(f_CurrentLinkageCtrlInfo);
                        HintProvider.ShowAutoCloseDialog(null, "参数设置成功");
                    }
                    break;
                case 0x01:
                    {
                        HintProvider.ShowAutoCloseDialog(null, "参数设置失败");
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

        protected override void DownParams()
        {
            LinkageCtrlInfo linkageCtrlInfo = GetLinkageCtrlInfoBeforeDown();
            if (linkageCtrlInfo == null)
            {
                return;
            }
            //检测IP地址重复
            if(!this.CheckIPRepeat(linkageCtrlInfo))
            {
                return;
            }
            string strWriteCmd = this.GetWriteCmdStr(linkageCtrlInfo);
            //判断长度
            if (strWriteCmd.Length < 36 + linkageCtrlInfo.CloudElevatorCount * 10) //18 + linkageCtrlInfo.CloudElevatorCount *5 字节
            {
                HintProvider.ShowAutoCloseDialog(null, "生成的报文长度错误，请检查设置的参数是否正确");
                return;
            }
            string writeReport = this.GetWriteReport(linkageCtrlInfo.DevId, AppConst.CMD_WORD_SET_LINKAGE_CTRL_PARAMS, strWriteCmd);
            string errMsg = string.Empty;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT));
            if (!this.UdpListener.SendData(endpoint, writeReport, ref errMsg))
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("设置参数失败，错误：{0}", errMsg));
            }
            f_CurrentLinkageCtrlInfo = linkageCtrlInfo;

            //发送报文成功开始计时
            this.BeginTick = Environment.TickCount;
            this.CoolingTick = 600;
            this.IsBusy = true;
            this.tmrCommunication.Start();
        }

        private bool CheckIPRepeat(LinkageCtrlInfo linkageCtrlInfo)
        {
            Dictionary<string, int> ipList = new Dictionary<string, int>();
            for(int i = 1; i <= linkageCtrlInfo.CloudElevatorCount; i++)
            {
                string ip = linkageCtrlInfo.CloudElevatorItems[i].DevIp;
                if(ipList.ContainsKey(ip))
                {
                    int devId = ipList[ip];
                    HintProvider.ShowAutoCloseDialog(null, string.Format("{0}号与{1}号云电梯IP地址重复!", devId, i));
                    return false;
                }
                ipList.Add(ip, i);
            }
            return true;
        }

        private string GetWriteCmdStr(LinkageCtrlInfo linkageCtrlInfo)
        {
            //协议版本，目前固定位01
            string strCmdStr = "01";
            //设备Ip
            strCmdStr += CommonUtils.GetHexByIP(linkageCtrlInfo.DevIp);
            //子网掩码
            strCmdStr += CommonUtils.GetHexByIP(linkageCtrlInfo.SubnetMask);
            //网关
            strCmdStr += CommonUtils.GetHexByIP(linkageCtrlInfo.GateWay);
            //云电梯UDP端口
            strCmdStr += StrUtils.IntToHex(linkageCtrlInfo.CloudUdpPort, 4);
            //第三方对讲机UDP端口
            strCmdStr += StrUtils.IntToHex(linkageCtrlInfo.ThirdUdpPort, 4);
            //云电梯数量
            strCmdStr += StrUtils.IntToHex(linkageCtrlInfo.CloudElevatorCount, 2);
            //8个云电梯设备信息
            for(int i = 1; i <= linkageCtrlInfo.CloudElevatorCount; i++)
            {
                strCmdStr += CommonUtils.GetHexByIP(linkageCtrlInfo.CloudElevatorItems[i].DevIp);
                strCmdStr += StrUtils.IntToHex(linkageCtrlInfo.CloudElevatorItems[i].CtrlProporties, 2);
            }
            //获取mac值
            strCmdStr += KeyMacOperate.GetMacEx(strCmdStr);
            return strCmdStr;
        }

        private LinkageCtrlInfo GetLinkageCtrlInfoBeforeDown()
        {
            if (!this.CheckUIValid())
            {
                return null;
            }
            LinkageCtrlInfo linkageCtrlInfo = this.GetLinkageCtrlInfoByFocusRow();
            if (linkageCtrlInfo == null)
            {
                return null;
            }
            linkageCtrlInfo.DevIp = this.edtDevIp.Text.Trim();
            linkageCtrlInfo.SubnetMask = this.edtSubnetMark.Text.Trim();
            linkageCtrlInfo.GateWay = this.edtGateWay.Text.Trim();
            linkageCtrlInfo.CloudUdpPort = StrUtils.StrToIntDef(this.edtCloudUdpPort.Text.Trim(), 0);
            linkageCtrlInfo.ThirdUdpPort = StrUtils.StrToIntDef(this.edtThirdUdpPort.Text.Trim(), 0);
            linkageCtrlInfo.CloudElevatorCount = this.cmbCloudCtrlCount.SelectedIndex;
            for(int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
            {
                if(i <= linkageCtrlInfo.CloudElevatorCount)
                {
                    linkageCtrlInfo.CloudElevatorItems[i].DevIp = f_UCloudElevatorItems[i].DevIp;
                    linkageCtrlInfo.CloudElevatorItems[i].CtrlProporties = f_UCloudElevatorItems[i].DevPropIndex;
                }
                else
                {
                    linkageCtrlInfo.CloudElevatorItems[i].DevIp = string.Empty;
                    linkageCtrlInfo.CloudElevatorItems[i].CtrlProporties = -1;
                }
            }
            return linkageCtrlInfo;
        }

        private bool CheckUIValid()
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
            if (string.IsNullOrWhiteSpace(this.edtCloudUdpPort.Text.Trim()))
            {
                HintProvider.ShowAutoCloseDialog(null, "云电梯UDP端口不能为空");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.edtThirdUdpPort.Text.Trim()))
            {
                HintProvider.ShowAutoCloseDialog(null, "第三方对讲UDP端口不能为空");
                return false;
            }
            for(int i = 1; i <= this.cmbCloudCtrlCount.SelectedIndex; i++)
            {
                if(string.IsNullOrWhiteSpace(f_UCloudElevatorItems[i].DevIp))
                {
                    HintProvider.ShowAutoCloseDialog(null, string.Format("{0}号云电梯IP地址不能为空", i));
                    return false;
                }
            }
            return true;
        }

        protected override void ExcuteDoubleClick()
        {
            LinkageCtrlInfo linkageCtrlInfo = this.GetLinkageCtrlInfoByFocusRow();
            if (linkageCtrlInfo == null)
            {
                return;
            }
            this.edtDevIp.Text = linkageCtrlInfo.DevIp;
            this.edtSubnetMark.Text = linkageCtrlInfo.SubnetMask;
            this.edtGateWay.Text = linkageCtrlInfo.GateWay;
            this.edtCloudUdpPort.Text = linkageCtrlInfo.CloudUdpPort.ToString();
            this.edtThirdUdpPort.Text = linkageCtrlInfo.ThirdUdpPort.ToString();
            this.cmbCloudCtrlCount.SelectedIndex = linkageCtrlInfo.CloudElevatorCount;
            for(int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
            {
                if(i <= linkageCtrlInfo.CloudElevatorCount)
                {
                    f_UCloudElevatorItems[i].DevIp = linkageCtrlInfo.CloudElevatorItems[i].DevIp;
                    f_UCloudElevatorItems[i].DevPropIndex = linkageCtrlInfo.CloudElevatorItems[i].CtrlProporties;
                    f_UCloudElevatorItems[i].ConStatues = (int)CloudElevatorItemUserControl.ConnectStatues.UNShow;
                }
                else
                {
                    f_UCloudElevatorItems[i].DevIp = string.Empty;
                    f_UCloudElevatorItems[i].DevPropIndex = -1;
                }
            }
        }

        private LinkageCtrlInfo GetLinkageCtrlInfoByFocusRow()
        {
            if (this.gvDevices.FocusedRowHandle < 0)
            {
                return null;
            }
            LinkageCtrlInfo linkageCtrlInfo = new LinkageCtrlInfo()
            {
                DevId = StrUtils.StrToIntDef(this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, DEV_ID).ToString(), 0),
                DevMac = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, DEV_MAC).ToString(),
                DevIp = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, DEV_IP).ToString(),
                SubnetMask = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, SUBNET_MARK).ToString(),
                GateWay = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, GATE_WAY).ToString(),
                CloudUdpPort = StrUtils.StrToIntDef(this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, CLOUD_ELEVATOR_UDP_PORT).ToString(), 0),
                ThirdUdpPort = StrUtils.StrToIntDef(this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, THIRD_INTERCOM_UDP_PORT).ToString(), 0),
                CloudElevatorCount = StrUtils.StrToIntDef(this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, CLOUD_ELEVATOR_COUNT).ToString(), 0),
            };
            for(int i = 1; i <= linkageCtrlInfo.CloudElevatorCount; i++)
            {
                linkageCtrlInfo.CloudElevatorItems[i].DevIp = this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, string.Format(CLOUD_ELEVATOR_ITEM_IP, i)).ToString();
                linkageCtrlInfo.CloudElevatorItems[i].CtrlProporties = CommonUtils.GetCloudElevatorPropertiesByName(this.gvDevices.GetRowCellValue(this.gvDevices.FocusedRowHandle, string.Format(CLOUD_ELEVATOR_ITEM_PROP, i)).ToString());
            }
            return linkageCtrlInfo;
        }
    }
}
