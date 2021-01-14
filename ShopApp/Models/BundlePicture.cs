using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ShopApp.DAL;

namespace ShopApp.Models
{
	public class BundlePicture : IConcurrencyAwareEntity
	{
		[ForeignKey("Bundle")]
		public int BundlePictureID { get; set; }

		[Column("PathToFile", TypeName = "nvarchar")]
		public string PathToFile { get; set; }

		[Column("BundleID")]
		public virtual Bundle Bundle { get; set; }
		public byte[] RowVersion { get; set; }
	}
}