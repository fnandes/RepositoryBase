using FactoryAlienDotNet;
using Moq;
using Dlp.Buy4.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Data.Entity.Infrastructure;

namespace RepositoryBase.Tests
{
    public class RepositoryTests
    {
        IProductRepository _repository;
        Mock<DbContext> _dbContextMock;
        Mock<DbSet<Product>> _productDbSetMock;
        IFactory<Product> _productFactory;

        public RepositoryTests()
        {
            _dbContextMock = new Mock<DbContext>();
            _productDbSetMock = new Mock<DbSet<Product>>();

            _repository = new ProductRepository(_dbContextMock.Object);
            _productFactory = FactoryAlien.Define<Product>();
        }

        [Fact]
        public void GetAll_ReturnsAllObjectsFromStore()
        {
        }

    }
}
