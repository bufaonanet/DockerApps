using DockerApps.Interfaces;
using DockerApps.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockerApps.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductLogic _productLogic;

        public ProductsController(IProductLogic productLogic)
        {
            _productLogic = productLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string category = "all")
        {
            Log.ForContext("Category",category)
                .Information("Starting controller action GetProdutcts", category);

            var products = await _productLogic.GetProductsForCategory(category);

            return Ok(products);
        }
    }
}
