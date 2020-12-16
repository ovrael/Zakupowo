namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class WorkingWithMessagesV1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Message", "Receiver_UserID", "dbo.User");
            DropForeignKey("dbo.Message", "Sender_UserID", "dbo.User");
            DropForeignKey("dbo.Message", "User_UserID", "dbo.User");
            DropIndex("dbo.Message", new[] { "Receiver_UserID" });
            DropIndex("dbo.Message", new[] { "Sender_UserID" });
            DropIndex("dbo.Message", new[] { "User_UserID" });
            //DropTable("dbo.Message");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Message",
                c => new
                {
                    MessageID = c.Int(nullable: false, identity: true),
                    Content = c.String(maxLength: 4000),
                    SentTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    Receiver_UserID = c.Int(),
                    Sender_UserID = c.Int(),
                    User_UserID = c.Int(),
                })
                .PrimaryKey(t => t.MessageID);

            CreateIndex("dbo.Message", "User_UserID");
            CreateIndex("dbo.Message", "Sender_UserID");
            CreateIndex("dbo.Message", "Receiver_UserID");
            AddForeignKey("dbo.Message", "User_UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.Message", "Sender_UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.Message", "Receiver_UserID", "dbo.User", "UserID");
        }
    }
}
