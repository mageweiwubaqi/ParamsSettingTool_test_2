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
using ITL.DataDefine;
using ITL.Framework;

namespace ITL.ParamsSettingTool
{
    public partial class CloudElevatorItemUserControl : GeneralUserControl
    {
        private int f_ItemId = 0;
        //private string f_DevIp = string.Empty;
        //private int f_DevPropIndex = -1;

       
        private int f_ConStatues = 0;
        public int ItemId
        {
            get
            {
                return f_ItemId;
            }
            set
            {
                if(f_ItemId != value)
                {
                    f_ItemId = value;
                    this.liDevIp.Text = string.Format("{0}号云电梯IP地址", f_ItemId);
                }
            }
        }

        public string DevIp
        {
            get
            {
                return this.edtDevIp.Text.Trim();
            }
            set
            {
                this.edtDevIp.Text = value;
            }
        }

        public enum ConnectStatues
        {
            /// <summary>
            /// 未连接
            /// </summary>
            [Description("UnConnected")]
            UnConnected = 0,
            /// <summary>
            /// 已连接
            /// </summary>
            [Description("Connected")]
            Connected = 1,
            /// <summary>
            /// 不显示
            /// </summary>
            [Description("UNShow")]
            UNShow = 9

        }

        /// <summary>
        /// 值为1表示已连接，为9不显示，其余未连接
        /// </summary>
        public int ConStatues
        {
            set
            {
                f_ConStatues = value;
                if (f_ConStatues == (int)ConnectStatues.UnConnected)                   
                {
                    this.lblConStatues.Text = "未连接";
                    this.lblConStatues.ForeColor = Color.Red;
                }
                else if (f_ConStatues == (int)ConnectStatues.Connected)
                {
                    this.lblConStatues.Text = "已连接";
                    this.lblConStatues.ForeColor = Color.Green;
                }
             }
        }

        public int DevPropIndex
        {
            get
            {
                return this.cmbCtrlProperties.SelectedIndex;
            }
            set
            {
                this.cmbCtrlProperties.SelectedIndex = value;
            }
        }


        public CloudElevatorItemUserControl()
        {
            InitializeComponent();
            lblConStatues.Text = " ";
        }

        protected override void InitUIOnLoad()
        {
            base.InitUIOnLoad();

            ControlUtilityTool.SetPanelControlBorderLines(this.pnlContainer, true, true, true, true);
            ControlUtilityTool.SetITLLayOutControlStyle(this.lcItem);
            ControlUtilityTool.SetITLTextEditStyle(this.edtDevIp);
            ControlUtilityTool.SetTextEditIPRegEx(this.edtDevIp);
            ControlUtilityTool.SetITLComboBoxEditStyle(this.cmbCtrlProperties);

            this.edtDevIp.Properties.MaxLength = 15;
            this.edtDevIp.KeyPress += CommonUtils.edtIp_KeyPress;

            this.InitDevPropCmbEdit();
        }

        protected override void InitUIEvents()
        {
            base.InitUIEvents();
            this.edtDevIp.Leave += this.edtDevIp_Leave;
        }


        /// <summary>
        /// 设备IP编辑框离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edtDevIp_Leave(object sender, EventArgs e)
        {

            //A类ip（1.0.0.0—127.255.255.255）默认掩码 255.0.0.0
            //B类ip（128.0.0.0—191.255.255.255）默认掩码 255.255.0.0
            //C类ip（192.0.0.0—223.255.255.255）默认掩码 255.255.255.0
            List<string> ips = this.edtDevIp.EditValue?.ToString()?.Split(".".ToCharArray())?.ToList();
            if (ips != null && ips.Count > 0)
            {
                int ip1 = StrUtils.StrToIntDef(ips.First());              
            }
        }


        private void InitDevPropCmbEdit()
        {
            this.cmbCtrlProperties.Properties.Items.Clear();
            this.cmbCtrlProperties.Properties.Items.Add(AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_ZERO);
            this.cmbCtrlProperties.Properties.Items.Add(AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_ONE);
            this.cmbCtrlProperties.Properties.Items.Add(AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_TWO);
            this.cmbCtrlProperties.Properties.Items.Add(AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_THREE);
            this.cmbCtrlProperties.SelectedIndex = -1;
        }
       
    }
}
