using ITL.DataDefine;
using ITL.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.ParamsSettingTool
{
    public static class CommandProcesserHelper
    {
        /// <summary>
        /// 命令起始字
        /// </summary>
        public const string CMD_START_FLAG = "F2";
        /// <summary>
        /// 命令结束字
        /// </summary>
        public const string CMD_END_FLAG = "F3";

        /// <summary>
        /// 命令转义字
        /// </summary>
        public const int CMD_ESCAPE_FLAG = 0xF0;

        /// <summary>
        /// 如果命令报文中存在需转义的字符>=0xF0则需进行转义 0xFX->0xF07X，   可参考Mifare通讯协议
        /// </summary>
        /// <param name="cmdData"></param>
        /// <returns></returns>
        public static string AddF0Escape(string cmdData)
        {
            string strRes = string.Empty;
            string strTmp = string.Empty;
            if (cmdData.Length % 2 != 0)
            {
                cmdData = cmdData + "F";
            }
            for (int index = 0; index < cmdData.Length; index += 2)
            {
                strTmp = StrUtils.CopySubStr(cmdData, index, 2);
                if (StrUtils.StrToIntDef(strTmp, 0, 16) >= CMD_ESCAPE_FLAG)
                {
                    strTmp = StrUtils.IntToHex(CMD_ESCAPE_FLAG, 2)
                        + StrUtils.IntToHex((StrUtils.StrToIntDef(strTmp, 0, 16) & 0x7F), 2);
                }
                strRes = strRes + strTmp;
            }
            return strRes;
        }



        /// <summary>
        /// 过滤返回的命令报文，如果报文中存在转义字符F0，则需要消除转义0xF07X->0xFX
        /// </summary>
        /// <param name="cmdData"></param>
        /// <returns></returns>
        public static string DelF0Escape(string cmdData)
        {
            string strRes = string.Empty;
            string strTmp = string.Empty;
            if (cmdData.Length % 2 != 0)
            {
                cmdData = cmdData + "F";
            }
            for (int index = 0; index < cmdData.Length; index += 2)
            {
                strTmp = StrUtils.CopySubStr(cmdData, index, 2);
                if (StrUtils.StrToIntDef(strTmp, 0, 16) == CMD_ESCAPE_FLAG)
                {
                    index += 2;
                    if (index >= cmdData.Length) break;
                    strTmp = StrUtils.CopySubStr(cmdData, index, 2);
                    strTmp = StrUtils.IntToHex((StrUtils.StrToIntDef(strTmp, 0, 16) | 0x80), 2);
                }
                strRes = strRes + strTmp;
            }
            return strRes;
        }

        /// <summary>
        /// 根据命令报文获取设备号
        /// </summary>
        /// <param name="strCmd"></param>
        /// <returns></returns>
        public static int GetDevIDByCmdInfo(string devIDInfo)
        {
            int devID = 0;
            int groupNo = StrUtils.StrToIntDef(StrUtils.CopySubStr(devIDInfo, 0, 2), 0, 16);

            if (groupNo == 0)
            {
                return devID;
            }

            devID = (groupNo - 1) * 255 + StrUtils.StrToIntDef(StrUtils.CopySubStr(devIDInfo, 2, 2), 0, 16);

            return devID;
        }

        /// <summary>
        /// 根据命令报文获取设备类型
        /// </summary>
        /// <param name="strCmd"></param>
        /// <returns></returns>
        public static DeviceType GetDevTypeByCmdInfo(string devTypeInfo)
        {

            int devType = StrUtils.StrToIntDef(devTypeInfo, 0, 16);

            return (DeviceType)devType;
        }
    }
}
