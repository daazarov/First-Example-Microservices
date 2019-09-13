using Microsoft.Extensions.DependencyInjection;

namespace Client.Mvc.Extensions
{
    public static class AddTransportServiceCollection
    {
        public static void AddTransport(this IServiceCollection services)
        {
            services.AddCors(e =>
            {
                e.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(host => true));
            });

            services.AddSignalR();
        }
    }
}
