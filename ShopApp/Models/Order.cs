using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ShopApp.DAL;

namespace ShopApp.Models
{
    public class Order : IConcurrencyAwareEntity
    {
        [ForeignKey("Owner")]
        public int OrderID { get; set; }
        public virtual ICollection<BucketItem> BucketItems { get; set;}
        public virtual User Owner { get; set; }
        public byte[] RowVersion { get; set; }
    }
}