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
    public class UnitOfWork(AppDBContext db) : IUnitOfWork
    {

        public readonly AppDBContext _db = db;
        public ICategoryRepository Category { get; private set; } = new CategoryRepository(db);

        public IProductRepository Product { get; private set; } = new ProductRepository(db);
        public ICompanyRepository Company { get; private set; } = new CompanyRepository(db);
        public IShoppingCartRepository ShoppingCart { get; private set; } = new ShoppingCartRepository(db);

        public IOrderDetailRepository OrderDetail { get; private set; } = new OrderDetailRepository(db);
        //public IOrderHeaderRepository OrderHedaer { get; private set; } = new OrderHeaderRepository(db);

        public IOrderHeaderRepository OrderHeader { get; private set; } = new OrderHeaderRepository(db);

        public IApplicationUserRepository ApplicationUser { get; private set; } = new ApplicationUserRepository(db);

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
