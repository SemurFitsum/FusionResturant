using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Fusion.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer
    {
        private async Task OnCheckOutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDTO checkoutHeaderDTO = JsonConvert.DeserializeObject<CheckoutHeaderDTO>(body);


        }
    }
}
