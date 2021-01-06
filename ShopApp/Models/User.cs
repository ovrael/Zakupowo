//using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ShopApp.DAL;

namespace ShopApp.Models
{
    public class User : IConcurrencyAwareEntity
    {
        public static string ErrorMessage { get; set; }

        //PO 1 SPRINCIE TESTUJEMY "varchar(200)"/ "text"

        [Key]
        public int UserID { get; set; }

        [Column("IsActive")]
        public bool IsActivated { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [MaxLength(50)]
        [Column("Email", TypeName = "nvarchar")]
        public string Email { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Column("Login", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Column("EncryptedPassword", TypeName = "nvarchar")]
        [MaxLength(200)]
        [JsonIgnore]
        public string EncryptedPassword { get; set; }

        [Required]
        [Column("FirstName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Column("LastName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Column("Phone", TypeName = "nvarchar")]
        [MaxLength(12)]
        public string Phone { get; set; }

        [Required]
        [Column("BirthDate", TypeName = "DateTime2")]
        public DateTime BirthDate { get; set; }


        [Column("CreationDate", TypeName = "DateTime2")]
        public DateTime CreationDate { get; set; }


        public virtual Bucket Bucket { get; set; }
        public virtual AvatarImage AvatarImage { get; set; }

        public virtual Order Order { get; set; }
        [JsonIgnore]
        public virtual ICollection<Auction> Auction { get; set; }
        [JsonIgnore]
        public virtual ICollection<Offer> Offers { get; set; }
       
        public virtual ICollection<Favourite> FavouriteOffer { get; set; }
        [JsonIgnore]
        public virtual ICollection<Bundle> Bundles { get; set; }

        public virtual ICollection<ShippingAdress> ShippingAdresses { get; set; }
        [JsonIgnore]
        public virtual ICollection<Transaction> Transactions { get; set; }

        [JsonIgnore]
        [InverseProperty("Sender")]
        public virtual ICollection<Message> SentMessages { get; set; }

        [InverseProperty("Receiver")]
        [JsonIgnore]
        public virtual ICollection<Message> ReceivedMessages { get; set; }

        [JsonIgnore]
        public byte[] RowVersion { get; set; }

        public string ShowBasicInformation()
        {
            string name = "Full name: " + FirstName + " " + LastName;
            string email = "E-mail: " + Email;
            string login = "Login: " + Login;
            string phone = "Phone: " + Phone;

            return name + " " + login + " " + email + " " + phone;
        }

   
        public List<Message> AllMesseges()
        {
            List<Message> messeges = new List<Message>();
            messeges.AddRange(SentMessages);
            messeges.AddRange(ReceivedMessages);

            messeges.Sort();

            return messeges;
        }
    }
}
