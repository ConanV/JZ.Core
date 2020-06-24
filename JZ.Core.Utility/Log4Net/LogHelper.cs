using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.Core.Utility.Log4Net
{
    public class LogHelper
    {
        // 异常
        private static readonly ILog logerror = LogManager.GetLogger(Log4NetRepository.loggerRepository.Name, "errLog");
        // 记录
        private static readonly ILog loginfo = LogManager.GetLogger(Log4NetRepository.loggerRepository.Name, "infoLog");
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        public static void Error(string throwMsg, Exception ex)
        {
            string errorMsg = string.Format("【异常描述】：{0} <br>【异常类型】：{1} <br>【异常信息】：{2} <br>【堆栈调用】：{3}",
                new object[] {
                    throwMsg,
                    ex.GetType().Name,
                    ex.Message,
                    ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            logerror.Error(errorMsg);
        }
        /// <summary>
        /// 普通信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(string message, Exception exception = null)
        {
            if (exception == null)
                loginfo.Info($"【日志信息】：{message}");
            else
                loginfo.Info($"【日志信息】：{message}<br>{exception}");
        }
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(string message, Exception exception = null)
        {
            if (exception == null)
                loginfo.Warn($"【日志警告信息】：{message}");
            else
                loginfo.Warn($"【日志警告信息】：{message}<br>{exception}");
        }
    }
}
