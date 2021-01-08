using ITL.DataDefine;
using ITL.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ITL.ParamsSettingTool
{
    public class LocalDBOperate
    {
        private string GetFilePath(string fileName)
        {
            string path = Path.Combine(Application.StartupPath, "DependentFiles", "LocalDBFiles");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);//创建新路径
            }
            return Path.Combine(path, fileName);
        }

        /// <summary>
        /// 根据设备ID获取对应的对应表数据集，使用前创建DeviceTableInfo对象，并复制DevId属性
        /// </summary>
        /// <param name="deviceTableInfo"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public bool GetTableFromDB(DeviceTableInfo deviceTableInfo)
        {
            if (deviceTableInfo == null)
            {
                return false;
            }
            FileStream readerFileStream = null;
            StreamReader recReader = null;
            try
            {
                string filePath = this.GetFilePath(deviceTableInfo.DevId.ToString());
                readerFileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                recReader = new StreamReader(readerFileStream);
                while (true)
                {
                    string recInfoJson = recReader.ReadLine();
                    if (string.IsNullOrWhiteSpace(recInfoJson))
                    {
                        break;
                    }
                    TableInfo tableInfo = JsonConvert.DeserializeObject<TableInfo>(recInfoJson);
                    if (tableInfo == null)
                    {
                        recReader.Close();
                        readerFileStream.Close();
                        File.Delete(filePath);
                        return false;
                    }
                    deviceTableInfo.TableList.Add(tableInfo.AuthId, tableInfo);
                }
                if(deviceTableInfo.TableList.Count <= 0)
                {
                    deviceTableInfo.InitDeviceTableInfoList();
                }
                recReader.Close();
                readerFileStream.Close();
                return true;
            }
            catch (Exception e)
            {
                RunLog.Log(string.Format("获取楼层对应表失败，设备ID：{0}，错误：{1}", deviceTableInfo.DevId, e.Message));
                return false;
            }
        }

        /// <summary>
        /// 保存指定设备ID的对应表数据集，使用前创建DeviceTableInfo对象，并复制DevId属性
        /// </summary>
        /// <param name="deviceTableInfo"></param>
        /// <returns></returns>
        public bool SaveTableToDB(DeviceTableInfo deviceTableInfo)
        {
            if (deviceTableInfo == null)
            {
                return false;
            }
            if (deviceTableInfo.TableList.Count <= 0)
            {
                return false;
            }
            StreamWriter recWriter = null;
            try
            {
                string filePath = this.GetFilePath(deviceTableInfo.DevId.ToString());
                recWriter = new StreamWriter(filePath, true);
                //recWriter.BaseStream.Seek(0, SeekOrigin.End);
                foreach (TableInfo recInfo in deviceTableInfo.TableList.Values)
                {
                    string recInfoJson = JsonConvert.SerializeObject(recInfo);
                    recWriter.WriteLine(recInfoJson);
                    recWriter.Flush();
                }
                recWriter.Close();
                return true;
            }
            catch (Exception e)
            {
                RunLog.Log(string.Format("保存楼层对应表失败，设备ID：{0}，错误：{1}", deviceTableInfo.DevId, e.Message));
                return false;
            }
        }
    }
}
