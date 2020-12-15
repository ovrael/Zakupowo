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
        [ForeignKey("Sender")]
        [Column("ChatID", Order = 1)]
        public int ChatID { get; set; }

        [Column("SenderID", Order = 2)]
        public User Sender { get; set; }

        [Column("ReceiverID", Order = 3)]
        public User Receiver { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public byte[] RowVersion { get; set; }

        //public override string ToString()
        //{
        //    return "\nSender: " + SenderID + "\nReceiver: " + ReceiverID + "\tMesseges count: " + Messages.Count;
        //}
    }
}