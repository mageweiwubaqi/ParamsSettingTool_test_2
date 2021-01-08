using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ITL.General;
using ITL.Public;

namespace ITL.ParamsSettingTool
{
    public partial class InputPsdForm : GeneralForm
    {
        public InputPsdForm()
        {
            InitializeComponent();
        }

        protected override void InitUIOnShown()
        {
            HintProvider.WaitingDone(Application.ProductName);
            base.InitUIOnShown();

            ControlUtilityTool.SetITLTextEditStyle(this.edtPsd);
            ControlUtilityTool.SetSuperToolTip(this.edtPsd, "密码必须由8位纯数字组成!");

            this.rgpPsd.SelectedIndex = 1;
            this.edtPsd.Properties.MaxLength = 8;
            this.edtPsd.KeyPress += CommonUtils.edtPort_KeyPress; //只能输入整形
            ControlUtilityTool.SetFocus(this.edtPsd);
            char a = '*';
            List<char> lisrt = new List<char>();
            lisrt.Add(a);
            //指定密码框样式
            this.edtPsd.Properties.PasswordChar = lisrt[0];//采用的样式
            
        }

        protected override void InitUIEvents()
        {
            base.InitUIEvents();
            this.rgpPsd.SelectedIndexChanged += this.rgpPsd_SelectedIndexChanged;
        }

        private void rgpPsd_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.rgpPsd.SelectedIndex)
            {
                case 0:
                    {
                        this.edtPsd.Visible = false;
                        this.btnOK.Text = "登  录";
                    }
                    break;
                case 1:
                    {
                        this.edtPsd.Text = string.Empty;
                        //this.edtPsd.Text = KeyMacOperate.GetMacEx("0100C0A87BD1FFFFFF00C0A87BFED3A24E01000000001A0AEB2A00000126");
                        this.edtPsd.Visible = true;
                        this.btnOK.Text = "保  存";
                        ControlUtilityTool.SetFocus(this.edtPsd);
                    }
                    break;
                default:
                    break;
            }
        }

        private bool CheckPsdValid()
        {
            if(this.rgpPsd.SelectedIndex == 0)
            {
                return true;
            }
            string psd = this.edtPsd.Text.Trim();
            if(psd.Length != 8)
            {
                HintProvider.ShowAutoCloseDialog(this, "密码必须由8位纯数字组成!");
                return false;
            }
            //密码不能为8个0,8个0为默认密码
            if(psd.Equals(KeyMacOperate.DEFAULT_SYSTEM_PSD))
            {
                HintProvider.ShowAutoCloseDialog(this, "密码不能为8个0,请重新输入!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(psd))
            {
                HintProvider.ShowAutoCloseDialog(this, "密码不能为空!");
                return false;
            }
            char[] charArr = psd.ToArray();
            if (charArr.Length < 6 || charArr.Length > 20)
            {
                HintProvider.ShowAutoCloseDialog(this, "密码必须6-20位之间!");
                return false;
            }
            int count = charArr.Length;
            int oneIndex = 1;
            int twoIndex = 1;
            int threeIndex = 1;
            for (int i = 1; i < count; i++)
            {
                int beforValue = int.Parse(charArr[i - 1].ToString());
                int curValue = int.Parse(charArr[i].ToString());
                if (beforValue == curValue)
                {
                    oneIndex++;
                }
                if (beforValue + 1 == curValue)
                {
                    twoIndex++;
                }
                if (beforValue == curValue + 1)
                {
                    threeIndex++;
                }
            }
            if (oneIndex == count)
            {
                HintProvider.ShowAutoCloseDialog(this, "密码不能为相同的数字!");
                return false;
            }
            if (twoIndex == count)
            {
                HintProvider.ShowAutoCloseDialog(this, "密码不能为顺序的连续数字!");
                return false;
            }
            if (threeIndex == count)
            {
                HintProvider.ShowAutoCloseDialog(this, "密码不能为逆序的连续数字!");
                return false;
            }
            return true;
        }

        private void SaveSystemPsd()
        {
            string psd = string.Empty;
            switch (this.rgpPsd.SelectedIndex)
            {
                case 0:
                    {
                        psd = KeyMacOperate.DEFAULT_SYSTEM_ENCRY_PSD;
                    }
                    break;
                case 1:
                    {
                        psd = this.edtPsd.Text.Trim();
                        psd = KeyMacOperate.GetEncryKey(psd);
                    }
                    break;
                default:
                    {
                        psd = KeyMacOperate.DEFAULT_SYSTEM_ENCRY_PSD;
                    }
                    break;
            }
            AppEnv.Singleton.SystemPsd = psd;
            AppXmlConfig.Singleton[AppXmlConfig.SYSTEM_PSD] = psd;
            AppXmlConfig.Singleton.Save();
        }

        protected override void ExcuteOKPerformClick()
        {          
            if(!this.CheckPsdValid())
            {
                return;
            }
            this.SaveSystemPsd();
            this.DialogResult = DialogResult.OK;
        }


    }
}
