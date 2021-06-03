namespace TestApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderDate_field_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "OrderDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "OrderDate", c => c.DateTime(nullable: false));
        }
    }
}
