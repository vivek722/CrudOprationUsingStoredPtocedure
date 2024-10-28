using Microsoft.AspNetCore.Mvc;

namespace Spprectise.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ProductController(IProductRepository productRepository) : Controller
    {

        [HttpPost("Add")]
        public async Task<IActionResult> Create(Product product)
        {
            await productRepository.AddProduct(product);
            return CreatedAtAction(nameof(getbyid), new { id = product.Id }, product);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await productRepository.DeleteProduct(id);
            return NoContent();
        }
        [HttpGet("getbyid")]
        public async Task<IActionResult> getbyid(int id)
        {
            return Ok(productRepository.GetByIdProduct(id));
        }
        [HttpGet("get")]
        public async Task<IActionResult> getProducts(Product product)
        {
            return Ok(productRepository.GetAllProducts());
        }
        [HttpPut("update")]
        public async Task<IActionResult> update(Product product)
        {
            await productRepository.UpdateProduct(product);
            return CreatedAtAction(nameof(getbyid), new { id = product.Id }, product);
        }
    }
}
