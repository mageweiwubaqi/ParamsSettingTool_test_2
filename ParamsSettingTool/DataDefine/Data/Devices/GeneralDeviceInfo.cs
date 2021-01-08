using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    public class GeneralDeviceInfo: BaseInfo
    {
        private int f_DevId = 0;
        private string f_DevMac = string.Empty;      
        private string f_DevIp = string.Empty;
        private string f_SubnetMask = string.Empty;
        private string f_GateWay = string.Empty;

        public int DevId
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DevId;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DevId = value;
                }
            }
        }

        public string DevMac
        {
            get
            {
                lock(f_Lock)
                {
                    return f_DevMac;
                }
            }
            set
            {
                lock(f_Lock)
                {
                    f_DevMac = value;
                }
            }
        }
             
        public string DevIp
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DevIp;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DevIp = value;
                }
            }
        }

        /// <summary>
        /// 子网掩码
        /// </summary>
        public string SubnetMask
        {
            get
            {
                lock (f_Lock)
                {
                    return f_SubnetMask;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_SubnetMask = value;
                }
            }
        }

        /// <summary>
        /// 网关
        /// </summary>
        public string GateWay
        {
            get
            {
                lock (f_Lock)
                {
                    return f_GateWay;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_GateWay = value;
                }
            }
        }
    }
}
