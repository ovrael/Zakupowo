namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageModelUpdated_AddedSentTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "SentTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Message", "SentTime");
        }
    }
}
