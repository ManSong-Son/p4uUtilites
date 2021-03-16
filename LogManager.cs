using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.IO;
using log4net;
using log4net.Config;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Windows.Forms;
using log4net.Core;
using System.Xml.Linq;

namespace p4uUtilities
{
    public enum LogLevels
    {
        Fatal,
        Error,
        Warn,
        Info,
        Debug
    }

    public class Logger
    {
        private static readonly object locker = new object();

        private ILog _Logger = null;

        public System.Windows.Forms.TextBox LinkTextBox { get; set; }

        public string Name { get; set; }
        
        //public readonly ILog ll
        //{
        //    get { return _Logger; }
        //}

        private string parentPath = String.Empty;
        private string ParentPath
        {
            get
            {
                if (String.IsNullOrEmpty(parentPath))
                {
                    return UtilitiesManager.PathInfo.Log;
                    //return Path.Combine("C:/Luli Sense/Log");
                    //return Path.Combine(
                    //    ProgramData, // ProgramData
                    //    EntryAssembly.FullName.Split('.')[0]);
                }
                else
                {
                    return parentPath;
                }
            }
            set
            {
                if (UtilFile.IsWritable(new DirectoryInfo(value)))
                {
                    parentPath = value;
                }
                this.Update();
            }
        }

        internal ILog Setup()
        {
            return this.Update(false);
        }

        internal ILog Update(bool overwrite = true)
        {
            if (!Directory.Exists(this.ParentPath))
                Directory.CreateDirectory(this.ParentPath);
            //if (!Directory.Exists(ConfigurationFolder))
            //    Directory.CreateDirectory(ConfigurationFolder);
            //if (!Directory.Exists(LogFolder))
            //    Directory.CreateDirectory(LogFolder);
            //if (!File.Exists(ConfigFilePath) || overwrite)
            //{
            //    var config = this.Serialize();
            //    File.Create(ConfigFilePath).Close();
            //    File.WriteAllText(ConfigFilePath, config.ToString());
            //}
            //var logger = LogManager.GetLogger(EntryAssembly, Type);
            //XmlConfigurator.ConfigureAndWatch(new FileInfo(ConfigFilePath));
            //if (!initialized)
            //{
            //    initialized = true;
            //}
            //return logger;
            return _Logger;
        }

        public Logger(string log)
        {
            Name = log;
            _Logger = log4net.LogManager.GetLogger(Name);
        }

        #region Log Append Func
        /// <summary>
        /// Fatal Log Append
        /// </summary>
        /// <param name="message"></param>
        public void appendFatalLog(string message)
        {
            lock (locker)
            {
                //DeleteLogFile();

                if (_Logger.IsFatalEnabled)
                    _Logger.Fatal(message);

                if (LinkTextBox != null)
                    LinkTextBox.AppendText($"[FATAL] {message}\r\n");
            }
        }

        public void appendFatalLog(string message, Type type)
        {
            lock (locker)
            {
                //DeleteLogFile();

                if (_Logger.IsInfoEnabled)
                    _Logger.Fatal($"{message} |{type}|");

                if (LinkTextBox != null)
                    LinkTextBox.AppendText($"[FATAL] {message} |{type}|\r\n");
            }
        }

        /// <summary>
        /// Error Log Append
        /// </summary>
        /// <param name="message"></param>
        public void appendErrorLog(string message)
        {
            lock (locker)
            {
                //DeleteLogFile();

                if (_Logger.IsErrorEnabled)
                    _Logger.Error(message);

                if (LinkTextBox != null)
                    LinkTextBox.AppendText($"[ERROR] {message}\r\n");
            }
        }

        public void appendErrorLog(string message, Type type)
        {
            lock (locker)
            {
                //DeleteLogFile();

                if (_Logger.IsInfoEnabled)
                    _Logger.Error($"{message} |{type}|");

                if (LinkTextBox != null)
                    LinkTextBox.AppendText($"[ERROR] {message} |{type}|\r\n");
            }
        }

        /// <summary>
        /// Warn Log Append
        /// </summary>
        /// <param name="message"></param>
        public void appendWarnLog(string message)
        {
            lock (locker)
            {
                //DeleteLogFile();

                if (_Logger.IsWarnEnabled)
                    _Logger.Warn(message);

                if (LinkTextBox != null)
                    LinkTextBox.AppendText($"[WARN ] {message}\r\n");
            }
        }

        public void appendWarnLog(string message, Type type)
        {
            lock (locker)
            {
                //DeleteLogFile();

                if (_Logger.IsInfoEnabled)
                    _Logger.Warn($"{message} |{type}|");

                if (LinkTextBox != null)
                    LinkTextBox.AppendText($"[WARN ] {message} |{type}|\r\n");
            }
        }

        /// <summary>
        /// Info Log Append
        /// </summary>
        /// <param name="message"></param>
        public void appendInfoLog(string message)
        {
            lock (locker)
            {
                //DeleteLogFile();

                if (_Logger.IsInfoEnabled)
                    _Logger.Info(message);

                if (LinkTextBox != null)
                    LinkTextBox.AppendText($"[INFO ] {message}\r\n");
            }
        }

        public void appendInfoLog(string message, Type type)
        {
            lock (locker)
            {
                //DeleteLogFile();

                if (_Logger.IsInfoEnabled)
                    _Logger.Info($"{message} |{type}|");

                if (LinkTextBox != null)
                    LinkTextBox.AppendText($"[INFO ] {message} |{type}|\r\n");
            }
        }

        /// <summary>
        /// Debug TRACE Log Append
        /// </summary>
        /// <param name="message"></param>
        public void appendDebugLog(string message)
        {
            lock (locker)
            {
                //DeleteLogFile();

                if (_Logger.IsDebugEnabled)
                    _Logger.Debug(message);

                if (LinkTextBox != null)
                    LinkTextBox.AppendText($"[DEBUG] {message}\r\n");
            }
        }

        public void appendDebugLog(string message, Type type)
        {
            lock (locker)
            {
                //DeleteLogFile();

                if (_Logger.IsInfoEnabled)
                    _Logger.Debug($"{message} |{type}|");

                if (LinkTextBox != null)
                    LinkTextBox.AppendText($"[DEBUG] {message} |{type}|\r\n");
            }
        }
        #endregion
    }

    public class TextBoxAppender : AppenderSkeleton
    {
        static public void SetupTextBoxAppend(TextBox textbox, string sLayerFormat)
        {
            TextBoxAppender textBoxAppender = new TextBoxAppender();
            textBoxAppender.AppenderTextBox = textbox;
            textBoxAppender.Threshold = log4net.Core.Level.All;
            ILayout layout = null;
            if (string.IsNullOrEmpty(sLayerFormat))
            {
                layout = new log4net.Layout.SimpleLayout();
            }
            else
            {
                PatternLayout layoutPattern = new PatternLayout(sLayerFormat);
                layout = layoutPattern;
            }
            textBoxAppender.Layout = layout;
            textBoxAppender.Name = string.Format("TextBoxAppender_{0}", textbox.Name);
            textBoxAppender.ActivateOptions();
            Hierarchy h = (Hierarchy)log4net.LogManager.GetRepository();
            h.Root.AddAppender(textBoxAppender);
        }


        private TextBox _textBox;
        public TextBox AppenderTextBox
        {
            get
            {
                return _textBox;
            }
            set
            {
                _textBox = value;
            }
        }

        override protected bool RequiresLayout
        {
            get { return true; }
        }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            if (_textBox == null)
                return;
            try
            {
                string sMessage = base.RenderLoggingEvent(loggingEvent);
                _textBox.BeginInvoke(new WriteMessageHandler(WriteMessage), sMessage);
            }
            catch
            {

            }
        }

        private delegate void WriteMessageHandler(string sMessage);

        private void WriteMessage(string sMessage)
        {
            if (_textBox.Lines.Length > 1000)
                _textBox.Clear();

            _textBox.AppendText(sMessage);
            _textBox.Select(_textBox.TextLength - 1, 0);
            _textBox.ScrollToCaret();
        }
    }

    public class LogManager
    {
        private static LogManager _instance;
        private static readonly object locker = new object();
        public ArrayList _iLog;
        //public Logger iLog_Common = null;
        //public Logger iLog_Socket = null;
        //public Logger iLog_Database = null;
        private log4net.Repository.ILoggerRepository _repository;
        private string ParentPath { get; set; }
        private string FileName { get; set; }

        public static LogManager Instance()
        {
            lock (locker)
            {
                if (_instance == null)
                    _instance = new LogManager();

                return _instance;
            }
        }

        public LogManager()
        {
            _repository = log4net.LogManager.GetRepository();
            var hierarchy = (Hierarchy)_repository;
            _iLog = new ArrayList();

            string strFilePath;
            //strDirPath = @"C:\Luli Sense\";
            //strDirPath = PathInfo.Data;
            this.ParentPath = UtilitiesManager.PathInfo.Data;
            this.FileName = "LogConfig.dat";
            DirectoryInfo dirInfo = new DirectoryInfo(this.ParentPath);
            if (dirInfo.Exists == false)
            {
                if (dirInfo != null)
                    dirInfo.Create();
            }
            strFilePath = this.ParentPath + this.FileName;

            ILogger iLogger = null;
            if (!File.Exists(strFilePath))
            {
                hierarchy.Root.Level = log4net.Core.Level.All;

                Logger newLogger = null;               

                var rollingAppender = new RollingFileAppender();
                rollingAppender.Name = "common";
                rollingAppender.File = $"{UtilitiesManager.PathInfo.Log}";
                rollingAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
                //rollingAppender.DatePattern = "'_" + rollingAppender.Name + "_'yyMMdd'.log'";
                rollingAppender.DatePattern = $"yyyy/MM/dd/'{rollingAppender.Name}.log'";
                rollingAppender.StaticLogFileName = false;
                rollingAppender.AppendToFile = true;
                rollingAppender.Layout = new PatternLayout("(%d) {%t} [%-5p] %m |%c|%n");
                rollingAppender.MaxFileSize = 1024 * 1024 * 5;
                rollingAppender.Threshold = Level.Info;
                rollingAppender.ActivateOptions();
                //hierarchy.Root.AddAppender(rollingAppender);
                newLogger = new Logger(rollingAppender.Name);
                iLogger = _repository.GetLogger(rollingAppender.Name);
                ((log4net.Repository.Hierarchy.Logger)iLogger).AddAppender(rollingAppender);
                ((log4net.Repository.Hierarchy.Logger)iLogger).Level = rollingAppender.Threshold;
                _iLog.Add(newLogger);

                rollingAppender = new RollingFileAppender();
                rollingAppender.Name = "socket";
                rollingAppender.File = $"{UtilitiesManager.PathInfo.Log}";
                rollingAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
                rollingAppender.DatePattern = $"yyyy/MM/dd/'{rollingAppender.Name}.log'";
                rollingAppender.StaticLogFileName = false;
                rollingAppender.AppendToFile = true;
                rollingAppender.Layout = new PatternLayout("(%d) {%t} [%-5p] %m |%c|%n");
                rollingAppender.MaxFileSize = 1024 * 1024 * 5;
                rollingAppender.Threshold = Level.Info;
                rollingAppender.ActivateOptions();
                //hierarchy.Root.AddAppender(rollingAppender);
                newLogger = new Logger(rollingAppender.Name);
                iLogger = _repository.GetLogger(rollingAppender.Name);
                ((log4net.Repository.Hierarchy.Logger)iLogger).AddAppender(rollingAppender);
                ((log4net.Repository.Hierarchy.Logger)iLogger).Level = rollingAppender.Threshold;
                _iLog.Add(newLogger);

                rollingAppender = new RollingFileAppender();
                rollingAppender.Name = "database";
                rollingAppender.File = $"{UtilitiesManager.PathInfo.Log}";
                rollingAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
                rollingAppender.DatePattern = $"yyyy/MM/dd/'{rollingAppender.Name}.log'";
                rollingAppender.StaticLogFileName = false;
                rollingAppender.AppendToFile = true;
                rollingAppender.Layout = new PatternLayout("(%d) {%t} [%-5p] %m |%c|%n");
                rollingAppender.MaxFileSize = 1024 * 1024 * 5;
                rollingAppender.Threshold = Level.Info;
                rollingAppender.ActivateOptions();
                //hierarchy.Root.AddAppender(rollingAppender);
                newLogger = new Logger(rollingAppender.Name);
                iLogger = _repository.GetLogger(rollingAppender.Name);
                ((log4net.Repository.Hierarchy.Logger)iLogger).AddAppender(rollingAppender);
                ((log4net.Repository.Hierarchy.Logger)iLogger).Level = rollingAppender.Threshold;
                _iLog.Add(newLogger);

                this.Update(true);
            }
            else
            {
                XmlConfigurator.Configure(new System.IO.FileInfo(strFilePath));

                IAppender[] appenders = hierarchy.GetAppenders();
                foreach (IAppender obj in appenders)
                {
                    if (obj.Name != "")
                        _iLog.Add(new Logger(obj.Name));
                }
            }
            XmlConfigurator.ConfigureAndWatch(new FileInfo(strFilePath));

            _repository.Configured = true;
        }

         public void Setup()
        {
            this.Update(false);
        }

        public void Update(bool overwrite = true)
        {
            string strFilePath = this.ParentPath + this.FileName;

            if (!Directory.Exists(this.ParentPath))
                Directory.CreateDirectory(this.ParentPath);
            if (!File.Exists(strFilePath) || overwrite)
            {
                var config = this.Serialize();
                config.Save(strFilePath);
                XmlConfigurator.ConfigureAndWatch(new FileInfo(strFilePath));
            }
        }

        private XDocument Serialize()
        {
            if (_repository == null)
                return new XDocument();

            var hierarchy = (Hierarchy)_repository;
            IAppender[] appenders = hierarchy.GetAppenders();
            ILogger iLogger = null;

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "utf-8", null));

            ///// root
            var rootLevel = new XElement("level");
            rootLevel.SetAttributeValue("value", Level.All);
            var rootInfo = new XElement("root", rootLevel);

            ArrayList arrAppender = new ArrayList();
            ArrayList arrLogger = new ArrayList();

            foreach (IAppender obj in appenders)
            {
                if (obj.GetType() == typeof(RollingFileAppender))
                {
                    var file = new XElement("file");
                    file.SetAttributeValue("value", UtilitiesManager.PathInfo.Log);

                    var appendToFile = new XElement("appendToFile");
                    appendToFile.SetAttributeValue("value", ((RollingFileAppender)obj).AppendToFile);

                    var datePattern = new XElement("datePattern");
                    datePattern.SetAttributeValue("value", ((RollingFileAppender)obj).DatePattern);

                    var staticLogFileName = new XElement("staticLogFileName");
                    staticLogFileName.SetAttributeValue("value", ((RollingFileAppender)obj).StaticLogFileName);

                    var rollingStyle = new XElement("rollingStyle");
                    rollingStyle.SetAttributeValue("value", ((RollingFileAppender)obj).RollingStyle);
                    rollingStyle.SetAttributeValue("value", Enum.GetName(typeof(RollingFileAppender.RollingMode), ((RollingFileAppender)obj).RollingStyle));

                    var conversionPattern = new XElement("conversionPattern");
                    conversionPattern.SetAttributeValue("value", ((PatternLayout)(((RollingFileAppender)obj).Layout)).ConversionPattern);

                    var layout = new XElement("layout", conversionPattern);
                    layout.SetAttributeValue("type", typeof(PatternLayout));

                    var appender = new XElement("appender", file, appendToFile, datePattern, staticLogFileName, rollingStyle, layout);
                    appender.SetAttributeValue("name", obj.Name);
                    appender.SetAttributeValue("type", typeof(RollingFileAppender));

                    arrAppender.Add(appender);

                    iLogger = hierarchy.GetLogger(obj.Name);

                    var loggerLevel = new XElement("level");
                    loggerLevel.SetAttributeValue("value", ((log4net.Repository.Hierarchy.Logger)iLogger).Level);
                    //if (((RollingFileAppender)obj).Threshold == null)
                    //    loggerLevel.SetAttributeValue("value", "INFO");
                    //else
                    //    loggerLevel.SetAttributeValue("value", ((RollingFileAppender)obj).Threshold);
                    var appenderRef = new XElement("appender-ref");
                    appenderRef.SetAttributeValue("ref", obj.Name);
                    var logger = new XElement("logger", loggerLevel, appenderRef);
                    logger.SetAttributeValue("name", obj.Name);

                    arrLogger.Add(logger);
                }
            }

            var logConfig = new XElement("log4net");
            logConfig.Add(rootInfo);

            foreach(var obj in arrLogger)
                logConfig.Add(obj);

            foreach (var obj in arrAppender)
                logConfig.Add(obj);

            xDoc.Add(logConfig);
            return xDoc;

            //return XDocument.Parse(xDoc.ToString());
            //return XDocument.Parse(logConfig.ToString());                
            //var configuration = new XElement("configuration", configSection, new XElement("log4net", appender, root));
            //return XDocument.Parse(configuration.ToString());
        }

        public IAppender GetAppender(string logger)
        {
            var hierarchy = (Hierarchy)_repository;
            IAppender[] appenders = hierarchy.GetAppenders();
            foreach (IAppender obj in appenders)
            {
                if (obj.Name == logger)
                    return obj;
            }

            return null;
        }

        public void Close()
        {
            if (_repository != null)
                _repository.Shutdown();
        }

        public void SetLoggingTextBox(string logger, System.Windows.Forms.TextBox link)
        {
            foreach (Logger obj in _iLog)
            {
                if (logger == "")
                {
                    obj.LinkTextBox = link;
                }
                else if (obj.Name == logger)
                {
                    obj.LinkTextBox = link;
                    break;
                }
            }
        }

        public void Logging(LogLevels level, string logger, string message)
        {
            bool bFound = false;
            foreach (Logger obj in _iLog)
            {
                if (logger == "all")
                {
                    switch (level)
                    {
                        case LogLevels.Fatal:
                            obj.appendFatalLog(message);
                            break;
                        case LogLevels.Error:
                            obj.appendErrorLog(message);
                            break;
                        case LogLevels.Warn:
                            obj.appendWarnLog(message);
                            break;
                        case LogLevels.Info:
                            obj.appendInfoLog(message);
                            break;
                        case LogLevels.Debug:
                            obj.appendDebugLog(message);
                            break;
                    }
                    bFound = true;
                }
                else if (logger == obj.Name)
                {
                    switch (level)
                    {
                        case LogLevels.Fatal:
                            obj.appendFatalLog(message);
                            break;
                        case LogLevels.Error:
                            obj.appendErrorLog(message);
                            break;
                        case LogLevels.Warn:
                            obj.appendWarnLog(message);
                            break;
                        case LogLevels.Info:
                            obj.appendInfoLog(message);
                            break;
                        case LogLevels.Debug:
                            obj.appendDebugLog(message);
                            break;
                    }

                    bFound = true;
                    break;
                }
            }

            if (bFound == false)
            {
                ILogger iLogger = null;
                var hierarchy = (Hierarchy)_repository;
                Logger newLogger = null;
                var rollingAppender = new RollingFileAppender();
                rollingAppender = new RollingFileAppender();
                rollingAppender.Name = logger;
                rollingAppender.File = $"{UtilitiesManager.PathInfo.Log}";
                rollingAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
                rollingAppender.DatePattern = $"yyyy/MM/dd/'{rollingAppender.Name}.log'";
                rollingAppender.StaticLogFileName = false;
                rollingAppender.AppendToFile = true;
                rollingAppender.Layout = new PatternLayout("(%d) {%t} [%-5p] %m |%c|%n");
                rollingAppender.MaxFileSize = 1024 * 1024 * 5;
                rollingAppender.Threshold = Level.Info;
                rollingAppender.ActivateOptions();
                //hierarchy.Root.AddAppender(rollingAppender);
                iLogger = _repository.GetLogger(rollingAppender.Name);
                ((log4net.Repository.Hierarchy.Logger)iLogger).AddAppender(rollingAppender);
                ((log4net.Repository.Hierarchy.Logger)iLogger).Level = rollingAppender.Threshold;
                newLogger = new Logger(rollingAppender.Name);
                _iLog.Add(newLogger);
                this.Update();

                if (newLogger != null)
                {
                    switch (level)
                    {
                        case LogLevels.Fatal:
                            newLogger.appendFatalLog(message);
                            break;
                        case LogLevels.Error:
                            newLogger.appendErrorLog(message);
                            break;
                        case LogLevels.Warn:
                            newLogger.appendWarnLog(message);
                            break;
                        case LogLevels.Info:
                            newLogger.appendInfoLog(message);
                            break;
                        case LogLevels.Debug:
                            newLogger.appendDebugLog(message);
                            break;
                    }
                }
            }
        }
        public void Logging(LogLevels level, string logger, string message, Type type)
        {
            bool bFound = false;
            foreach (Logger obj in _iLog)
            {
                if (logger == "all")
                {
                    switch (level)
                    {
                        case LogLevels.Fatal:
                            obj.appendFatalLog(message, type);
                            break;
                        case LogLevels.Error:
                            obj.appendErrorLog(message, type);
                            break;
                        case LogLevels.Warn:
                            obj.appendWarnLog(message, type);
                            break;
                        case LogLevels.Info:
                            obj.appendInfoLog(message, type);
                            break;
                        case LogLevels.Debug:
                            obj.appendDebugLog(message, type);
                            break;
                    }

                    bFound = true;
                }
                else if (logger == obj.Name)
                {
                    switch (level)
                    {
                        case LogLevels.Fatal:
                            obj.appendFatalLog(message, type);
                            break;
                        case LogLevels.Error:
                            obj.appendErrorLog(message, type);
                            break;
                        case LogLevels.Warn:
                            obj.appendWarnLog(message, type);
                            break;
                        case LogLevels.Info:
                            obj.appendInfoLog(message, type);
                            break;
                        case LogLevels.Debug:
                            obj.appendDebugLog(message, type);
                            break;
                    }
                    bFound = true;
                    break;
                }
            }

            if (bFound == false)
            {
                ILogger iLogger = null;
                var hierarchy = (Hierarchy)_repository;
                Logger newLogger = null;
                var rollingAppender = new RollingFileAppender();
                rollingAppender = new RollingFileAppender();
                rollingAppender.Name = logger;
                rollingAppender.File = $"{UtilitiesManager.PathInfo.Log}";
                rollingAppender.RollingStyle = RollingFileAppender.RollingMode.Composite;
                rollingAppender.DatePattern = $"yyyy/MM/dd'/{rollingAppender.Name}.log'";
                rollingAppender.StaticLogFileName = false;
                rollingAppender.AppendToFile = true;
                rollingAppender.Layout = new PatternLayout("(%d) {%t} [%-5p] %m |%c|%n");
                //rollingAppender.MaxFileSize = 1024;
                rollingAppender.MaximumFileSize = "5KB";
                rollingAppender.MaxSizeRollBackups = 30;
                rollingAppender.Threshold = Level.Info;
                rollingAppender.CountDirection = 1;
                rollingAppender.ActivateOptions();
                //hierarchy.Root.AddAppender(rollingAppender);
                newLogger = new Logger(rollingAppender.Name);
                iLogger = _repository.GetLogger(rollingAppender.Name);
                ((log4net.Repository.Hierarchy.Logger)iLogger).AddAppender(rollingAppender);
                ((log4net.Repository.Hierarchy.Logger)iLogger).Level = rollingAppender.Threshold;
                _iLog.Add(newLogger);
                this.Update();

                if (newLogger != null)
                {
                    switch (level)
                    {
                        case LogLevels.Fatal:
                            newLogger.appendFatalLog(message, type);
                            break;
                        case LogLevels.Error:
                            newLogger.appendErrorLog(message, type);
                            break;
                        case LogLevels.Warn:
                            newLogger.appendWarnLog(message, type);
                            break;
                        case LogLevels.Info:
                            newLogger.appendInfoLog(message, type);
                            break;
                        case LogLevels.Debug:
                            newLogger.appendDebugLog(message, type);
                            break;
                    }
                }
            }
        }
    }

    public class Log
    {
        //private static bool _logInitialized;
        private  readonly object _lock = new object();
        
        public void Init()
        {
            LogManager.Instance();
        }

        public void Close()
        {
            LogManager.Instance().Close();
        }
       
        public void SetLoggingTextBox(string logger, System.Windows.Forms.TextBox link)
        {
            LogManager.Instance().SetLoggingTextBox(logger, link);
        }
        public void Logging(LogLevels level, string logger, string message)
        {
            LogManager.Instance().Logging(level, logger, message);
        }
        public void Logging(LogLevels level, string logger, string message, Type type)
        {
            LogManager.Instance().Logging(level, logger, message, type);
        }
    }
}
