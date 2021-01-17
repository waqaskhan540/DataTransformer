using Data.Transformer.Domain.Configuration;
using Data.Transformer.Domain.FileStorage;
using Data.Transformer.Domain.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileValidator _fileValidator;
        private readonly IJsonJobStorage _jsonJobStorage;
        private readonly IDbJobStorage _dbJobStorage;        
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(
            IFileValidator fileValidator, 
            IJsonJobStorage jsonJobStorage,
            IDbJobStorage dbJobStorage,
            IOptions<DataTransformerConfiguration> config,
            ILogger<FileUploadService> logger)
        {
            _fileValidator = fileValidator;
            _jsonJobStorage = jsonJobStorage;
            _dbJobStorage = dbJobStorage;            
            _logger = logger;
        }
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            _logger.LogInformation($"uploading file {file.FileName} to storage locations");

            var (isValid, extension, message) = _fileValidator.Validate(file);
            if (!isValid)
            {
                throw new ArgumentException(message);
            }

            var fileIdentity = $"{Guid.NewGuid()}{extension}";

            //store file at SourcePath for json tranformation
            await _jsonJobStorage.StoreFile(file, fileIdentity);

            //store file at SourcePath for DB transformation
            await _dbJobStorage.StoreFile(file, fileIdentity);

            _logger.LogInformation($"file {file.FileName} uploaded to storage locations with identity {fileIdentity}");

            return fileIdentity;
        }
               
    }
}
