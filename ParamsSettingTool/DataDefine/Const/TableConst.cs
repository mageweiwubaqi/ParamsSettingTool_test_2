using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    class TableConst
    {
        /// <summary>
        /// {"id":"   ID
        /// </summary>
        public const string STR_ID = "//{"id/":/"";
        /// <summary>
        /// ",   逗号与收尾双引号
        /// </summary>
        public const string STR_SPLIT = "22 2C";
        /// <summary>
        /// //"floorName":"  楼层名称
        /// </summary>
        public const string STR_FLOORNAME = "22 66 6C 6F 6F 72 4E 61 6D 65 22 3A 22";
        /// <summary>
        /// "actualFloorNum":"   实际楼层
        /// </summary>
        public const string STR_ACTUALFLOOR_NUM = "22 61 63 74 75 61 6C 46 6C 6F 6F 72 4E 75 6D 22 3A 22";
        /// <summary>
        /// //"detectedFloorNum":"   检测楼层
        /// </summary>
        public const string STR_DETECTEDFLOOR_NUM = "22 64 65 74 65 63 74 65 64 46 6C 6F 6F 72 4E 75 6D 22 3A 22";
        /// <summary>
        /// //"terminalNum":"   端子号
        /// </summary>
        public const string STR_TERMINAL_NUM = "22 74 65 72 6D 69 6E 61 6C 4E 75 6D 22 3A 22";
        /// <summary>
        /// //"terminalNumIntercom":"   对对讲楼层端子号
        /// </summary>
        public const string STR_TERMINAL_NUM_INTERCOM = "22 74 65 72 6D 69 6E 61 6C 4E 75 6D 49 6E 74 65 72 63 6F 6D 22 3A 22";
        /// <summary>
        /// "terminalNumSlave1"   第一副操纵盘端子号
        /// </summary>
        public const string STR_TERMINALNUMSLAVE1_HEX = "22 74 65 72 6d 69 6e 61 6c 4e 75 6d 53 6c 61 76 65 31 22";//
        /// <summary>
        /// "terminalNumSlave2"   第二副操纵盘端子号
        /// </summary>
        public const string STR_TERMINALNUMSLAVE2_HEX = "22 74 65 72 6d 69 6e 61 6c 4e 75 6d 53 6c 61 76 65 32 22";//
        /// <summary>
        /// "}   结束的大括号
        /// </summary>
        public const string STR_END = "22 7D";
    }
}
