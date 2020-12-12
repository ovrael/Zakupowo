using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopApp.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public virtual User Buyer { get; set; }
        public virtual User Seller { get; set; }
        public virtual ICollection<BucketItem> BucketItems { get; set; }
 
    }
}