using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Data;
using WebApplication6.Dto;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        Appcontext Appcontext;
        public ProductsController( Appcontext appcontext)
        {
            this.Appcontext = appcontext;
        }
        [HttpPost]
        [Authorize]
        public IActionResult addproducts( productdto productdto)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                product.Name = productdto.Name;
                product.Description = productdto.Description;
                product.Price = productdto.Price;
                product.Quantity = productdto.Quantity;
                product.imglink = productdto.Imagelink;
                product.CateID = productdto.CateID;
                product.Type = productdto.Type;
                Appcontext.Products.Add(product);
                Appcontext.SaveChanges(true);
                return Ok();
            }
            return BadRequest( ModelState);


        }
        [HttpDelete("{id:int}")]
        public IActionResult deleteproduct(int id)
        {
            Product product=Appcontext.Products.FirstOrDefault(p=>p.ProductID==id);
            if (product != null)
            {
                Appcontext.Products.Remove(product);
                Appcontext.SaveChanges(true);
                return Ok();

            }
            return BadRequest( ModelState);
        }
        [HttpPut("{id:int}")]
        public IActionResult editproduct(int id, productdto productdto)
        {
            if (ModelState.IsValid)
            {
                Product product = Appcontext.Products.FirstOrDefault(c=>c.ProductID==id);
                product.Name = productdto.Name;
                product.Description = productdto.Description;
                product.Price = productdto.Price;
                product.Quantity = productdto.Quantity;
                product.imglink = productdto.Imagelink;
                product.CateID = productdto.CateID;
                product.Type = productdto.Type;
                Appcontext.Products.Update(product);
                Appcontext.SaveChanges(true);
                return Ok();
            }
            return BadRequest(ModelState);

        }
        [HttpGet("{id:int}")]
        public IActionResult getproduct(int id)
        {
            if (ModelState.IsValid)
            {
                Product product = Appcontext.Products.FirstOrDefault(p=> p.ProductID==id);
                if(product != null)
                {
                    return Ok(product);
                }
                return NotFound("Product not found");
               
            }
            return BadRequest(ModelState);
        }
        [HttpGet("{name:alpha}")]
        public IActionResult getproductbyname(string name)
        {
            if (ModelState.IsValid)
            {
                Product product = Appcontext.Products.FirstOrDefault(p => p.Name == name);
                if (product != null)
                {
                    return Ok(product);
                }
                return NotFound("Product not found");

            }
            return BadRequest(ModelState);
        }
        [HttpGet(" get by type {type:alpha}")]
        public IActionResult getproductbytype(string type)
        {
            if (ModelState.IsValid)
            {
                Product product = Appcontext.Products.FirstOrDefault(p => p.Type == type);
                if (product != null)
                {
                    return Ok(product);
                }
                return NotFound("Product not found");

            }
            return BadRequest(ModelState);
        }
        [HttpGet("ShowAllProducts")]
        public IActionResult ShowAllProducts()
        {
            List<Product> products = Appcontext.Products.ToList();
            return Ok(products);
        }
       
    }
}
