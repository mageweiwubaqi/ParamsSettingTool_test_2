using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.ParamsSettingTool
{
    public class AppEnv
    {
        private object f_Lock = new object();

        private int f_CmdNumber = 0;
        private string f_SystemPsd = string.Empty;

        private int f_UdpCount = 0;

       
        public int CmdNumber
        {
            get
            {
                lock(f_Lock)
                {
                    return f_CmdNumber;
                }
            }
            set
            {
                lock(f_Lock)
                {
                    f_CmdNumber = value;
                }
            }
        }

        public int UdpCount
        {
            get
            {
                lock (f_Lock)
                {
                    return f_UdpCount;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_UdpCount = value;
                }
            }
        }



        public string SystemPsd
        {
            get
            {
                lock (f_Lock)
                {
                    return f_SystemPsd;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_SystemPsd = value;
                }
            }
        }
        public AppEnv()
        {
            
        }

        #region 单例模式
        private volatile static AppEnv f_AppEnv = null;

        private static readonly object f_LockHelper = new object();
        public static AppEnv Singleton
        {
            get
            {
                if (f_AppEnv == null)
                {
                    lock (f_LockHelper)
                    {
                        if (f_AppEnv == null)
                        {
                            f_AppEnv = new AppEnv();
                        }
                    }
                }
                return f_AppEnv;
            }
        }
        #endregion
    }
}
