namespace TrashPickupProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedhascustomerdetailstoapplicationuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "HasCustomerDetails", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "HasCustomerDetails");
        }
    }
}
