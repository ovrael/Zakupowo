using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ShopApp.DAL;

namespace ShopApp.Models
{
    public class AvatarImage : IConcurrencyAwareEntity
    {
        [ForeignKey("User")]
        public int AvatarImageID { get; set; }

        [Column("PathToFile", TypeName = "nvarchar")]
        public string PathToFile { get; set; }

        [Column("UserID")]
        public virtual User User { get; set; }
        [JsonIgnore]
        public byte[] RowVersion { get; set; }
    }
}