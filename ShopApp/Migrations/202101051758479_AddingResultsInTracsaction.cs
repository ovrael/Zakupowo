namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingResultsInTracsaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "IsAccepted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Transaction", "IsChosen", c => c.Boolean(nullable: false));
            DropColumn("dbo.Transaction", "Result");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transaction", "Result", c => c.String());
            DropColumn("dbo.Transaction", "IsChosen");
            DropColumn("dbo.Transaction", "IsAccepted");
        }
    }
}
