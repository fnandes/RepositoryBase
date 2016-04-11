using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryBase.Tests
{
    public class ProductRepository : Dlp.Buy4.RepositoryBase.EntityFramework.RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(DbContext context)
            : base(context)
        {
        }
    }
}
