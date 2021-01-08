using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.ParamsSettingTool
{
    public class LinkCtrlParam
    {
        public string version { get; set; }
        public data data { get; set; }

    }

    public class data
    {
        public string BackDtNum { get; set; }
        public string BackYqkIp { get; set; }
        public string DelayOutCall { get; set; }
        public string DoorNum { get; set; }
        public string  FrontDtNum { get; set; }
        public string FrontYqkIp { get; set; }
        public string InstallFloorName { get; set; }
        public string FrontLinkSta { get; set; }
        public string BackLinkSta { get; set; }
    }
}
