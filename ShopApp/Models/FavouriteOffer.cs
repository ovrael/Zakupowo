using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopApp.Models
{
    public class FavouriteOffer
    {
        [Key]
        public int FavouriteOfferID { get; set; }

        [Column("UserID")]
        public virtual User User { get; set; }

        [Column("OfferID")]
        public virtual Offer Offer { get; set; }
    }
}