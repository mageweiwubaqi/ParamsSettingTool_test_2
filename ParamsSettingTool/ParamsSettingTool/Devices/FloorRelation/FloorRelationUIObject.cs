using ITL.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.ParamsSettingTool.SettingCenter
{
    public class FloorRelationUIObject
    {

        public static string UNDEFINE_FLAG = "-"; //未定义楼层参数
        public static string UNDEFINE_FLAG_999 = "999"; //未定义楼层参数

        /// <summary>
        ///  控制器号
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// 楼层权限标识
        /// </summary>
        public int FloorNo { get; set; }

        /// <summary>
        /// 楼层按键名
        /// </summary>
        public string FloorName { get; set; } = "";

        /// <summary>
        /// 真实楼层   ---- 后台返回值为999   UI显示为 -
        /// </summary>
        public string FloorReal { get; set; }

        /// <summary>
        /// 检测楼层 --- 更新楼层对应表
        /// </summary>
        public string CheckFloor { get; set; }

        /// <summary>
        ///  接线端子号
        /// </summary>
        public string FloorTerminalNo { get; set; }
     
        /// <summary>
        /// 楼层按键名（对讲楼层）
        /// </summary>
        public string IntercomFloorName { get; set; }
        /// <summary>
        /// 接线端子号（对讲楼层）
        /// </summary>
        public string TerminalNumIntercom { get; set; }

        /// <summary>
        /// 副操纵盘端子号
        /// </summary>
        public string TerminalNumSlave1 { get; set; }

        /// <summary>
        /// 残疾人操纵盘端子号
        /// </summary>
        public string TerminalNumSlave2 { get; set; }

        public FloorRelationUIObject()
        {
           
        }
        public FloorRelationUIObject(int deviceId, int floorNo)
        {
            this.DeviceId = deviceId;
            this.FloorNo = floorNo;
            this.FloorTerminalNo = floorNo == 0? UNDEFINE_FLAG:floorNo.ToString();
            this.TerminalNumIntercom = floorNo == 0 ? UNDEFINE_FLAG : floorNo.ToString();//0401
        }
        public FloorRelationUIObject(int deviceId, int floorNo, int terminalNo, int intercomTerminalNo)
        {
            this.DeviceId = deviceId;
            this.FloorNo = floorNo;
            this.FloorTerminalNo = terminalNo == 0 ? UNDEFINE_FLAG : terminalNo.ToString();
            this.TerminalNumIntercom = intercomTerminalNo == 0 ? UNDEFINE_FLAG : terminalNo.ToString();
        }

        /// <summary>
        /// 将字符格式楼层参数转换为整型
        /// </summary>
        /// <param name="floorNo"></param>
        /// <returns></returns>
        public static int convertUIToFloorParam(string floorParam)
        {
            if (UNDEFINE_FLAG.Equals(floorParam))
            {
                return 0;
            }
            return StrUtils.StrToIntDef(floorParam, 0);
        }
        /// <summary>
        /// 将字符格式楼层参数转换为整型 真实楼层
        /// </summary>
        /// <param name="floorParam"></param>
        /// <returns></returns>
        public static int convertUIToFloorParamRealFloor(string floorParam)
        {
            if (UNDEFINE_FLAG.Equals(floorParam))
            {
                return 999;
            }
            return StrUtils.StrToIntDef(floorParam, 0);
        }

        /// <summary>
        /// 将字符格式楼层参数转换为整型
        /// </summary>
        /// <param name="floorNo"></param>
        /// <returns></returns>
        public static string convertFloorParamToUI(int floorParam)
        {
            if (0 == floorParam)
            {
                return UNDEFINE_FLAG;
            }
            if (999 == floorParam)
            {
                return UNDEFINE_FLAG;
            }
            return floorParam.ToString();
        }
    }
}
