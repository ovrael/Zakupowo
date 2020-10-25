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
        //Stocking should be an ENUM
        public string Stocking { get; set; }
        [Required]
        public double InStock { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        //[ForeignKey("OfferID")]
        public virtual User User { get; set; }
        public virtual Bundle Bundle { get; set; }
    }
}