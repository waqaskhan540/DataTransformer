using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Persistence.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        public string Key { get; set; }
        public string ArtikelCode { get; set; }
        public string ColorCode { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float DiscountPrice { get; set; }
        public string DeliveredIn { get; set; }
        public string Q1 { get; set; }
        public string Size { get; set; }

        public string Color { get; set; }

    }
}



