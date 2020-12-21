namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class WorkinOnChatsV1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Message", "Receiver_UserID", "dbo.User");
            DropForeignKey("dbo.Message", "Sender_UserID", "dbo.User");
            DropIndex("dbo.Message", new[] { "Receiver_UserID" });
            DropIndex("dbo.Message", new[] { "Sender_UserID" });
            AlterColumn("dbo.Message", "Content", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Message", "Receiver_UserID", c => c.Int());
            AlterColumn("dbo.Message", "Sender_UserID", c => c.Int());
            CreateIndex("dbo.Message", "Receiver_UserID");
            CreateIndex("dbo.Message", "Sender_UserID");
            AddForeignKey("dbo.Message", "Receiver_UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.Message", "Sender_UserID", "dbo.User", "UserID");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Message", "Sender_UserID", "dbo.User");
            DropForeignKey("dbo.Message", "Receiver_UserID", "dbo.User");
            DropIndex("dbo.Message", new[] { "Sender_UserID" });
            DropIndex("dbo.Message", new[] { "Receiver_UserID" });
            AlterColumn("dbo.Message", "Sender_UserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Message", "Receiver_UserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Message", "Content", c => c.String(nullable: false, maxLength: 4000));
            CreateIndex("dbo.Message", "Sender_UserID");
            CreateIndex("dbo.Message", "Receiver_UserID");
            AddForeignKey("dbo.Message", "Sender_UserID", "dbo.User", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.Message", "Receiver_UserID", "dbo.User", "UserID", cascadeDelete: true);
        }
    }
}
