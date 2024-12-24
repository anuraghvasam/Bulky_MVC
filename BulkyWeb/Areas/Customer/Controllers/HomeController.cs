
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _uOW;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork uOW)
        {
            _logger = logger;
            _uOW = uOW;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _uOW.Product.GetAll(includeProps: "Category");
            return View(products);

        }

        public IActionResult Details(int productId)
        {
            ShoppingCart shoppingCart = new()
            {
                Product = _uOW.Product.Get(u => u.Id == productId, includeProps: "Category"),

                Count = 1,

                ProductId = productId
            };


            return View(shoppingCart);
            //return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _uOW.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _uOW.ShoppingCart.Update(cartFromDb);
                _uOW.Save();
            }
            else
            {
                //add cart record
                _uOW.ShoppingCart.Add(shoppingCart);
                _uOW.Save();
                //HttpContext.Session.SetInt32(SD.SessionCart,
                //_uOW.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";




            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
