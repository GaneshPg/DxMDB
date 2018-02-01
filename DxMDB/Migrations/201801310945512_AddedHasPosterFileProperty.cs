namespace DxMDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHasPosterFileProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "HasPosterFile", c => c.Boolean(nullable: false));
            AddColumn("dbo.Movies", "HasPosterUrl", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "HasPosterUrl");
            DropColumn("dbo.Movies", "HasPosterFile");
        }
    }
}
