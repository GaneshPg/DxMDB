namespace DxMDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredForGender : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Actors", "Gender", c => c.String());
            AlterColumn("dbo.Producers", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Producers", "Gender", c => c.String(nullable: false));
            AlterColumn("dbo.Actors", "Gender", c => c.String(nullable: false));
        }
    }
}
