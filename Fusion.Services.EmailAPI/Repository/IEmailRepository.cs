using Fusion.Services.EmailAPI.Messages;

namespace Fusion.Services.EmailAPI.Repository
{
    public interface IEmailRepository
    {
        Task SendAndLogEmail(UpdatePaymentResultMessage message);
    }
}
