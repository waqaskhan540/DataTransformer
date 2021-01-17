using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.Validators
{
    public interface IFileValidator
    {
        (bool IsValid, string Extension, string Message) Validate(IFormFile file);
    }
}
