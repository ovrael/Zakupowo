namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferModelChanged_AddedOfferStateEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offer", "OfferState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offer", "OfferState");
        }
    }
}
