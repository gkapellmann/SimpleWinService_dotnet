
using System.Runtime.CompilerServices;
using System.Text;

namespace VeronicaGen.Logger {
    public static class Log {

        private const int maxLogCount = 5000;
        private static List<string> logData = new List<string>();

        public static void EmptyLine([CallerMemberName] string source = "") {
            if (LoggerConfigs.showLog)
                Console.WriteLine("(" + DateTime.Now.ToString() + ") " + source + "> ...");
            if (LoggerConfigs.saveLog) {
                logData.Add("(" + DateTime.Now.ToString() + ") " + source + "> ...");
                CheckIfDumpNeeded();
            }
        }

        public static void Number<T>(string name, T number, int preEmptyLines = 0, int postEmptyLines = 0, [CallerMemberName] string source = "") {
            Text(name + ": " + number.ToString(), preEmptyLines, postEmptyLines, source);
        }

        public static void Text(string message, int preEmptyLines = 0, int postEmptyLines = 0, [CallerMemberName] string source = "") {

            for (int i = 0; i < preEmptyLines; i++)
                EmptyLine(source);

            if (message is null) {
                message = "[null]";
            }
            
            if(LoggerConfigs.showLog) 
                Console.WriteLine("(" + DateTime.Now.ToString() + ") " + source + "> " + message);
            if(LoggerConfigs.saveLog) {
                logData.Add("(" + DateTime.Now.ToString() + ") " + source + "> " + message);
                CheckIfDumpNeeded();
            }
            for (int i = 0; i < postEmptyLines; i++)
                EmptyLine(source);
        }

        public static void Text(StringBuilder message, int preEmptyLines = 0, int postEmptyLines = 0, [CallerMemberName] string source = "") {

            for (int i = 0; i < preEmptyLines; i++)
                EmptyLine(source);

            if (message is null) {
                Console.WriteLine("[null]");
                return;
            }

            if (LoggerConfigs.showLog)
                Console.WriteLine("(" + DateTime.Now.ToString() + ") " + source + "> " + message.ToString());
            if (LoggerConfigs.saveLog) {
                logData.Add("(" + DateTime.Now.ToString() + ") " + source + "> " + message.ToString());
                CheckIfDumpNeeded();
            }

            for (int i = 0; i < postEmptyLines; i++)
                EmptyLine(source);
        }

        public static void AdvText(string message, int preEmptyLines = 0, int postEmptyLines = 0, [CallerMemberName] string source = "") {
            if (LoggerConfigs.showAdvLog) {
                Text("[Advanced] " + message, preEmptyLines, postEmptyLines, source);
            }
        }

        public static void AdvText(StringBuilder message, int preEmptyLines = 0, int postEmptyLines = 0, [CallerMemberName] string source = "") {
            if (LoggerConfigs.showAdvLog) {
                message.Insert(0, "[Advanced] ");
                Text(message, preEmptyLines, postEmptyLines, source);
            }
        }

        public static void Exception(Exception ex, [CallerMemberName] string source = "") {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" ");
            sb.AppendLine("   ****** Exception ******* ");
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine("Location: " + source);
            sb.AppendLine("Source: " + ex.Source);
            sb.AppendLine("Message: " + ex.Message);
            if (LoggerConfigs.showAdvLog)
                sb.AppendLine("Stack Trace: " + ex.StackTrace);
            sb.AppendLine("   ************************ ");

            if (LoggerConfigs.showLog)
                Console.WriteLine(sb.ToString());
            if (LoggerConfigs.saveLog) {
                logData.Add(sb.ToString());
                CheckIfDumpNeeded();
            }
            EmptyLine(source);
        }

        private static void CheckIfDumpNeeded() {
            if (logData.Count > maxLogCount) {
                Task.Factory.StartNew(async () => await SaveCurrentLog());
            }
        }

        public static async Task SaveCurrentLog() {
            if (LoggerConfigs.saveLog) {
                var temp = new List<string>(logData);
                logData.Clear();
                string logDir = LoggerConfigs.appName + "_Log-" + DateTime.Now.ToString("yyyyMMdd_hh-mm") + ".txt";
                await File.AppendAllLinesAsync(logDir, temp);
            }
        }

    }
}
