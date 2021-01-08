using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    /// <summary>
    /// 群控器的云电梯与云群控器属性
    /// </summary>
    public class GroupLinkageCtrlItemInfo : BaseInfo
    {
        private int f_ItemId = 0;
        private string f_DevIp = string.Empty;
        /// <summary>
        /// 状态
        /// </summary>
        private int f_ConStatues = 0;
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


        public int ConStatues
        {
            get
            {
                lock (f_Lock)
                {
                    return f_ConStatues;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_ConStatues = value;
                }
            }
        }
    }
}
