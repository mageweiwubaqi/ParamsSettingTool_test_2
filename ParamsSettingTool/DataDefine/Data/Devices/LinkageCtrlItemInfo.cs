using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    public class LinkageCtrlItemInfo: BaseInfo
    {
        private int f_ItemId = 0;
        private string f_DevIp = string.Empty;
        private int f_CtrlProporties = 0;

        public int ItemId
        {
            get
            {
                lock (f_Lock)
                {
                    return f_ItemId;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_ItemId = value;
                }
            }
        }

        public string DevIp
        {
            get
            {
                lock (f_Lock)
                {
                    return f_DevIp;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_DevIp = value;
                }
            }
        }

        public int CtrlProporties
        {
            get
            {
                lock (f_Lock)
                {
                    return f_CtrlProporties;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_CtrlProporties = value;
                }
            }
        }
    }
}
