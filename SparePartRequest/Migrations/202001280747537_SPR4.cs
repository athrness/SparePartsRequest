namespace SparePartRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SPR4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Requests", name: "ApplicationUser_Id", newName: "ApplicationUserID");
            RenameIndex(table: "dbo.Requests", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Requests", name: "IX_ApplicationUserID", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Requests", name: "ApplicationUserID", newName: "ApplicationUser_Id");
        }
    }
}
