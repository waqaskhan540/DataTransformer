using Data.Transformer.Domain.Events;
using Data.Transformer.Domain.FileStorage;
using Data.Transformer.Domain.Models;
using Data.Transformer.Domain.Parsers;
using Data.Transformer.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.Transformers
{
    public class DbTransformer : INotificationHandler<FileUploadedEvent>
    {
        private readonly IDbJobStorage _jobStorage;
        private readonly ICsvParser<ProductDto> _productsCsvParser;
        private readonly IProductService _productService;
        private readonly ILogger<DbTransformer> _logger;
        public DbTransformer(
            IDbJobStorage jsonJobStorage,
            ICsvParser<ProductDto> productsParser,
            IProductService productService,
            ILogger<DbTransformer> logger)
        {
            _jobStorage = jsonJobStorage;
            _productsCsvParser = productsParser;
            _productService = productService;
            _logger = logger;
        }
        public async Task Handle(FileUploadedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Db Transformer started.");

            //Move the file from Src folder to Processing folder
            var file = _jobStorage.MoveFileToProcessing(notification.FileName);

            _logger.LogInformation($"Db Transformer Processing {file}");

            try
            {                
                //Parse and serialize csv file
                var productsList = _productsCsvParser.ParseInBatches(file, batchSize: 1000);

                //insert in database
                foreach(var products in productsList)
                    await _productService.Insert(products.ToList());

                //remove file from processing
                string filename = file.Substring(file.LastIndexOf("\\") + 1);
                _jobStorage.RemoveFileFromProcessing(filename);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed processing {file} , moving file to failed folder",e);

                string filename = file.Substring(file.LastIndexOf("\\") + 1);
                _jobStorage.MoveFileToFailed(filename);                
            }
          
        }
    }
}
