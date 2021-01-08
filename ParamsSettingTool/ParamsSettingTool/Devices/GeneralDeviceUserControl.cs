using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ITL.General;
using ITL.Public;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using ITL.DataDefine;
using ITL.Framework;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;

namespace ITL.ParamsSettingTool
{
    //public string SubnetMask { get; set; }

    //public string GateWay { get; set; }

    public partial class GeneralDeviceUserControl : GeneralUserControl
    {
        protected const string IS_SELECT = "is_select";
        protected const string DEV_ID = "dev_id";
        protected const string DEV_MAC = "dev_mac";
        protected const string DEV_IP = "dev_ip";
        protected const string SUBNET_MARK = "subnet_mark";
        protected const string GATE_WAY = "gate_way";
        protected Dictionary<string, hintInfo> FindCount;//Dev_ID IP 搜索次数
        private const string DEV_ID_ALIAS = "设备ID";
        private const string DEV_MAC_ALIAS = "设备MAC地址";
        private const string DEV_IP_ALIAS = "设备IP地址";
        private const string SUBNET_MARK_ALIAS = "子网掩码";  
        private const string GATE_WAY_ALIAS = "网关";
        private bool IsDownParm = false; //是否属于下载参数

        //private const string FMT_STATUS_SELECTED_ZH = "<color={0}>{1}</color>";  //中文

        private object f_Lock = new object();
        protected DataTable f_DevicesDataTable = null;
        private DeviceType f_DevType = DeviceType.None;
        private int f_BeginTick = 0;
        private int f_CoolingTick = 0;
        private bool f_IsBusy = false;
        private int f_SearchDevicesCount = 0;
        private int f_RowNo = 1;
        private UdpListener f_UdpListener = null;
        private string IsDownLoadDevLength = string.Empty;
        private bool IsDoubleClick = false;
       
        public UdpListener UdpListener
        {
            get
            {
                lock (f_Lock)
                {
                    return f_UdpListener;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_UdpListener = value;
                }
            }
        }

        public DeviceType DevType
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DevType;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DevType = value;
                }
            }
        }

        public bool IsBusy
        {
            get
            {
                lock (f_Lock)
                {
                    return f_IsBusy;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_IsBusy = value;
                }
            }
        }

        public bool DownParm
        {
            get
            {
                lock (f_Lock)
                {
                    return IsDownParm;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    IsDownParm = value;
                }
            }
        }

        public int BeginTick
        {
            get
            {
                lock (f_Lock)
                {
                    return f_BeginTick;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_BeginTick = value;
                }
            }
        }

        public int CoolingTick
        {
            get
            {
                lock (f_Lock)
                {
                    return f_CoolingTick;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_CoolingTick = value;
                }
            }
        }

        public GeneralDeviceUserControl()
        {
            InitializeComponent();
        }

        protected override void InitUIOnLoad()
        {
            base.InitUIOnLoad();

            ControlUtilityTool.SetITLGridViewStyle(this.gvDevices, true);
            ControlUtilityTool.SetControlDefaultColor(this.pnlOperateArea, ControlUtilityTool.PubBackColorNormal);
            gvDevices.OptionsBehavior.Editable = true; //不允许对单元格进行编辑
            this.InitDevicesDataTable();
            this.InitDevicesGridView(this.gvDevices);
        }

        protected override void InitUIEvents()
        {
            base.InitUIEvents();
            this.gpcDevices.CustomButtonClick += this.gpcDevicesInfo_CustomButtonClick;
            this.SizeChanged += this.GeneralDeviceUserControl_SizeChanged;
            this.gpcOperateArea.CustomButtonClick += this.gpcOperateArea_CustomButtonClick;
            this.gvDevices.DoubleClick += this.gvDevices_DoubleClick;
            gvDevices.CustomDrawCell += gvDevices_CustomDrawCell;
            gvDevices.ShownEditor += gvDevices_ShownEditor;


        }
        //public string DevId { get; set; }

        //public string DevType { get; set; }


        //public string DevMac { get; set; }

        //public string DevIp { get; set; }


        //public string SubnetMask { get; set; }

        //public string GateWay { get; set; }

        //public string VerSion { get; set; }


        protected virtual void InitDevicesDataTable()
        {
            f_DevicesDataTable = new DataTable();
            f_DevicesDataTable.Columns.Add(DEV_ID, typeof(int));
            f_DevicesDataTable.Columns.Add(DEV_MAC, typeof(string));
            f_DevicesDataTable.Columns.Add(DEV_IP, typeof(string));
            f_DevicesDataTable.Columns.Add(SUBNET_MARK, typeof(string));
            f_DevicesDataTable.Columns.Add(GATE_WAY, typeof(string));
         
            
        }

        protected virtual void InitDevicesGridView(GridView grdView)
        {
            grdView.Columns.Clear();
            //设备ID
            var colNo = ControlUtilityTool.AddGridViewColum(grdView, IS_SELECT, DEV_ID_ALIAS);
            colNo.OptionsColumn.AllowEdit = true;
            colNo.Width = 100;
            colNo.OptionsColumn.FixedWidth = true;
            var chkedt = this.GetRepositoryCheckEdit(grdView);
            chkedt.GlyphAlignment = HorzAlignment.Near;
            colNo.ColumnEdit = chkedt;
            colNo.ShowButtonMode = ShowButtonModeEnum.ShowAlways;

            //设备MAC地址
            this.AddOneGridViewColumn(grdView, DEV_MAC, DEV_MAC_ALIAS, 150);

            //设备IP地址
            this.AddOneGridViewColumn(grdView, DEV_IP, DEV_IP_ALIAS, 120);

            //子网掩码
            this.AddOneGridViewColumn(grdView, SUBNET_MARK, SUBNET_MARK_ALIAS, 120);

            //网关
            this.AddOneGridViewColumn(grdView, GATE_WAY, GATE_WAY_ALIAS, 120);

        }

        protected GridColumn AddOneGridViewColumn(GridView grdView, string fieldName, string caption, int columnWidth)
        {
            GridColumn oneColumn = ControlUtilityTool.AddGridViewColum(grdView, fieldName, caption);
            oneColumn.OptionsColumn.AllowFocus = false;   //去掉焦点
            oneColumn.Width = columnWidth;
            oneColumn.OptionsColumn.FixedWidth = true;
            oneColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            oneColumn.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            return oneColumn;
        }

        /// <summary>
        /// 向指定GridView中增加一列
        /// </summary>
        /// <param name="gridView">目标gridview</param>
        /// <param name="fieldName">列名</param>
        /// <param name="capiton">显示名</param>
        /// <returns></returns>
        protected GridColumn AddGridViewColum(ColumnView gridView, string fieldName, string capiton = "")
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
        /// <summary>
        /// 设置columnEdit = CheckEdit
        /// </summary>
        /// <returns></returns>
        private RepositoryItemCheckEdit GetRepositoryCheckEdit(GridView grdView)
        {
            RepositoryItemCheckEdit chk = new RepositoryItemCheckEdit();
            chk.AllowFocused = false;
            chk.NullStyle = StyleIndeterminate.Unchecked;
            chk.EditValueChanged += (sender, e) =>
            {
                grdView.PostEditor();
            };
            return chk;
        }
        /// <summary>
        /// 搜索设备
        /// </summary>
        private bool SearchDevices()
        {            
            string writeReport = this.GetWriteReport(0, AppConst.CMD_WORD_SEARCH_DEVIDES,AppEnv.Singleton.SystemPsd);
            string errMsg = string.Empty;

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, 
                StrUtils.StrToIntDef(AppXmlConfig.Singleton[AppXmlConfig.UDP_PURPOSE_PORT].ToString(), AppConst.UDP_PURPOSE_PORT)
                );
            if (!this.UdpListener.SendData(endpoint, writeReport, ref errMsg))
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("搜索设备失败，错误：{0}", errMsg));
                return false;
            }
            return true;                   
        }

        /// <summary>
        /// 获取命令报文
        /// </summary>
        /// <param name="devId"></param>
        /// <param name="commandWord"></param>
        /// <param name="sendData"></param>
        /// <returns></returns>
        protected string GetWriteReport(int devId, string cmdWord, string sendData)
        {
            string strCmdData = string.Empty;

            int ctrlNo = devId;
            int groupID = 0;

            if(devId == 0)
            {
                groupID = 0;
                ctrlNo = 0;
            }
            else
            {
                groupID = ((ctrlNo - 1) / 0xFF) + 1;
                ctrlNo = (ctrlNo % 0xFF);
                //解决255的倍数问题
                if (ctrlNo == 0)
                {
                    ctrlNo = 0xFF;
                }
            }           
            
            //设备组号
            string strDevGroupID = StrUtils.IntToHex(groupID, 2);
            //设备地址
            string strDevCtrlID = StrUtils.IntToHex(ctrlNo, 2);
            //设备类型
            string strDevType = StrUtils.IntToHex((int)this.DevType, 2);
            //流水号
            string strCmdSerialNo = this.GetCmdNumber();
            //命令字
            string strCmdWord = cmdWord;
            //生成固定部分的命令报文
            strCmdData = strDevGroupID + strDevCtrlID + StrUtils.IntToHex((int)DevType, 2) + strCmdSerialNo + strCmdWord + sendData;
            // 计算校验位
            string strXorCheck = StrUtils.GetXORCheck(strCmdData);

            RunLog.Log("************CheckXor strCmdData : " + strCmdData);
            // 命令报文＋校验位
            strCmdData = strCmdData + strXorCheck;

            // 格式化命令报文，如果报文中出现>= 0xF0的数据，则做特定处理
            // 特别注意：校验位也需检测

            strCmdData = CommandProcesserHelper.AddF0Escape(strCmdData);

            RunLog.Log("************ AddF0Escape strCmdData :" + strCmdData);

            
            // 组合报文，加上起始字和结束字
            strCmdData = CommandProcesserHelper.CMD_START_FLAG + strCmdData + CommandProcesserHelper.CMD_END_FLAG;

            IsDownLoadDevLength = strCmdData;
            RunLog.Log(strCmdData);
            return strCmdData;
        }

        private string GetCmdNumber()
        {
            if(AppEnv.Singleton.CmdNumber < 0xEF)
            {
                AppEnv.Singleton.CmdNumber += 1;
            }
            else
            {
                AppEnv.Singleton.CmdNumber = 0;
            }
            return StrUtils.IntToHex(AppEnv.Singleton.CmdNumber, 2);
        }

        public void RecvCallBack(UdpClient client, IPEndPoint ipEndPoint, byte[] recvBuf)
        {
            if (!this.IsHandleCreated)
            {
                return;

            }

            string recvData = StrUtils.BytesToHexStr(recvBuf);

            //if (IsDownLoadDevLength.Length > 1200)
            //{
            //    recvData = IsDownLoadDevLength;
            //    //return;
            //    IsDownLoadDevLength = string.Empty;
            //}
            RunLog.Log("Rcv:" + recvData);
            //string recvData = StrUtils.BytesToHexStr(recvBuf);
            this.BeginInvoke(new Action<string>(this.AnalysisRecvData), recvData);          
        }

        private void AnalysisRecvData(string strData)
        {
            //非忙碌状态，不解析数据
            //if(!this.IsBusy)
            //{
            //    return;
            //}
            //去F0
            string strCmdStr = CommandProcesserHelper.DelF0Escape(strData);
            //判断报文长度
            if (strCmdStr.Length < 18)
            {
                RunLog.Log(string.Format("返回的报文长度错误，报文：{0}", strCmdStr));
                HintProvider.ShowAutoCloseDialog(null, string.Format("返回的报文校验错误，错误：{0}", strCmdStr), HintIconType.Err);
                return;
            }
            //判断校验码
            string requiredCheckValue = StrUtils.GetXORCheck(strCmdStr, 2, strCmdStr.Length - 6).ToUpper();
            string factCheckValue = StrUtils.CopySubStr(strCmdStr, strCmdStr.Length - 4, 2);
            if (string.Compare(requiredCheckValue, factCheckValue, true) != 0)
            {
                RunLog.Log(string.Format("返回的报文校验错误，报文：{0}", strCmdStr));
                HintProvider.ShowAutoCloseDialog(null, string.Format("返回的报文校验错误，错误：{0}", strCmdStr), HintIconType.Err);

                return;
            }
            //判断返回的命令状态
            string strCmdStatus = StrUtils.CopySubStr(strCmdStr, 12, 2);
            //string strCmdStatus = StrUtils.CopySubStr(strCmdStr, 10, 2);
            if (StrUtils.HexStrToInt(strCmdStatus) != CommonUtils.RES_OK)
            {
                if (strCmdStatus == "7F")
                {
                    string strCmdStatus2 = StrUtils.CopySubStr(strCmdStr, 18, 2);
                    if (strCmdStatus2 == "02")
                    {
                        HintProvider.ShowAutoCloseDialog(null, string.Format("需要初始化设备，才能重新设置网络参数"));
                        return;
                    }
                }

                RunLog.Log(string.Format("命令状态错误：{0}", CommonUtils.GetErrMsgByCode(StrUtils.HexStrToInt(strCmdStatus))));

                string Error = string.Format("命令状态错误：{0}", CommonUtils.GetErrMsgByCode(StrUtils.HexStrToInt(strCmdStatus)));
                HintProvider.ShowAutoCloseDialog(null, string.Format(Error), HintIconType.Err);

                return;
            }
            else if (StrUtils.HexStrToInt(strCmdStatus) == CommonUtils.RES_OK)
            {
                string strDevID =  CommandProcesserHelper.GetDevIDByCmdInfo(StrUtils.CopySubStr(strCmdStr, 2, 4)).ToString();
                if (IsDownParm || AppConst.IsDownParmCloudLinkage)
                {
                    HintProvider.ShowAutoCloseDialog(null, string.Format("下载成功"));
                    AppConst.IsDownParmCloudLinkage = false;
                    IsDownParm = false;
                }
                
            }

            DeviceType devType = CommandProcesserHelper.GetDevTypeByCmdInfo(StrUtils.CopySubStr(strCmdStr, 6, 2));
            if (devType == DeviceType.CloudLinkageInfoCtrl)
            {
                //云联动器  
                string strCmdStatusLink = StrUtils.CopySubStr(strCmdStr, 15, 4);

                if (strCmdStatusLink == "0121" || strCmdStatusLink == "012E" || strCmdStatusLink == "0100")
                {
                    HintProvider.ShowAutoCloseDialog(null, "下载成功", HintIconType.OK);
                    Thread.Sleep(1000);
                }
            }

            //重置
            IsDownLoadDevLength = string.Empty;

            this.AnalysisRecvDataEx(strCmdStr);
        }

        protected virtual void AnalysisRecvDataEx(string strCmdStr)
        {

        }

        private void gpcDevicesInfo_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            if (e.Button.Properties.Caption == "搜索设备")
            {
                if(!this.ExcuteSearchDevices())
                {
                    return;
                }
                //if(this.IsBusy)
                //{
                //    return;
                //}
                //f_DevicesDataTable.Clear();
                //if(!this.SearchDevices())
                //{
                //    return;
                //}
                ////发送报文成功开始计时
                //this.BeginTick = Environment.TickCount;
                //this.CoolingTick = 3000;
                //this.IsBusy = true;
                //f_SearchDevicesCount = 2; //剩余自动广播次数
                HintProvider.StartWaiting(null, "正在搜索设备", "", Application.ProductName, showDelay: 0, showCloseButtonDelay: int.MaxValue);
                //this.tmrCommunication.Start();
            }
        }

        protected bool ExcuteSearchDevices()
        {
            if (this.IsBusy)
            {
                return false;
            }
            FindCount = new Dictionary<string, hintInfo>();
            f_DevicesDataTable.Clear();
        
            if (!this.SearchDevices())
            {
                return false;
            }
            //发送报文成功开始计时
            this.BeginTick = Environment.TickCount;
            this.CoolingTick = 3000;
            this.IsBusy = true;
            f_SearchDevicesCount = 0; //剩余自动广播次数
            //HintProvider.StartWaiting(null, "正在搜索设备", "", Application.ProductName, showDelay: 0, showCloseButtonDelay: int.MaxValue);
            this.tmrCommunication.Start();
            return true;
        }

        protected virtual void GeneralDeviceUserControl_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void gpcOperateArea_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            if (e.Button.Properties.Caption == "下载参数")
            {
                if (this.IsBusy)
                {
                    return;
                }
                this.DownParams();
            }
            //if (e.Button.Properties.Caption == "读取本地楼层对应表")
            //{
            //    IsDownParm = true;
            //}
            //if (e.Button.Properties.Caption == "下载到设备")
            //{
            //    IsDownParm = true;
            //}
        }

        protected virtual void DownParams()
        {
            
        }

        private void gvDevices_DoubleClick(object sender, EventArgs e)
        {
            IsDoubleClick = true;
            
            this.ExcuteDoubleClick();
            
        }

        protected virtual void ExcuteDoubleClick()
        {

        }

        private void tmrCommunication_Tick(object sender, EventArgs e)
        {
            if ((Environment.TickCount - this.BeginTick >= this.CoolingTick)
                || (!this.IsBusy))
            {
                this.tmrCommunication.Stop();
                this.IsBusy = false;
                this.tmrCommunication.Interval = 100;
                HintProvider.WaitingDone(Application.ProductName);
            }
            if(this.tmrCommunication.Interval != 800)
            {
                this.tmrCommunication.Interval = 800;
                return;
            }
            //如果是广播命令，则继续广播，间隔为900
            if(f_SearchDevicesCount > 0)
            {
            //    f_SearchDevicesCount--;
             //   this.SearchDevices();
            }
        }

        /// <summary>
        /// 解决界面变化时闪烁的问题
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;////用双缓冲从下到上绘制窗口的所有子孙
                return cp;
            }
        }

        /// <summary>
        /// 参数下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_DownLoadParm_Click(object sender, EventArgs e)
        {
            if (this.IsBusy)
            {
                return;
            }
        //    IsDownParm = true;
             this.DownParams();
        }

        private void Btn_ReadCloudFloorTable_Click(object sender, EventArgs e)
        {
            
        }
       /// <summary>
       /// 下载楼层对应表
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void Btn_DownLoadDev_Click(object sender, EventArgs e)
        {
            IsDownParm = true;
        }
        /// <summary>
        /// 读取楼层对应表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ReadLocalFloorTable_Click(object sender, EventArgs e)
        {
            //IsDownParm = true;
        }

        private void gvDevices_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            var dev = this.gvDevices.GetRow(e.RowHandle) as DataRowView;
            if (dev == null)
            {
                return;
            }
            if (e.Column == gvDevices.Columns[IS_SELECT])
            {
                DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo viewInfo = ((e.Cell as GridCellInfo).ViewInfo as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo);
                DevExpress.Utils.Drawing.CheckObjectInfoArgs checkInfo = (DevExpress.Utils.Drawing.CheckObjectInfoArgs)viewInfo.CheckInfo;
                checkInfo.Caption = dev[DEV_ID].ToString();
                checkInfo.Graphics = e.Graphics;
                viewInfo.CheckPainter.CalcObjectBounds(checkInfo);
                return;
            }
        }
        //显示电梯编号
        private void gvDevices_ShownEditor(object sender, EventArgs e)
        {
            if (gvDevices.FocusedColumn != gvDevices.Columns[IS_SELECT])
            {
                return;
            }
            var dev = gvDevices.GetRow(gvDevices.FocusedRowHandle) as DataRowView;
            if (dev == null)
            {
                return;
            }
            CheckEdit chkEdt = gvDevices.ActiveEditor as CheckEdit;
            chkEdt.Properties.Caption = dev[DEV_ID].ToString();
        }
    }
    public class hintInfo
    {
        public int FontCount;//次数
        public bool IsHint;//是否提示
    }
}
