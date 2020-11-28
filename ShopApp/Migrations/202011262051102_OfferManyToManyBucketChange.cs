namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferManyToManyBucketChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Offer", "Bucket_BucketID", "dbo.Bucket");
            DropIndex("dbo.Offer", new[] { "Bucket_BucketID" });
            CreateTable(
                "dbo.OfferBucket",
                c => new
                    {
                        Offer_OfferID = c.Int(nullable: false),
                        Bucket_BucketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Offer_OfferID, t.Bucket_BucketID })
                .ForeignKey("dbo.Offer", t => t.Offer_OfferID, cascadeDelete: true)
                .ForeignKey("dbo.Bucket", t => t.Bucket_BucketID, cascadeDelete: true)
                .Index(t => t.Offer_OfferID)
                .Index(t => t.Bucket_BucketID);
            
            DropColumn("dbo.Offer", "Bucket_BucketID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offer", "Bucket_BucketID", c => c.Int());
            DropForeignKey("dbo.OfferBucket", "Bucket_BucketID", "dbo.Bucket");
            DropForeignKey("dbo.OfferBucket", "Offer_OfferID", "dbo.Offer");
            DropIndex("dbo.OfferBucket", new[] { "Bucket_BucketID" });
            DropIndex("dbo.OfferBucket", new[] { "Offer_OfferID" });
            DropTable("dbo.OfferBucket");
            CreateIndex("dbo.Offer", "Bucket_BucketID");
            AddForeignKey("dbo.Offer", "Bucket_BucketID", "dbo.Bucket", "BucketID");
        }
    }
}
