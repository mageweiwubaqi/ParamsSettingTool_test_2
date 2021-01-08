using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    /// <summary>
    /// 云群控制器.
    /// </summary>
    public class CloudGroupCloudLinkageInfo : GeneralDeviceInfo
    {
        private bool f_DHCPEnable = false;      
        private string f_DNSServerIp = string.Empty;
        private string f_EIServerIp = string.Empty;
        private int f_EIServerPort = 0;
        private int f_LinkagePort = 0;
        private string f_ProjectNo = "00000000";
        private string f_VerSion = string.Empty;
        private string f_GroupDev_Type = string.Empty;
        private string f_MainGroupDev_IP = string.Empty;

        private bool f_IsCommGroupDev = false;  //是否普通群控器

        private string f_NewBenchmarkIP = string.Empty;
        private string f_NewBenchmarkPort = string.Empty;
        private bool f_HasNewBenchmark = false;



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

        /// <summary>
        ///新基点IP
        /// </summary>
        public string NewBenchmarkIP
        {
            get
            {
                lock (f_Lock)
                {
                    return f_NewBenchmarkIP;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_NewBenchmarkIP = value;
                }
            }
        }

        /// <summary>
        ///新基点端口
        /// </summary>
        public string NewBenchmarkPort
        {
            get
            {
                lock (f_Lock)
                {
                    return f_NewBenchmarkPort;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_NewBenchmarkPort = value;
                }
            }
        }

        /// <summary>
        /// 是否具有新基点属性
        /// </summary>
        public bool HasNewBenchmark
        {
            get
            {
                lock (f_Lock)
                {
                    return f_HasNewBenchmark;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_HasNewBenchmark = value;
                }
            }
        }

    }
}
