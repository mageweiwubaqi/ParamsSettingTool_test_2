using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    public class DeviceTableInfo: BaseInfo
    {
        private int f_DevId = 0;
        private Dictionary<int, TableInfo> f_TableList = null;

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

        public Dictionary<int, TableInfo> TableList
        {
            get
            {
                lock (f_Lock)
                {
                    return f_TableList;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_TableList = value;
                }
            }
        }

        public DeviceTableInfo()
        {
            this.TableList = new Dictionary<int, TableInfo>();
        }

        public void InitDeviceTableInfoList()
        {
            this.TableList.Clear();
            for(int i = 1; i <= 112; i++)
            {
                TableInfo tableInfo = new TableInfo()
                {
                    AuthId = i,
                    TerminalNo = i,
                    IntercomTerminalNo = i,
                    RealFloorNo = i,
                    StatusFloorNo = i,
                    FloorName = i + "层"
                };
                this.TableList.Add(tableInfo.AuthId, tableInfo);
            }
        }
    }
}
