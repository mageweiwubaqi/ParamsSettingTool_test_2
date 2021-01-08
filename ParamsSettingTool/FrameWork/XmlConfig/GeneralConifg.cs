using ITL.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ITL.Framework
{

    /// <summary>
    /// 配置文件通用类型，配置文件结构为二层树结构 
    /// 配置块-配置项(Key=value)
    /// </summary>
    public class GeneralConifg
    {
        #region 成员变量

        protected const string CONFIG_SET_NAME = "settings";
        protected const string CONFIG_SECTION = "config_section";
        protected const string CONFIG_KEY = "config_key";
        protected const string CONFIG_KEY_TYPE = "key_type";
        protected const string CONFIG_VALUE = "config_value";
        protected const string CONFIG_VALUE_TYPE = "value_type";

        /// <summary>
        /// 线程安全锁
        /// </summary>
        protected object f_Lock = new object();


        /// <summary>
        /// 是否已加载
        /// </summary>
        private bool f_IsLoad = false;

        /// <summary>
        /// 线程安全锁，单例模式专用
        /// </summary>
        protected static readonly object f_LockHelper = new object();
        /// <summary>
        /// 错误信息
        /// </summary>
        private string f_LastErrMsg = string.Empty;

        /// <summary>
        /// 配置文件扩展名，即文件类型 eg:xml,db
        /// </summary>
        private string f_ConfigFileExtensionName;

        /// <summary>
        /// 配置文件路径及名称(不包含配置文件扩展名，即文件类型)
        /// </summary>
        private string f_ConfigFileWithOutExtension;



        /// <summary>
        /// 配置文件完成路径及名称
        /// </summary>
        private string f_ConfigFile;

        /// <summary>
        /// 设置信息（sectionName:<Key=Value>）
        /// </summary>
        private ConcurrentDictionary<string, ConcurrentDictionary<object, object>> f_SettingList;


        /// <summary>
        /// 默认设置信息[可选]（Key=Value）,若文件不存在，将自动创建，并根据DefaultSettingList进行初始化
        /// 若访问到值不存在，将自动使用DefaultSettingList中到对应值
        /// </summary>
        private ConcurrentDictionary<string, ConcurrentDictionary<object, object>> f_DefaultSettingList;


        /// <summary>
        /// 错误信息
        /// </summary>
        public string LastErrMsg
        {
            get
            {
                lock (f_Lock)
                {
                    return f_LastErrMsg;
                }
            }
            protected set
            {
                lock (f_Lock)
                {
                    f_LastErrMsg = value;
                }
            }
        }

        /// <summary>
        /// 是否已加载
        /// </summary>
        public bool IsLoad
        {
            get
            {
                lock (f_Lock)
                {
                    return f_IsLoad;
                }
            }
            protected set
            {
                lock (f_Lock)
                {
                    f_IsLoad = value;
                }
            }
        }

        /// <summary>
        /// 配置文件路径及名称(不包含配置文件扩展名，即文件类型)
        /// </summary>
        public string ConfigFileWithOutExtension
        {
            get
            {
                lock (f_Lock)
                {
                    return f_ConfigFileWithOutExtension;
                }
            }
            protected set
            {
                lock (f_Lock)
                {
                    if (f_ConfigFileWithOutExtension != value)
                    {
                        f_ConfigFileWithOutExtension = value;
                        CheckLoad();
                    }

                }
            }
        }


        /// <summary>
        ///   配置文件扩展名，即文件类型 eg:xml,db
        /// </summary>
        public string ConfigFileExtensionName
        {
            get
            {
                lock (f_Lock)
                {
                    return f_ConfigFileExtensionName;
                }
            }
            protected set
            {
                lock (f_Lock)
                {

                    if(f_ConfigFileExtensionName != value)
                    {
                        f_ConfigFileExtensionName = value;
                        CheckLoad();
                    }
                }
            }
        }


        /// <summary>
        ///   配置文件扩展名，即文件类型 eg:xml,db
        /// </summary>
        public virtual string ConfigFile
        {
            get
            {
                lock (f_Lock)
                {
                    f_ConfigFile = Path.ChangeExtension(f_ConfigFileWithOutExtension, f_ConfigFileExtensionName);
                    return f_ConfigFile;
                }
            }

        }


        /// <summary>
        /// Xml文件操作对象
        /// </summary>
        public ConcurrentDictionary<string, ConcurrentDictionary<object, object>> SettingList
        {
            get
            {
                lock (f_Lock)
                {
                    return f_SettingList;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_SettingList = value;
                }
            }
        }


        /// <summary>
        /// 默认设置信息[可选]（Key=Value）,若文件不存在，将自动创建，并根据DefaultSettingList进行初始化
        /// 若访问到值不存在，将自动使用DefaultSettingList中到对应值
        /// </summary>
        public ConcurrentDictionary<string, ConcurrentDictionary<object, object>> DefaultSettingList
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DefaultSettingList;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DefaultSettingList = value;
                }
            }
        }


        #endregion

        #region 构造函数
        //public GeneralConifg(ConcurrentDictionary<string, ConcurrentDictionary<object, object>> defautlSettingList, string configFile)
        //{
        //    f_LastErrMsg = string.Empty;
        //    f_SettingList = new ConcurrentDictionary<string, ConcurrentDictionary<object, object>>();
        //    f_DefaultSettingList = defautlSettingList;
        //    f_ConfigFileWithOutExtension = configFile;

        //}

        public GeneralConifg() //: this(null, string.Empty)
        {
            f_DefaultSettingList = new ConcurrentDictionary<string, ConcurrentDictionary<object, object>>();
            f_SettingList = new ConcurrentDictionary<string, ConcurrentDictionary<object, object>>();
            f_IsLoad = false;
            InitDefaultListAndConfigFile();
        }


        /// <summary>
        /// 检查是否已加载配置，若未加载，则进行加载
        /// </summary>
        private void CheckLoad()
        {
            if ((!IsLoad) && (File.Exists(ConfigFile)))
            {
                this.Load();
                IsLoad = true;
            }
        }
        #endregion


        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        protected virtual bool LoadSettings()
        {
            CheckRepeatKey();
            return false;
        }


        /// <summary>
        ///保存配置
        /// </summary>
        /// <param name="settingList"></param>
        /// <returns></returns>
        protected virtual bool SaveSettings(ConcurrentDictionary<string, ConcurrentDictionary<object, object>> settingList)
        {
            return true;
        }


        /// <summary>
        /// 初始化默认列表并指定配置文件路径和名称
        /// </summary>
        /// <returns></returns>
        protected virtual void InitDefaultListAndConfigFile()
        {

        }


        /// <summary>
        /// 增加默认配置项目
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected void AddDefaultConfig(string sectionName, object keyName, object value)
        {
            bool isAddOk = false;
            ConcurrentDictionary<object, object> oneSection = null;
            if (DefaultSettingList.ContainsKey(sectionName))
            {
                oneSection = DefaultSettingList[sectionName];
            }
            else
            {
                oneSection = new ConcurrentDictionary<object, object>();
                isAddOk = DefaultSettingList.TryAdd(sectionName, oneSection);
                if (!isAddOk)
                {
                    RunLog.Log(string.Format("Add default configuration seciton:{0} failed", sectionName));
                }
            }

            if (oneSection.ContainsKey(keyName))
            {
                RunLog.Log(string.Format("Duplicate default configuration:{0}", keyName));
                //oneSection[keyName] = value;
            }
            else
            {
                isAddOk = oneSection.TryAdd(keyName, value);
                if (!isAddOk)
                {
                    RunLog.Log(string.Format("Add default configuration:{0} {1} failed", keyName, value));
                }
            }

        }


        #region 私有函数


        /// <summary>
        /// 检测重复Key(防呆设计)
        /// </summary>
        private void CheckRepeatKey()
        {
            string sectionName = string.Empty;

            ConcurrentDictionary<object, object> hashSetting = new ConcurrentDictionary<object, object>();
            //ConcurrentDictionary<object, object> oneSection = null;
            foreach (var checkItem in SettingList)
            {

                string checkSectionName = checkItem.Key;

                ConcurrentDictionary<object, object> checkOneSection = checkItem.Value;
                foreach (var item in checkOneSection)
                {
                    if (hashSetting.ContainsKey(item.Key))
                    {
                        RunLog.Log(string.Format("Do not allow duplicate key:{0}", item.Key));
                    }
                    else
                    {
                        hashSetting.TryAdd(item.Key, item.Value);
                    }
                }

            }
        }
        /// <summary>
        /// 根据Key获取第一个找到到配置值,
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private object GetValueByKey(object keyName)
        {
            LastErrMsg = string.Empty;
            try
            {
                object valueObj = null;
                bool isAddOk = false;
                string sectionName = string.Empty;
                ConcurrentDictionary<object, object> oneSection = null;
                foreach (var item in SettingList)
                {
                    sectionName = item.Key;
                    oneSection = item.Value;
                    if (oneSection.ContainsKey(keyName))
                    {
                        valueObj = oneSection[keyName];
                        return valueObj;
                    }
                }
                //如果在加载的正式配置列表中没有，但在默认配置项列表中存在，则取默认值,并将默认值加入到正式列表中用于后续保存
                foreach (var defaultItem in DefaultSettingList)
                {
                    sectionName = defaultItem.Key;
                    oneSection = defaultItem.Value;
                    if (!oneSection.ContainsKey(keyName))
                    {
                        continue;
                    }
                    valueObj = oneSection[keyName];
                    if (valueObj == null)
                    {
                        continue;
                    }


                    if (SettingList.ContainsKey(sectionName))
                    {
                        oneSection = SettingList[sectionName];
                    }
                    else
                    {
                        oneSection = new ConcurrentDictionary<object, object>();
                        isAddOk = SettingList.TryAdd(sectionName, oneSection);
                        if (!isAddOk)
                        {
                            LastErrMsg = string.Format("Can not find the value of {0} and add section:{0} failed!", keyName, sectionName);
                            return null;
                        }
                    }
                    isAddOk = oneSection.TryAdd(keyName, valueObj);
                    if (!isAddOk)
                    {
                        LastErrMsg = string.Format("Can not find the value of {0} and add failed!", keyName);
                        return null;
                    }
                    return valueObj;
                }
            }
            catch (Exception e)
            {
                LastErrMsg = string.Format("get the value of {0} failed!  {1}", keyName, ConfigHelper.GetExceptionInfo(e));
                return null;
            }
            return null;
        }



        /// <summary>
        /// 根据section-Key获取找到到配置值,
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private object GetValueByKey(string sectionName, object keyName)
        {
            LastErrMsg = string.Empty;
            try
            {
                object valueObj = null;
                bool isAddOk = false;
                //string sectionName = string.Empty;
                ConcurrentDictionary<object, object> oneSection = null;

                //如果在正式列表中找到，则直接返回对应到配置值
                if (SettingList.ContainsKey(sectionName))
                {
                    oneSection = SettingList[sectionName];
                    if (oneSection.ContainsKey(keyName))
                    {
                        valueObj = oneSection[keyName];
                        return valueObj;
                    }
                }


                //如果在加载的正式配置列表中没有，但在默认配置项列表中存在，则取默认值,并将默认值加入到正式列表中用于后续保存
                if (DefaultSettingList.ContainsKey(sectionName))
                {
                    oneSection = DefaultSettingList[sectionName];
                    if (oneSection.ContainsKey(keyName))
                    {
                        valueObj = oneSection[keyName];


                        if (SettingList.ContainsKey(sectionName))
                        {
                            oneSection = SettingList[sectionName];
                        }
                        else
                        {
                            oneSection = new ConcurrentDictionary<object, object>();
                            isAddOk = SettingList.TryAdd(sectionName, oneSection);
                            if (!isAddOk)
                            {
                                LastErrMsg = string.Format("Can not find the value of {0} and add section:{0} failed!", keyName, sectionName);
                                return null;
                            }
                        }
                        isAddOk = oneSection.TryAdd(keyName, valueObj);
                        if (!isAddOk)
                        {
                            LastErrMsg = string.Format("Can not find the value of {0} and add failed!", keyName);
                            return null;
                        }
                        return valueObj;
                    }
                }

            }
            catch (Exception e)
            {
                LastErrMsg = string.Format("get the value of {0} failed!  {1}", keyName, ConfigHelper.GetExceptionInfo(e));
                return null;
            }
            return null;
        }

        /// <summary>
        /// 设置指定Key的配置值，若key不存在，则设置无效
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        private void SetKeyValue(object keyName, object value)
        {

            LastErrMsg = string.Empty;
            try
            {
                string sectionName = string.Empty;
                ConcurrentDictionary<object, object> oneSection = null;

                foreach (var item in SettingList)
                {
                    sectionName = item.Key;
                    oneSection = item.Value;
                    if (!oneSection.ContainsKey(keyName))
                    {
                        continue;
                    }
                    oneSection[keyName] = value;


                }

            }
            catch (Exception e)
            {
                LastErrMsg = string.Format("Set the value:{0} of {1} failed!  {2}", value, keyName, ConfigHelper.GetExceptionInfo(e));
            }
        }




        /// <summary>
        /// 设置指定setion-Key的配置值,若Key不存在，将自动添加Key并设置值
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <param name="value"></param>
        private void SetKeyValue(string sectionName, object keyName, object value)
        {

            LastErrMsg = string.Empty;
            try
            {

                //string sectionName = string.Empty;
                bool isAddOk = false;
                ConcurrentDictionary<object, object> oneSection = null;
                if (SettingList.ContainsKey(sectionName))
                {
                    oneSection = SettingList[sectionName];
                }
                else
                {
                    oneSection = new ConcurrentDictionary<object, object>();
                    isAddOk = SettingList.TryAdd(sectionName, oneSection);
                    if (!isAddOk)
                    {
                        LastErrMsg = string.Format("Add section:{0} failed!", sectionName);
                        return;
                    }
                }

                if (oneSection.ContainsKey(keyName))
                {
                    oneSection[keyName] = value;
                }
                else
                {
                    isAddOk = oneSection.TryAdd(keyName, value);
                    if (!isAddOk)
                    {
                        LastErrMsg = string.Format("Add name:{0} value:{1}  failed!", keyName, value);
                    }
                }


            }
            catch (Exception e)
            {
                LastErrMsg = string.Format("Set the value:{0} of {1} failed!  {2}", value, keyName, ConfigHelper.GetExceptionInfo(e));
            }
        }
        #endregion

        #region  公共函数


        public bool Load()
        {
            LastErrMsg = string.Empty;
            try
            {
                try
                {
                    //检测提供到文件路径是否是合法格式的文件路径
                    Path.GetFullPath(ConfigFile);
                }
                catch (Exception e)
                {
                    LastErrMsg = string.Format("Invalid config file:{0}!{1}", ConfigFile, ConfigHelper.GetExceptionInfo(e));
                    return false;
                }

                if (File.Exists(ConfigFile))
                {
                    return LoadSettings();
                }
                else
                {
                    bool isSaveOK = SaveSettings(DefaultSettingList);
                    if (!isSaveOK)
                    {
                        return false;
                    }
                    return LoadSettings();
                }
            }
            catch (Exception e)
            {
                LastErrMsg = string.Format("Open config file:{0} failed!{1}", ConfigFile, ConfigHelper.GetExceptionInfo(e));
                return false;
            }
        }


        public bool Save()
        {
            return SaveSettings(SettingList);
        }


        /// <summary>
        /// 根据Key读写配置信息
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>

        public object this[object keyName]
        {
            get
            {
                lock (f_Lock)
                {

                    return GetValueByKey(keyName);
                }
            }

            set
            {
                lock (f_Lock)
                {
                    SetKeyValue(keyName, value);
                }
            }
        }



        /// <summary>
        /// 根据section-key读写配置
        /// </summary>
        /// <param name="section"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public object this[string section, object keyName]
        {
            get
            {
                lock (f_Lock)
                {
                    return GetValueByKey(section, keyName);
                }
            }
            set
            {
                lock (f_Lock)
                {
                    SetKeyValue(section, keyName, value);
                }
            }

        }
        #endregion
    }
}
