using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace triviaFinal.Services
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
            var messages = await _queueClient.ReceiveMessagesAsync(1);
            return messages.Value.FirstOrDefault();
        }

        public async Task DeleteMessageAsync(string messageId, string popReceipt)
        {
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }
    }   
}    
