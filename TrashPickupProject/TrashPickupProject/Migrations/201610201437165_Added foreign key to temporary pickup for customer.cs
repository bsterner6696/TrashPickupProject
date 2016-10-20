namespace TrashPickupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedforeignkeytotemporarypickupforcustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TemporaryPickups", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.TemporaryPickups", "CustomerId");
            AddForeignKey("dbo.TemporaryPickups", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TemporaryPickups", "CustomerId", "dbo.Customers");
            DropIndex("dbo.TemporaryPickups", new[] { "CustomerId" });
            DropColumn("dbo.TemporaryPickups", "CustomerId");
        }
    }
}
