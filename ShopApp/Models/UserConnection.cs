using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ShopApp.DAL;

namespace ShopApp.Models
{
    public class UserConnection : IConcurrencyAwareEntity
    {
        public int UserConnectionID { get; set; }
        public string UserName { get; set; }
        public string ConnectionID { get; set; }

        public byte[] RowVersion { get; set; }
    }
}