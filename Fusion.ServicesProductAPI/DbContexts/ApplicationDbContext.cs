using Fusion.ServicesProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Fusion.ServicesProductAPI.DbContexts
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
