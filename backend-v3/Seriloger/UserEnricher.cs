using Serilog.Core;
using Serilog.Events;

namespace backend_v3.Seriloger
{
    public class UserEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserEnricher() : this(new HttpContextAccessor())
        {
        }

        public UserEnricher(HttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            string ip = $"{_httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("IP", ip));
        }
    }
}
