namespace TestApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Surname = c.String(),
                        Name = c.String(),
                        Patronymic = c.String(),
                        Gender = c.Int(nullable: false),
                        BirthDate = c.DateTime(),
                        UnitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Units", t => t.UnitId)
                .Index(t => t.UnitId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Counterparty = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.AuthorId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        HeadId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.HeadId)
                .Index(t => t.HeadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Units", "HeadId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "UnitId", "dbo.Units");
            DropForeignKey("dbo.Orders", "AuthorId", "dbo.Employees");
            DropIndex("dbo.Units", new[] { "HeadId" });
            DropIndex("dbo.Orders", new[] { "AuthorId" });
            DropIndex("dbo.Employees", new[] { "UnitId" });
            DropTable("dbo.Units");
            DropTable("dbo.Orders");
            DropTable("dbo.Employees");
        }
    }
}
