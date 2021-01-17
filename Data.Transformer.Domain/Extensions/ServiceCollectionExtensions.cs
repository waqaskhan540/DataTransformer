using AutoMapper;
using Data.Transformer.Domain.BackgroundServices;
using Data.Transformer.Domain.Configuration;
using Data.Transformer.Domain.FileStorage;
using Data.Transformer.Domain.Models;
using Data.Transformer.Domain.Parsers;
using Data.Transformer.Domain.Services;
using Data.Transformer.Domain.Validators;
using Data.Transformer.Persistence.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Transformer.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddTransient<IFileStorageService, FileStorageService>();
            services.AddTransient<IFileUploadService, FileUploadService>();

            services.AddTransient<IFileValidator, FileValidator>();

            services.AddTransient<IJsonFileStorageService, JsonFileStorageService>();
            services.AddTransient<ICsvParser<ProductDto>, ProductCsvParser>();

            // services.AddTransient<IJsonTransformationProcessor, JsonTransformationProcessor>();
            //services.AddTransient<ISqlTransformationProcessor, SqlTransformationProcessor>();

            services.AddTransient<IJsonTransformationService, JsonTransformationService>();
            services.AddTransient<ISqlTransformationService, SqlTransformationService>();
            services.AddTransient<IProductService, ProductService>();


            IJobConfig jsonJObConfig = new JsonJobConfig
            {
                SourcePath = configuration["JsonJobConfig:SourcePath"],
                ProcessingPath = configuration["JsonJobConfig:ProcessingPath"],
                FailedPath = configuration["JsonJobConfig:FailedPath"],
                ProcessedPath = configuration["JsonJobConfig:ProcessedPath"]
            };

            IJobConfig dbJobConfig = new DbJobConfig
            {
                SourcePath = configuration["DbJobConfig:SourcePath"],
                ProcessingPath = configuration["DbJobConfig:ProcessingPath"],
                FailedPath = configuration["DbJobConfig:FailedPath"],
                ProcessedPath = configuration["DbJobConfig:ProcessedPath"]
            };


            services.AddTransient<IJsonJobStorage>(s => new JsonJobStorage(jsonJObConfig));
            services.AddTransient<IDbJobStorage>(s => new DbJobStorage(dbJobConfig));



            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDto, Product>();
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

        }
        
    }
}
