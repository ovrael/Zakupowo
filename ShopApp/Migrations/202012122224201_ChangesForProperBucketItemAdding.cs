namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesForProperBucketItemAdding : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BundleBucket", "Bundle_BundleID", "dbo.Bundle");
            DropForeignKey("dbo.BundleBucket", "Bucket_BucketID", "dbo.Bucket");
            DropIndex("dbo.BundleBucket", new[] { "Bundle_BundleID" });
            DropIndex("dbo.BundleBucket", new[] { "Bucket_BucketID" });
            AddColumn("dbo.BucketItem", "Bundle_BundleID", c => c.Int());
            CreateIndex("dbo.BucketItem", "Bundle_BundleID");
            AddForeignKey("dbo.BucketItem", "Bundle_BundleID", "dbo.Bundle", "BundleID");
            DropTable("dbo.BundleBucket");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BundleBucket",
                c => new
                    {
                        Bundle_BundleID = c.Int(nullable: false),
                        Bucket_BucketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Bundle_BundleID, t.Bucket_BucketID });
            
            DropForeignKey("dbo.BucketItem", "Bundle_BundleID", "dbo.Bundle");
            DropIndex("dbo.BucketItem", new[] { "Bundle_BundleID" });
            DropColumn("dbo.BucketItem", "Bundle_BundleID");
            CreateIndex("dbo.BundleBucket", "Bucket_BucketID");
            CreateIndex("dbo.BundleBucket", "Bundle_BundleID");
            AddForeignKey("dbo.BundleBucket", "Bucket_BucketID", "dbo.Bucket", "BucketID", cascadeDelete: true);
            AddForeignKey("dbo.BundleBucket", "Bundle_BundleID", "dbo.Bundle", "BundleID", cascadeDelete: true);
        }
    }
}
