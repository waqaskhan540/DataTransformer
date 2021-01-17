using AutoMapper;
using Data.Transformer.Domain.Models;
using Data.Transformer.Persistence.Entities;
using Data.Transformer.Persistence.Repositories;
using Data.Transformer.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(
            IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task Insert(List<ProductDto> products)
        {
            //create entity objects
            var entities = _mapper.Map<List<Product>>(products);


            //get only unique records with unique keys
            var keys = entities.Select(x => x.Key).Distinct().ToArray();

            //get existing keys
            var existing_product_keys = _productRepository.Get()
                                            .Where(p => keys.Contains(p.Key))
                                            .Select(x => x.Key)
                                            .ToArray();

            //ignore duplicate records
            var product_entities = entities
                                    .Where(p => !existing_product_keys.Contains(p.Key))
                                    .ToList();

            //insert records in Db
            if (product_entities.Any())
            {
                using (var transaction = _unitOfWork.Database.BeginTransaction())
                {
                    try
                    {
                        await _productRepository.AddProductsAsync(product_entities);
                        await _unitOfWork.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }



        }


    }
}

