using System;

namespace ITL.Framework
{
    public enum LogType { ltNone=0x00, ltInfo, ltWarning, ltError, ltException,ltDebug};

    public interface ILogService
    {
        bool Open(object logObject);
        void Close();
        bool Log(object logContent,LogType logType, object logSource);
        //bool LogInformation(object logSource, object logContent);
        //bool LogWarning(object logSource, object logContent);
        //bool LogError(object logSource, object logContent);
        //bool LogException(object logSource, object logContent);

       
       // LogType LogLevel { get; set; }

        void SetLogLevel(LogType loglevel);

        void SetIsDebug(bool isDebug);

        void SetIsImmediateFlush(bool isImmediateFlush);
       // bool ImmediateFlush { get; set; }
    }

    public interface ITextFileLogService: ILogService
    {

        void SetLogFileMaxCount(uint logFileMaxCount);
        //uint LogFileMaxCount { get; set; }
    }
}
