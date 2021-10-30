using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using khi.Forms;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.File;

namespace khi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var jsonFormatter = new JsonFormatter(renderMessage: true);
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .WriteTo.File(jsonFormatter, "logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Debug()
                .MinimumLevel.Debug()
                .CreateLogger();

            Application.ThreadException += (sender, args) =>
                Log.Logger.Error(args.Exception, "An unhandled UI exception occured");

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                Log.Logger.Error((Exception)args.ExceptionObject, "An unhandled exception occured");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
