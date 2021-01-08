using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ITL.ParamsSettingTool
{
    public class CloudFloorTableBaseInfo
    {
        public string deviceName { get; set; } //楼层控制器名称
        public string deviceUnique { get; set; } //项目编号

        public string deviceType { get; set; }
        public string deviceMemo { get; set; }
        public string deviceId { get; set; }
        public string floorNo { get; set; }
        public string deviceStatus { get; set; }

        public string data { get; set; }
    }
    public class JsonData
    {
        public string data { get; set; }
    }

    public class ProNo
    {
        public string deviceUnique { get; set; } //项目编号
    }
    public class CloudFloorTableItemList
    {
        /// <summary>
        /// Item列表
        /// </summary>
        public List<ProNo> C_FloorTableItem { get; set; }
    }
    public class  FloorTableInfo
    {
        public string logicalFloor { get; set; }
        public string terminalFloor { get; set; }
        public string naturalFloor { get; set; }
        public string logicFloor { get; set; }
        public string detectFloor { get; set; }
        public string terminalNumIntercom { get; set; }
        public string floorNum { get; set; }

    }
    public class ResponseInfo
    {
        public int msgCode { get; }
        public string msg { get; }
        public FloorTableInfo data { get; set; }
        //List<GetCloudFloorTableInfo> data 

    }

    public class GetCloudFloorTableInfo
    {
        public string deviceName { get; set; }
        public string deviceType { get; set; }
        public string deviceMemo { get; set; }
        public string deviceId { get; set; }
        public string FloorNo { get; set; }
        public string deviceUnique { get; set; }
        public string deviceStatus { get; set; }

    }
    public class RootObjectdeviceUnique
    {
        public List<CloudFloorTableBaseInfo> f_deviceUnique { get; set; }
    }
    }
