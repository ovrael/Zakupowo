namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatingOfferModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offer", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offer", "Description");
        }
    }
}
