namespace RestaurantHelper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reservations", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "UserId", c => c.Int(nullable: false));
        }
    }
}
