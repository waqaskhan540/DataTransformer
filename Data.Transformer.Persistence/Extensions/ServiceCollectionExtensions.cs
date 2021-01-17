using Data.Transformer.Persistence.Context;
using Data.Transformer.Persistence.Repositories;
using Data.Transformer.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Transformer.Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<DatabaseContext>(options =>
                             options.UseSqlServer(connectionString,
                             config => config.MigrationsAssembly("Data.Transformer.Persistence")
                             ));

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>(); ;
            services.AddScoped<IDatabaseContext, DatabaseContext>();

            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
