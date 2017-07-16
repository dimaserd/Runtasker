namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Messages", name: "ReceiverGuid", newName: "ReceiverId");
            RenameColumn(table: "dbo.Messages", name: "SenderGuid", newName: "SenderId");
            RenameIndex(table: "dbo.Messages", name: "IX_SenderGuid", newName: "IX_SenderId");
            RenameIndex(table: "dbo.Messages", name: "IX_ReceiverGuid", newName: "IX_ReceiverId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Messages", name: "IX_ReceiverId", newName: "IX_ReceiverGuid");
            RenameIndex(table: "dbo.Messages", name: "IX_SenderId", newName: "IX_SenderGuid");
            RenameColumn(table: "dbo.Messages", name: "SenderId", newName: "SenderGuid");
            RenameColumn(table: "dbo.Messages", name: "ReceiverId", newName: "ReceiverGuid");
        }
    }
}
