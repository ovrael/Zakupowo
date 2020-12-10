namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferAndBundleChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offer", "CreationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Bundle", "OffersPriceSum", c => c.Double(nullable: false));
            AddColumn("dbo.Bundle", "BundlePrice", c => c.Double(nullable: false));
            AddColumn("dbo.Bundle", "CreationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Bundle", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Bundle", "Title", c => c.String(nullable: false, maxLength: 400));
            DropColumn("dbo.Bundle", "BundlePriceSum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bundle", "BundlePriceSum", c => c.Double(nullable: false));
            AlterColumn("dbo.Bundle", "Title", c => c.String(nullable: false, maxLength: 400, fixedLength: true, unicode: false));
            DropColumn("dbo.Bundle", "IsActive");
            DropColumn("dbo.Bundle", "CreationDate");
            DropColumn("dbo.Bundle", "BundlePrice");
            DropColumn("dbo.Bundle", "OffersPriceSum");
            DropColumn("dbo.Offer", "CreationDate");
        }
    }
}
