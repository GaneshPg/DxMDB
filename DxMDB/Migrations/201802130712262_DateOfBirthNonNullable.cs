namespace DxMDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateOfBirthNonNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Actors", "DateOfBirth", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Producers", "DateOfBirth", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Producers", "DateOfBirth", c => c.DateTime());
            AlterColumn("dbo.Actors", "DateOfBirth", c => c.DateTime());
        }
    }
}
