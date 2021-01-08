using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ITL.Framework
{
    public class XmlConfig : GeneralConifg
    {


        public XmlConfig():base()
        {
            ConfigFileExtensionName = ".xml";
        }

        //public XmlConfig(ConcurrentDictionary<string, ConcurrentDictionary<object, object>> defautlSettingList, string configFile) 
        //    :base(defautlSettingList,configFile)
        //{

        //}


        #region  私有函数
        protected override bool LoadSettings()
        {
            LastErrMsg = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigFile);
            XmlNode rootNode = xmlDoc.SelectSingleNode(CONFIG_SET_NAME);
            foreach (XmlNode sectionNode in rootNode.ChildNodes)
            {
                string sectionName = sectionNode.Name;

                foreach (XmlNode itemNode in sectionNode.ChildNodes)
                {
                    string keyTypeName = itemNode.Attributes[CONFIG_KEY_TYPE]?.Value?.ToString();
                    string valueTypeName = itemNode.Attributes[CONFIG_VALUE_TYPE]?.Value?.ToString();
                    Type keyType = Type.GetType(keyTypeName);
                    Type valueType = Type.GetType(valueTypeName);
                    this[sectionName, itemNode.Name.Format(keyType)] = itemNode.InnerText.Format(valueType);

                    //if (!SettingList.ContainsKey(sectionNode.Name.Format(keyType)))
                    //{
                    //    bool isAddOk = SettingList.TryAdd(sectionNode.Name.Format(keyType), sectionNode.InnerText.Format(valueType));
                    //    if (!isAddOk)
                    //    {
                    //        LastErrMsg = string.Format("Load xml file failed, can not add node key:{0} value:{1} to SettingList", sectionNode.Name, sectionNode.InnerText);
                    //        return false;
                    //    }
                    //}
                }
             

           

         

            }
            return true;
        }

        protected override bool SaveSettings(ConcurrentDictionary<string, ConcurrentDictionary<object,object>> settingList)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //建立Xml的定义声明
            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(dec);

            ////创建根节点
            XmlElement rootElement = xmlDoc.CreateElement(CONFIG_SET_NAME);
            xmlDoc.AppendChild(rootElement);
            XmlElement xmlElment = null;
            foreach (var sectionItem in settingList)
            {
                string sectionName = sectionItem.Key;
                ConcurrentDictionary<object, object> oneSection = sectionItem.Value;
                XmlElement sectionElement = xmlDoc.CreateElement(sectionName);
                rootElement.AppendChild(sectionElement);
                foreach (var keyValueItem in oneSection)
                {
                    xmlElment = xmlDoc.CreateElement(keyValueItem.Key.ToString());
                    xmlElment.InnerText = keyValueItem.Value.ToString();
                    xmlElment.SetAttribute(CONFIG_KEY_TYPE, keyValueItem.Key?.GetType().ToString());
                    xmlElment.SetAttribute(CONFIG_VALUE_TYPE, keyValueItem.Value?.GetType().ToString());
                    sectionElement.AppendChild(xmlElment);
                }
  
            }
            xmlDoc.Save(ConfigFile);

            return true;
        }
        #endregion
    }


}