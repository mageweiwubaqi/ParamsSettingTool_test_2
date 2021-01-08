/*******************************************************************************
文 件 名：$TextFileLogService.cs$
说    明：此文件封装日志类，并提供__Logger单例全局日志对象实例。
          1.日志格式为纯文本格式，并按日生成。
          2.日志存放位置为主程序\log子目录下。
          3.日志文件数量可由配置文件限制。
             
          此实现基于delphi版本TLogger实现。


创 建 人：刘杰钦
创建时间：2012-07-05
版 本 号：V1.0
版权说明：版权1997-2012(c) 深圳市旺龙智能科技有限公司 
*******************************************************************************/

/*----------------------------------文件说明-------------------------------------
  日志类会在以下情形下对旧日志文件进行清理。
  1.实例创建时
  2.切换日志文件的时
  3.修改LogFileMaxCount属性时
------------------------------------------------------------------------------*/

/*----------------------------------修改历史-------------------------------------
修改时间：
修 改 人：
修改摘要：

修改时间：
修 改 人：
修改摘要：
-------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;

namespace ITL.Framework
{
    /// <summary>
    /// 此类实现ITextFileLogService文本日志接口，按日产生文本日志，日志编码为utf-8.
    /// 可以设置最大日志文件生成量，若日志文件累积超过设定值，此类会自动清理删除较旧
    /// 的日志文件。
    /// </summary>
    public sealed class RunLog:ITextFileLogService
    {
        /// <summary>
        /// 内部常量定义
        /// </summary>
        private const string LOG_SUB_FOLDER=@"log\" ;
        private const string LOG_FILENAME_FORMAT = @"log_{0}.txt" ; //example: log_20110709.txt
        private const string LOG_FILE_HEADER_FORMAT =
            @"
            日志文件创建于: {0}
                     
            日志类型: ?-未知 i-一般 !-警告 x-Error X-异常
            =========================================================

            ";
                    
        private const string LOG_FILE_SEPARATOR = "-------------------------------------------------------------------------------------------------";
        private const string LOG_MSG_TITLE  = "类型\t日期时间\t\t源\t\t\t\t日志内容";
        private const string LOG_MSG_FORMAT = "{0}\t{1,-24}{2,-32} {3}";

        //
        private const string ERR_OPEN_LOGFILE = "创建日志文件(%s)失败!";
        private const string MSG_SWITCH_TO_LOGFILE =  @">>>>>>>切换日志文件到:{0}";
        private const string MSG_SWITCH_FROM_LOGFILE = @"<<<<<<<本日志文件切换自:{0}";

        private const string LOG_FILE_FILTER = @"log_*.txt";

        private static bool f_IsDebug = false;  //是否是Debug状态

 



        #region 单例模式实现
        //静态初始化
        private volatile static RunLog f_Instance = null;

        private static readonly object f_lock = new object();

        //public static void Log(string errorMessage, string v)
        //{
        //    throw new NotImplementedException();
        //}

        //private LogConsole f_LogConsole = null;

      
        private static ITextFileLogService SingletonInstance
        {
            get
            {
                lock (f_lock)
                {
                    return f_Instance??(f_Instance = new RunLog());
                }
            }
        }

        //public static object Singleton { get; internal set; }
        #endregion



        /// <summary>
        /// 类成员
        /// </summary>
        private string f_LogFolder;
        private StreamWriter f_Logger;
        private LogType f_LogLevel;
        private bool f_Enable = false;
        private uint f_LogFileMaxCount;
        private string f_CurrentLogFileName;
        private string f_CurrentDate;
        private bool f_ImmediateFlush;
        private frmLog f_LogForm = null;
        private frmLog LogForm
        {
            get
            {
                lock (f_Lock)
                {
                    return f_LogForm;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_LogForm = value;
                }
            }
        }


        /// <summary>
        /// 是否是Debug模式，Debug模式将弹出日志显示窗
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                lock (f_SaticLock)
                {
                    return f_IsDebug;
                }
            }
            set
            {
                lock (f_SaticLock)
                {
                    f_IsDebug = value;
                }
            }
        }

        private RunLog()
        {
            f_LogFolder = "";
            f_LogLevel = (uint)LogType.ltNone;
            f_Logger = null;
            f_Enable = false;
            f_LogFileMaxCount = 0;
            f_ImmediateFlush = true;
           if (IsDebug)
            {
                //f_LogConsole = new LogConsole();
                Thread logThread = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    LogForm = new frmLog();
                    LogForm.ShowDialog();
                }
                );
                logThread.Start();
            }

            string strLogPath = Path.Combine(Application.StartupPath, LOG_SUB_FOLDER);
            if (!((ILogService)this).Open(strLogPath))
            {
                return;
            };
            f_Enable = true;
        }

        string GetCurrentDateString()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        string GenerateCurrentLogFileName()
        {
            f_CurrentDate = GetCurrentDateString();
            return string.Format(LOG_FILENAME_FORMAT, f_CurrentDate);
        }

        /// <summary>
        /// 创建内部日志对象和初始化操作。
        /// </summary>
        /// <param name="logFolder"></param>
        /// <returns></returns>
        private bool CreateLogger(string logFolder)
        {
            f_LogFolder = logFolder;
            f_CurrentLogFileName = GenerateCurrentLogFileName();
            f_LogLevel = LogType.ltException;
            try
            {
                Directory.CreateDirectory(f_LogFolder);
                CleanupLogFolder();
                return OpenLogFile();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建日志文件。
        /// </summary>
        /// <param name="bSwitch"></param>
        /// <returns></returns>
        private bool OpenLogFile(bool bSwitch = false)
        {
            string logFile = f_LogFolder + f_CurrentLogFileName;
            bool bExisted = File.Exists(logFile);
            try
            {
                if (bExisted)
                {   
                    if (false == bSwitch)
                    {//如果不是日志切换操作，这保留已有日志的内容，如：同一天多次启动系统
                        f_Logger = new StreamWriter(logFile, true);
                        f_Logger.BaseStream.Seek(0, SeekOrigin.End);
                        WriteFileSeparator();
                    }
                    else
                    {//如果切换日志文件发现文件已经存在，则清除已有内容
                        f_Logger = new StreamWriter(logFile, false);
                        //f_Logger.BaseStream.Seek(0, SeekOrigin.Begin);
                        WriteLogFileHeader();
                    }
                }
                else
                {//为新日志文件写入文件头
                    f_Logger = new StreamWriter(logFile, false);
                    WriteLogFileHeader();
                }
            }
            catch
            {
                ((ITextFileLogService)this).Close();
                return false;
            }
            return true;
        }

        private void WriteFileSeparator()
        {
            WriteLog(LOG_FILE_SEPARATOR);
        }

        private void WriteLogFileHeader()
        {
            string hdrStr = string.Format(LOG_FILE_HEADER_FORMAT, DateTime.Now.ToString());
            WriteLog(hdrStr);
            WriteLog(LOG_MSG_TITLE);
            WriteFileSeparator();
        }
        private static object f_SaticLock = new object();
        private object f_Lock = new object();
        private void WriteLog(string aStr)
        {
            lock (f_Lock)
            {
                if (f_Logger == null) return;
                f_Logger.WriteLine(aStr);
                if (f_ImmediateFlush) f_Logger.Flush();
            }
        }
        /// <summary>
        /// 根据f_LogFileMaxCount设置值删除多余日志文件。
        /// </summary> 
        private void CleanupLogFolder()
        {
            //日志文件数量无限制,返回
            if (f_LogFileMaxCount  == 0) return;

            string [] logFiles = Directory.GetFiles(f_LogFolder, LOG_FILE_FILTER);
            if (logFiles.Length <= f_LogFileMaxCount) return;
            
            int delCount = 0;
            int iIndex=0;
            while (delCount < logFiles.Length - f_LogFileMaxCount && iIndex < logFiles.Length)
            {
                if (false == Path.GetFileName(logFiles[iIndex]).Equals(f_CurrentLogFileName, StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        File.Delete(logFiles[iIndex]);
                        delCount++;
                    }
                    catch
                    {
                    }
                }
                iIndex++;
            }
        }

        /// <summary>
        /// 以下为接口实现
        /// </summary>


        /// <summary>
        /// 创建日志
        /// </summary>
        /// <param name="logObject">为日志存放的目录</param>
        /// <returns>true or false</returns>
        bool ILogService.Open(object logObject)
        {
            string logFolder = logObject.ToString().Trim();
            if (logFolder == "") 
           {
                logFolder = Path.Combine(Application.StartupPath, LOG_SUB_FOLDER);
            } 
            if (logFolder[logFolder.Length - 1] != Path.DirectorySeparatorChar)
            {
                logFolder += Path.DirectorySeparatorChar;
            }
            return CreateLogger(logFolder);
        }

        void ILogService.Close()
        {
            if (f_Logger!=null)
            {
                f_Logger.Close();
                f_Logger = null;
            }
        }

        private string LogTypeToStr(LogType logType)
        {
            string sRet = "";
            switch (logType)
            {
                case LogType.ltInfo: sRet = "[i]"; break;
                case LogType.ltWarning: sRet = "[!]"; break;
                case LogType.ltError: sRet = "[x]"; break;
                case LogType.ltException: sRet = "[X]"; break;
            }
            return sRet;
        }

        /// <summary>
        /// 日志文件切换，变更当前操作的日志文件
        /// </summary>
        private void SwitchLogFile()
        {
            string nowDate = GetCurrentDateString();
            if (nowDate.Equals(f_CurrentDate)) return;

            string oldLogFile = f_CurrentLogFileName;

            f_CurrentLogFileName = GenerateCurrentLogFileName();

            WriteLog(string.Format(MSG_SWITCH_TO_LOGFILE, f_CurrentLogFileName));
            //关闭当前日志文件.
            ((ITextFileLogService)this).Close();

            //切换到新的日志文件
            OpenLogFile(true); 
            WriteLog(string.Format(MSG_SWITCH_FROM_LOGFILE,oldLogFile));
            CleanupLogFolder();
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logType">日志类型</param>
        /// <param name="logSource">日志源（模块名）</param>
        /// <param name="logContent">日志信息</param>
        /// <returns></returns>
        bool ILogService.Log(object logContent,LogType logType, object logSource)
        {
            if (!f_Enable)
            {
                return false;
            }
            if (logType > f_LogLevel) return false;
            SwitchLogFile();
            string logMsg = string.Format(LOG_MSG_FORMAT, LogTypeToStr(logType),
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), logSource, logContent);
            if(IsDebug)
            {
                while (LogForm == null)
                {
                    Thread.Sleep(50);
                }
                LogForm?.WriteLog(logMsg);
                WriteLog(logMsg);
            }
            else
            {
                WriteLog(logMsg);
            }

            return true;
        }
      

        /// <summary>
        /// 设置日志记录级别，所有<= f_Level类型的日志都会被记录。
        /// </summary>
        /// <param name="loglevel"></param>
        void ILogService.SetLogLevel(LogType loglevel)
        {
            f_LogLevel = loglevel;
        }


        void ILogService.SetIsDebug(bool isDebug)
        {
            f_IsDebug = isDebug;
        }

        /// <summary>
        /// 此属性用于设定是否即时把缓冲中的内容写入日志文件。默认值为true
        /// </summary>
        /// <param name=""></param>
        void ILogService.SetIsImmediateFlush(bool isImmediateFlush)
        {
            f_ImmediateFlush = isImmediateFlush;
        }

        void ITextFileLogService.SetLogFileMaxCount(uint logFileMaxCount)
        {
            if (f_LogFileMaxCount != logFileMaxCount)
            {
                f_LogFileMaxCount = logFileMaxCount;
                CleanupLogFolder();
            }
        }


        public static void Log(object logContent,LogType logType = LogType.ltInfo, object logSource = null, int stackDepth = 2)
        {
            if (logSource == null)
            {
                logSource = GetMethodName(stackDepth);
            }
            SingletonInstance.Log(logContent,logType, logSource);
        }


        /// <summary>
        /// 根据调用堆栈深度获取当前函数名
        /// </summary>
        /// <param name="stackDepth"></param>
        /// <returns></returns>
        private static string GetMethodName(int stackDepth)
        {

            string strStackInfo = string.Empty;
            try
            {
                StackTrace stack = new StackTrace();

                var methodBase = stack.GetFrame(stackDepth).GetMethod();
                var Class = methodBase.ReflectedType;
                //var Namespace = Class.Namespace;     
                strStackInfo = string.Format("{0}.{1}",Class.Name, methodBase.Name);


            }
            catch (Exception e)
            {
                strStackInfo = string.Format("Get mothodname failed! Exception message:{1}", e.Message);
            }

            return strStackInfo;
        }

        //public static bool LogInformation(object logSource, object logContent)
        //{
        //    return SingletonInstance.WriteLog(logContent,LogType.ltInfo, logSource);
        //}

        //public static bool LogWarning(object logSource, object logContent)
        //{
        //    return SingletonInstance.WriteLog(logContent,LogType.ltWarning, logSource);
        //}

        //public static bool LogError(object logSource, object logContent)
        //{
        //    return SingletonInstance.WriteLog(logContent, LogType.ltError, logSource);
        //}

        //public static bool LogException(object logSource, object logContent)
        //{
        //    return SingletonInstance.WriteLog(logContent,LogType.ltException, logSource);
        //}

        /// <summary>
        /// 设置日志记录级别，所有<= f_Level类型的日志都会被记录。
        /// </summary>
        /// <param name="loglevel"></param>
        public static void SetLogLevel(LogType loglevel)
        {
            SingletonInstance.SetLogLevel(loglevel);
        }


        public static void SetIsDebug(bool isDebug)
        {
            RunLog.IsDebug = isDebug;
           // SingletonInstance.SetIsDebug(isDebug);
        }

        /// <summary>
        /// 此属性用于设定是否即时把缓冲中的内容写入日志文件。默认值为true
        /// </summary>
        /// <param name=""></param>
        public static void SetIsImmediateFlush(bool isImmediateFlush)
        {
            SingletonInstance.SetIsImmediateFlush(isImmediateFlush);
        }

        public static void SetLogFileMaxCount(uint logFileMaxCount)
        {

            SingletonInstance.SetLogFileMaxCount(logFileMaxCount);
        }


        public static string GetExceptionInfo(Exception e)
        {
            string strInfo = string.Format(" Exception:{0} StackTrace:{1},InnerException:{2},Message:{3}", e, e.StackTrace, e.InnerException, e.Message);
            return strInfo;
        }

        public static string GetCurrentMethodInfo(params object[] methodParamValues)
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1);
            //获取上层函数信息调用 GetFrame(1)
            var methodBase = stackFrame.GetMethod();
            string strInfo = string.Empty;
            ParameterInfo[] parameterInfos = methodBase.GetParameters();
            for (int paraIndex = 0; paraIndex < parameterInfos.Length; paraIndex++)
            {
                if (paraIndex < methodParamValues.Length)
                {
                    strInfo += string.Format("{0}:{1};", parameterInfos[paraIndex].Name, methodParamValues[paraIndex]);
                }
            }
            strInfo = string.Format("{0} [{1}]", methodBase.Name, strInfo);
            return strInfo;

        }



        /// <summary>
        /// 获取当前函数异常信息及函数相关信息
        /// </summary>
        /// <param name="e"></param>
        /// <param name="methodParamValues"></param>
        /// <returns></returns>
        public static string GetCurrentMethodExceptionInfo(Exception e, params object[] methodParamValues)
        {
            string strInfo = string.Format("{0} {1} ", GetCurrentMethodInfo(methodParamValues), GetExceptionInfo(e));
            return strInfo;

        }

    }

    public delegate void LogEvent(LogType logType, object logSource, object logContent);
}
