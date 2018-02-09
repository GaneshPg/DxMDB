namespace DxMDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "YearOfRelease", c => c.Int(nullable: false));
            DropColumn("dbo.Movies", "Yor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "Yor", c => c.Int(nullable: false));
            DropColumn("dbo.Movies", "YearOfRelease");
        }
    }
}
