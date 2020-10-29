﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Required]
        [Column("FirstName", TypeName = "char")]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [Column("LastName", TypeName = "char")]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [Column("Phone", TypeName = "char")]
        [MaxLength(12)]
        public string Phone { get; set; }
        [Required]
        [Column("Country", TypeName = "char")]
        [MaxLength(50)]
        public string Country { get; set; }
        [Required]
        [Column("City", TypeName = "char")]
        [MaxLength(50)]
        public string City { get; set; }
        [Column("BirthDate", TypeName = "datetime2")]
        public DateTime BirthDate { get; set; }
        [Column("CreationDate", TypeName = "datetime2")]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public virtual Bucket Bucket { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Auction> Auction { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
        public virtual ICollection<Bundle> Bundles { get; set; }
    }
}