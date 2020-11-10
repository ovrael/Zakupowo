namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShippingAdressChanged1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShippingAdress", "User_UserID", "dbo.User");
            DropIndex("dbo.ShippingAdress", new[] { "User_UserID" });
            AlterColumn("dbo.ShippingAdress", "User_UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.ShippingAdress", "User_UserID");
            AddForeignKey("dbo.ShippingAdress", "User_UserID", "dbo.User", "UserID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShippingAdress", "User_UserID", "dbo.User");
            DropIndex("dbo.ShippingAdress", new[] { "User_UserID" });
            AlterColumn("dbo.ShippingAdress", "User_UserID", c => c.Int());
            CreateIndex("dbo.ShippingAdress", "User_UserID");
            AddForeignKey("dbo.ShippingAdress", "User_UserID", "dbo.User", "UserID");
        }
    }
}
