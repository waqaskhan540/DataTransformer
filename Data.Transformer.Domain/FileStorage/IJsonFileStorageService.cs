using Data.Transformer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.FileStorage
{
    public interface IJsonFileStorageService
    {
        Task StoreAsJson(string filename, string jsonString);
    }
}
