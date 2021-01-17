using Data.Transformer.Domain.FileStorage;
using Data.Transformer.Domain.Models;
using Data.Transformer.Domain.Parsers;
using Data.Transformer.Domain.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.BackgroundServices
{
    public class SqlTransformationService : ISqlTransformationService
    {
        private readonly IDbJobStorage _jobStorage;
        private readonly ICsvParser<ProductDto> _productsCsvParser;
        private readonly IProductService _productService;
        public SqlTransformationService(
            IDbJobStorage jsonJobStorage,
            ICsvParser<ProductDto> productsParser,
            IProductService productService)
        {
            _jobStorage = jsonJobStorage;
            _productsCsvParser = productsParser;
            _productService = productService;
        }
       
        public async Task Start()
        {
            var files = _jobStorage.GetFilesToProcess();
            foreach (var file in files)
            {
                await ProcessFile(file);
            }
            
        }

        private async Task ProcessFile(string fiteItem)
        {
            //Move the file from Src folder to Processing folder
            var file = _jobStorage.MoveFileToProcessing(fiteItem);

            try { 
                //Parse and serialize csv file
                var productsList = _productsCsvParser.ParseInBatches(file, batchSize: 5000);

                //insert in database
                foreach (var products in productsList)
                    await _productService.Insert(products.ToList());

                //remove file from processing
                string filename = file.Substring(file.LastIndexOf("\\") + 1);
                _jobStorage.RemoveFileFromProcessing(filename);
            }
            catch (Exception)
            {
                //move to failed folder to be picked up later
                string filename = file.Substring(file.LastIndexOf("\\") + 1);
                _jobStorage.MoveFileToFailed(filename);
            }
        }
    }


}
