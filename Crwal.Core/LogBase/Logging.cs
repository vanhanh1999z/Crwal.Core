using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Crwal.Core.Log
{
    public static class Logging
    {
        private static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        private static TelegramBotClient teleBot = new Telegram.Bot.TelegramBotClient("5656809793:AAGc85FTWbztWkPIg5e13rFGw2rCUbIxoVU");
        private static readonly string idTele = "973167438";
        public static void Infomation(this string message)
        {
            //await teleBot.SendTextMessageAsync(idTele, message);
            _log.Info("--- " + message);
        }
        public static void Fatal(this string message)
        {
            //await teleBot.SendTextMessageAsync(idTele, message);
            _log.Fatal("--- " + message);
        }

        public static void Warning(this string message)
        {
            //await teleBot.SendTextMessageAsync(idTele, message);
            _log.Warn("--- " + message);
        }

        public static async Task ErrorAsync(this string ex)
        {
            await teleBot.SendTextMessageAsync(idTele, ex.ToString());
            _log.Error("--- Đã cõ lỗi xảy ra: " + ex);
        }

        public static async void Error(Exception ex)
        {
            await teleBot.SendTextMessageAsync(idTele, ex.ToString());
            _log.Error($"--- Đã cõ lỗi xảy ra: {ex.ToString()} ", ex);
        }
        public static async void Error(Exception ex, string more)
        {
            await teleBot.SendTextMessageAsync(idTele, ex.ToString());
            _log.Error($"--- Đã cõ lỗi xảy ra: ${more}", ex);
            _log.Error($"--- ---Nội dung lỗi: ${ex.ToString()}", ex);
        }
        private static int GetLineException(Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            return frame.GetFileLineNumber();
        }
        public static void Init()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget(Constants.FileTarget)
            {
                FileName = Constants.DefaultFilePath + DateTime.Now.ToString("yyyy-MM-dd") + Constants.DefaultFileName,
                ArchiveNumbering = NLog.Targets.ArchiveNumberingMode.Rolling
            };
            var logconsole = new NLog.Targets.ConsoleTarget(Constants.ConsoleTarget);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            NLog.LogManager.Configuration = config;
            _log.Info("Đã khởi tạo logging thành công");
            _log.Info("--- Chương trình bắt đầu khởi chạy ---");
        }
    }
}
