using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ShopApp.DAL;

namespace ShopApp.Models
{
    public class Order : IConcurrencyAwareEntity
    {
        [ForeignKey("User")]
        public int OrderID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BucketItem> BucketItems { get; set;}
        [JsonIgnore]
        public byte[] RowVersion { get; set; }
    }
}