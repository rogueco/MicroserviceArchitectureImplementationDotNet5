using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ILogger = DnsClient.Internal.ILogger;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            IEnumerable<Product> products = await _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            Product product = await _productRepository.GetProduct(id);

            if (product != null) return Ok(product);
            _logger.LogError($"Product with id: {id}, hasn't been found in database.");
            return NotFound();

        }

        [Route("[action]/{category}")]
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            IEnumerable<Product> product = await _productRepository.GetProductByCategory(category);
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int) HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.Create(product);

            return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product value)
        {
            return Ok(await _productRepository.Update(value));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _productRepository.Delete(id));
        }
    }
}