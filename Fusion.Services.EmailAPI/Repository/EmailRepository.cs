using Fusion.Services.EmailAPI.DbContexts;
using Fusion.Services.EmailAPI.Messages;
using Fusion.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Fusion.Services.EmailAPI.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;

        public EmailRepository(DbContextOptions<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendAndLogEmail(UpdatePaymentResultMessage message)
        {
            // implement an email sender or call some other class library
            EmailLog emailLog = new EmailLog()
            {
                Email = message.Email,
                EmailSent = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully."
            };

            await using var _db = new ApplicationDbContext(_dbContext);
            _db.EmailLogs.Add(emailLog);

            await _db.SaveChangesAsync();
        }
    }
}
