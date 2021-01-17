using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.Configuration
{
    public class DataTransformerConfiguration
    {                      
        public long MaxFileSize { get; set; }
        public string AllowedExtensions { get; set; }
    }
}
