using System.Text;
using Hangfire.Dashboard;

namespace ECommerce.Api.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public HangfireAuthorizationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var authHeader = httpContext.Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authHeader) ||
                !authHeader.StartsWith("Basic "))
            {
                Challenge(httpContext);
                return false;
            }

            var encoded = authHeader["Basic ".Length..].Trim();
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));

            var parts = credentials.Split(':', 2);

            if (parts.Length != 2)
            {
                Challenge(httpContext);
                return false;
            }

            var username = _configuration["Hangfire:UserName"];
            var password = _configuration["Hangfire:Password"];

            if (parts[0] != username || parts[1] != password)
            {
                Challenge(httpContext);
                return false;
            }

            return true;
        }

        private static void Challenge(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
        }
    }
}
