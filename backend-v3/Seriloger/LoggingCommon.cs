using backend_v3.Context;
using Serilog.Context;
using Ilogger = Serilog.ILogger;

namespace backend_v3.Seriloger
{
    public class LoggingCommon
    {
        private readonly Ilogger _logger;
        private readonly AppDbContext _context;
        public LoggingCommon(Ilogger logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void AddLoggingInformation(string? message, string? userId, string? phanLoai)
        {
            var user_id = _context.Users.FirstOrDefault(x => x.Id == userId);
            using (LogContext.PushProperty("PhanLoai", $"{phanLoai}"))
            using (LogContext.PushProperty("UserName", $"{user_id.Username ?? "anonymous"}"))
            {
                _logger.Information($"{message}");
            }
        }

        public void AddLoggingError(string? message, string? userId, string? phanLoai)
        {
            var user_id = _context.Users.FirstOrDefault(x => x.Id == userId);
            using (LogContext.PushProperty("PhanLoai", $"{phanLoai}"))
            using (LogContext.PushProperty("UserName", $"{user_id.Username ?? "anonymous"}"))
            {
                _logger.Error($"{message}");
            }
        }
    }
}
