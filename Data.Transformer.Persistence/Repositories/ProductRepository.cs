using Data.Transformer.Persistence.Context;
using Data.Transformer.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _dbContext;

        public ProductRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddProductsAsync(IEnumerable<Product> products)
        {
            //using var transaction = _dbContext.Database.BeginTransaction();
            //try
            //{
                await _dbContext.Products.AddRangeAsync(products);
                //await _dbContext.SaveChangesAsync();
            //    await transaction.CommitAsync();                
            //}
            //catch (Exception e)
            //{
            //    await transaction.RollbackAsync();
            //    throw;
            //}

        }

        public async Task<bool> AnyAsync()
        {
            return await _dbContext.Products.AnyAsync();
        }

        public IQueryable<Product> Get()
        {
            return _dbContext.Products;
        }
    }
}
