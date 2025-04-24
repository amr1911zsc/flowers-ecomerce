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
    public class CategoryController : ControllerBase
    {
        Appcontext appcontext;
        public CategoryController(Appcontext appcontext)
        {
            this.appcontext = appcontext;

        }
        [HttpGet]
        public IActionResult Getcaregory( Category_Dto category_Dto)
        {
            List<Category> categories = appcontext.Categories.ToList();
            return Ok(categories);
     
            
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory(Category_Dto categorydto)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category();
                category.Name = categorydto.Name;
                appcontext.Categories.Add(category);
                appcontext.SaveChanges();
                return Ok($" category {category.Name} added");
            }
            return BadRequest();


        }
        [HttpPut("Edit{id:int}")]
        [Authorize]
        public IActionResult EditCategory( Category_Dto categoryDto ,int id)
        {   
            if (ModelState.IsValid)
            {
                Category category = appcontext.Categories.FirstOrDefault(p=>p.CateID == id);
                if (category == null)
                {
                    return BadRequest();
                }
                category.Name= categoryDto.Name;
                appcontext.Categories.Update(category);
                appcontext.SaveChanges(true);
                return Ok();

            }
            return BadRequest(ModelState);
        }
        [HttpDelete]
        [Authorize(Roles ="Admin")]
        public IActionResult DeleteCategory(int id)
        {
            Category category=appcontext.Categories.FirstOrDefault(p=> p.CateID == id);
            appcontext.Categories.Remove(category);
            appcontext.SaveChanges();
            return Ok();

        }




    }
}
