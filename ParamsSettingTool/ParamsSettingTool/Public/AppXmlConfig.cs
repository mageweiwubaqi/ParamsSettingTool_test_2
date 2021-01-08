using ITL.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ITL.ParamsSettingTool
{
    public class AppXmlConfig : ParamsSettingToolConfig
    {
        public const string UDP_SOURCE_PORT = "udp_source_port";
        public const string UDP_PURPOSE_PORT = "udp_purpose_port";
        public const string SYSTEM_PSD = "system_psd";


        private volatile static AppXmlConfig f_Singleton = null;


        protected override void InitDefaultList()
        {
            base.InitDefaultList();
            AddDefaultItem(UDP_SOURCE_PORT, "12580");
            AddDefaultItem(UDP_PURPOSE_PORT, "10086");
            AddDefaultItem(SYSTEM_PSD, KeyMacOperate.DEFAULT_SYSTEM_ENCRY_PSD);

        }

        /// <summary>
        /// 单例模式
        /// </summary>
        public static AppXmlConfig Singleton
        {
            get
            {
                if (f_Singleton == null)
                {
                    lock (f_LockHelper)
                    {
                        if (f_Singleton == null)
                        {
                            f_Singleton = new AppXmlConfig();
                        }
                    }
                }
                return f_Singleton;
            }
        }


    }
}
