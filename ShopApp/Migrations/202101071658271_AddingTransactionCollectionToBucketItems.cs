namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTransactionCollectionToBucketItems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BucketItem", "Transaction_TransactionID", "dbo.Transaction");
            DropIndex("dbo.BucketItem", new[] { "Transaction_TransactionID" });
            CreateTable(
                "dbo.TransactionBucketItem",
                c => new
                    {
                        Transaction_TransactionID = c.Int(nullable: false),
                        BucketItem_BucketItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Transaction_TransactionID, t.BucketItem_BucketItemID })
                .ForeignKey("dbo.Transaction", t => t.Transaction_TransactionID, cascadeDelete: true)
                .ForeignKey("dbo.BucketItem", t => t.BucketItem_BucketItemID, cascadeDelete: true)
                .Index(t => t.Transaction_TransactionID)
                .Index(t => t.BucketItem_BucketItemID);
            
            DropColumn("dbo.BucketItem", "Transaction_TransactionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BucketItem", "Transaction_TransactionID", c => c.Int());
            DropForeignKey("dbo.TransactionBucketItem", "BucketItem_BucketItemID", "dbo.BucketItem");
            DropForeignKey("dbo.TransactionBucketItem", "Transaction_TransactionID", "dbo.Transaction");
            DropIndex("dbo.TransactionBucketItem", new[] { "BucketItem_BucketItemID" });
            DropIndex("dbo.TransactionBucketItem", new[] { "Transaction_TransactionID" });
            DropTable("dbo.TransactionBucketItem");
            CreateIndex("dbo.BucketItem", "Transaction_TransactionID");
            AddForeignKey("dbo.BucketItem", "Transaction_TransactionID", "dbo.Transaction", "TransactionID");
        }
    }
}
