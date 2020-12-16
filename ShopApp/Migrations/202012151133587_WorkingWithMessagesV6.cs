namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkingWithMessagesV6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "Receiver_UserID", c => c.Int());
            AddColumn("dbo.Message", "Sender_UserID", c => c.Int());
            AddColumn("dbo.Message", "User_UserID", c => c.Int());
            CreateIndex("dbo.Message", "Receiver_UserID");
            CreateIndex("dbo.Message", "Sender_UserID");
            CreateIndex("dbo.Message", "User_UserID");
            AddForeignKey("dbo.Message", "Receiver_UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.Message", "Sender_UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.Message", "User_UserID", "dbo.User", "UserID");
            DropColumn("dbo.Message", "SenderID");
            DropColumn("dbo.Message", "ReceiverID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Message", "ReceiverID", c => c.Int(nullable: false));
            AddColumn("dbo.Message", "SenderID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Message", "User_UserID", "dbo.User");
            DropForeignKey("dbo.Message", "Sender_UserID", "dbo.User");
            DropForeignKey("dbo.Message", "Receiver_UserID", "dbo.User");
            DropIndex("dbo.Message", new[] { "User_UserID" });
            DropIndex("dbo.Message", new[] { "Sender_UserID" });
            DropIndex("dbo.Message", new[] { "Receiver_UserID" });
            DropColumn("dbo.Message", "User_UserID");
            DropColumn("dbo.Message", "Sender_UserID");
            DropColumn("dbo.Message", "Receiver_UserID");
        }
    }
}
