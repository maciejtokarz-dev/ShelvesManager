using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MyApp.Areas.Identity.Data;
using MyApp.Models;

namespace MyApp.Data
{
    public class MyAppContext : IdentityDbContext<MyAppUser>
    {
        public MyAppContext(DbContextOptions<MyAppContext> options) : base(options) { }
        
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Product> Products { get; set; }
        
    }
}
