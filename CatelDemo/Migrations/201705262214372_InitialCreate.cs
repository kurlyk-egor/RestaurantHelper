namespace RestaurantHelper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AmountExcessActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExcessSum = c.Int(nullable: false),
                        DishId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Description = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dishes", t => t.DishId, cascadeDelete: true)
                .Index(t => t.DishId);
            
            CreateTable(
                "dbo.Dishes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                        Info = c.String(),
                        PicturePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Text = c.String(),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(maxLength: 20),
                        Password = c.String(maxLength: 20),
                        Name = c.String(maxLength: 30),
                        Phone = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiscountActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DiscountSum = c.Int(nullable: false),
                        DishId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Description = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dishes", t => t.DishId, cascadeDelete: true)
                .Index(t => t.DishId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Age = c.Int(nullable: false),
                        Position = c.String(),
                        WorkDays = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ManagerAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReviewId = c.Int(nullable: false),
                        Text = c.String(nullable: false, maxLength: 255),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientReviews", t => t.ReviewId, cascadeDelete: true)
                .Index(t => t.ReviewId);
            
            CreateTable(
                "dbo.OrderedDishes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        DishId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        OrderedPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dishes", t => t.DishId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.DishId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ReservationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reservations", t => t.ReservationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ReservationId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TableId = c.Int(nullable: false),
                        FirstTime = c.DateTime(nullable: false),
                        LastTime = c.DateTime(nullable: false),
                        Day = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tables", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        SeatsNumber = c.Int(nullable: false),
                        Top = c.Int(nullable: false),
                        Left = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderedDishes", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "ReservationId", "dbo.Reservations");
            DropForeignKey("dbo.Reservations", "TableId", "dbo.Tables");
            DropForeignKey("dbo.OrderedDishes", "DishId", "dbo.Dishes");
            DropForeignKey("dbo.ManagerAnswers", "ReviewId", "dbo.ClientReviews");
            DropForeignKey("dbo.DiscountActions", "DishId", "dbo.Dishes");
            DropForeignKey("dbo.ClientReviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.AmountExcessActions", "DishId", "dbo.Dishes");
            DropIndex("dbo.Reservations", new[] { "TableId" });
            DropIndex("dbo.Orders", new[] { "ReservationId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.OrderedDishes", new[] { "DishId" });
            DropIndex("dbo.OrderedDishes", new[] { "OrderId" });
            DropIndex("dbo.ManagerAnswers", new[] { "ReviewId" });
            DropIndex("dbo.DiscountActions", new[] { "DishId" });
            DropIndex("dbo.ClientReviews", new[] { "UserId" });
            DropIndex("dbo.AmountExcessActions", new[] { "DishId" });
            DropTable("dbo.Tables");
            DropTable("dbo.Reservations");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderedDishes");
            DropTable("dbo.ManagerAnswers");
            DropTable("dbo.Employees");
            DropTable("dbo.DiscountActions");
            DropTable("dbo.Users");
            DropTable("dbo.ClientReviews");
            DropTable("dbo.Dishes");
            DropTable("dbo.AmountExcessActions");
        }
    }
}
