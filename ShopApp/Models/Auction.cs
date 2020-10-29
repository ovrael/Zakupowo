using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopApp.Models
{
    public class Auction
    {
        [Key]
        public int AuctionID {get; set;}
        [ForeignKey("User")]
        public virtual User Bidder { get; set; }
        [ForeignKey("User")]
        public virtual User Winner { get; set; }
        public double Price { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}