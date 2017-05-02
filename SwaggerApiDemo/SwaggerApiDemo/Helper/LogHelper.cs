using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;

namespace Dev_Log4Net.Utilities
{
    /// <summary>
    /// 日志记录级别的枚举
    /// </summary>
    public enum LogLevel
    {
        Info,
        Debug,
        Warn,
        Error,
        Fatal
    }

    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        ///  日志记录器
        /// </summary>
        private static readonly log4net.ILog _log = LogManager.GetLogger("WebLogger");

        /// <summary>
        ///  日志帮助类实例
        /// </summary>
        private static LogHelper _logger;

        /// <summary>
        /// 静态实例
        /// </summary>
        public static LogHelper CurrentLogger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new LogHelper();
                }
                return _logger;
            }
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        ///  初始化配置
        /// </summary>
        /// <param name="configFile"></param>
        public static void SetConfig(string configFile)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="ex">异常</param>
        public void Debug(string msg, Exception ex)
        {
            _log.Debug(msg, ex);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg">信息</param>
        public void Debug(string msg)
        {
            _log.Debug(msg);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="ex">异常</param>
        public void Error(string msg, Exception ex)
        {
            _log.Error(msg, ex);
        }

        /// <summary>
        /// 致命的错误
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="ex">异常</param>
        public void Fatal(string msg, Exception ex)
        {
            _log.Fatal(msg, ex);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="ex">异常</param>
        public void Info(string msg, Exception ex)
        {
            _log.Info(msg, ex);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="msg">信息</param>
        public void Info(string msg)
        {
            _log.Info(msg);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="ex">异常</param>
        public void Warn(string msg, Exception ex)
        {
            _log.Warn(msg, ex);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">信息</param>
        public void Warn(string msg)
        {
            _log.Warn(msg);
        }

        /// <summary>
        /// 日志级别
        /// </summary>
        /// <returns></returns>
        public LogLevel CurrentLogLevel()
        {
            if (_log.IsDebugEnabled)
            {
                return LogLevel.Debug;
            }
            if (_log.IsWarnEnabled)
            {
                return LogLevel.Warn;
            }
            if (_log.IsInfoEnabled)
            {
                return LogLevel.Info;
            }
            if (_log.IsErrorEnabled)
            {
                return LogLevel.Error;
            }

            return LogLevel.Fatal;
        }
    }

}
