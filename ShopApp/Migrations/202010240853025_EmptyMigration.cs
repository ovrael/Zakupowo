namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmptyMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "LastName", c => c.String());
            AlterColumn("dbo.User", "Password", c => c.String());
            AlterColumn("dbo.User", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.User", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.User", "LastName", c => c.String(nullable: false));
        }
    }
}
