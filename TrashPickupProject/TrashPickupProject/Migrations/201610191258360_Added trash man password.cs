namespace TrashPickupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedtrashmanpassword : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "StreetAddress", c => c.String());
            AlterColumn("dbo.Customers", "Name", c => c.String());
            AlterColumn("dbo.Customers", "DayOfPickup", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "DayOfPickup", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "StreetAddress", c => c.String(nullable: false));
        }
    }
}
