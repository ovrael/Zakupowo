using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace ShopApp.Models
{
    public enum CategoryEnum
    {
        Elektronika = 1,
        ModaIUroda = 2,
        Meble = 3,
        Pozostale = 4,
        SlawekPo2Piwach = 5
    }
    public class Category
    { 
        [Key]
        public int CategoryID { get; set; }
        [Required]
        public CategoryEnum CategoryName { get; set; }
        [Required]
        public string CategoryDescription { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }

    }
}