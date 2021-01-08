using ITL.DataDefine;
using ITL.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ITL.ParamsSettingTool
{
    public static class CommonUtils
    {
        //返回状态码
        public const int RES_OK = 0x00;
        public const int ERR_NO_SUPPORT = 0x79;
        public const int ERR_PARAMS = 0x7A;
        public const int ERR_NO_DATA = 0x7B;
        public const int ERR_CHECK = 0x7C;
        public const int ERR_NO_AUTH = 0x7D;
        public const int ERR_DEVICE_DAMAGE = 0x7E;
        public const int ERR_ACCEPT_FAIL = 0x7F;

        public const string ERR_NO_SUPPORT_ALIAS = "设备不支持该命令";
        public const string ERR_PARAMS_ALIAS = "参数错误";
        public const string ERR_NO_DATA_ALIAS = "没有数据可读取";
        public const string ERR_CHECK_ALIAS = "校验错误";
        public const string ERR_NO_AUTH_ALIAS = "没有权限";
        public const string ERR_DEVICE_DAMAGE_ALIAS = "设备损坏";
        public const string ERR_ACCEPT_FAIL_ALIAS = "命令接受不成功";

        public static string GetErrMsgByCode(int errCode)
        {
            switch (errCode)
            {
                case CommonUtils.ERR_NO_SUPPORT:     return CommonUtils.ERR_NO_SUPPORT_ALIAS;
                case CommonUtils.ERR_PARAMS:         return CommonUtils.ERR_PARAMS_ALIAS;
                case CommonUtils.ERR_NO_DATA:        return CommonUtils.ERR_NO_DATA_ALIAS;
                case CommonUtils.ERR_CHECK:          return CommonUtils.ERR_CHECK_ALIAS;
                case CommonUtils.ERR_NO_AUTH:        return CommonUtils.ERR_NO_AUTH_ALIAS;
                case CommonUtils.ERR_DEVICE_DAMAGE:  return CommonUtils.ERR_DEVICE_DAMAGE_ALIAS;
                case CommonUtils.ERR_ACCEPT_FAIL:    return CommonUtils.ERR_ACCEPT_FAIL_ALIAS;
                default:
                    return string.Empty;
            }
        }

        public static string GetMacByHex(string strHex)
        {
            string strMac = string.Empty;
            for(int i = 0; i <= 5; i++)
            {
                strMac += StrUtils.CopySubStr(strHex, i * 2, 2);
                strMac += "_";
            }
            return StrUtils.CopySubStr(strMac, 0, strMac.Length - 1);
        }

        public static string GetIPByHex(string strHex)
        {
            string strIp = string.Empty;
            for(int i = 0; i <= 3; i++)
            {
                strIp += StrUtils.StrToIntDef(StrUtils.CopySubStr(strHex, i * 2, 2), 0, 16).ToString();
                strIp += ".";
            }
            return StrUtils.CopySubStr(strIp, 0, strIp.Length - 1);
        }

        /// <summary>
        /// //从指定字符串中找到命令起始字符串， eg: subHexStr =  "F2" 或 subHexStr = "F3"
        /// </summary>
        /// <param name="subHexStr"></param>
        /// <param name="totalHexStr"></param>
        /// <returns></returns>
        public static int PosITL(string subHexStr, string totalHexStr)
        {
            if (subHexStr.Length != 2)
            {
                return -1;
            }
            string strTmp = string.Empty;
            for (int index = 0; index < totalHexStr.Length - 1; index += 2)
            {
                strTmp = StrUtils.CopySubStr(totalHexStr, index, 2);
                if (strTmp == subHexStr)
                {
                    return index;
                }
            }
            return -1;
        }

        public static string GetHexByIP(string ip)
        {
            string strHex = string.Empty;
            List<string> list = new List<string>(ip.Split('.'));
            for(int i = 0; i < list.Count; i++)
            {
                strHex += StrUtils.IntToHex(StrUtils.StrToIntDef(list[i], 0), 2);
            }
            return strHex;
        }

        public static string GetNameByCloudElevatorProperties(int elevatorProperties)
        {
            switch (elevatorProperties)
            {
                case 0x00: return AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_ZERO;
                case 0x01: return AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_ONE;
                case 0x02: return AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_TWO;
                case 0x03: return AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_THREE;
                default:
                    return string.Empty;
            }
        }

        public static int GetCloudElevatorPropertiesByName(string elevatorProportyName)
        {
            if(elevatorProportyName == AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_ZERO)
            {
                return 0x00;
            }
            if (elevatorProportyName == AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_ONE)
            {
                return 0x01;
            }
            if (elevatorProportyName == AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_TWO)
            {
                return 0x02;
            }
            if (elevatorProportyName == AppConst.CLOUD_ELEVATOR_PROPERTY_NAME_THREE)
            {
                return 0x03;
            }
            return 0x04;
        }

        public static void edtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
                return;
            }
            bool flag = System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"^[1-9]\d*|0$");
            if (!flag)
            {
                e.KeyChar = '\0';
            }
        }

        public static void edtIp_KeyPress(object sender, KeyPressEventArgs e)
        {
            //允许输入数字、小数点、删除键
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != (char)('.'))
            {
                e.KeyChar = '\0';
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="isCover">是否覆盖</param>
        public static void AddToUpdate(this Dictionary<string, hintInfo> dic,string key)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].FontCount += 1;
            }
            else
            {
                hintInfo info = new hintInfo();
                info.FontCount = 1;
                info.IsHint = false;
                dic.Add(key, info);
            }
        }

        /// <summary>
        /// 按键名称
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        public static string GetKeyNameByHex(string strHex)
        {
            string strMac = string.Empty;
            for (int i = 0; i <= 5; i++)
            {
                strMac += StrUtils.CopySubStr(strHex, i * 2, 2);
                strMac += "_";
            }
            return StrUtils.CopySubStr(strMac, 0, strMac.Length - 1);
        }
        /// <summary>
        /// 将一条十六进制字符串转换为ASCII
        /// </summary>
        /// <param name="hexstring">一条十六进制字符串</param>
        /// <returns>返回一条ASCII码</returns>
        public static string HexStringToASCII(string hexstring)
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }


            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                //c[i] = Convert.ToChar(a);
                c[i] = Convert.ToChar(a);
            }

            string b = new string(c);

            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(b);
            string result = Encoding.UTF8.GetString(bytes);

            return result;
            //return b;
        }
        /// <summary>
        /// 16进制字符串转换为二进制数组
        /// </summary>
        /// <param name="hexstring">用空格切割字符串</param>
        /// <returns>返回一个二进制字符串</returns>
        public static byte[] HexStringToBinary(string hexstring)
        {


            string[] tmpary = hexstring.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }
            return buff;
        }

        /// <summary>
        /// Hex转换UTF-8
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
                throw new ArgumentException("hex is not a valid number!", "hex");
            }
            // 需要将 hex 转换成 byte 数组。
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message.
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }

        /// 从字符串转换到16进制表示的字符串
        /// 编码,如"utf-8","gb2312"
        /// 是否每字符用逗号分隔
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                //s += " ";//空格
                //不知道为啥以前要补一个空格，如果报错了再把上面的注释打开吧。
                         //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", ",");
                }
            }
            return str.ToLower();
        }

    }
}
