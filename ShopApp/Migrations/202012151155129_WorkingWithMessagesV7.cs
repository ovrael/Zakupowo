namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkingWithMessagesV7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "User_UserID1", c => c.Int());
            CreateIndex("dbo.Message", "User_UserID1");
            AddForeignKey("dbo.Message", "User_UserID1", "dbo.User", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Message", "User_UserID1", "dbo.User");
            DropIndex("dbo.Message", new[] { "User_UserID1" });
            DropColumn("dbo.Message", "User_UserID1");
        }
    }
}
