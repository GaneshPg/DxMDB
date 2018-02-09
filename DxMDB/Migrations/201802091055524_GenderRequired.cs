namespace DxMDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenderRequired : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actors", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.Producers", "DateOfBirth", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Actors", "Gender", c => c.String(nullable: false));
            AlterColumn("dbo.Producers", "Gender", c => c.String(nullable: false));
            DropColumn("dbo.Actors", "DOB");
            DropColumn("dbo.Producers", "DOB");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Producers", "DOB", c => c.DateTime(nullable: false));
            AddColumn("dbo.Actors", "DOB", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Producers", "Gender", c => c.String());
            AlterColumn("dbo.Actors", "Gender", c => c.String());
            DropColumn("dbo.Producers", "DateOfBirth");
            DropColumn("dbo.Actors", "DateOfBirth");
        }
    }
}
