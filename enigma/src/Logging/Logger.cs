using Serilog;
using System.IO;

namespace Logging {

public static class Logger
{
    static Logger()
    {

        String LOG_DIR_PATH = "./logs";

        if ( !Directory.Exists(LOG_DIR_PATH) ) {
          Directory.CreateDirectory(LOG_DIR_PATH);
        }

        Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Debug()
          .WriteTo.File(LOG_DIR_PATH+"/enigma.log", 
              rollingInterval: RollingInterval.Day,
              outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
          .CreateLogger();

    }

    public static Serilog.ILogger For<T>() => Log.ForContext<T>();
    public static void Flush() => Log.CloseAndFlush();

}

}
