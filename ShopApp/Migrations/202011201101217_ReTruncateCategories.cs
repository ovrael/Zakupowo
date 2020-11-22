namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReTruncateCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offer", "Category_CategoryID", c => c.Int());
            CreateIndex("dbo.Offer", "Category_CategoryID");
            AddForeignKey("dbo.Offer", "Category_CategoryID", "dbo.Category", "CategoryID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offer", "Category_CategoryID", "dbo.Category");
            DropIndex("dbo.Offer", new[] { "Category_CategoryID" });
            DropColumn("dbo.Offer", "Category_CategoryID");
        }
    }
}
