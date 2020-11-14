namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedModelsV1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CategoryOffer", "Category_CategoryID", "dbo.Category");
            DropForeignKey("dbo.CategoryOffer", "Offer_OfferID", "dbo.Offer");
            DropIndex("dbo.CategoryOffer", new[] { "Category_CategoryID" });
            DropIndex("dbo.CategoryOffer", new[] { "Offer_OfferID" });
            AddColumn("dbo.Offer", "Category_CategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Offer", "Category_CategoryID");
            AddForeignKey("dbo.Offer", "Category_CategoryID", "dbo.Category", "CategoryID", cascadeDelete: true);
            DropTable("dbo.CategoryOffer");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CategoryOffer",
                c => new
                    {
                        Category_CategoryID = c.Int(nullable: false),
                        Offer_OfferID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_CategoryID, t.Offer_OfferID });
            
            DropForeignKey("dbo.Offer", "Category_CategoryID", "dbo.Category");
            DropIndex("dbo.Offer", new[] { "Category_CategoryID" });
            DropColumn("dbo.Offer", "Category_CategoryID");
            CreateIndex("dbo.CategoryOffer", "Offer_OfferID");
            CreateIndex("dbo.CategoryOffer", "Category_CategoryID");
            AddForeignKey("dbo.CategoryOffer", "Offer_OfferID", "dbo.Offer", "OfferID", cascadeDelete: true);
            AddForeignKey("dbo.CategoryOffer", "Category_CategoryID", "dbo.Category", "CategoryID", cascadeDelete: true);
        }
    }
}
