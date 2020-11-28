namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveICollectionFromBucketItemToBucket : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BucketItemBucket", "BucketItem_BucketItemID", "dbo.BucketItem");
            DropForeignKey("dbo.BucketItemBucket", "Bucket_BucketID", "dbo.Bucket");
            DropIndex("dbo.BucketItemBucket", new[] { "BucketItem_BucketItemID" });
            DropIndex("dbo.BucketItemBucket", new[] { "Bucket_BucketID" });
            AddColumn("dbo.BucketItem", "Bucket_BucketID", c => c.Int());
            CreateIndex("dbo.BucketItem", "Bucket_BucketID");
            AddForeignKey("dbo.BucketItem", "Bucket_BucketID", "dbo.Bucket", "BucketID");
            DropTable("dbo.BucketItemBucket");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BucketItemBucket",
                c => new
                    {
                        BucketItem_BucketItemID = c.Int(nullable: false),
                        Bucket_BucketID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BucketItem_BucketItemID, t.Bucket_BucketID });
            
            DropForeignKey("dbo.BucketItem", "Bucket_BucketID", "dbo.Bucket");
            DropIndex("dbo.BucketItem", new[] { "Bucket_BucketID" });
            DropColumn("dbo.BucketItem", "Bucket_BucketID");
            CreateIndex("dbo.BucketItemBucket", "Bucket_BucketID");
            CreateIndex("dbo.BucketItemBucket", "BucketItem_BucketItemID");
            AddForeignKey("dbo.BucketItemBucket", "Bucket_BucketID", "dbo.Bucket", "BucketID", cascadeDelete: true);
            AddForeignKey("dbo.BucketItemBucket", "BucketItem_BucketItemID", "dbo.BucketItem", "BucketItemID", cascadeDelete: true);
        }
    }
}
