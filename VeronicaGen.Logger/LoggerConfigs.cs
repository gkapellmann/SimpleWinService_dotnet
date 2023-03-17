
namespace VeronicaGen.Logger {
    public static class LoggerConfigs {

        public static readonly string appName = AppDomain.CurrentDomain.FriendlyName;
        public static readonly string logsDirectory = "Logs";

        public static string logLocation = logsDirectory;

        public static bool showLog { get; set; }
        public static bool showAdvLog { get; set; }
        public static bool saveLog { get; set; }

    }
}
