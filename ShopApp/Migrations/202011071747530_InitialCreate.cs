namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auction",
                c => new
                    {
                        AuctionID = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        Title = c.String(nullable: false, maxLength: 400, fixedLength: true, unicode: false),
                        Descriptioon = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        User_UserID = c.Int(),
                        Winner_UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AuctionID)
                .ForeignKey("dbo.User", t => t.User_UserID)
                .ForeignKey("dbo.User", t => t.Winner_UserID, cascadeDelete: true)
                .Index(t => t.User_UserID)
                .Index(t => t.Winner_UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50, fixedLength: true, unicode: false),
                        Login = c.String(nullable: false, maxLength: 50, fixedLength: true, unicode: false),
                        EncryptedPassword = c.String(nullable: false, maxLength: 200, fixedLength: true, unicode: false),
                        FirstName = c.String(nullable: false, maxLength: 50, fixedLength: true, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 50, fixedLength: true, unicode: false),
                        Phone = c.String(maxLength: 12, fixedLength: true, unicode: false),
                        Country = c.String(maxLength: 50, fixedLength: true, unicode: false),
                        City = c.String(maxLength: 50, fixedLength: true, unicode: false),
                        BirthDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Auction_AuctionID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Auction", t => t.Auction_AuctionID)
                .Index(t => t.Auction_AuctionID);
            
            CreateTable(
                "dbo.Bucket",
                c => new
                    {
                        BucketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BucketID)
                .ForeignKey("dbo.User", t => t.BucketID)
                .Index(t => t.BucketID);
            
            CreateTable(
                "dbo.Bundle",
                c => new
                    {
                        BundleID = c.Int(nullable: false, identity: true),
                        BundlePriceSum = c.Double(nullable: false),
                        Title = c.String(nullable: false, maxLength: 400, fixedLength: true, unicode: false),
                        User_UserID = c.Int(),
                        Bucket_BucketID = c.Int(),
                    })
                .PrimaryKey(t => t.BundleID)
                .ForeignKey("dbo.User", t => t.User_UserID)
                .ForeignKey("dbo.Bucket", t => t.Bucket_BucketID)
                .Index(t => t.User_UserID)
                .Index(t => t.Bucket_BucketID);
            
            CreateTable(
                "dbo.Offer",
                c => new
                    {
                        OfferID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Stocking = c.String(),
                        InStock = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                        Bucket_BucketID = c.Int(),
                        Bundle_BundleID = c.Int(),
                        User_UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OfferID)
                .ForeignKey("dbo.Bucket", t => t.Bucket_BucketID)
                .ForeignKey("dbo.Bundle", t => t.Bundle_BundleID)
                .ForeignKey("dbo.User", t => t.User_UserID, cascadeDelete: true)
                .Index(t => t.Bucket_BucketID)
                .Index(t => t.Bundle_BundleID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.Int(nullable: false),
                        CategoryDescription = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        TransactionID = c.Int(nullable: false, identity: true),
                        Buyer_UserID = c.Int(),
                        Seller_UserID = c.Int(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionID)
                .ForeignKey("dbo.User", t => t.Buyer_UserID)
                .ForeignKey("dbo.User", t => t.Seller_UserID)
                .ForeignKey("dbo.User", t => t.User_UserID)
                .Index(t => t.Buyer_UserID)
                .Index(t => t.Seller_UserID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.CategoryOffer",
                c => new
                    {
                        Category_CategoryID = c.Int(nullable: false),
                        Offer_OfferID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_CategoryID, t.Offer_OfferID })
                .ForeignKey("dbo.Category", t => t.Category_CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Offer", t => t.Offer_OfferID, cascadeDelete: true)
                .Index(t => t.Category_CategoryID)
                .Index(t => t.Offer_OfferID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Auction", "Winner_UserID", "dbo.User");
            DropForeignKey("dbo.User", "Auction_AuctionID", "dbo.Auction");
            DropForeignKey("dbo.Transaction", "User_UserID", "dbo.User");
            DropForeignKey("dbo.Transaction", "Seller_UserID", "dbo.User");
            DropForeignKey("dbo.Transaction", "Buyer_UserID", "dbo.User");
            DropForeignKey("dbo.Bucket", "BucketID", "dbo.User");
            DropForeignKey("dbo.Bundle", "Bucket_BucketID", "dbo.Bucket");
            DropForeignKey("dbo.Bundle", "User_UserID", "dbo.User");
            DropForeignKey("dbo.Offer", "User_UserID", "dbo.User");
            DropForeignKey("dbo.CategoryOffer", "Offer_OfferID", "dbo.Offer");
            DropForeignKey("dbo.CategoryOffer", "Category_CategoryID", "dbo.Category");
            DropForeignKey("dbo.Offer", "Bundle_BundleID", "dbo.Bundle");
            DropForeignKey("dbo.Offer", "Bucket_BucketID", "dbo.Bucket");
            DropForeignKey("dbo.Auction", "User_UserID", "dbo.User");
            DropIndex("dbo.CategoryOffer", new[] { "Offer_OfferID" });
            DropIndex("dbo.CategoryOffer", new[] { "Category_CategoryID" });
            DropIndex("dbo.Transaction", new[] { "User_UserID" });
            DropIndex("dbo.Transaction", new[] { "Seller_UserID" });
            DropIndex("dbo.Transaction", new[] { "Buyer_UserID" });
            DropIndex("dbo.Offer", new[] { "User_UserID" });
            DropIndex("dbo.Offer", new[] { "Bundle_BundleID" });
            DropIndex("dbo.Offer", new[] { "Bucket_BucketID" });
            DropIndex("dbo.Bundle", new[] { "Bucket_BucketID" });
            DropIndex("dbo.Bundle", new[] { "User_UserID" });
            DropIndex("dbo.Bucket", new[] { "BucketID" });
            DropIndex("dbo.User", new[] { "Auction_AuctionID" });
            DropIndex("dbo.Auction", new[] { "Winner_UserID" });
            DropIndex("dbo.Auction", new[] { "User_UserID" });
            DropTable("dbo.CategoryOffer");
            DropTable("dbo.Transaction");
            DropTable("dbo.Category");
            DropTable("dbo.Offer");
            DropTable("dbo.Bundle");
            DropTable("dbo.Bucket");
            DropTable("dbo.User");
            DropTable("dbo.Auction");
        }
    }
}
