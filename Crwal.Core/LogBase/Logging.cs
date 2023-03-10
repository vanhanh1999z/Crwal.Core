using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using Telegram.Bot;

namespace Crwal.Core.Log
{
    public static class Logging
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private static readonly TelegramBotClient teleBot =
            new TelegramBotClient("5656809793:AAGc85FTWbztWkPIg5e13rFGw2rCUbIxoVU");

        private static readonly string idTele = "973167438";

        public static void Infomation(this string message)
        {
            _log.Info("--- " + message);
        }

        public static void Fatal(this string message)
        {
            _log.Fatal("--- " + message);
        }

        public static void Warning(this string message)
        {
            _log.Warn("--- " + message);
        }

        public static async Task ErrorAsync(this string ex)
        {
            await teleBot.SendTextMessageAsync(idTele, ex);
            _log.Error("--- Đã cõ lỗi xảy ra: " + ex);
        }

        public static void Error(Exception ex)
        {
            _log.Error($"--- Đã cõ lỗi xảy ra: {ex}", ex);
        }

        public static void Error(string ex)
        {
            _log.Error($"--- Đã cõ lỗi xảy ra: {ex}");
        }

        public static void Error(Exception ex, string more)
        {
            //await teleBot.SendTextMessageAsync(idTele, ex.ToString());
            _log.Error($"--- Đã cõ lỗi xảy ra: ${more}", ex.Message);
            _log.Error($"--- ---Nội dung lỗi: ${ex}", ex.Message);
        }

        private static int GetLineException(Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            return frame.GetFileLineNumber();
        }

        public static void Init()
        {
            var config = new LoggingConfiguration();
            var logfile = new FileTarget(Constants.FileTarget)
            {
                FileName = Constants.DefaultFilePath + DateTime.Now.ToString("yyyy-MM-dd") + Constants.DefaultFileName,
                ArchiveNumbering = ArchiveNumberingMode.Rolling
            };
            var logconsole = new ConsoleTarget(Constants.ConsoleTarget);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            LogManager.Configuration = config;
            _log.Info("Đã khởi tạo logging thành công");
            _log.Info("--- Chương trình bắt đầu khởi chạy ---");
        }

        public static void Init(string projectName)
        {
            var config = new LoggingConfiguration();
            var logfile = new FileTarget(Constants.FileTarget)
            {
                FileName = Constants.DefaultFilePath + DateTime.Now.ToString("yyyy-MM-dd") + projectName +
                           Constants.DefaultFileName,
                ArchiveNumbering = ArchiveNumberingMode.Rolling
            };
            var logconsole = new ConsoleTarget(Constants.ConsoleTarget);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            LogManager.Configuration = config;
            _log.Info("Đã khởi tạo logging thành công");
            _log.Info("--- Chương trình bắt đầu khởi chạy ---");
        }
        public static void Init(string projectName, string path)
        {
            var config = new LoggingConfiguration();
            var logfile = new FileTarget(Constants.FileTarget)
            {
                FileName = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd") + projectName + Constants.DefaultFileName),
                ArchiveNumbering = ArchiveNumberingMode.Rolling
            };
            var logconsole = new ConsoleTarget(Constants.ConsoleTarget);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            LogManager.Configuration = config;
            _log.Info("Đã khởi tạo logging thành công");
            _log.Info("--- Chương trình bắt đầu khởi chạy ---");
        }
    }
}