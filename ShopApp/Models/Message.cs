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
    public class Message : IConcurrencyAwareEntity, IComparable<Message>
    {
        [Key]
        [Column("MessageID", Order = 1)]
        public int MessageID { get; set; }
        //public int SenderID { get; set; }
        //public int ReceiverID { get; set; }

        //[Column("SenderID", Order = 2)]
        //[ForeignKey("SenderID")]
        public virtual User Sender { get; set; }

        //[Column("ReceiverID", Order = 3)]
        //[ForeignKey("ReceiverID")]
        public virtual User Receiver { get; set; }

        [Column("Content", Order = 4, TypeName = "nvarchar")]
        public string Content { get; set; }

        [Column("SentTime", Order = 5, TypeName = "DateTime2")]
        public DateTime SentTime { get; set; }

        public bool IsRead { get; set; }

        public byte[] RowVersion { get; set; }

        public int CompareTo(Message other)
        {
            if (other == null)
                return 1;

            else
                return this.SentTime.CompareTo(other.SentTime) * -1;
        }

        public override string ToString()
        {
            return "\nMessageID: " + MessageID + "\nSender: " + Sender.Login + "\nReceiver: " + Receiver.Login + "\nContent: " + Content;
        }

    }
}