using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopApp.Models
{
    public class Bucket
    {
        [ForeignKey("User")]
        public int BucketID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Bundle> Bundles { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
    }
}