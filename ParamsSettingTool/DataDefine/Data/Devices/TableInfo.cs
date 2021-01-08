using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    public class TableInfo
    {

        private object f_Lock = new object();

        private int f_AuthId = 0;
        private int f_TerminalNo = 0;
        private int f_IntercomTerminalNo = 0;
        private int f_RealFloorNo = 0;
        private int f_StatusFloorNo = 0;
        private string f_FloorName = string.Empty;

        public int AuthId
        {
            get
            {
                lock (f_Lock)
                {
                    return f_AuthId;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_AuthId = value;
                }
            }
        }

        public int TerminalNo
        {
            get
            {
                lock (f_Lock)
                {
                    return f_TerminalNo;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_TerminalNo = value;
                }
            }
        }

        public int IntercomTerminalNo
        {
            get
            {
                lock (f_Lock)
                {
                    return f_IntercomTerminalNo;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_IntercomTerminalNo = value;
                }
            }
        }

        public int RealFloorNo
        {
            get
            {
                lock (f_Lock)
                {
                    return f_RealFloorNo;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_RealFloorNo = value;
                }
            }
        }

        public int StatusFloorNo
        {
            get
            {
                lock (f_Lock)
                {
                    return f_StatusFloorNo;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_StatusFloorNo = value;
                }
            }
        }

        public string FloorName
        {
            get
            {
                lock (f_Lock)
                {
                    return f_FloorName;
                }

            }
            set
            {
                lock (f_Lock)
                {
                    f_FloorName = value;
                }
            }
        }
    }
}
