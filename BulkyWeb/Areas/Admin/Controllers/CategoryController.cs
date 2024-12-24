
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Bulky.Utility;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _uOW;
        public CategoryController(IUnitOfWork uOW)
        {
            _uOW = uOW;
        }

        public IActionResult Index()
        {
            List<Category> categoryList = _uOW.Category.GetAll().ToList();
            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name cannot exactly match Display Order");
            }

            if (ModelState.IsValid)
            {

                _uOW.Category.Add(category);
                _uOW.Save();

                TempData["success"] = $"{category.Name}-Created Successfully.";

                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = _uOW.Category.Get(c => c.Id == id);
            //Category? category1 = _dbContext.Catogories.Where(c => c.Id == id).FirstOrDefault();
            //Category? category2 = _dbContext.Catogories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {

            if (ModelState.IsValid)
            {
                _uOW.Category.Update(category);
                _uOW.Save();

                TempData["success"] = $"{category.Name}-Updated Successfully.";
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = _uOW.Category.Get(c => c.Id == id);
            //Category? category1 = _dbContext.Catogories.Where(c => c.Id == id).FirstOrDefault();
            //Category? category2 = _dbContext.Catogories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? category = _uOW.Category.Get(u => u.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            _uOW.Category.Remove(category);
            _uOW.Save();
            TempData["success"] = $"{category.Name}-Deleted Successfully.";
            return RedirectToAction("Index", "Category");
        }

    }

}

