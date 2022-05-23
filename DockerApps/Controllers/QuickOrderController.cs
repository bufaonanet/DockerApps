using DockerApps.Interfaces;
using DockerApps.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DockerApps.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuickOrderController : ControllerBase
    {
        private readonly ILogger<QuickOrderController> _logger;
        private readonly IQuickOrderLogic _orderLogic;

        public QuickOrderController(ILogger<QuickOrderController> logger, IQuickOrderLogic orderLogic)
        {
            _logger = logger;
            _orderLogic = orderLogic;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuickOrder(QuickOrder orderInfo)
        {
            _logger.LogInformation($"Submitting order for {orderInfo.Quantity} of {orderInfo.ProductId}.");

            var order = await _orderLogic.PlaceQuickOrder(orderInfo, 1234); // would get customer id from authN system/User claims
            return Created(nameof(SubmitQuickOrder), order);
        }
    }
}
