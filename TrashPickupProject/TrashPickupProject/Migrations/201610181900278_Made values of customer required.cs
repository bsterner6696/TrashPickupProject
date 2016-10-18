namespace TrashPickupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Madevaluesofcustomerrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "StreetAddress", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "DayOfPickup", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "DayOfPickup", c => c.String());
            AlterColumn("dbo.Customers", "Name", c => c.String());
            AlterColumn("dbo.Customers", "StreetAddress", c => c.String());
        }
    }
}
