namespace DxMDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTypeForGender : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actors", "Gender", c => c.String(nullable: false));
            AddColumn("dbo.Producers", "Gender", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producers", "Gender");
            DropColumn("dbo.Actors", "Gender");
        }
    }
}
