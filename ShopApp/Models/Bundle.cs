using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ShopApp.DAL;
namespace ShopApp.Models
{
	public class Bundle : IConcurrencyAwareEntity
	{
		[Key]
		public int BundleID { get; set; }

		[Required]
		public double OffersPriceSum { get; set; }

		[Required]
		public double BundlePrice { get; set; }

		[Required]
		[Column("Title", TypeName = "nvarchar")]
		[MaxLength(400)]
		public string Title { get; set; }

		[Column("CreationDate", TypeName = "DateTime2")]
		public DateTime CreationDate { get; set; }

		[Required]
		public bool IsActive { get; set; }

		public virtual User User { get; set; }

		public virtual BundlePicture BundlePicture { get; set; }
		public virtual ICollection<Offer> Offers { get; set; }
		public virtual ICollection<BucketItem> BucketItems { get; set; }
		public virtual ICollection<Favourite> Favourites { get; set; }
		public byte[] RowVersion { get; set; }
	}
}