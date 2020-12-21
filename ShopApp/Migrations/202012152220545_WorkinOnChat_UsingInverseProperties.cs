namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class WorkinOnChat_UsingInverseProperties : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Message", new[] { "SenderID" });
            DropIndex("dbo.Message", new[] { "ReceiverID" });
            RenameColumn(table: "dbo.Message", name: "ReceiverID", newName: "Receiver_UserID");
            RenameColumn(table: "dbo.Message", name: "SenderID", newName: "Sender_UserID");
            AlterColumn("dbo.Message", "Sender_UserID", c => c.Int());
            AlterColumn("dbo.Message", "Receiver_UserID", c => c.Int());
            CreateIndex("dbo.Message", "Receiver_UserID");
            CreateIndex("dbo.Message", "Sender_UserID");
        }

        public override void Down()
        {
            DropIndex("dbo.Message", new[] { "Sender_UserID" });
            DropIndex("dbo.Message", new[] { "Receiver_UserID" });
            AlterColumn("dbo.Message", "Receiver_UserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Message", "Sender_UserID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Message", name: "Sender_UserID", newName: "SenderID");
            RenameColumn(table: "dbo.Message", name: "Receiver_UserID", newName: "ReceiverID");
            CreateIndex("dbo.Message", "ReceiverID");
            CreateIndex("dbo.Message", "SenderID");
        }
    }
}
