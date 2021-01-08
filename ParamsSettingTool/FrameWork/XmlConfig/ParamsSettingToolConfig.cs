using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ITL.Framework
{
    public class ParamsSettingToolConfig : XmlConfig
    {
        protected override void InitDefaultListAndConfigFile()
        {
            base.InitDefaultListAndConfigFile();
         
            ConfigFileWithOutExtension = Path.Combine(Application.StartupPath, string.Format("{0}", nameof(ParamsSettingToolConfig)));
            InitDefaultList();

        }

        protected virtual void InitDefaultList()
        {
           // InitDefaultListAndConfigFile();
        }

        protected virtual void AddDefaultItem(object keyName, object value)
        {
            AddDefaultConfig(GetType().Name, keyName, value);
        }

    }
}
