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
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}