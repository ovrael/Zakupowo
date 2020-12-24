namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserConnectionModelChanged_AddedCreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserConnection", "CreationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserConnection", "CreationDate");
        }
    }
}
