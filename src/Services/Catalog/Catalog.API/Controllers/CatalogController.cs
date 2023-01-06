using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
   public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _Repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _Repository = repository;
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          var products= await _Repository.GetProducts();
            return Ok(products);
        }

        [HttpGet ("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
           var result= await _Repository.GetProduct(id);
            if (result == null)
            {
                _logger.LogError($"Product with id : + {id}, not found");
                return NotFound();
            }
            return Ok(result);
        }
        [Route("[action]/{category}",Name ="GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _Repository.GetProductByCategory(category);
            return Ok(products);

        }
        [HttpPost]
        [ProducesResponseType(typeof(Product),(int) HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _Repository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }
        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
           return Ok (await _Repository.UpdateProduct(product));
        }
        [HttpDelete("{id:length(24)}",Name ="DeleteProduct")]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct (string id)
        {
            return Ok(await _Repository.DeleteProduct(id));
        }
    }
}
