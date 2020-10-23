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
    
        public int UserID { get; set; }
        [DisplayName("Imię")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        public string FirstName { get; set; }
        [DisplayName("Nazwisko")]
       
        public string LastName { get; set; }
        [DisplayName("Hasło")]
      
        public string Password { get; set; }
       
        public string Email { get; set; }
        [DisplayName("Zdjęcie")]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageUpload {get; set;}

        public User()
        {
            ImagePath = "~/AppFiles/Images/image.jpg";
        }
        public virtual ICollection<Offer> Offers { get; set; }
    }
}