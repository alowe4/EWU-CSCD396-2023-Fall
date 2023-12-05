using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using System.Text.Json;

namespace Final.Services
{
    public class QueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureStorageConnection");
            _queueClient = new QueueClient(connectionString, "quickstartqueues");
        }

        public async Task SendMessageAsync(object message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            await _queueClient.SendMessageAsync(jsonMessage);
        }

        public async Task<QueueMessage> ReceiveMessageAsync()
        {
            QueueProperties properties = _queueClient.GetProperties();
            int cachedMessagesCount = properties.ApproximateMessagesCount;
            if (cachedMessagesCount > 0)
            {
                var messages = await _queueClient.ReceiveMessagesAsync(1);
                return messages.Value.FirstOrDefault();
            }
            else
            {
                await SendMessageAsync("error, no questions in queue");
                var messages = await _queueClient.ReceiveMessagesAsync(1);
                return messages.Value.FirstOrDefault();
            }
        }

        public async Task DeleteMessageAsync(string messageId, string popReceipt)
        {
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }
    }
}
