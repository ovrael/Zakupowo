//using Microsoft.AspNet.Identity.EntityFramework;
using System;
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

        public static string ErrorMessage { get; set; }

        //PO 1 SPRINCIE TESTUJEMY "varchar(200)"/ "text"

        [Key]
        public int UserID { get; set; }

        [Required (ErrorMessage="To pole jest wymagane")]
        [MaxLength(50)]
        [Column("Email", TypeName = "char")]
        public string Email { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Column("Login", TypeName = "char")]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
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

        [Column("Phone", TypeName = "char")]
        [MaxLength(12)]
        public string Phone { get; set; }

        [Column("Country", TypeName = "char")]
        [MaxLength(50)]
        public string Country { get; set; }

        [Column("City", TypeName = "char")]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [Column("BirthDate", TypeName = "datetime2")]
        public DateTime BirthDate { get; set; }

        [Column("CreationDate", TypeName = "datetime2")]
        public DateTime CreationDate { get; set; }

        public virtual Bucket Bucket { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Auction> Auction { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
        public virtual ICollection<Bundle> Bundles { get; set; }

        public string showBasicInformation()
        {
            string name = "Full name: " + FirstName + " " + LastName;
            string email = "E-mail: " + Email;
            string login = "Login: " + Login;

            return name + " " + login + " " + email;
        }

    }
}
