namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingOrderTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        RowVersion = c.Binary(),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.User", t => t.OrderID)
                .Index(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "OrderID", "dbo.User");
            DropIndex("dbo.Order", new[] { "OrderID" });
            DropTable("dbo.Order");
        }
    }
}
