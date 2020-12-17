namespace ShopApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMessageModel_BackedToNoIDversion : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Message", name: "ReceiverID_UserID", newName: "Receiver_UserID");
            RenameColumn(table: "dbo.Message", name: "SenderID_UserID", newName: "Sender_UserID");
            RenameIndex(table: "dbo.Message", name: "IX_ReceiverID_UserID", newName: "IX_Receiver_UserID");
            RenameIndex(table: "dbo.Message", name: "IX_SenderID_UserID", newName: "IX_Sender_UserID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Message", name: "IX_Sender_UserID", newName: "IX_SenderID_UserID");
            RenameIndex(table: "dbo.Message", name: "IX_Receiver_UserID", newName: "IX_ReceiverID_UserID");
            RenameColumn(table: "dbo.Message", name: "Sender_UserID", newName: "SenderID_UserID");
            RenameColumn(table: "dbo.Message", name: "Receiver_UserID", newName: "ReceiverID_UserID");
        }
    }
}
