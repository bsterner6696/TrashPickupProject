namespace TrashPickupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeddatabasesforcustomerandtemporaryPickup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Zipcode = c.Int(nullable: false),
                        StreetAddress = c.String(),
                        Name = c.String(),
                        DayOfPickup = c.String(),
                        Balance = c.Double(nullable: false),
                        TemporaryPickupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TemporaryPickups", t => t.TemporaryPickupId, cascadeDelete: true)
                .Index(t => t.TemporaryPickupId);
            
            CreateTable(
                "dbo.TemporaryPickups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        DayOfWeek = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "TemporaryPickupId", "dbo.TemporaryPickups");
            DropIndex("dbo.Customers", new[] { "TemporaryPickupId" });
            DropTable("dbo.TemporaryPickups");
            DropTable("dbo.Customers");
        }
    }
}
