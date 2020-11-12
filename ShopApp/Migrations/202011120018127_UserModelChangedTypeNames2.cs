namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserModelChangedTypeNames2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "Login", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "EncryptedPassword", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.User", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "Phone", c => c.String(maxLength: 12));
            AlterColumn("dbo.User", "Country", c => c.String(maxLength: 50));
            AlterColumn("dbo.User", "City", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "City", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.User", "Country", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.User", "Phone", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.User", "LastName", c => c.String(nullable: false, unicode: false, storeType: "text"));
            AlterColumn("dbo.User", "FirstName", c => c.String(nullable: false, unicode: false, storeType: "text"));
            AlterColumn("dbo.User", "EncryptedPassword", c => c.String(nullable: false, unicode: false, storeType: "text"));
            AlterColumn("dbo.User", "Login", c => c.String(nullable: false, unicode: false, storeType: "text"));
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false, unicode: false, storeType: "text"));
        }
    }
}
