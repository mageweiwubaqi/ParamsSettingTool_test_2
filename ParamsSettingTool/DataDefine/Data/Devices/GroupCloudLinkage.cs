using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    /// <summary>
    /// 群控制器.
    /// </summary>
    public class GroupCloudLinkage : GeneralDeviceInfo
    {
        private bool f_DHCPEnable = false;      
        private string f_DNSServerIp = string.Empty;

        private int f_DevStatues;

        #region 群控器这部分信息暂时不需要

        private string f_EIServerIp = string.Empty;
        private int f_EIServerPort = 0;
        private int f_LinkagePort = 0;
        private string f_ProjectNo = "00000000";

        #endregion

        private string f_VerSion = string.Empty;
        private string f_GroupDev_Type = string.Empty;
        private string f_MainGroupDev_IP = string.Empty;

        private bool f_IsCommGroupDev = false;  //是否普通群控器

        private int f_CloudElevatorCount = 0;
        private int f_CloudGroupCtrlCount = 0;

        private Dictionary<int, GroupLinkageCtrlItemInfo> f_CloudElevatorItems = null;
        private Dictionary<int, GroupLinkageCtrlItemInfo> f_CloudGroupCtrlItems = null;



        /// <summary>
        /// 版本号
        /// </summary>
        public string VerSion
        {
            get
            {
                lock (f_Lock)
                {
                    return f_VerSion;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_VerSion = value;
                }
            }
        }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectNo
        {
            get
            {
                lock (f_Lock)
                {
                    return f_ProjectNo;
                }
                
            }
            set
            {
                lock (f_Lock)
                {
                    f_ProjectNo = value;
                }
            }
        }
        /// <summary>
        /// DHCP功能
        /// </summary>
        public bool DHCPEnable
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DHCPEnable;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DHCPEnable = value;
                }
            }
        }
     
        /// <summary>
        /// DNS服务器
        /// </summary>
        public string DNSServerIp
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DNSServerIp;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DNSServerIp = value;
                }
            }
        }

        /// <summary>
        /// DNS服务器
        /// </summary>
        public int DevStatues
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DevStatues;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DevStatues = value;
                }
            }
        }
       
        /// <summary>
        /// 线下物联平台服务器IP
        /// </summary>
        public string EIServerIp
        {
            get
            {
                lock (f_Lock)
                {
                    return f_EIServerIp;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_EIServerIp = value;
                }
            }
        }

        /// <summary>
        /// 线下物联平台服务器端口
        /// </summary>
        public int EIServerPort
        {
            get
            {
                lock (f_Lock)
                {
                    return f_EIServerPort;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_EIServerPort = value;
                }
            }
        }

        /// <summary>
        /// 联动器端口
        /// </summary>
        public int LinkagePort
        {
            get
            {
                lock (f_Lock)
                {
                    return f_LinkagePort;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_LinkagePort = value;
                }
            }
        }

        /// <summary>
        /// 群控器设备类型
        /// </summary>
        public string GroupDevType
        {
            get
            {
                lock (f_Lock)
                {
                    return f_GroupDev_Type;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_GroupDev_Type = value;
                }
            }
        }

        /// <summary>
        /// 主群控器IP
        /// </summary>
        public string MainGroupDevIP
        {
            get
            {
                lock (f_Lock)
                {
                    return f_MainGroupDev_IP;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_MainGroupDev_IP = value;
                }
            }
        }

        /// <summary>
        /// 是否普通群控器
        /// </summary>
        public bool IsCommGroupDev
        {
            get
            {
                lock (f_Lock)
                {
                    return f_IsCommGroupDev;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_IsCommGroupDev = value;
                }
            }
        }

        public Dictionary<int, GroupLinkageCtrlItemInfo> CloudElevatorItems
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

        public Dictionary<int, GroupLinkageCtrlItemInfo> CloudGropCtrlItems
        {
            get
            {
                lock (f_Lock)
                {
                    return f_CloudGroupCtrlItems;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_CloudGroupCtrlItems = value;
                }
            }
        }

        /// <summary>
        /// 云电梯数量
        /// </summary>
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

        /// <summary>
        /// 云群控器数量
        /// </summary>
        public int CloudGroupCtrlCount
        {
            get
            {
                lock (f_Lock)
                {
                    return f_CloudGroupCtrlCount;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_CloudGroupCtrlCount = value;
                }
            }
        }

        /// <summary>
        /// 构造函数，先根据传入的设备数量分别创建云电梯和云群控器的数据
        /// </summary>
        public GroupCloudLinkage()
        {
            this.InitCtrlItems();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitCtrlItems() //
        {
            this.CloudElevatorItems = new Dictionary<int, GroupLinkageCtrlItemInfo>();
            //最大数量8个
            for (int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_ELEVATOR_MAX_COUNT; i++)
            {
                GroupLinkageCtrlItemInfo ctrlItemInfo = new GroupLinkageCtrlItemInfo()
                {
                    ItemId = i
                };
                this.CloudElevatorItems.Add(i, ctrlItemInfo);
            }

            this.CloudGropCtrlItems = new Dictionary<int, GroupLinkageCtrlItemInfo>();
            for (int i = 1; i <= AppConst.LINKAGE_CTRL_CLOUD_GROUPLINKAGE_MAX_COUNT; i++)
            {
                //最大数量2个
                GroupLinkageCtrlItemInfo ctrlItemInfo = new GroupLinkageCtrlItemInfo()
                {
                    ItemId = i
                };
                this.CloudGropCtrlItems.Add(i, ctrlItemInfo);
            }
        }




    }
}
