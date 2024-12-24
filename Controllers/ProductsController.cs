using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers.RequestHelpers;
using WebApplication1.Data;
using WebApplication1.Data.Implementation.Specifications;
using WebApplication1.Models;
using WebApplication1.Models.Interfaces;

namespace WebApplication1.Controllers
{
    
    public class ProductsController(IGnericRepository<Product> repo) : BaseApiController
    {
        
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {

            var spec = new ProductSpecification(specParams);
            
            return await CreatePagesResult(repo, spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if(product==null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.Add(product);
            if (await repo.SaveAllAsync()) { 
                return CreatedAtAction("GetProduct", new {id=product.Id}, product);
            }
            return BadRequest("Problem creating issue");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if(product.Id != id || !ProductExists(id))
            {
                return BadRequest("Cannot update this product");
            }
            repo.Update(product);

            if (await repo.SaveAllAsync()) { 

                return NoContent();
            }

            return BadRequest("issue for updating Product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            repo.Remove(product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("issue for Deleting Product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new ListSpecification();
            return Ok(await repo.ListAsync(spec));
        }
        private bool ProductExists(int id)
        {
            return repo.Exists(id);
        }
    }
}
