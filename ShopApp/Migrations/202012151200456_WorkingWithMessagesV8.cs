namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkingWithMessagesV8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Message", "MessageID", "dbo.User");
            DropForeignKey("dbo.Message", "User_UserID", "dbo.User");
            DropIndex("dbo.Message", new[] { "MessageID" });
            DropIndex("dbo.Message", new[] { "User_UserID" });
            DropTable("dbo.Message");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        MessageID = c.Int(nullable: false),
                        Content = c.String(maxLength: 4000),
                        SentTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.MessageID);
            
            CreateIndex("dbo.Message", "User_UserID");
            CreateIndex("dbo.Message", "MessageID");
            AddForeignKey("dbo.Message", "User_UserID", "dbo.User", "UserID");
            AddForeignKey("dbo.Message", "MessageID", "dbo.User", "UserID");
        }
    }
}
