using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ShopApp.DAL;
namespace ShopApp.Models
{
	public class BucketItem : IConcurrencyAwareEntity
	{
		public int BucketItemID { get; set; }
		public int Quantity { get; set; }
		public double TotalPrice { get; set; }
		public bool IsChosen { get; set; }
		public virtual Offer Offer { get; set; }
		public virtual Bucket Bucket { get; set; }
        public byte[] RowVersion { get ; set ; }
    }
}