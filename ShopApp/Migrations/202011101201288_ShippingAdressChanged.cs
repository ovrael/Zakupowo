namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShippingAdressChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingAdress", "FlatNumber", c => c.String(maxLength: 10, fixedLength: true, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShippingAdress", "FlatNumber");
        }
    }
}
