using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using ShopApp.DAL;
namespace ShopApp.Models
{
    public class Category : IConcurrencyAwareEntity
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string CategoryDescription { get; set; }

        [JsonIgnore]
        public virtual ICollection<Offer> Offers { get; set; }
        public byte[] RowVersion { get ; set ; }
    }
}