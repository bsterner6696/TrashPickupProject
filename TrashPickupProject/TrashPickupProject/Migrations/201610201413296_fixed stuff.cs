namespace TrashPickupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedstuff : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Customers", name: "ApplicationUserEmail", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.Customers", name: "IX_ApplicationUserEmail", newName: "IX_ApplicationUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Customers", name: "IX_ApplicationUserId", newName: "IX_ApplicationUserEmail");
            RenameColumn(table: "dbo.Customers", name: "ApplicationUserId", newName: "ApplicationUserEmail");
        }
    }
}
