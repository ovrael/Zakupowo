namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResetCodeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordReset",
                c => new
                    {
                        PasswordResetID = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(),
                        PasswordResetCode = c.String(),
                        CodeCreationTime = c.DateTime(nullable: false),
                        CodeExpirationTime = c.DateTime(nullable: false),
                        TriesCount = c.Int(nullable: false),
                        Used = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PasswordResetID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PasswordReset");
        }
    }
}
