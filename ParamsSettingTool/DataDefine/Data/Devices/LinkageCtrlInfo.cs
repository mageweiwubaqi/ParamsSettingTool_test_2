using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    public class LinkageCtrlInfo: GeneralDeviceInfo
    {
        private int f_CloudUdpPort = 0;
        private int f_ThirdUdpPort = 0;
        private int f_CloudElevatorCount = 0;
        private Dictionary<int, LinkageCtrlItemInfo> f_CloudElevatorItems = null;

        public int CloudUdpPort
        {
            get
            {
                lock (f_Lock)
                {
                    return f_CloudUdpPort;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_CloudUdpPort = value;
                }
            }
        }

        public int ThirdUdpPort
        {
            get
            {
                lock (f_Lock)
                {
                    return f_ThirdUdpPort;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_ThirdUdpPort = value;
                }
            }
        }

        public int CloudElevatorCount
        {
            get
            {
                lock (f_Lock)
                {
                    return f_CloudElevatorCount;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_CloudElevatorCount = value;
                }
            }
        }

        public Dictionary<int, LinkageCtrlItemInfo> CloudElevatorItems
        {
            get
            {
                lock (f_Lock)
                {
                    return f_CloudElevatorItems;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_CloudElevatorItems = value;
                }
            }
        }

        public LinkageCtrlInfo()
        {
            this.InitCtrlItems();
        }

        private void InitCtrlItems() //最大数量8个
        {
            this.CloudElevatorItems = new Dictionary<int, LinkageCtrlItemInfo>();
            for(int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
            {
                LinkageCtrlItemInfo ctrlItemInfo = new LinkageCtrlItemInfo()
                {
                    ItemId = i
                };
                this.CloudElevatorItems.Add(i, ctrlItemInfo);
            }
        }
    }
}
