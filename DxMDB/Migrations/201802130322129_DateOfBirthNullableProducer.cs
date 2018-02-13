namespace DxMDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateOfBirthNullableProducer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Producers", "DateOfBirth", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Producers", "DateOfBirth", c => c.DateTime(nullable: false));
        }
    }
}
