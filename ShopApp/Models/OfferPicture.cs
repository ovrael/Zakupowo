using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ShopApp.DAL;

namespace ShopApp.Models
{
    public class OfferPicture : IConcurrencyAwareEntity
    {
        [Key]
        public int OfferImageID { get; set; }

        [Column("PathToFile", TypeName = "nvarchar")]
        public string PathToFile { get; set; }

        [Column("OfferID")]
        public virtual Offer Offer { get; set; }
        public byte[] RowVersion { get ; set ; }
    }
}