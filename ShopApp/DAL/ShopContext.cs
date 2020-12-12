using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.AccessControl;

namespace ShopApp.DAL
{
    public class ShopContext : DbContext
    {
        public ShopContext() : base("ZakupowoDatabase") { }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Bundle> Bundles { get; set; }
        public virtual DbSet<BucketItem> BucketItems { get; set; }
        public virtual DbSet<Bucket> Buckets { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Favourite> Favourites { get; set; }
        public virtual DbSet<ShippingAdress> ShippingAdresses { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<ShopApp.Models.AvatarImage> AvatarImages { get; set; }
    }
}