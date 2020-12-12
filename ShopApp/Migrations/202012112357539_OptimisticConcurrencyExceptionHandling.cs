namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptimisticConcurrencyExceptionHandling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Auction", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.User", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.AvatarImage", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Bucket", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.BucketItem", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Offer", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Bundle", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Favourite", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Category", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.OfferPicture", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.ShippingAdress", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Transaction", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "RowVersion");
            DropColumn("dbo.ShippingAdress", "RowVersion");
            DropColumn("dbo.OfferPicture", "RowVersion");
            DropColumn("dbo.Category", "RowVersion");
            DropColumn("dbo.Favourite", "RowVersion");
            DropColumn("dbo.Bundle", "RowVersion");
            DropColumn("dbo.Offer", "RowVersion");
            DropColumn("dbo.BucketItem", "RowVersion");
            DropColumn("dbo.Bucket", "RowVersion");
            DropColumn("dbo.AvatarImage", "RowVersion");
            DropColumn("dbo.User", "RowVersion");
            DropColumn("dbo.Auction", "RowVersion");
        }
    }
}
