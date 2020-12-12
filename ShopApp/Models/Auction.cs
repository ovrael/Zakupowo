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
        [Required]
        public double Price { get; set; }
        [Required]
        [Column("Title", TypeName = "char")]
        [MaxLength(400)]
        public string Title { get; set; }
        [Required]
        public string Descriptioon { get; set; }
        [Required]
        public virtual ICollection<User> Bidder { get; set; }
        [Required]
        public virtual User Winner { get; set; }
        [Column("StartDate", TypeName = "datetime2")]
        public DateTime StartDate { get; set; }
        [Column("EndDate", TypeName = "datetime2")]
        public DateTime EndDate { get; set; }
    }
}