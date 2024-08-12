using CRUD_Operations.ActionFilter;
using CRUD_Operations.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRUD_Operations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductsController :ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var username = User.Identity.Name;
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)!.Value;//getting the id
            var recoreds = _context.Set<Product>().ToList();
            return Ok(recoreds);
        }
        [HttpGet]
        [Route("{Key}")]
        [LogSensitiveAction]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Product>> GetById([FromRoute(Name = "Key")]int id)
        {
            var recoreds = _context.Set<Product>().Find(id);
            return recoreds== null? NotFound() :Ok(recoreds);
        }


        [HttpPost]
        [Route("")]
        public ActionResult<int> CreateProduct(Product product)
        {
            product.Id = 0;
            _context.Set<Product>().Add(product);
            _context.SaveChanges();
            return Ok(product.Id);
        }

        [HttpPut]
        [Route("")]
        public ActionResult UpdateProduct(Product product)
        {
            var existingProduct = _context.Set<Product>().Find(product.Id);
            existingProduct.Sku = product.Sku;
            existingProduct.Name = product.Name;
            _context.Set<Product>().Update(existingProduct);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var existingProduct = _context.Set<Product>().Find(id);
            _context.Set<Product>().Remove(existingProduct);
            _context.SaveChanges();
            return Ok();
        }
    }
}
