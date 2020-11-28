namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBucketItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OfferBucket", "Offer_OfferID", "dbo.Offer");
            DropForeignKey("dbo.OfferBucket", "Bucket_BucketID", "dbo.Bucket");
            DropForeignKey("dbo.Bundle", "Bucket_BucketID", "dbo.Bucket");
            DropIndex("dbo.Bundle", new[] { "Bucket_BucketID" });
            DropIndex("dbo.OfferBucket", new[] { "Offer_OfferID" });
            DropIndex("dbo.OfferBucket", new[] { "Bucket_BucketID" });
            CreateTable(
                "dbo.BucketItem",
                c => new
                    {
                        BucketItemID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                        Offer_OfferID = c.Int(),
                        Transaction_TransactionID = c.Int(),
                    })
                .PrimaryKey(t => t.BucketItemID)
                .ForeignKey("dbo.Offer", t => t.Offer_OfferID)
                .ForeignKey("dbo.Transaction", t => t.Transaction_TransactionID)
                .Index(t => t.Offer_OfferID)
                .Index(t => t.Transaction_TransactionID);
            
            CreateTable(
                "dbo.BucketItemBucket",
                c => new
                    {
                        BucketItem_BucketItemID = c.Int(nullable: false),
                        Bucket_BucketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BucketItem_BucketItemID, t.Bucket_BucketID })
                .ForeignKey("dbo.BucketItem", t => t.BucketItem_BucketItemID, cascadeDelete: true)
                .ForeignKey("dbo.Bucket", t => t.Bucket_BucketID, cascadeDelete: true)
                .Index(t => t.BucketItem_BucketItemID)
                .Index(t => t.Bucket_BucketID);
            
            CreateTable(
                "dbo.BundleBucket",
                c => new
                    {
                        Bundle_BundleID = c.Int(nullable: false),
                        Bucket_BucketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Bundle_BundleID, t.Bucket_BucketID })
                .ForeignKey("dbo.Bundle", t => t.Bundle_BundleID, cascadeDelete: true)
                .ForeignKey("dbo.Bucket", t => t.Bucket_BucketID, cascadeDelete: true)
                .Index(t => t.Bundle_BundleID)
                .Index(t => t.Bucket_BucketID);
            
            AddColumn("dbo.Bucket", "TotalBucketPrice", c => c.Double(nullable: false));
            AddColumn("dbo.Offer", "InStockOriginaly", c => c.Double(nullable: false));
            AddColumn("dbo.Offer", "InStockNow", c => c.Double(nullable: false));
            DropColumn("dbo.Bundle", "Bucket_BucketID");
            DropColumn("dbo.Offer", "InStock");
            DropTable("dbo.OfferBucket");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OfferBucket",
                c => new
                    {
                        Offer_OfferID = c.Int(nullable: false),
                        Bucket_BucketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Offer_OfferID, t.Bucket_BucketID });
            
            AddColumn("dbo.Offer", "InStock", c => c.Double(nullable: false));
            AddColumn("dbo.Bundle", "Bucket_BucketID", c => c.Int());
            DropForeignKey("dbo.BucketItem", "Transaction_TransactionID", "dbo.Transaction");
            DropForeignKey("dbo.BundleBucket", "Bucket_BucketID", "dbo.Bucket");
            DropForeignKey("dbo.BundleBucket", "Bundle_BundleID", "dbo.Bundle");
            DropForeignKey("dbo.BucketItem", "Offer_OfferID", "dbo.Offer");
            DropForeignKey("dbo.BucketItemBucket", "Bucket_BucketID", "dbo.Bucket");
            DropForeignKey("dbo.BucketItemBucket", "BucketItem_BucketItemID", "dbo.BucketItem");
            DropIndex("dbo.BundleBucket", new[] { "Bucket_BucketID" });
            DropIndex("dbo.BundleBucket", new[] { "Bundle_BundleID" });
            DropIndex("dbo.BucketItemBucket", new[] { "Bucket_BucketID" });
            DropIndex("dbo.BucketItemBucket", new[] { "BucketItem_BucketItemID" });
            DropIndex("dbo.BucketItem", new[] { "Transaction_TransactionID" });
            DropIndex("dbo.BucketItem", new[] { "Offer_OfferID" });
            DropColumn("dbo.Offer", "InStockNow");
            DropColumn("dbo.Offer", "InStockOriginaly");
            DropColumn("dbo.Bucket", "TotalBucketPrice");
            DropTable("dbo.BundleBucket");
            DropTable("dbo.BucketItemBucket");
            DropTable("dbo.BucketItem");
            CreateIndex("dbo.OfferBucket", "Bucket_BucketID");
            CreateIndex("dbo.OfferBucket", "Offer_OfferID");
            CreateIndex("dbo.Bundle", "Bucket_BucketID");
            AddForeignKey("dbo.Bundle", "Bucket_BucketID", "dbo.Bucket", "BucketID");
            AddForeignKey("dbo.OfferBucket", "Bucket_BucketID", "dbo.Bucket", "BucketID", cascadeDelete: true);
            AddForeignKey("dbo.OfferBucket", "Offer_OfferID", "dbo.Offer", "OfferID", cascadeDelete: true);
        }
    }
}
