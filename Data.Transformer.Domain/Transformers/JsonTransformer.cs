using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Data.Transformer.Domain.Events;
using Data.Transformer.Domain.FileStorage;
using Data.Transformer.Domain.Models;
using Data.Transformer.Domain.Parsers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Data.Transformer.Domain.Transformers
{
    public class JsonTransformer : INotificationHandler<FileUploadedEvent>
    {
        private readonly IJsonJobStorage _jobStorage;
        private readonly ICsvParser<ProductDto> _productsCsvParser;
        private readonly IJsonFileStorageService _jsonFileStorageService;
        private readonly ILogger<JsonTransformer> _logger;
        public JsonTransformer(
            IJsonJobStorage jsonJobStorage, 
            ICsvParser<ProductDto> productsParser,
            IJsonFileStorageService jsonFileStorageService,
            ILogger<JsonTransformer> logger)
        {
            _jobStorage = jsonJobStorage;
            _productsCsvParser = productsParser;
            _jsonFileStorageService = jsonFileStorageService;
            _logger = logger;
        }
        public async Task Handle(FileUploadedEvent notification, CancellationToken cancellationToken)
        {

            _logger.LogInformation("JsonTransformer strated.");
            //Move the file from Src folder to Processing folder
            var file = _jobStorage.MoveFileToProcessing(notification.FileName);

            _logger.LogInformation($"Processing {file}");
            try
            {
                //Parse and serialize csv file
                var products = _productsCsvParser.Parse(file);
                var jsonString = JsonSerializer.Serialize(products,new JsonSerializerOptions { 
                    WriteIndented = true
                });

                //get path to destination folder
                string destination = _jobStorage.GetPathToProcessedFolder();
                string filename = $"{destination}\\{Guid.NewGuid()}.json";

                //store file as json in the destination folder
                await _jsonFileStorageService.StoreAsJson(filename, jsonString);
                                
                _jobStorage.RemoveFileFromProcessing(file.Substring(file.LastIndexOf("\\") + 1));
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed processing {file} , moving file to failed folder", e);

                string filename = file.Substring(file.LastIndexOf("\\") + 1);
                _jobStorage.MoveFileToFailed(filename);
            }
            


            
        }
    }
}
