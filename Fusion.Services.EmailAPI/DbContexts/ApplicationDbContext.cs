using Fusion.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Fusion.Services.EmailAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<EmailLog> EmailLogs { get; set; }    
    }
}
