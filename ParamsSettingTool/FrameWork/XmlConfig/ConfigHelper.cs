using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ITL.Framework
{
    public static class ConfigHelper
    {

        public static string GetExceptionInfo(Exception e)
        {
            string strInfo = string.Format(" Exception:{0} StackTrace:{1},InnerException:{2},Message:{3}", e, e.StackTrace, e.InnerException, e.Message);
            return strInfo;
        }

        /// <summary>
        /// 将字符串格式化成指定的数据类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Format(this string str, Type type)
        {
            object obj = ChangeStringToType(str, type);
            return obj;
        }

        public static object ChangeStringToType(string value, Type type)
        {
            return Convert.ChangeType(value, type);
        }
    }
}
