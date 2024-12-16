using EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contexts
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        public DbSet<Category> categories { get; set; }
        public DbSet<User> users { get; set; }
		public DbSet<Comment> comments { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductProperty> productproperty { get; set; }
		public DbSet<CategoryMarketPlace> categoriesMarketplace { get; set; }
		public DbSet<Log> Logs { get; set; }
		public DbSet<SubCategoryMarketPlace> subCategoriesMarketplace { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
		}
    }
}
