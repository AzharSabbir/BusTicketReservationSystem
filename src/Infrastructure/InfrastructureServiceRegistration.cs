using Application.Contracts.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("BusTicketReservationSystemDb")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IBusScheduleRepository, BusScheduleRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            return services;
        }
    }
}