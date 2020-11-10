namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShippingAdressUpdate_DeletedFlatNumber : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShippingAdress", "FlatNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShippingAdress", "FlatNumber", c => c.String(maxLength: 10, fixedLength: true, unicode: false));
        }
    }
}
