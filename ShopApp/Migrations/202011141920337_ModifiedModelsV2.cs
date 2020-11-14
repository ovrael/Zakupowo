namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedModelsV2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Offer", "Category_CategoryID", "dbo.Category");
            DropIndex("dbo.Offer", new[] { "Category_CategoryID" });
            AddColumn("dbo.Offer", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Offer", "Category_CategoryID", c => c.Int());
            CreateIndex("dbo.Offer", "Category_CategoryID");
            AddForeignKey("dbo.Offer", "Category_CategoryID", "dbo.Category", "CategoryID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offer", "Category_CategoryID", "dbo.Category");
            DropIndex("dbo.Offer", new[] { "Category_CategoryID" });
            AlterColumn("dbo.Offer", "Category_CategoryID", c => c.Int(nullable: false));
            DropColumn("dbo.Offer", "IsActive");
            CreateIndex("dbo.Offer", "Category_CategoryID");
            AddForeignKey("dbo.Offer", "Category_CategoryID", "dbo.Category", "CategoryID", cascadeDelete: true);
        }
    }
}
