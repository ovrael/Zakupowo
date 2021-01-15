using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopApp.Models
{
        //Nie stworzymy kodu do resetowania jeśli dane się nie zgadzają tj. Email musi być mailem i musi być powiązany z użytkownikiem
    public class PasswordReset
    {
        [Key]
        public int PasswordResetID { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordResetCode { get; set; }
        public DateTime CodeCreationTime { get; set; }
        public DateTime CodeExpirationTime { get; set; }
        public int TriesCount { get; set; }
        public bool Used { get; set; }

        public PasswordReset()
        {
        
        }

        public PasswordReset(string EmailAddress, string PasswordResetCode)
        {
            this.EmailAddress = EmailAddress;
            CodeCreationTime = DateTime.UtcNow;
            CodeExpirationTime = CodeCreationTime.AddMinutes(10);
            this.PasswordResetCode = PasswordResetCode;   
        }
    }
}