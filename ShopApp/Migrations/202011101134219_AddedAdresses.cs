namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAdresses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShippingAdress",
                c => new
                    {
                        AdressID = c.Int(nullable: false, identity: true),
                        Country = c.String(nullable: false, maxLength: 75, fixedLength: true, unicode: false),
                        City = c.String(nullable: false, maxLength: 75, fixedLength: true, unicode: false),
                        Street = c.String(nullable: false, maxLength: 75, fixedLength: true, unicode: false),
                        PremisesNumber = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        PostalCode = c.String(maxLength: 15, fixedLength: true, unicode: false),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.AdressID)
                .ForeignKey("dbo.User", t => t.User_UserID)
                .Index(t => t.User_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShippingAdress", "User_UserID", "dbo.User");
            DropIndex("dbo.ShippingAdress", new[] { "User_UserID" });
            DropTable("dbo.ShippingAdress");
        }
    }
}
