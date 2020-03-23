using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakerDrop.Entities
{
    public class Product : BaseEntity
    {
        
        public decimal Price { get; set; }

        public int CategoryID { get; set; }
        public string ImageURL { get; set; }
        public virtual Category Category { get; set; }
    }
}
