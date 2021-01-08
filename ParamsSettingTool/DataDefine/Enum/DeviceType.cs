using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public enum DeviceType
    {
        /*
        目前已有的设备类型有:
        0x00：广播所有设备
        0x21：云电梯（层控型）
        0x27：云对讲主机
        0x2E：协议控制器
        0x80：云群控器
        */
        /// <summary>
        /// 广播所有设备
        /// </summary>
        None = 0x00,
        
        /// <summary>
        /// 云电梯（层控型）
        /// </summary>
        CloudElevator = 0x21,
        
        /// <summary>
        /// 云对讲主机
        /// </summary>
        CloudIntercom = 0x27,
        
        /// <summary>
        /// 协议控制器
        /// </summary>
        LinkageCtrl = 0x2E,
        
        /// <summary>
        /// 云群控器
        /// </summary>
        CloudGroupCloudLink = 0x80,

        /// <summary>
        /// 群控器
        /// </summary>
        GroupCloudLink = 0x81,
        /// <summary>
        /// 云门禁
        /// </summary>
        GroupEntrance = 0x11,

        /// <summary>
        /// 云联动器
        /// </summary>
        CloudLinkageInfoCtrl = 0x16
    }
}
