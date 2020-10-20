using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopApp.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}