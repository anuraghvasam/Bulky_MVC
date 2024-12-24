using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		public readonly AppDBContext _db;
		public ProductRepository(AppDBContext db) : base(db)
		{
			_db = db;
		}
		public void Update(Product product)
		{
			var product_FromDB = _db.Products.FirstOrDefault(x => x.Id == product.Id);

			if (product_FromDB != null)
			{
				product_FromDB.Title = product.Title;
				product_FromDB.Description = product.Description;
				product_FromDB.CategoryId = product.CategoryId;
				product_FromDB.Price = product.Price;
				product_FromDB.ListPrice = product.ListPrice;
				product_FromDB.ISBN = product.ISBN;
				product_FromDB.Author = product.Author;
				product_FromDB.PriceFifty = product.PriceFifty;
				product_FromDB.PriceHundred = product.PriceHundred;

				if (product.ImageUrl != null)
				{
					product_FromDB.ImageUrl = product.ImageUrl;
				}
			}
			//_db.Products.Update(product);
		}
	}
}
