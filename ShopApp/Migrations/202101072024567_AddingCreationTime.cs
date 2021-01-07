namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCreationTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaction", "CreationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Bucket", "TotalBucketPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bucket", "TotalBucketPrice", c => c.Double(nullable: false));
            DropColumn("dbo.Transaction", "CreationDate");
        }
    }
}
