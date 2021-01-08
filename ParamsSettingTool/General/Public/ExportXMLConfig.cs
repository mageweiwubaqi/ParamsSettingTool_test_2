using ITL.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.General
{
    public class ExportXMLConfig: ParamsSettingToolConfig
    {
        public const string EXPORT_PATH = "export_path";
        public const string AUTO_OPEN = "export_auto_open";

        private volatile static ExportXMLConfig f_Singleton = null;

        protected override void InitDefaultList()
        {
            base.InitDefaultList();
            AddDefaultItem(EXPORT_PATH, "");
            AddDefaultItem(AUTO_OPEN, "0");
        }

        /// <summary>
        /// 单例模式
        /// </summary>
        public static ExportXMLConfig Singleton
        {
            get
            {
                if (f_Singleton == null)
                {
                    lock (f_LockHelper)
                    {
                        if (f_Singleton == null)
                        {
                            f_Singleton = new ExportXMLConfig();
                        }
                    }
                }
                return f_Singleton;
            }
        }
    }
}
