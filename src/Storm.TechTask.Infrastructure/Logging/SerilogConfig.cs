using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Debugging;

namespace Storm.TechTask.Infrastructure.Logging
{
    public static class SerilogConfig
    {
        public const string PropNameUsername = "Username";

        private static ILogger? SecurityLogger { get; set; }


        #region Bootstrap logger

        public static void AddBootstrapLogging()
        {
            SetupSerilogSelfLogging(true);
            Log.Logger = new LoggerConfiguration()
                                    .WriteTo.Console()
                                    .CreateBootstrapLogger();
            Log.Information("Bootstrapping app");
        }

        private static void SetupSerilogSelfLogging(bool selfLogToFile)
        {
            if (selfLogToFile)
            {
                try
                {
                    var file = File.CreateText("Logs/serilog-self-log.txt");
                    SelfLog.Enable(TextWriter.Synchronized(file));
                }
                catch
                {
                    SelfLog.Enable(msg => Debug.WriteLine(msg));
                }
            }
            else
            {
                SelfLog.Enable(msg => Debug.WriteLine(msg));
            }
        }

        #endregion


        #region Common & Security logger

        public static ILogger GetSecurityLogger(IConfiguration config)
        {
            if (SecurityLogger == null) // This isn't thread-safe, but I don't think it matters.
            {
                SecurityLogger = new LoggerConfiguration()
                                        .SetupSecurityConfig(config)
                                        .CreateLogger();
            }
            return SecurityLogger;
        }

        public static LoggerConfiguration SetupCommonConfig(this LoggerConfiguration loggerConfig, HostBuilderContext context)
        {
            return loggerConfig.ReadFrom.Configuration(context.Configuration, sectionName: "CommonLogger")
                                .Enrich.FromLogContext();
        }

        private static LoggerConfiguration SetupSecurityConfig(this LoggerConfiguration loggerConfig, IConfiguration config)
        {
            return loggerConfig.ReadFrom.Configuration(config, sectionName: "SecurityLogger")
                                .Enrich.FromLogContext();
        }

        #endregion
    }

}
