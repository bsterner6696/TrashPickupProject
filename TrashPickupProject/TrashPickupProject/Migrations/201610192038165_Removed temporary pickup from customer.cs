namespace TrashPickupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removedtemporarypickupfromcustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "TemporaryPickupId", "dbo.TemporaryPickups");
            DropIndex("dbo.Customers", new[] { "TemporaryPickupId" });
            DropColumn("dbo.Customers", "TemporaryPickupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "TemporaryPickupId", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "TemporaryPickupId");
            AddForeignKey("dbo.Customers", "TemporaryPickupId", "dbo.TemporaryPickups", "Id", cascadeDelete: true);
        }
    }
}
