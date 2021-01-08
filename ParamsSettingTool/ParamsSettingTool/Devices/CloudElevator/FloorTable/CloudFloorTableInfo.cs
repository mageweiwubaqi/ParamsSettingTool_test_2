using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.ParamsSettingTool
{
    public class CloudFloorTableInfo : CloudFloorTableBaseInfo
    {
        public string actualFloorNum { get; set; } //实际楼层
        public string detectedFloorNum { get; set; } //检测楼侧

        public string floorName { get; set; } //按键名称

        public string id { get; set; } //权限标识

        public string terminalNum { get; set; } //端子号

        public string terminalNumIntercom { get; set; }
    }
}
