using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    public class DownTableInfo: BaseInfo
    {
        private string f_FloorTable = string.Empty;
        private string f_IntercomFloorTable = string.Empty;
        private string f_RealFloorTable = string.Empty;
        private string f_StatusFloorTable = string.Empty;

        public string FloorTable
        {
            get
            {
                lock (f_Lock)
                {
                    return f_FloorTable;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_FloorTable = value;
                }
            }
        }

        public string IntercomFloorTable
        {
            get
            {
                lock (f_Lock)
                {
                    return f_IntercomFloorTable;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_IntercomFloorTable = value;
                }
            }
        }

        public string RealFloorTable
        {
            get
            {
                lock (f_Lock)
                {
                    return f_RealFloorTable;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_RealFloorTable = value;
                }
            }
        }

        public string StatusFloorTable
        {
            get
            {
                lock (f_Lock)
                {
                    return f_StatusFloorTable;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_StatusFloorTable = value;
                }
            }
        }
    }
}
