namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletingBadRowVersionOfOrder : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Order", "RowVersion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "RowVersion", c => c.Binary());
        }
    }
}
