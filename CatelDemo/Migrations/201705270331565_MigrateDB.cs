namespace RestaurantHelper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AmountExcessActions", newName: "BonusActions");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.BonusActions", newName: "AmountExcessActions");
        }
    }
}
