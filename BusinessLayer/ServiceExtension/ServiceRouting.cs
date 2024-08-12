using BusinessLayer.IServices;
using BusinessLayer.Managers;
using Microsoft.Extensions.DependencyInjection;
using EntityLayer.MappingProfiles;
using AutoMapper;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories;
namespace BusinessLayer.ServiceExtension
{
    public static class ServiceRouting
    {
        public static void AddServiceRouting(this IServiceCollection service)
        {
            service.AddScoped<ITrendyolService, TrendyolManager>();
            service.AddScoped<IEmotinalAnalysis, EmotionalAnalysis>();
            service.AddScoped<IProductService, ProductManager>();
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<ICategoryService, CategoryManager>();
            service.AddScoped<ICategoryRepository, CategoryRepository>();
        }
        public static void AddMappingServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(CategoryProfile));
        }
    }
}
