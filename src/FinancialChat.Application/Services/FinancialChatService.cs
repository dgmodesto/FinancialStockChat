using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Services
{
    public class FinancialChatService : IFinancialChatService
    {
        private readonly ILogger<FinancialChatService> _logger;
        private readonly IBus _bus;

        public FinancialChatService(ILogger<FinancialChatService> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task SendRequestStockByCode(Message message)
        {
            _logger.LogInformation($"[{nameof(FinancialChatService)}-{  nameof(SendRequestStockByCode) }] : Sending request to queue");
            var financialChatRequestQueueName = Environment.GetEnvironmentVariable("FINANCIAL_CHAT_STOCK_QUEUE_REQUEST");
            await SendMessageForQueueAsync(message, financialChatRequestQueueName);
        }

        public async Task SendResponseStockByCode(Message message)
        {
            _logger.LogInformation($"[{nameof(FinancialChatService)}-{  nameof(SendResponseStockByCode) }] : Sending response to queue");
            var financialChatResponseQueueName = Environment.GetEnvironmentVariable("FINANCIAL_CHAT_STOCK_QUEUE_RESPONSE");
            await SendMessageForQueueAsync(message, financialChatResponseQueueName);
        }

        private async Task SendMessageForQueueAsync(Message message, string queueName)
        {
            try
            {
                _logger.LogInformation($"RABBITMQ Queue: {queueName}");

                Uri uri = new Uri($"queue:{queueName}");
                var endpoint = await _bus.GetSendEndpoint(uri);
                await endpoint.Send(message);

            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(FinancialChatService)}-{nameof(SendResponseStockByCode) }] : Error when send to queue - Error Description: { ex.Message }");

                throw ex;
            }

            /*
             A utilização da interface IBus configurada no arquivo Startup e injetamos o objeto IBus no construtor do controlador
             A definição do nome fila como revenueKindsQueue e a criação de uma nova url : ‘amqp://guest:guest@localhost:5672/revenueKindsQueue’.
             Se a fila não existir o RabbitMQ vai criar para nós
             A obtenção de um endpoint para o qual vamos enviar o objeto Ticket (RevenueKind Model)
             O envio da mensagem para o RabbitMQ usando o método Send;
             */
        }
    }
}
