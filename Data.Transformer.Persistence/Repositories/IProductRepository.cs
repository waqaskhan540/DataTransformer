using Data.Transformer.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Persistence.Repositories
{
    public interface IProductRepository
    {
        Task AddProductsAsync(IEnumerable<Product> products);
        Task<bool> AnyAsync();

        IQueryable<Product> Get();
    }
}
