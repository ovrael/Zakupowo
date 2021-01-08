namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserModel_AddedTwoTransactionsList : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TransactionBucketItem", newName: "BucketItemTransaction");
            DropForeignKey("dbo.Transaction", "User_UserID", "dbo.User");
            DropIndex("dbo.Transaction", new[] { "User_UserID" });
            DropPrimaryKey("dbo.BucketItemTransaction");
            AddPrimaryKey("dbo.BucketItemTransaction", new[] { "BucketItem_BucketItemID", "Transaction_TransactionID" });
            DropColumn("dbo.Transaction", "User_UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transaction", "User_UserID", c => c.Int());
            DropPrimaryKey("dbo.BucketItemTransaction");
            AddPrimaryKey("dbo.BucketItemTransaction", new[] { "Transaction_TransactionID", "BucketItem_BucketItemID" });
            CreateIndex("dbo.Transaction", "User_UserID");
            AddForeignKey("dbo.Transaction", "User_UserID", "dbo.User", "UserID");
            RenameTable(name: "dbo.BucketItemTransaction", newName: "TransactionBucketItem");
        }
    }
}
