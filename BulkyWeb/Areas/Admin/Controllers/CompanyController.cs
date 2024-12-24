
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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _uOW;

        public CompanyController(IUnitOfWork uOW, IWebHostEnvironment webHostEnvironment)
        {
            _uOW = uOW;
        }

        public IActionResult Index()
        {
            List<Company> CompanyList = _uOW.Company.GetAll().ToList();


            return View(CompanyList);
        }

        public IActionResult Upsert(int? id)
        {


            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company company = _uOW.Company.Get(c => c.Id == id);

                return View(company);
            }



            //ViewBag.CategoryList = CategoryList;

        }

        [HttpPost]
        public IActionResult Upsert(Company company, IFormFile? file)
        {


            if (ModelState.IsValid)
            {



                if (company.Id == 0)
                    _uOW.Company.Add(company);
                else
                    _uOW.Company.Update(company);
                _uOW.Save();

                TempData["success"] = $"{company.Name}-Created Successfully.";

                return RedirectToAction("Index", "Company");
            }
            else
            {

                return View(company);
            }

        }

        //public IActionResult Edit(int? id)
        //{
        //	if (id == null || id == 0)
        //	{
        //		return NotFound();
        //	}

        //	Company? product = _uOW.Company.Get(c => c.Id == id);
        //	//Company? Company1 = _dbContext.Catogories.Where(c => c.Id == id).FirstOrDefault();
        //	//Company? Company2 = _dbContext.Catogories.Find(id);

        //	if (product == null)
        //	{
        //		return NotFound();
        //	}

        //	return View(product);
        //}


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> CompanyList = _uOW.Company.GetAll().ToList();

            return Json(new { data = CompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _uOW.Company.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _uOW.Company.Remove(companyToBeDeleted);
            _uOW.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion

    }

}

