namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserModelChanged_AddedAvatarImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvatarImage",
                c => new
                    {
                        AvatarImageID = c.Int(nullable: false),
                        PathToFile = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.AvatarImageID)
                .ForeignKey("dbo.User", t => t.AvatarImageID)
                .Index(t => t.AvatarImageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AvatarImage", "AvatarImageID", "dbo.User");
            DropIndex("dbo.AvatarImage", new[] { "AvatarImageID" });
            DropTable("dbo.AvatarImage");
        }
    }
}
