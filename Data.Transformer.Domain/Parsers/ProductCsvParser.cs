using CsvHelper;
using Data.Transformer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.Parsers
{
    public class ProductCsvParser : ICsvParser<ProductDto>
    {
        public IEnumerable<ProductDto> Parse(string file)
        {
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<ProductDto>();
                return records.ToList();
            }
        }

        public IEnumerable<IEnumerable<ProductDto>> ParseInBatches(string file, int batchSize)
        {
            int rowCount = 0;
            var products = new List<ProductDto>();

            using (var sr = new StreamReader(file))
            {
                string line = null;
                sr.ReadLine();//ignore header

                while ((line = sr.ReadLine()) != null)
                {                    
                    rowCount++;
                    
                    var columns = line.Split(',');
                    var product = new ProductDto
                    {
                        Key = columns[0],
                        ArtikelCode = columns[1],
                        ColorCode = columns[2],
                        Description = columns[3],
                        Price = float.Parse(columns[4]),
                        DiscountPrice = float.Parse(columns[5]),
                        DeliveredIn = columns[6],
                        Q1 = columns[7],
                        Size = columns[8],
                        Color = columns[9]
                    };
                    products.Add(product);

                    if (rowCount == batchSize)
                    {
                        rowCount = 0;
                        yield return products;
                        products = new List<ProductDto>();
                    }
                }
            }
            
            if (products != null)
                yield return products;


        }
    }
}
