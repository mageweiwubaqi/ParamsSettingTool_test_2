using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ITL.Framework
{
    public static class StrUtils
    {
        /// <summary>
        /// 获取中文字符编码
        /// </summary>
        /// <returns></returns>
        public static Encoding GetChsEncoding()
        {
            try
            {
                var chs = Encoding.GetEncoding("gb2312");
                return chs;
            }
            catch
            {
                return Encoding.Default;
            }
        }

        //将一个字符串的各位的ASCII值转换为2位16进制数，然后连接起来。用于中文转16进制数
        public static string ASCIIStrToHexStr(string aASCIIStr)
        {
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");

            byte[] bytes = chs.GetBytes(aASCIIStr);

            string str = "";

            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X2}", bytes[i]);
            }

            return str;
        }

        /// <summary>
        /// 字符串转十六进制
        /// </summary>
        /// <param name="aASCIIStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string StrToHexStr(string aASCIIStr, string charset)
        {
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);

            byte[] bytes = chs.GetBytes(aASCIIStr);

            string str = "";

            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X2}", bytes[i]);
            }

            return str;
        }

        //将存有2位16进制数ASCII值的字符串还原为对应的字符，与ASCIIStrToHex恰好相反
        //HEX字符串转换为ASCII字符串
        public static string HEXStrToASCIIStr(string HEXStr)
        {
            if (HEXStr == null)
            {
                return string.Empty;
            }
            string asciiStr = string.Empty;
            if (HEXStr.Length % 2 != 0)
            {
                HEXStr = HEXStr + "0";
            }
            for (int index = 0; index < HEXStr.Length; index += 2)
            {
                string subStr = CopySubStr(HEXStr, index, 2);
                int ascValue = StrToIntDef(subStr, 0, 16);
                if (ascValue <= 128)
                {
                    asciiStr += (char)ascValue;
                }
                else  //中文需额外处理
                {
                    if (HEXStr.Length < index + 4)
                    {
                        asciiStr += string.Format("{0:X2}", ascValue);
                    }
                    else
                    {
                        byte[] array = new byte[2];
                        array[0] = (byte)(Convert.ToInt32(HEXStr.Substring(index, 2), 16));
                        array[1] = (byte)(Convert.ToInt32(HEXStr.Substring(index + 2, 2), 16));
                        asciiStr += Encoding.GetEncoding("gb2312").GetString(array);
                        index += 2;
                    }
                }
            }
            //byte[] array = HexStrToBytes(HEXStr);
            //asciiStr += Encoding.GetEncoding("gb2312").GetString(array);
            return asciiStr;
        }


        //将存有16进制数的字符串的每两项转换为16进制数存入byte[]数组中
        public static byte[] HexStrToBytes(string hexstring)
        {
            int index, intLen;
            string tmpHexStr, convertStr, v_strTmp;
            tmpHexStr = hexstring;
            convertStr = "";
            //保证tempHexstring长度是偶数
            if (tmpHexStr.Length % 2 != 0)
            {
                tmpHexStr += "F";
            }
            //用空格将tempHexstring两个字符为一组打散
            intLen = tmpHexStr.Length / 2;
            for (index = 0; index < intLen; index++)
            {
                v_strTmp = CopySubStr(tmpHexStr,2 * index, 2);
                if (index == intLen - 1)
                {
                    convertStr += v_strTmp;
                }
                else
                {
                    convertStr += v_strTmp + " ";
                }
            }
            //转换为byte数组
            string[] tmpary = convertStr.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }
            return buff;
        }

        ////将aHex的各位转换为4位2进制数
        //public static string HexStrToBinStr(string aHex)
        //{
        //    string astrRes = "";
        //    string[] Temp = {"0000", "0001", "0010", "0011","0100","0101","0110","0111",
        //                       "1000","1001","1010","1011","1100","1101","1110","1111"};
        //    for (int i = 0; i < aHex.Length; i++)
        //    {
        //        //if(int.TryParse())
        //        //astrRes = astrRes + 
        //        try
        //        {
        //            int index = Convert.ToInt32(aHex.Substring(i, 1), 16);
        //            astrRes = astrRes + Temp[index];
        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }
        //    return astrRes;
        //}

        //使用NewSubStr替换SourceStr中从指定索引位置StartIndex开始的内容，并返回新的字符串
        public static string ReplaceSubStr(string SourceStr, string NewSubStr, int StartIndex)
        {

            if (string.IsNullOrEmpty(SourceStr) || string.IsNullOrEmpty(NewSubStr)
                || (SourceStr.Length < StartIndex + NewSubStr.Length))
            {
                return SourceStr;
            }
            string strResult = string.Empty;
            //strResult = Regex.Replace(SourceStr, "(?<=^.{" + StartIndex.ToString() + "}).", SubStr);
            strResult = SourceStr.Remove(StartIndex, NewSubStr.Length).Insert(StartIndex, NewSubStr); ;
            return strResult;
        }

        //将十进制字符串(1~7)转换为十六进制字符串
        public static string DecTimeStrToHexStr(string DecStr)
        {
            if (string.IsNullOrEmpty(DecStr))
            {
                return string.Empty;
            }
            string strBin = string.Empty;
            for (int index = 0; index < DecStr.Length; index++)
            {
                switch (DecStr[index])
                {
                    case '0': strBin += "000"; break;
                    case '1': strBin += "001"; break;
                    case '2': strBin += "010"; break;
                    case '3': strBin += "011"; break;
                    case '4': strBin += "100"; break;
                    case '5': strBin += "101"; break;
                    case '6': strBin += "110"; break;
                    case '7': strBin += "111"; break;
                    default:
                        break;
                }

            }
            return BinStrToHexStr(strBin);
        }

        //将二进制字符串转换为十六进制字符串
        public static string BinStrToHexStr(string BinStr)
        {
            if (BinStr == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder(string.Empty);
            for (int index = 0; index < BinStr.Length; index += 4)
            {
                if (index + 4 > BinStr.Length) //剩余不足4位二进制的字符串不做处理
                {
                    continue;
                }
                string tmpStr = BinStr.Substring(index, 4);
                int intValue = Convert.ToInt32(tmpStr, 2);
                sb.Append(Convert.ToString(intValue, 16));
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 16进制字符串转 int
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        public static int HexStrToInt(string hexStr)
        {
            try
            {
                int intResult = 0;
                intResult = Convert.ToInt32(hexStr, 16);
                return intResult;
            }
            catch (Exception e)
            {
                RunLog.Log(e.Message);
                return 0;             
            }
        }

        //将十六进制字符串转换为二进制字符串
        public static string HexStrToBinStr(string hexStr)
        {
            if (!CheckIsHEXStr(hexStr))
            {
                RunLog.Log(string.Format("{0} is not hex string ,can not HexStrToBinStr.",hexStr));
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder(string.Empty);
            try
            {
                for (int index = 0; index < hexStr.Length; index++)
                {
                    int intValue = StrToIntDef(hexStr[index].ToString(), fromBase: 16);
                    sb.Append(FormatStrLen(Convert.ToString(intValue, 2), "0", 4, false));
                }
            }
            catch (Exception e)
            {
                RunLog.Log(string.Format("{0} HexStrToBinStr failed!{1}",hexStr,e.Message));
                return string.Empty;
            }
            return sb.ToString();
            //return Convert.ToString(Convert.ToInt32(HexStr, 16), 2);
        }

        //用指定的字符格式化字符串，isAddAfter =  true 表示在后面添加，否则在前面添加,isCutMore是否截断超出intLen部分的字符串
        public static string FormatStrLen(string strOld, string strSub, int intLen, bool isAddAfter = true, bool isCutMore = true)
        {
            // string strTmp = string.Empty;
            StringBuilder sbTmp = new StringBuilder(string.Empty);
            int intAddLen;
            if (strOld == null)
            {
                strOld = string.Empty;
            }
            if (strSub == null)
            {
                strSub = "0";
            }
            intAddLen = intLen - strOld.Length;
            for (int index = 0; index < intAddLen; index++)
            {
                //strTmp = strTmp + strSub;
                sbTmp.Append(strSub);
            }
            if (isAddAfter)
            {
                //strTmp =  strOld + strTmp;
                sbTmp.Insert(0, strOld);
            }
            else
            {
                //strTmp = strTmp + strOld;
                sbTmp.Append(strOld);
            }
            // return strTmp.Substring(0, intLen);
            if (isCutMore)
            {
                return sbTmp.ToString().Substring(0, intLen);
            }
            else
            {
                return sbTmp.ToString();
            }

        }

        //格式化字符串，
        public static string FormatStrLen(string aOldStr, char aChar, int aIntLen, bool isAddAfter = true)
        {
            if (!isAddAfter)
            {
                return aOldStr.PadLeft(aIntLen, aChar);
            }
            else
            {
                return aOldStr.PadRight(aIntLen, aChar);
            }
        }

        //判断字符串是否全是数字
        public static bool IsNumeric(string astr)
        {
            if (string.IsNullOrEmpty(astr)) //验证这个参数是否为空
            {
                return false;   //是，就返回False
            }
            for (int i = 0; i < astr.Length; i++)
            {
                if (!Char.IsDigit(astr[i]))
                {
                    return false;
                }
            }
            return true;                                //是，就返回True
        }

        //将byte[]数组每一项转换为2位16进制数连接起来形成一个字符串，与HexStringToBytes恰好相反
        public static string BytesToHexStr(byte[] bytes)
        {
            return BytesToHexStr(bytes, 0, bytes.GetLength(0));
        }

        //将byte[]数组每一项转换为2位16进制数连接起来形成一个字符串，与HexStrToBytes恰好相反
        public static string BytesToHexStr(byte[] bytes,int startIndex, int len)
        {
            string hexStr = string.Empty;
            if (bytes == null)
            {
                return hexStr;
            }

            if ((startIndex >= bytes.Length) || (startIndex + len - 1 >= bytes.Length))
            {
                return hexStr;
            }
           
            StringBuilder strBuilderData = new StringBuilder();

            for (int i = startIndex; i < len; i++)
            {
                strBuilderData.Append(bytes[i].ToString("X2"));
            }
            hexStr = strBuilderData.ToString();
         
            return hexStr;
        }


        /// <summary>
        /// 将字符串str转为整形，此函数切忌在可能频繁触发异常的地方使用，频繁异常会导致程序性能大大降低
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defValue">转换失败返回默认值defValue</param>
        /// <param name="fromBase">fromBase 代表进制 它必须是 2、8、10 或 16。</param>
        /// <returns></returns>
        public static int StrToIntDef(string str, int defValue = 0, int fromBase = 10)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return defValue;
            }
            int intRes = defValue;
            try
            {
                if (fromBase == 10)
                {
                    if (int.TryParse(str, out intRes))
                    {
                        return intRes;
                    }
                }
                intRes =  Convert.ToInt32(str, fromBase);
            }
            catch (Exception e)
            {
                RunLog.Log(e.Message,stackDepth:4);
                intRes = defValue;
            }
            return intRes;
        }

        //字符串转整形，转换失败返回默认值def
        //public static int StrToIntDef(string astr, int def)
        //{
        //    int intres;
        //    if (int.TryParse(astr, out intres))
        //    {
        //        return intres;
        //    }
        //    else
        //    {
        //        return def;
        //    }
        //}



        //字符串转Hex
        public static string StrToHex(string astr, int alen)
        {
            return string.Format("{0:X" + alen.ToString() + "}", StrToIntDef(astr, 0));

        }

        //整形转Hex：alen表示多少位
        public static string IntToHex(int astr, int alen)
        {
            return string.Format("{0:X" + alen.ToString() + "}", astr);

        }



        //字符串转浮点型，转换失败返回默认值def
        public static double StrToDoubleDef(string astr, double def)
        {
            double doubleres;
            if (double.TryParse(astr, out doubleres))
            {
                return doubleres;
            }
            else
            {
                return def;
            }
        }

        public static decimal StrToDecimalDef(string astr, decimal def)
        {
            decimal decimals;
            if(decimal.TryParse(astr, out decimals))
            {
                return decimals;
            }
            else
            {
                return def;
            }
        }

        //获取字符串长度，一个汉字算2位
        public static int GetByteStrLength(string astr)
        {
            return GetChsEncoding().GetBytes(astr).Length;
        }


        // 生成指定命令报文的异或校验码，ALow、ALength为报文的其实范围
        public static string GetXORCheck(string aStrData, int aLow = -1, int aLegnth = 0)
        {
            if (aLow < 0) aLow = 0;
            if (aLegnth <= 0) aLegnth = aStrData.Length;
            string strRes = "00";
            byte[] buffer = HexStrToBytes(CopySubStr(aStrData, aLow,aLegnth));
           
            if (buffer == null) return strRes;

            int bufferLen = buffer.Length;

            switch (bufferLen)
            {
                case 0: return strRes;
                case 1: return IntToHex(buffer[0], 2);
                default:
                    byte bcc = buffer[0];
                    for (int index = 1; index < bufferLen; index++)
                    {
                        bcc ^= buffer[index];
                    }
                    strRes = IntToHex(bcc, 2);
                    return strRes;
            }
        }

        //将每2位作为十进制数转换为16进制数：用于操作员密码加密
        public static string EncryptPassword(string aToEncrypt)
        {
            if ((aToEncrypt == null) || (aToEncrypt.Trim() == "") || ((aToEncrypt.Length % 2) != 0))
            {
                return "";
            }
            string result = "";
            for (int i = 0; i < aToEncrypt.Length; i += 2)
            {
                result += Convert.ToInt32(aToEncrypt.Substring(i, 2)).ToString("x2");
            }
            return result.ToUpper();
        }

        //将每2位作为16进制数转换为2位10进制数：用于操作员密码解密
        public static string DecryptPassword(string aToDecrypt)
        {
            if ((aToDecrypt == null) || (aToDecrypt.Trim() == "") || ((aToDecrypt.Length % 2) != 0))
            {
                return "";
            }
            string result = "";
            string temp = "";
            for (int i = 0; i < aToDecrypt.Length; i += 2)
            {
                temp = (Convert.ToInt32(aToDecrypt.Substring(i, 2), 16) + 100).ToString();
                result += temp.Substring(temp.Length - 2, 2);
            }
            return result.ToUpper();
        }

        //获取版本号
        public static string GetFileVersion(string aFilePath)
        {
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(@aFilePath);
            return myFileVersionInfo.FileVersion;
        }


        //仿照Delphi TStringList  将“1,2,a,33,1c”字符串存放到一个List<string>中，函数内部创建此List<string>
        public static List<string> GetDelimitedList(string aSourceStr, string aDelimtedText = ",")
        {
            string[] strAry = aSourceStr.Split(aDelimtedText.ToCharArray());
            return strAry.ToList();

        }

        //仿照Delphi TStringList  将“1,2,4,33,15”字符串存放到一个List<string>中，函数内部创建此List<int>
        public static List<int> GetIntDelimitedList(string aSourceStr, string aDelimtedText = ",")
        {
            List<int> delimitedList = new List<int>();
            delimitedList.Clear();
            string[] strAry = aSourceStr.Split(aDelimtedText.ToCharArray());
            foreach (string str in strAry)
            {
                delimitedList.Add(StrUtils.StrToIntDef(str, 0));
            }
            return delimitedList;

        }

        //仿照Delphi TStringList  将一个List<string>的值依次用分隔符隔开变成一个字符串，如“1,2,a,33,1c”
        public static string GetDelimitedText(List<string> strList, string aDelimtedText = ",")
        {
            return string.Join(aDelimtedText, strList.ToArray());
        }

        //仿照Delphi TStringList 并扩展  将一个List<object>的值依次用分隔符隔开变成一个字符串，如“1,2,a,33,1c”
        public static string GetDelimitedText(List<int> intList, string aDelimtedText = ",")
        {
            return string.Join(aDelimtedText, intList.ToArray());
        }

        //字符串处理函数 010001011 -> 2,6,8,9
        public static string BinStrToCommaText(string aSourceStr)
        {

            StringBuilder sbComma = new StringBuilder(string.Empty);
            for (int index = 0; index < aSourceStr.Length; index++)
            {
                if (aSourceStr[index] != '0')
                {
                    sbComma.Append("," + (index + 1).ToString());
                }
            }
            string strRes = sbComma.ToString();
            if (strRes.Length > 0)
            {
                if (strRes[0] == ',')
                {
                    strRes = strRes.Remove(0, 1);
                }
            }
            return strRes;
        }

        //字符串处理函数 List<int> 如 2,6,8,9 -> 010001011 
        public static string ListToBinStr(List<int> intList)
        {
            StringBuilder sb = new StringBuilder(string.Empty);
            int intMax = 0;
            for (int index = 0; index < intList.Count; index++)
            {
                if (intMax < intList[index])
                {
                    intMax = intList[index];
                }
            }

            string strTmp = string.Empty;
            strTmp = strTmp.PadRight(intMax, '0');

            int intTmp = 0;
            for (int index = 0; index < intList.Count; index++)
            {
                intTmp = intList[index];
                strTmp = StrUtils.ReplaceSubStr(strTmp, "1", intTmp - 1);
            }
            return strTmp;
        }

        //字符串处理函数 List<int> 如 010001011 -> 2,6,8,9
        public static List<int> BinStrToList(string aBinStr)
        {
            string strComma = BinStrToCommaText(aBinStr);
            return GetIntDelimitedList(strComma, ",");
        }

        //获取字符串，屏蔽异常
        public static string CopySubStr(string aSourceStr, int aStartIndex, int aCopyLen)
        {
            if (string.IsNullOrWhiteSpace(aSourceStr))
            {
                return string.Empty;
            }
            if (aSourceStr.Length < aStartIndex + 1) //若索引直接越界则返回空字符串
            {
                return string.Empty;
            }
            if (aCopyLen > aSourceStr.Length - aStartIndex) //若需复制到的的字符串索引已越界则返回所有能够提供的字符串即可
            {
                return aSourceStr.Substring(aStartIndex, aSourceStr.Length - aStartIndex);
            }
            return aSourceStr.Substring(aStartIndex, aCopyLen);
        }

        /// <summary>
        /// 截取包含中英文混合字符
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetCnEnSubString(string sourceStr, int length)
        {
            int len = 0;
            StringBuilder sb = new StringBuilder();
            char[] tmp = sourceStr.ToCharArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (len >= length)
                {
                    break;
                }
                else
                {
                    if (((int)tmp[i]) < 255)//大于255的都是汉字或特殊字符
                    {
                        len++;
                    }
                    else
                    {
                        len += 2;
                    }
                    sb.Append(tmp[i]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 移出指定长度的字符串
        /// </summary>
        /// <param name="aSourceStr"></param>
        /// <param name="aStartIndex"></param>
        /// <param name="aCopyLen"></param>
        /// <returns></returns>
        public static string RemoveSubStr(string aSourceStr, int aStartIndex, int aDeleteLen)
        {

            if (aSourceStr.Length < aStartIndex + 1) //若索引直接越界则返回原字符串
            {
                return aSourceStr;
            }
            if (aDeleteLen > aSourceStr.Length - aStartIndex) //若需删除的字符串索引已越界则删除所有能够提供的字符串即可
            {
                return aSourceStr.Remove(aStartIndex, aSourceStr.Length - aStartIndex);
            }
            return aSourceStr.Remove(aStartIndex, aDeleteLen);
        }


        //字符串处理函数 1,2,3,4,6,7,10,12  --> 1-4,6-7,10,12
        public static string GetShortFormatString(string aSourceStr, int aMinNo = 1, int aMaxNo = 10000, string linkStr = "-", string aDelimtedText = ",")
        {
            aSourceStr = aSourceStr.Trim();


            if (string.IsNullOrEmpty(aSourceStr))
            {
                return string.Empty;
            }

            List<string> strList = GetDelimitedList(aSourceStr, aDelimtedText);
            StringBuilder strRes = new StringBuilder(string.Empty);
            int start = -1;
            int end = -1;
            for (int index = aMinNo; index <= aMaxNo; index++)
            {
                if (strList.IndexOf(index.ToString()) >= 0)
                {
                    if (start == -1)
                    {
                        start = index;
                    }
                    else
                    {
                        if ((((strList.IndexOf((index + 1).ToString()) < 0) && (index < aMaxNo)))
                            || (index == aMaxNo))
                        {
                            end = index;
                            strRes.Append(start.ToString() + linkStr + end.ToString() + aDelimtedText);
                            start = -1;
                        }
                    }
                }
                else
                {
                    if (start != -1)
                    {
                        strRes.Append(start.ToString() + aDelimtedText);
                        start = -1;
                    }
                }
                if (start == aMaxNo)
                {
                    strRes.Append(start.ToString() + aDelimtedText);
                }

            }

            string strTmp = strRes.ToString();
            if ((strTmp.Length > 0) && (strTmp.Substring(strTmp.Length - 1, 1) == aDelimtedText.ToString()))
            {
                strTmp = strTmp.Remove(strTmp.Length - 1, 1);
            }
            return strTmp;
        }

        //字符串处理函数 1-4,6-7,10,12  --> 1,2,3,4,6,7,10,12
        public static string GetLongFormaString(string aSourceStr, string linkStr = "-", string aDelimtedText = ",")
        {
            aSourceStr = aSourceStr.Trim();
            if (string.IsNullOrEmpty(aSourceStr))
            {
                return string.Empty;
            }

            List<string> strList = GetDelimitedList(aSourceStr, aDelimtedText);
            StringBuilder strRes = new StringBuilder(string.Empty);

            string strGroup = string.Empty;

            int start = -1;
            int end = -1;
            int intPos = -1;
            for (int index = 0; index < strList.Count; index++)
            {
                strGroup = strList[index];
                intPos = strGroup.IndexOf(linkStr);
                if (intPos == -1)
                {
                    if (!string.IsNullOrEmpty(strGroup.Trim()))
                    {
                        strRes.Append(strGroup + aDelimtedText);
                    }
                }
                else
                {
                    start = StrToIntDef(strGroup.Substring(0, intPos), -1);
                    end = StrToIntDef(strGroup.Substring(intPos + 1, strGroup.Length - intPos - 1), -1);

                    if ((start != -1) && (end != -1))
                    {
                        for (int i = start; i <= end; i++)
                        {
                            strRes.Append(i.ToString() + aDelimtedText);
                        }
                    }
                }

            }

            string strTmp = strRes.ToString();
            if ((strTmp.Length > 0) && (strTmp.Substring(strTmp.Length - 1, 1) == aDelimtedText.ToString()))
            {
                strTmp = strTmp.Remove(strTmp.Length - 1, 1);
            }
            return strTmp;
        }

        //时间字符串转换为实际时间 XX年XX月XX日XX时XX分XX秒  format默认值为CPU卡内的时间字符串格式
        public static DateTime StrToDateTime(string s, string format = "yyMMddHHmmss")
        {
            try
            {
                return DateTime.ParseExact(s, format, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>        /// 时间字符串转换为实际时间, 转换失败则返回null
        /// </summary>
        /// <param name="s"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? StrToDateTimeNull(string s, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            try
            {
                return DateTime.ParseExact(s, format, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //组装SQL查询字段字符串
        public static string GetInquireFields(Dictionary<string, string> aFieldsAndAliasList)
        {
            string strSql = string.Empty;

            foreach (var field in aFieldsAndAliasList)
            {
                if (field.Value != string.Empty)
                {
                    strSql += string.Format(", {0} as {1}", field.Key, field.Value);
                }
                else
                {
                    strSql += string.Format(", {0}", field.Key);
                }

            }
            if (strSql != string.Empty)
            {
                strSql = strSql.Remove(0, 1);
            }
            return strSql;
        }


        /// <summary>
        ///  //判断是否是十六进制字符串
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="strLen">若还需判断字符串长度，则指定此值</param>
        /// <returns></returns>
        public static bool CheckIsHEXStr(string strData, int strLen = -1)
        {
            if (string.IsNullOrEmpty(strData))
            {
                return false;
            }
            //需判断长度
            if (strLen > 0)
            {
                if ((string.IsNullOrEmpty(strData)) || (strData.Length != strLen))
                {
                    return false;
                }
            }
            string strTmp = strData.ToUpper();
            //依次判断每一个字符是否属于十六进制
            for (int index = 0; index < strTmp.Length; index++)
            {
                if ((!System.Text.RegularExpressions.Regex.IsMatch(strTmp[index].ToString(), "[0-9]"))
                   && (!System.Text.RegularExpressions.Regex.IsMatch(strTmp[index].ToString(), "[A-F]")))
                {
                    return false;
                }

            }
            return true;
        }


        /// <summary>
        /// 获取十六进制CRC校验
        /// </summary>
        /// <param name="SourceStr"></param>
        /// <returns></returns>
        public static string GetCRC16HexStr(string SourceStr)
        {
            //Encoding chs = Encoding.GetEncoding("gb2312");
            //string strTmp = HEXStrToASCIIStr(SourceStr);
            //byte[] bytes = chs.GetBytes(strTmp);
            byte[] bytes = HexStrToBytes(SourceStr);
           
            return IntToHex(CalCRC_16new(bytes, 0, bytes.Length), 4);
        }


        /// <summary>
        /// CalCRC_16new X^16 + X^12 + X^5 + 1  高字节在前.低字节在后
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="start"></param>
        /// <param name="DataSzie"></param>
        /// <returns></returns>
        public static ushort CalCRC_16new(byte[] Data, ushort start, int DataSzie)
        {
            ushort iXor = start;
            ushort iCRC = start;
            ushort intTmp = 0;
            while (intTmp < DataSzie)
            {
                iCRC = (ushort)(iCRC ^ (ushort)Data[iXor] << 8);
                int index = 0;
                while (index < 8)
                {
                    if (iCRC >= 0x8000)
                    {
                        iCRC = (ushort)(iCRC << 1 ^ 0x1021);
                    }
                    else
                    {
                        iCRC = (ushort)(iCRC << 1);
                    }
                    index += 1;
                }
                iXor += 1;
                intTmp += 1;
            }
            //return (ushort)iCRC;
            return iCRC;
        }

        /// <summary>
        /// 将字符串S转为整形，转换失败返回默认值Default， fromBase 代表进制 它必须是 2、8、10 或 16。
        /// </summary>
        /// <param name="S"></param>
        /// <param name="Default"></param>
        /// <param name="fromBase"></param>
        /// <returns></returns>
        public static Int64 StrToInt64Def(string S, Int64 Default = 0, int fromBase = 10)
        {
            try
            {
                return Convert.ToInt64(S, fromBase);
            }
            catch (Exception)
            {
                return Default;
            }
        }

        //整形转Hex
        public static string Int64ToHex(Int64 int64, int alen)
        {
            return Convert.ToString(int64, 16);
        }

        /// <summary>
        /// 获取字符串的32位MD5值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5(string input)
        {
            MD5 md5 = MD5.Create();
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            string result = "";
            // 将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            foreach (var s in bytes)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                result += s.ToString("X2");
            }
            return result;
        }


        /// <summary>
        /// 解密字符串函数，如用于license的解密
        /// </summary>
        /// <param name="encryptStr">待解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptString(string encryptStr)
        {
            if ((string.IsNullOrWhiteSpace(encryptStr)) || (encryptStr.Length < 3))
            {
               // strErr = string.Format("The encrypt string:{0} is not invalid!", encryptStr);
                return string.Empty;
            }
            string hexString = "0123456789ABCDEFabcdef";
            encryptStr = CopySubStr(encryptStr,1, encryptStr.Length - 2);
            try
            {
                byte[] tmpB = Convert.FromBase64String(encryptStr);
                encryptStr = Encoding.ASCII.GetString(tmpB).ToUpper();

                StringBuilder sb = new StringBuilder();
                // 将每2位16进制整数组装成一个字节
                byte[] array = new byte[1];
                for (int i = 2; i < encryptStr.Length; i += 4)
                {
                    array[0] = (byte)(Convert.ToInt32((hexString.IndexOf(encryptStr[i]) << 4 | hexString.IndexOf(encryptStr[i + 1]))));
                    sb.Append(Convert.ToString(Encoding.ASCII.GetString(array)));
                }
 
                return HEXStrToASCIIStr(sb.ToString());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="asciiStr"></param>
        /// <returns></returns>
        public static string EncryptString(string decryptStr)
        {
            string hexstr = ASCIIStrToHexStr(decryptStr);
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(hexstr);
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            Random ran = new Random();
            string encryptStr = string.Empty;
            int[] btRan = new int[] { 0x8b00, 0x5300, 0x6700, 0x2c00, 0x5b00, 0x6a00, 0x7d00, 0x3e00 };
            string[] base64Ran = new string[] { "A", "B", "C", "D", "E", "a", "b", "c", "d", "e" };
            //转换hex编码
            for (int i = 0; i < bytes.Length; i++)
            {
                string tmp = Convert.ToString((bytes[i] + btRan[ran.Next(0, btRan.Length - 1)]), 16).ToString();//0x8b00特质码
                sb.Append(tmp);
            }
            // str = sb.ToString();
            encryptStr = string.Format("{0}{1}{2}", base64Ran[ran.Next(0, base64Ran.Length - 1)], Convert.ToBase64String((ASCIIEncoding.ASCII.GetBytes(sb.ToString()))), base64Ran[ran.Next(1, base64Ran.Length - 2)]);
            return encryptStr;
        }

        /// <summary>
        /// 判断是否是一个合法的IP地址
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static bool CheckIsValidIP(string strIP)
        {
            string byteMask = @"(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(25[0-5]))";
            string ipMask = byteMask + @"\." + byteMask + @"\." + byteMask + @"\." + byteMask;
            ipMask = string.Format(@"^{0}\.{0}\.{0}\.{0}$", byteMask);
            //string ipPattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$";
            return Regex.IsMatch(strIP, ipMask);
        }

        /// <summary>
        /// 补充字符串
        /// </summary>
        /// <param name="str">需要补充的字符串</param>
        /// <param name="cLen">长度</param>
        /// <param name="s">补充字符</param>
        /// <returns></returns>
        public static string ComplementedStr(string str,int cLen,string s ,bool isFirst = true)
        {
            if (str.Length < cLen)
            {
                for (int i = str.Length; i < cLen; i++)
                {
                    if (isFirst)
                    {
                        str = s + str;
                    }
                    else
                    {
                        str += s;
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 将String转换为布尔值
        /// </summary>
        /// <param name="strBool"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>

        public static bool StrToBoolDef(string strBool, bool defaultValue)
        {
            try
            {
                return Convert.ToBoolean(strBool);
            }
            catch (Exception e)
            {
                RunLog.Log(string.Format("{0} can not change to bool,{1}", strBool,e.Message));
                return defaultValue;
            }
          
        }
    }
}
