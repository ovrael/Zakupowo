namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedBundlePictureModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BundlePicture",
                c => new
                    {
                        BundlePictureID = c.Int(nullable: false),
                        PathToFile = c.String(maxLength: 4000),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.BundlePictureID)
                .ForeignKey("dbo.Bundle", t => t.BundlePictureID)
                .Index(t => t.BundlePictureID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BundlePicture", "BundlePictureID", "dbo.Bundle");
            DropIndex("dbo.BundlePicture", new[] { "BundlePictureID" });
            DropTable("dbo.BundlePicture");
        }
    }
}
