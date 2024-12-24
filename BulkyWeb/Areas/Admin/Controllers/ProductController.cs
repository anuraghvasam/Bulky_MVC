
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Bulky.Models.ViewModels;
using Bulky.DataAccess.Repository;
using Microsoft.IdentityModel.Tokens;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _uOW;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork uOW, IWebHostEnvironment webHostEnvironment)
        {
            _uOW = uOW;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> ProductList = _uOW.Product.GetAll(includeProps: "Category").ToList();


            return View(ProductList);
        }

        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = _uOW.Category.GetAll().Select(category => new SelectListItem
            {
                Text = category.Name,
                Value = category.Id.ToString()
            });

            ProductViewModel productViewModel = new()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _uOW.Product.Get(p => p.Id == id);

                return View(productViewModel);
            }



            //ViewBag.CategoryList = CategoryList;

        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
        {


            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = String.Concat(Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));

                    string productPath = Path.Combine(wwwRootPath, @"Images\product");

                    if (!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                    {
                        string old_ImageUrl = Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.TrimStart('/'));

                        if (System.IO.File.Exists(old_ImageUrl)) System.IO.File.Delete(old_ImageUrl);
                    }


                    using (var stream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    productViewModel.Product.ImageUrl = Path.Combine(@"\Images\product", fileName);

                }

                if (productViewModel.Product.Id == 0)
                    _uOW.Product.Add(productViewModel.Product);
                else
                    _uOW.Product.Update(productViewModel.Product);
                _uOW.Save();

                TempData["success"] = $"{productViewModel.Product.Title}-Created Successfully.";

                return RedirectToAction("Index", "Product");
            }
            else
            {
                productViewModel.CategoryList = _uOW.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productViewModel);
            }

        }

        //public IActionResult Edit(int? id)
        //{
        //	if (id == null || id == 0)
        //	{
        //		return NotFound();
        //	}

        //	Product? product = _uOW.Product.Get(c => c.Id == id);
        //	//Product? Product1 = _dbContext.Catogories.Where(c => c.Id == id).FirstOrDefault();
        //	//Product? Product2 = _dbContext.Catogories.Find(id);

        //	if (product == null)
        //	{
        //		return NotFound();
        //	}

        //	return View(product);
        //}

        [HttpPost]
        public IActionResult Edit(Product product)
        {

            if (ModelState.IsValid)
            {
                _uOW.Product.Update(product);
                _uOW.Save();

                TempData["success"] = $"{product.Title}-Updated Successfully.";
                return RedirectToAction("Index", "Product");
            }

            return View();
        }



        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? product = _uOW.Product.Get(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            _uOW.Product.Remove(product);
            _uOW.Save();
            TempData["success"] = $"{product.Title}-Deleted Successfully.";
            return RedirectToAction("Index", "Product");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> ProductList = _uOW.Product.GetAll(includeProps: "Category").ToList();

            return Json(new { data = ProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _uOW.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }


            _uOW.Product.Remove(productToBeDeleted);
            _uOW.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion

    }

}

