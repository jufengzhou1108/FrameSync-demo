using Fantasy.Platform.Net;
using NLog;

namespace Fantasy
{
    /// <summary>
    /// 使用 NLog 实现的日志记录器。
    /// </summary>
    public class NLog : ILog
    {
        private readonly Logger _logger;

        public NLog(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        // 🔥 修复：接口要求的 Initialize 签名变了
        public void Initialize(string name, ProcessMode processMode)
        {
            switch (processMode)
            {
                case ProcessMode.Develop:
                    {
                        LogManager.Configuration.RemoveRuleByName("ServerDebug");
                        LogManager.Configuration.RemoveRuleByName("ServerTrace");
                        LogManager.Configuration.RemoveRuleByName("ServerInfo");
                        LogManager.Configuration.RemoveRuleByName("ServerWarn");
                        LogManager.Configuration.RemoveRuleByName("ServerError");
                        break;
                    }
                case ProcessMode.Release:
                    {
                        LogManager.Configuration.RemoveRuleByName("ConsoleTrace");
                        LogManager.Configuration.RemoveRuleByName("ConsoleDebug");
                        LogManager.Configuration.RemoveRuleByName("ConsoleInfo");
                        LogManager.Configuration.RemoveRuleByName("ConsoleWarn");
                        LogManager.Configuration.RemoveRuleByName("ConsoleError");
                        break;
                    }
            }
        }

        // 🔥 下面全部是接口要求的新方法（一个都不能少）
        public void Trace(string message) => _logger.Trace(message);
        public void Warning(string message) => _logger.Warn(message);
        public void Info(string message) => _logger.Info(message);
        public void Debug(string message) => _logger.Debug(message);
        public void Error(string message) => _logger.Error(message);
        public void Fatal(string message) => _logger.Fatal(message);

        public void Trace(string message, params object[] args) => _logger.Trace(message, args);
        public void Warning(string message, params object[] args) => _logger.Warn(message, args);
        public void Info(string message, params object[] args) => _logger.Info(message, args);
        public void Debug(string message, params object[] args) => _logger.Debug(message, args);
        public void Error(string message, params object[] args) => _logger.Error(message, args);
        public void Fatal(string message, params object[] args) => _logger.Fatal(message, args);

        // 🔥 新增：带 exception 参数的重载
        public void Trace(string message, string ex) => _logger.Trace($"{message}\n{ex}");
        public void Warning(string message, string ex) => _logger.Warn($"{message}\n{ex}");
        public void Info(string message, string ex) => _logger.Info($"{message}\n{ex}");
        public void Debug(string message, string ex) => _logger.Debug($"{message}\n{ex}");
        public void Error(string message, string ex) => _logger.Error($"{message}\n{ex}");
        public void Fatal(string message, string ex) => _logger.Fatal($"{message}\n{ex}");

        // 🔥 新增：message + exception + 格式化参数
        public void Trace(string message, string ex, params object[] args) => _logger.Trace($"{message}\n{ex}", args);
        public void Warning(string message, string ex, params object[] args) => _logger.Warn($"{message}\n{ex}", args);
        public void Info(string message, string ex, params object[] args) => _logger.Info($"{message}\n{ex}", args);
        public void Debug(string message, string ex, params object[] args) => _logger.Debug($"{message}\n{ex}", args);
        public void Error(string message, string ex, params object[] args) => _logger.Error($"{message}\n{ex}", args);
        public void Fatal(string message, string ex, params object[] args) => _logger.Fatal($"{message}\n{ex}", args);
    }
}