using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ShopApp.DAL
{
    public class ShopContext : DbContext
    {
        public ShopContext() : base("Shop") { }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}