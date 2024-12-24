using BulkyWeb_Razor.Data;
using BulkyWeb_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWeb_Razor.Pages.Categories
{
	public class IndexModel : PageModel
	{
		private readonly AppDBContext _dbContext;
		public List<Category> categoryList { get; set; }
		public IndexModel(AppDBContext db)
		{
			_dbContext = db;
		}
		public void OnGet()
		{
			categoryList = _dbContext.Catogories.ToList();
		}
	}
}
