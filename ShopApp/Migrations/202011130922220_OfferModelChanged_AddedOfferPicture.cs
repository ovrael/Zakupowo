namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferModelChanged_AddedOfferPicture : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OfferPicture",
                c => new
                    {
                        OfferImageID = c.Int(nullable: false, identity: true),
                        PathToFile = c.String(maxLength: 4000),
                        Offer_OfferID = c.Int(),
                    })
                .PrimaryKey(t => t.OfferImageID)
                .ForeignKey("dbo.Offer", t => t.Offer_OfferID)
                .Index(t => t.Offer_OfferID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OfferPicture", "Offer_OfferID", "dbo.Offer");
            DropIndex("dbo.OfferPicture", new[] { "Offer_OfferID" });
            DropTable("dbo.OfferPicture");
        }
    }
}
