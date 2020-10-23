namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatingUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Password", c => c.String());
            AddColumn("dbo.User", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Email");
            DropColumn("dbo.User", "Password");
        }
    }
}
