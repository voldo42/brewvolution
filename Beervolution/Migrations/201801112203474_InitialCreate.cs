namespace Beervolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Beers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Manufacturer = c.String(),
                        Type = c.String(),
                        InclusiveKit = c.Boolean(nullable: false),
                        TargetPercentage = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Brews",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartingGravity = c.Double(nullable: false),
                        FinalGravity = c.Double(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        BottleDate = c.DateTime(nullable: false),
                        SecondaryFermentationDate = c.DateTime(),
                        Beer_ID = c.Int(),
                        Variables_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Beers", t => t.Beer_ID)
                .ForeignKey("dbo.Variables", t => t.Variables_ID)
                .Index(t => t.Beer_ID)
                .Index(t => t.Variables_ID);
            
            CreateTable(
                "dbo.Variables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FermentableType = c.String(),
                        FermentableAmount = c.Double(nullable: false),
                        WaterType = c.String(),
                        TotalVolume = c.Double(nullable: false),
                        HopDetails = c.String(),
                        BrewTemp = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Brews", "Variables_ID", "dbo.Variables");
            DropForeignKey("dbo.Brews", "Beer_ID", "dbo.Beers");
            DropIndex("dbo.Brews", new[] { "Variables_ID" });
            DropIndex("dbo.Brews", new[] { "Beer_ID" });
            DropTable("dbo.Variables");
            DropTable("dbo.Brews");
            DropTable("dbo.Beers");
        }
    }
}
