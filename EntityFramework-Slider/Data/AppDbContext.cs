using EntityFramework_Slider.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SliderInfo> SliderInfos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Advantage> Advantages { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Say> Says { get; set; }
        public DbSet<Instagram> Instagrams { get; set; }

    }
}
