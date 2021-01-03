namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MessageModelChanged_AddedIsReadProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "IsRead", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Message", "IsRead");
        }
    }
}
