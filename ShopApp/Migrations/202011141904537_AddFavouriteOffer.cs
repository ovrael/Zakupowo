namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFavouriteOffer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FavouriteOffer",
                c => new
                    {
                        FavouriteOfferID = c.Int(nullable: false, identity: true),
                        Offer_OfferID = c.Int(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.FavouriteOfferID)
                .ForeignKey("dbo.Offer", t => t.Offer_OfferID)
                .ForeignKey("dbo.User", t => t.User_UserID)
                .Index(t => t.Offer_OfferID)
                .Index(t => t.User_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavouriteOffer", "User_UserID", "dbo.User");
            DropForeignKey("dbo.FavouriteOffer", "Offer_OfferID", "dbo.Offer");
            DropIndex("dbo.FavouriteOffer", new[] { "User_UserID" });
            DropIndex("dbo.FavouriteOffer", new[] { "Offer_OfferID" });
            DropTable("dbo.FavouriteOffer");
        }
    }
}
