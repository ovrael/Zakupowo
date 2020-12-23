namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddedUserConnectionsForChat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserConnection",
                c => new
                {
                    UserConnectionID = c.Int(nullable: false, identity: true),
                    UserName = c.String(),
                    ConnectionID = c.String(),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                })
                .PrimaryKey(t => t.UserConnectionID);

        }

        public override void Down()
        {
            DropTable("dbo.UserConnection");
        }
    }
}
