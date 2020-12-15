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
    public class Chat : IConcurrencyAwareEntity
    {
        [Key]
        public int ChatID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }

        [ForeignKey("SenderID")]
        public virtual User Sender { get; set; }

        [ForeignKey("ReceiverID")]
        public virtual User Receiver { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public byte[] RowVersion { get; set; }
    }
}