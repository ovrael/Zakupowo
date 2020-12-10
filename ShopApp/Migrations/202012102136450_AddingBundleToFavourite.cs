namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBundleToFavourite : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FavouriteOffer", newName: "Favourite");
            AddColumn("dbo.Favourite", "Bundle_BundleID", c => c.Int());
            CreateIndex("dbo.Favourite", "Bundle_BundleID");
            AddForeignKey("dbo.Favourite", "Bundle_BundleID", "dbo.Bundle", "BundleID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Favourite", "Bundle_BundleID", "dbo.Bundle");
            DropIndex("dbo.Favourite", new[] { "Bundle_BundleID" });
            DropColumn("dbo.Favourite", "Bundle_BundleID");
            RenameTable(name: "dbo.Favourite", newName: "FavouriteOffer");
        }
    }
}
