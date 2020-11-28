namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsChosenBoolOnBucketItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BucketItem", "IsChosen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BucketItem", "IsChosen");
        }
    }
}
