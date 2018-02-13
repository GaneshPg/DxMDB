namespace DxMDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateOfBirthNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Actors", "DateOfBirth", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Actors", "DateOfBirth", c => c.DateTime(nullable: false));
        }
    }
}
