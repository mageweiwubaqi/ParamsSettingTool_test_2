using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.BandedGrid;
using ITL.ParamsSettingTool.SettingCenter;
using ITL.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ITL.ParamsSettingTool
{
    public partial class CloudFloorTableQuickSetForm : Form
    {
        public string f_QuickSetInfo_StratAuthFlag = string.Empty;
        public string f_QuickSetInfo_EndAuthFlag = string.Empty;
        public string f_QuickSetInfo_StartDevNo = string.Empty;

        public string f_edt_KeyNameNo = string.Empty;
        public string f_cbbE_Start = string.Empty;

        public bool f_QuickSetInfo_FloorTerminalNo = false;
        public bool f_QuickSetInfo_CheckFloor = false;
        public bool f_QuickSetInfo_FloorName = false;
        public bool f_QuickSetInfo_TerminalNumSlave1 = false;
        public bool f_QuickSetInfo_TerminalNumSlave2 = false;


        public enum QuickSetType
        {
            
        }

        public CloudFloorTableQuickSetForm()
        {
            InitializeComponent();

            InitGrid();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        public void InitGrid()
        {

            cbbE_Start.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;//设置只读
            cBbE_EndAuthFlag.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;//设置只读
            cbbE_StartAuthFlag.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;//设置只读

            string[] strComboBox = new string[] { };

            for (int i = 1; i <= 112; i++)
            {
                cbbE_StartAuthFlag.Properties.Items.Add(i.ToString());
                cBbE_EndAuthFlag.Properties.Items.Add(i.ToString());
                cbbE_Start.Properties.Items.Add(i.ToString());
            }

            rdogrpSetType.SelectedIndex = 0;
        }
        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_QsetcloudFloorTable_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_QSetcloudFloorTable_OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.cbbE_StartAuthFlag.Text.Trim()))
            {
                HintProvider.ShowAutoCloseDialog(null, "开始标识不能为空");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.cBbE_EndAuthFlag.Text.Trim()))
            {
                HintProvider.ShowAutoCloseDialog(null, "结束标识不能为空");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.cbbE_Start.Text.Trim()))
            {
                HintProvider.ShowAutoCloseDialog(null, "开始标号不能为空");
                return;
            }
            //if((!RaBtn_DevNo.Checked) && (!RaBtn_CheckFloor.Checked) && (!RaBtn_KeyName.Checked))
            //{
            //    HintProvider.ShowAutoCloseDialog(null, "请选择类别");
            //    return;
            //}
            f_QuickSetInfo_StratAuthFlag = cbbE_StartAuthFlag.Text;
            f_QuickSetInfo_EndAuthFlag = cBbE_EndAuthFlag.Text;
            f_QuickSetInfo_StartDevNo = cbbE_Start.Text;

            int StartAuthFlag = int.Parse(f_QuickSetInfo_StratAuthFlag);
            int EndAuthFlag = int.Parse(f_QuickSetInfo_EndAuthFlag);

            string errMsg = "结束标识不能小于开始标识";
            if(EndAuthFlag < StartAuthFlag)
            {
                HintProvider.ShowAutoCloseDialog(null, string.Format("错误：{0}", errMsg));

            }
            else
            {
                this.Close();
                this.DialogResult = DialogResult.OK;
            }

        }
        public string QuickSetInfo_StratAuthFlag
        {
            set { f_QuickSetInfo_StratAuthFlag = value; }
            get { return f_QuickSetInfo_StratAuthFlag; }
        }
        public string QuickSetInfo_EndAuthFlag
        {
            set { f_QuickSetInfo_EndAuthFlag = value; }
            get { return f_QuickSetInfo_EndAuthFlag; }
        }
        public string QuickSetInfo_StartDevNo
        {
            set { f_QuickSetInfo_StartDevNo = value; }
            get { return f_QuickSetInfo_StartDevNo; }
        }


        private void rdogrpSetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            f_QuickSetInfo_FloorTerminalNo = false;
            f_QuickSetInfo_CheckFloor = false;
            f_QuickSetInfo_FloorName = false;
            f_QuickSetInfo_TerminalNumSlave1 = false;
            f_QuickSetInfo_TerminalNumSlave2 = false;

            int selectedIndex = rdogrpSetType.SelectedIndex;
            var selectedValue = rdogrpSetType.Properties.Items[selectedIndex].Value.ToString();
            switch (selectedValue)
            {
                case nameof(FloorRelationUIObject.FloorName):
                    {
                        f_QuickSetInfo_FloorName = true;
                        lbc_StartNameNo.Text = "起始按键名称：";
                        break;
                    }

                case nameof(FloorRelationUIObject.CheckFloor):
                    {
                        f_QuickSetInfo_CheckFloor = true;
                        lbc_StartNameNo.Text = "起始检测楼层：";
                        break;
                    }
                case nameof(FloorRelationUIObject.FloorTerminalNo):
                    {
                        f_QuickSetInfo_FloorTerminalNo = true;
                        lbc_StartNameNo.Text = "起始端子号：";
                        break;
                    }
                case nameof(FloorRelationUIObject.TerminalNumSlave1):
                    {
                        f_QuickSetInfo_TerminalNumSlave1 = true;
                        lbc_StartNameNo.Text = "起始第一副操纵盘端子号：";
                        break;
                    }
                case nameof(FloorRelationUIObject.TerminalNumSlave2):
                    {
                        f_QuickSetInfo_TerminalNumSlave2 = true;
                        lbc_StartNameNo.Text = "起始第二副操纵盘端子号：";
                        break;
                    }

                default:
                    break;
            }

            f_cbbE_Start = cbbE_Start.Text.ToString();
        }
    }
}
