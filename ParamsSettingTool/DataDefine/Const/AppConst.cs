using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    public static class AppConst
    {
        /// <summary>
        /// udp监听源默认端口号
        /// </summary>
        public const int UDP_SOURCE_PORT = 12580;
        /// <summary>
        /// udp监听目的默认端口号
        /// </summary>
        public const int UDP_PURPOSE_PORT = 10086;  
        //命令字
        /// <summary>
        /// 搜索设备
        /// </summary>
        public const string CMD_WORD_SEARCH_DEVIDES = "1000";
        /// <summary>
        /// 设置云电梯参数
        /// </summary>
        public const string CMD_WORD_SET_CLOUD_ELEVATOR_PARAMS = "1010";
        /// <summary>
        /// 设置群控器参数
        /// </summary>
        public const string CMD_WORD_SET_GROUPLIKE_PARAMS = "1010";
        /// <summary>
        /// 设置云群控器参数
        /// </summary>
        public const string CMD_WORD_SET_CLOUD_GROUPLIKE_PARAMS = "1010";
        /// <summary>
        /// 设置门禁参数
        /// </summary>
        public const string CMD_WORD_SET_CLOUD_Entrance_PARAMS = "1010";
        /// <summary>
        /// 设置联动器参数
        /// </summary>
        public const string CMD_WORD_SET_LINKAGE_CTRL_PARAMS = "1010";
        /// <summary>
        /// 下载楼层对应表
        /// </summary>
        public const string CMD_WORD_SET_DOWN_FLOOR_TABLE = "5A04";
        /// <summary>
        /// 下载对讲楼层对应表
        /// </summary>
        public const string CMD_WORD_SET_DOWN_INTERCOM_FLOOR_TABLE = "5A06";
        /// <summary>
        /// 下载真实楼层对应表
        /// </summary>
        public const string CMD_WORD_SET_DOWN_REAL_FLOOR_TABLE = "5A08";
        /// <summary>
        /// 下载状态检测对应表
        /// </summary>
        public const string CMD_WORD_SET_DOWN_STATUS_FLOOR_TABLE = "5A20";
        /// <summary>
        /// 设置云联动器参数
        /// </summary>
        public const string CMD_WORD_SET_CLOUD_LINKCTRL_PARAMS = "1012";
        /// <summary>
        /// 下载楼层对应表
        /// </summary>
        public const string CMD_WORD_SET_DOWN_DOWNLOAD_FLOOR_TABLE = "5A30";
        /// <summary>
        /// 读取楼层对应表
        /// </summary>
        public const string CMD_WORD_SET_DOWN_READ_FLOOR_TABLE = "5A31";
        /// <summary>
        /// 联动控制器可连接的云电梯最大数量
        /// </summary>
        public const int LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT = 8;
        /// <summary>
        /// 云群控器最大连接的群控器数量
        /// </summary>
        public const int LINKAGE_CTRL_CLOUD_GROUPLINKAGE_MAX_COUNT = 2;
        /// <summary>
        /// 云电梯控制器属性名称，不区分正背门
        /// </summary>
        public const string CLOUD_ELEVATOR_PROPERTY_NAME_ZERO = "不区分正背门";
        /// <summary>
        /// 云电梯控制器属性名称，正门
        /// </summary>
        public const string CLOUD_ELEVATOR_PROPERTY_NAME_ONE = "正门";
        /// <summary>
        /// 云电梯控制器属性名称，背门
        /// </summary>
        public const string CLOUD_ELEVATOR_PROPERTY_NAME_TWO = "背门";
        /// <summary>
        /// 云电梯控制器属性名称，单台控制器正背门
        /// </summary>
        public const string CLOUD_ELEVATOR_PROPERTY_NAME_THREE = "单台控制器正背门";

        public static bool IsDownParmCloudLinkage = false;
    }
}
