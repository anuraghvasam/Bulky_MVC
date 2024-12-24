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
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		public readonly AppDBContext _db;
		public CategoryRepository(AppDBContext db) : base(db)
		{
			_db = db;
		}


		public void Update(Category category)
		{
			_db.Catogories.Update(category);
		}
	}
}
