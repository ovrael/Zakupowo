using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ShopApp.DAL;

namespace ShopApp.Models
{
    public class Transaction : IConcurrencyAwareEntity
    {
        [Key]
        public int TransactionID { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsChosen { get; set; }
        public virtual User Buyer { get; set; }
        public virtual User Seller { get; set; }
        public virtual ICollection<BucketItem> BucketItems { get; set; }
        public byte[] RowVersion { get; set; }
    }
}