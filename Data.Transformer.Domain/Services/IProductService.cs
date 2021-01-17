using Data.Transformer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.Services
{
    public interface IProductService
    {
        Task Insert(List<ProductDto> products);
    }
}
