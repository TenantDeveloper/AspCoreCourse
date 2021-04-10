using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistent;
using Persistent.Entities;

namespace Ef_Core_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Get() 
        {
            var Categories = _context.Categories.ToList();
            return Ok(Categories); // response 200;
        }

        [HttpGet("Exist")]
        public IActionResult IsExist(string name)
        {
          // bool exist =   _context.Categories.;

            return Ok();
        }


        [HttpGet("Search")]
        public IActionResult Search([FromQuery] string name)
        {
            var categories = _context.Categories.Where(c => c.Name.StartsWith(name)).ToList()
                  .Where(c => c.Products.Any());
            return Ok(categories);
        }


        [HttpPost("AddCategory")]
        public IActionResult AddCategory(Category category)
        {
            _context.Categories.Add(category); // sql query
            _context.SaveChanges();
            return Ok(); // 204 
        }

        // update
        [HttpPut("UpdateCategory/{id:int}")]
        public IActionResult UpdateCategory (int id  , Category c)
        {
            // step 1 : get object depends on id

            // Category category = _context.Categories.Find(id); // row from sql

             var category = _context.Categories.SingleOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();

            category.Name = c.Name;

            _context.Categories.Update(category);
            _context.SaveChanges();
            return Ok(); // 204 
        }

        [HttpDelete("DeleteCategory/{id:int}")]
        public IActionResult DeleteCategory (int id)
        {

            Category category = _context.Categories.Find(id); // row from sql
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok();
        }

    }
}