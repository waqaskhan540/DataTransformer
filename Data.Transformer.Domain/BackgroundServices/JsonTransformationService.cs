using Data.Transformer.Domain.FileStorage;
using Data.Transformer.Domain.Models;
using Data.Transformer.Domain.Parsers;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.BackgroundServices
{
    public class JsonTransformationService : IJsonTransformationService
    {

        private readonly IJsonJobStorage _jobStorage;
        private readonly ICsvParser<ProductDto> _productsCsvParser;
        private readonly IJsonFileStorageService _jsonFileStorageService;
        public JsonTransformationService(
            IJsonJobStorage jsonJobStorage,
            ICsvParser<ProductDto> productsParser,
            IJsonFileStorageService jsonFileStorageService)
        {
            _jobStorage = jsonJobStorage;
            _productsCsvParser = productsParser;
            _jsonFileStorageService = jsonFileStorageService;
        }

        public async Task Start()
        {
            var files = _jobStorage.GetFilesToProcess();

            foreach(var file in files)
            {
                await ProcessFile(file);
            }
        }

        private async Task ProcessFile(string fileItem)
        {
            //Move the file from Src folder to Processing folder
            var file = _jobStorage.MoveFileToProcessing(fileItem);
            try
            {
                //Parse and serialize csv file
                var products = _productsCsvParser.Parse(file);
                var jsonString = JsonSerializer.Serialize(products, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                //get path to destination folder
                string destination = _jobStorage.GetPathToProcessedFolder();
                string filename = $"{destination}\\{Guid.NewGuid()}.json";

                //store file as json in the destination folder
                await _jsonFileStorageService.StoreAsJson(filename, jsonString);
                _jobStorage.RemoveFileFromProcessing(file.Substring(file.LastIndexOf("\\") + 1));
            }
            catch (Exception)
            {
                string filename = file.Substring(file.LastIndexOf("\\") + 1);
                _jobStorage.MoveFileToFailed(filename);
            }
        }
    }
}
