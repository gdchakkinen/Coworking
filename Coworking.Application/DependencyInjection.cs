using Microsoft.Extensions.DependencyInjection;
using Coworking.Application.Servicos;
using Coworking.Application.Interfaces;


namespace Coworking.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IReservaService, ReservaService>();         

            return services;
        }
    }
}
