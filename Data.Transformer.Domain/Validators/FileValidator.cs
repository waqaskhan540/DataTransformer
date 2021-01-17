using Data.Transformer.Domain.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.Validators
{
    public class FileValidator : IFileValidator
    {
       

        private readonly DataTransformerConfiguration _config;
        public FileValidator(IOptions<DataTransformerConfiguration> config)
        {
            _config = config.Value;
        }
        public (bool IsValid, string Extension, string Message) Validate(IFormFile file)
        {
            bool isValid = false;
            string errorMessage = string.Empty;

            if(file == null)
            {
                errorMessage = "Unable to upload file : file not found.";
                return (isValid, null, errorMessage);
            }

            var allowedExtensions = _config.AllowedExtensions.Split(",");
            var maxFileSizeInMB = _config.MaxFileSize;

            var extension = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLowerInvariant();            
            if (!allowedExtensions.Contains(extension))
            {
                errorMessage = "Unable to upload file : File type not supported.";
            }else if((file.Length/ (1024 * 1024) > maxFileSizeInMB))
            {
                errorMessage = $"Unable to upload file : File size exceeds allowed limit of {maxFileSizeInMB} MBs.";
            }else 
            {
                isValid = true;
            }

            return (isValid, extension, errorMessage);
        }
    }
}
