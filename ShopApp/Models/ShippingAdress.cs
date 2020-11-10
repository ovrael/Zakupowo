using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopApp.Models
{
    public class ShippingAdress
    {
        public static string ErrorMessage { get; set; }

        [Key]
        public int AdressID { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [MaxLength(75)]
        [Column("Country", TypeName = "char")]
        public string Country { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [MaxLength(75)]
        [Column("City", TypeName = "char")]
        public string City { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [MaxLength(75)]
        [Column("Street", TypeName = "char")]
        public string Street { get; set; }

        [MaxLength(10)]
        [Column("FlatNumber", TypeName = "char")]
        public string FlatNumber { get; set; }

        [MaxLength(10)]
        [Column("PremisesNumber", TypeName = "char")]
        public string PremisesNumber { get; set; }

        [MaxLength(15)]
        [Column("PostalCode", TypeName = "char")]
        public string PostalCode { get; set; }

        [Required]
        [Column("UserID")]
        public virtual User User { get; set; }
    }
}