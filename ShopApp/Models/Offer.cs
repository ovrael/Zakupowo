using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopApp.Models
{
    public class Offer
    {
        [Key]
        public int OfferID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        //Stocking should be an ENUM

        [Column("CreationDate", TypeName = "DateTime2")]
        public DateTime CreationDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string Stocking { get; set; }
        [Required]
        public double InStockOriginaly { get; set; }
        [Required]
        public double InStockNow { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual Bundle Bundle { get; set; }
        public virtual ICollection<BucketItem> BucketItems { get; set; }
        public virtual ICollection<FavouriteOffer> FavouriteOffer { get; set; }
        public virtual ICollection<OfferPicture> OfferPictures { get; set; }
    }
}