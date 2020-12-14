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
    public class Message : IConcurrencyAwareEntity
    {
        [Key]
        public int MessageID { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }

        [Column("Content", TypeName = "nvarchar")]
        public string Content { get; set; }


        [Column("SentTime", TypeName = "DateTime2")]
        public DateTime SentTime { get; set; }

        public byte[] RowVersion { get; set; }
    }
}