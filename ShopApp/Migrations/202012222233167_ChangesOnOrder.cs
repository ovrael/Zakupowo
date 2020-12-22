namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesOnOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderBucketItem",
                c => new
                    {
                        Order_OrderID = c.Int(nullable: false),
                        BucketItem_BucketItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Order_OrderID, t.BucketItem_BucketItemID })
                .ForeignKey("dbo.Order", t => t.Order_OrderID, cascadeDelete: true)
                .ForeignKey("dbo.BucketItem", t => t.BucketItem_BucketItemID, cascadeDelete: true)
                .Index(t => t.Order_OrderID)
                .Index(t => t.BucketItem_BucketItemID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderBucketItem", "BucketItem_BucketItemID", "dbo.BucketItem");
            DropForeignKey("dbo.OrderBucketItem", "Order_OrderID", "dbo.Order");
            DropIndex("dbo.OrderBucketItem", new[] { "BucketItem_BucketItemID" });
            DropIndex("dbo.OrderBucketItem", new[] { "Order_OrderID" });
            DropTable("dbo.OrderBucketItem");
        }
    }
}
