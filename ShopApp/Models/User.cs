using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopApp.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Column("Email", TypeName = "char")]
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }
        [Required]
        [Column("Login", TypeName = "char")]
        [MaxLength(50)]
        public string Login { get; set; }
        [Required]
        [Column("EncryptedPassword", TypeName = "char")]
        [MaxLength(200)]
        public string EncryptedPassword { get; set; }
        //[ForeignKey("UserID")]
        public virtual Bucket Bucket { get; set; }
        public virtual ICollection<Auction> Auction { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
        public virtual ICollection<Bundle> Bundles { get; set; }
    }
}