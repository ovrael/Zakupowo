namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingProperRowVersionOfOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "RowVersion");
        }
    }
}
