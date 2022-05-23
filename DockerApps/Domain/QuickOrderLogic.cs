using DockerApps.Integrations;
using DockerApps.Interfaces;
using DockerApps.Models;
using DockerApps.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DockerApps.Domain
{
    public class QuickOrderLogic : IQuickOrderLogic
    {
        private readonly ICarvedRockRepository _repo;
        private readonly ILogger<QuickOrderLogic> _logger;
        private readonly IOrderProcessingNotification _orderProcessingNotification;

        public QuickOrderLogic(ILogger<QuickOrderLogic> logger,
                               IOrderProcessingNotification orderProcessingNotification, 
                               ICarvedRockRepository repo)
        {
            _logger = logger;
            _orderProcessingNotification = orderProcessingNotification;
            _repo = repo;
        }
        public async Task<Guid> PlaceQuickOrder(QuickOrder order, int customerId)
        {
            _logger.LogInformation("Placing order and sending update for inventory...");
            var orderId = Guid.NewGuid();
            
            // persist order to database or wherever
            await _repo.SubmitNewQuickOrder(order, customerId, orderId);

            // post "orderplaced" event to rabbitmq
            _orderProcessingNotification.QuickOrderReceived(order, customerId, orderId);

            return orderId;
        }
    }
}
