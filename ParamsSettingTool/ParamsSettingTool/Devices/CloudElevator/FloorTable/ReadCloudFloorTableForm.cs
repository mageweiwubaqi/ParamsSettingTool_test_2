using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using ITL.Framework;
using ITL.Public;

namespace ITL.ParamsSettingTool
{
    //public partial class ReadCloudFloorTableForm :Form
    //权限标识---naturalFloor
    //按键名称---logicalFloor
    //实际楼层---floorNum
    //端子号---  terminalFloor
    //检测楼层---detectFloor
    public partial class ReadCloudFloorTableForm : Form
    {
        private string f_deviceUnique = string.Empty;
        List<object> list = new List<object>();
        //public ResponseInfo UserControl_FloorTable = null;
        public ResponseInfo FloorTableTest = null;

        public string testStrB = string.Empty;
        public ReadCloudFloorTableForm()
        {
            InitializeComponent();

            TextEdit_ProNo.Properties.MaxLength = 8;
        }
        //关闭
        private void Btn_ReadcloudFloorTable_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //获取项目编号
        private void Btn_ReadcloudFloorTable_GetData_Click(object sender, EventArgs e)
        {
            try
            {
                if(TextEdit_ProNo.Text.Length < 8)
                {
                    HintProvider.ShowAutoCloseDialog(null, string.Format("请输入8位项目编号"));
                    return;
                }
                //string HttpAddress = "15w07q0502.51mypc.cn:15234/smartCard/syncdata/device/elevator/getCloudElevatorList?";//http地址
                string _uSysCode = TextEdit_ProNo.Text;//传递的参数  00000126;

                //string _sAppKey = "";
                DateTime currentTime = System.DateTime.Now;
                string createTime = currentTime.ToString("yyyyMMddHHmmss");
                string _MD5 = "projectId" + createTime + "1177BE55278MMN5412365UHBN85214BE";
                string _sSign = Md5Func(_MD5);

                string postString = "projectId=" + _uSysCode + "&sign=" + _sSign + "&createTime=" + createTime;//传递的参数  00000126
                byte[] postData = Encoding.UTF8.GetBytes(postString);//编码  

                //RunLog.Log("获取项目请求：  ccccccccccccc POSTString  " + postString);

                iniFileControl ReadIniConfig = new iniFileControl(Application.StartupPath + @"\CloudAdd.ini");
                string socketHttp = ReadIniConfig.IniReadValue("云服务配置", "地址");
                //string url = "http://test.smartcard.jia-r.com/smartCard/syncdata/device/elevator/getCloudElevatorList";//地址  
                string url = socketHttp;

                WebClient webClient = new WebClient();
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式
                byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
                string srcString = Encoding.UTF8.GetString(responseData);//解码  

                //RunLog.Log("接收项目：  ccccccccccccc srcString  " + srcString);

                //srcString = CommonUtils.ToHex(srcString, "utf-8", true);
                //CloudFloorTableBaseInfo FloorTableSerializer = JsonConvert.DeserializeObject<CloudFloorTableBaseInfo>(srcString);

                //string Test = Md5Func("projectId201903091355181177BE55278MMN5412365UHBN85214BE");



                comboBox_FloorName.Items.Clear(); //清空列表

                for (int i = 0; i <= 112; i++)
                {
                    try
                    {
                        string JsonData = cutJson(srcString, "data", i);

                        if (JsonData == null)
                        {
                            return;
                        }

                        CloudFloorTableBaseInfo FloorTable = JsonConvert.DeserializeObject<CloudFloorTableBaseInfo>(JsonData);
                        string f_deviceName = FloorTable.deviceName;
                        f_deviceUnique = FloorTable.deviceUnique;

                        //comboBox_FloorName
                        // 添加项目
                        comboBox_FloorName.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboBox_FloorName.Items.Add(f_deviceName);
                        //添加list
                        var Listueq = new { ID = i, deviceName = f_deviceName, deviceUnique = f_deviceUnique };

                        list.Add(Listueq);
                    }
                    catch (Exception ex)
                    {

                        RunLog.Log(ex);
                    }

                }
            }
            catch(Exception elog)
            {
                string errMsg = "获取项目编号失败";
                HintProvider.ShowAutoCloseDialog(null, string.Format("通讯失败，错误：{0}", errMsg));

                RunLog.Log(elog);
            }


            //string JsonData = cutJson(srcString, "data", 1);
            //CloudFloorTableBaseInfo FloorTable = JsonConvert.DeserializeObject<CloudFloorTableBaseInfo>(JsonData);

        }

        public string cutJson(string json, string item, int index)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            string value = jo[item][index].ToString();
            return value;
        }


        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        public static string Md5Func(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = string.Empty;
            for (int i = 0; i < md5Data.Length; i++)
            {
              
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            //使用 PadLeft 和 PadRight 进行轻松地补位
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        public ResponseInfo SendCloudFloorHex
        {
            set { FloorTableTest = value; }
            get { return FloorTableTest; }
        }
        private void Btn_ReadcloudFloorTable_OK_Click(object sender, EventArgs e)
        {
            try
            {
                string devUnique = string.Empty;
                if (list != null)
                {
                    foreach (var one in list)
                    {
                        PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(one);
                        PropertyDescriptor pdID = pdc.Find("deviceUnique", true);
                        string lhcodeunique = pdID.GetValue(one).ToString();
                        PropertyDescriptor pdDevName = pdc.Find("deviceName", true);
                        string lhcodedeviceName = pdDevName.GetValue(one).ToString();

                        if (lhcodedeviceName == comboBox_FloorName.Text.ToString())
                        {
                            devUnique = lhcodeunique;
                        }
                    }
                }
                string _uSysCode = devUnique;//传递的参数  0000012610080;
                DateTime currentTime = System.DateTime.Now;
                string createTime = currentTime.ToString("yyyyMMddHHmmss");

                string _MD5 = "deviceUnique" + createTime + "1177BE55278MMN5412365UHBN85214BE";
                string _sSign = Md5Func(_MD5);
                RunLog.Log("_MD5" + _MD5);
                string postString = /*"projectId=" + TextEdit_ProNo.Text.ToString() + */"deviceUnique=" + _uSysCode + "&sign=" + _sSign + "&createTime=" + createTime;//传递的参数  00000126
                byte[] postData = Encoding.UTF8.GetBytes(postString);//编码  

                //RunLog.Log("请求：  ccccccccccccc deviceUnique  " + _uSysCode);

                //RunLog.Log("请求：  ccccccccccccc POSTString  " + postString);

                iniFileControl ReadIniConfig = new iniFileControl(Application.StartupPath + @"\CloudAdd.ini");
                string socketHttp = ReadIniConfig.IniReadValue("云楼层对应表配置", "地址");
                //string url = "http://test.smartcard.jia-r.com/smartCard/syncdata/device/elevator/getElevatorFloorConfig";//地址  
                string url = socketHttp.ToString();

                WebClient webClient = new WebClient();
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式
                byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
                string srcString = Encoding.UTF8.GetString(responseData);//解码  


                RunLog.Log("1 云接收 ：ccccccccccccc srcString  " + srcString + "Length = " + srcString.Length);
                if (srcString.Length == 50)
                {
                    HintProvider.ShowAutoCloseDialog(null, string.Format("获取楼层映射表异常，请前往物业后台重置。"));
                    this.Close();
                    return;
                }

                ResponseInfo FloorTable = JsonConvert.DeserializeObject<ResponseInfo>(srcString);

                RunLog.Log("2 云接收 ：ccccccccccccc srcString  " + srcString);

                this.Close();


                SendCloudFloorHex = FloorTable; //
                RunLog.Log("Read 实际楼层对比：SendCloudFloorHex ********* :" + FloorTable.data.logicFloor);

                this.DialogResult = DialogResult.OK;

            }
            catch (Exception ex)
            {
                string errMsg = "获取楼层对应表失败";
                HintProvider.ShowAutoCloseDialog(null, string.Format("通讯失败，错误：{0}", errMsg));

                RunLog.Log(ex);
            }

        }

    }
}
