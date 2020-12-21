namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkingWithMessagesV111 : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.User", t => t.Receiver_UserID)
                .ForeignKey("dbo.User", t => t.Sender_UserID)
                .ForeignKey("dbo.User", t => t.User_UserID)
                .Index(t => t.Receiver_UserID)
                .Index(t => t.Sender_UserID)
                .Index(t => t.User_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Message", "User_UserID", "dbo.User");
            DropForeignKey("dbo.Message", "Sender_UserID", "dbo.User");
            DropForeignKey("dbo.Message", "Receiver_UserID", "dbo.User");
            DropIndex("dbo.Message", new[] { "User_UserID" });
            DropIndex("dbo.Message", new[] { "Sender_UserID" });
            DropIndex("dbo.Message", new[] { "Receiver_UserID" });
            DropTable("dbo.Message");
        }
    }
}
