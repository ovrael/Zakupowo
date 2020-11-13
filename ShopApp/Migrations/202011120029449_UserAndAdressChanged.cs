namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAndAdressChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ShippingAdress", "Country", c => c.String(nullable: false, maxLength: 75));
            AlterColumn("dbo.ShippingAdress", "City", c => c.String(nullable: false, maxLength: 75));
            AlterColumn("dbo.ShippingAdress", "Street", c => c.String(nullable: false, maxLength: 75));
            AlterColumn("dbo.ShippingAdress", "PremisesNumber", c => c.String(maxLength: 10));
            AlterColumn("dbo.ShippingAdress", "PostalCode", c => c.String(maxLength: 15));
            DropColumn("dbo.User", "Country");
            DropColumn("dbo.User", "City");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "City", c => c.String(maxLength: 50));
            AddColumn("dbo.User", "Country", c => c.String(maxLength: 50));
            AlterColumn("dbo.ShippingAdress", "PostalCode", c => c.String(maxLength: 15, fixedLength: true, unicode: false));
            AlterColumn("dbo.ShippingAdress", "PremisesNumber", c => c.String(maxLength: 10, fixedLength: true, unicode: false));
            AlterColumn("dbo.ShippingAdress", "Street", c => c.String(nullable: false, maxLength: 75, fixedLength: true, unicode: false));
            AlterColumn("dbo.ShippingAdress", "City", c => c.String(nullable: false, maxLength: 75, fixedLength: true, unicode: false));
            AlterColumn("dbo.ShippingAdress", "Country", c => c.String(nullable: false, maxLength: 75, fixedLength: true, unicode: false));
        }
    }
}
