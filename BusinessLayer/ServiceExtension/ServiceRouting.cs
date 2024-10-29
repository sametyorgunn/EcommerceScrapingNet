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
            service.AddScoped<ITrendyolCategoriesRepository, TrendyolCategoriesRepository>();
            service.AddScoped<IN11Service, N11Manager>();
            service.AddScoped<ICommentService, CommentManager>();
            service.AddScoped<ICommentRepository, CommentRepository>();


			service.AddAutoMapper(typeof(ProductProfile));
			service.AddAutoMapper(typeof(CategoryProfile));
			service.AddAutoMapper(typeof(TrendyolCategoryProfile));
			service.AddAutoMapper(typeof(CommentProfile));
		}
       
    }
}
