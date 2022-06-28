
using Serilog;

using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Infrastructure.Logging
{
    public class LoggingService : ILoggingService
    {
        public ILogger CommonLogger { get; set; }
        public ILogger SecurityLogger { get; set; }

        public LoggingService(ILogger commonLogger, ILogger securityLogger)
        {
            this.CommonLogger = commonLogger;
            this.SecurityLogger = securityLogger;
        }
    }

}
