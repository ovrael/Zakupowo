using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ShopApp.DAL;

namespace ShopApp.Models
{
    public class ShippingAdress : IConcurrencyAwareEntity
    {
        public static string ErrorMessage { get; set; }

        [Key]
        public int AdressID { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [MaxLength(75)]
        [Column("Country", TypeName = "nvarchar")]
        public string Country { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [MaxLength(75)]
        [Column("City", TypeName = "nvarchar")]
        public string City { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [MaxLength(75)]
        [Column("Street", TypeName = "nvarchar")]
        public string Street { get; set; }

        [MaxLength(10)]
        [Column("PremisesNumber", TypeName = "nvarchar")]
        public string PremisesNumber { get; set; }

        [MaxLength(15)]
        [Column("PostalCode", TypeName = "nvarchar")]
        public string PostalCode { get; set; }

        [Required]
        [Column("UserID")]
        public virtual User User { get; set; }
        public byte[] RowVersion { get ; set ; }
    }
}