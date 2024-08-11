using BusinessLayer.IServices;
using BusinessLayer.Managers;
using Microsoft.Extensions.DependencyInjection;
using EntityLayer.MappingProfiles;
using AutoMapper;
namespace BusinessLayer.ServiceExtension
{
    public static class ServiceRouting
    {
        public static void AddServiceRouting(this IServiceCollection service)
        {
            service.AddScoped<ITrendyolService, TrendyolManager>();
            service.AddScoped<IEmotinalAnalysis, EmotionalAnalysis>();
        }
        public static void AddMappingServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductProfile)); // AutoMapper'ı kaydetme
        }
    }
}
