namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TruncateCategories : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Offer", "Category_CategoryID", "dbo.Category");
            DropIndex("dbo.Offer", new[] { "Category_CategoryID" });
            DropColumn("dbo.Offer", "Category_CategoryID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offer", "Category_CategoryID", c => c.Int());
            CreateIndex("dbo.Offer", "Category_CategoryID");
            AddForeignKey("dbo.Offer", "Category_CategoryID", "dbo.Category", "CategoryID");
        }
    }
}
