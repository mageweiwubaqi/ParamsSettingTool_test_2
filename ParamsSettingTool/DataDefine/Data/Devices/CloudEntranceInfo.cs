using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    /// <summary>
    /// 云群控制器.
    /// </summary>
    public class CloudEntranceInfo : GeneralDeviceInfo
    {
        private bool f_DHCPEnable = false;
        private string f_DNSServerIp = string.Empty;
        private string f_EIServerIp = string.Empty;
        private int f_EIServerPort = 0;
        private int f_LinkagePort = 0;

        private string f_ProjectNo = "00000000";
        private string f_VerSion = string.Empty;
        private string f_linkAgeIP = string.Empty;
        private string f_ConnVersion = string.Empty;
        private int f_DevType = 0;

        private string f_MJLinkagePram = string.Empty;
        private string f_DevStatus = string.Empty;

        private string f_NewBenchmarkIP = string.Empty;
        private string f_NewBenchmarkPort = string.Empty;
        private bool f_HasNewBenchmark = false;



        public string ConnVersion
        {
            get
            {
                lock (f_Lock)
                {
                    return f_ConnVersion;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_ConnVersion = value;
                }
            }
        }

        public string linkAgeIP
        {
            get
            {
                lock (f_Lock)
                {
                    return f_linkAgeIP;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_linkAgeIP = value;
                }
            }
        }


        //版本号
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

        //项目编号
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
        ///门禁类型
        /// </summary>
        public int DevType
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DevType;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DevType = value;
                }
            }
        }


        ///// <summary>
        ///// 门参数
        ///// </summary>
        //public string MJLinkagePram
        //{
        //    get
        //    {
        //        lock (f_Lock)
        //        {
        //            return f_MJLinkagePram;
        //        }
        //    }
        //    set
        //    {
        //        lock (f_Lock)
        //        {
        //            f_MJLinkagePram = value;
        //        }
        //    }
        //}

        /// <summary>
        ///设备状态
        /// </summary>
        public string DevStatus
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DevStatus;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DevStatus = value;
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
