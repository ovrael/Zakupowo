namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkinOnChatsV2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Message", "User_UserID", "dbo.User");
            DropIndex("dbo.Message", new[] { "Receiver_UserID" });
            DropIndex("dbo.Message", new[] { "Sender_UserID" });
            DropIndex("dbo.Message", new[] { "User_UserID" });
            RenameColumn(table: "dbo.Message", name: "Receiver_UserID", newName: "ReceiverID");
            RenameColumn(table: "dbo.Message", name: "Sender_UserID", newName: "SenderID");
            AlterColumn("dbo.Message", "ReceiverID", c => c.Int(nullable: false));
            AlterColumn("dbo.Message", "SenderID", c => c.Int(nullable: false));
            CreateIndex("dbo.Message", "SenderID");
            CreateIndex("dbo.Message", "ReceiverID");
            DropColumn("dbo.Message", "User_UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Message", "User_UserID", c => c.Int());
            DropIndex("dbo.Message", new[] { "ReceiverID" });
            DropIndex("dbo.Message", new[] { "SenderID" });
            AlterColumn("dbo.Message", "SenderID", c => c.Int());
            AlterColumn("dbo.Message", "ReceiverID", c => c.Int());
            RenameColumn(table: "dbo.Message", name: "SenderID", newName: "Sender_UserID");
            RenameColumn(table: "dbo.Message", name: "ReceiverID", newName: "Receiver_UserID");
            CreateIndex("dbo.Message", "User_UserID");
            CreateIndex("dbo.Message", "Sender_UserID");
            CreateIndex("dbo.Message", "Receiver_UserID");
            AddForeignKey("dbo.Message", "User_UserID", "dbo.User", "UserID");
        }
    }
}
