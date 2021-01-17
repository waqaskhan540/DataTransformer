using Data.Transformer.Domain.Configuration;
using Data.Transformer.Domain.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Data.Transformer.Domain.FileStorage
{
    public class JsonFileStorageService : IJsonFileStorageService
    {
       
        public async Task StoreAsJson(string filename, string jsonString)
        {
            await File.AppendAllTextAsync(filename, jsonString);
        }
    }
}
