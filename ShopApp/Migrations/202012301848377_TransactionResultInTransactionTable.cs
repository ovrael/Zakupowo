namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionResultInTransactionTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "Result", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transaction", "Result");
        }
    }
}
