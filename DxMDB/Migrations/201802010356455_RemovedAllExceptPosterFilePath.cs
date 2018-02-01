namespace DxMDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedAllExceptPosterFilePath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "PosterFilePath", c => c.String());
            DropColumn("dbo.Movies", "PosterUrl");
            DropColumn("dbo.Movies", "HasPosterFile");
            DropColumn("dbo.Movies", "HasPosterUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "HasPosterUrl", c => c.Boolean(nullable: false));
            AddColumn("dbo.Movies", "HasPosterFile", c => c.Boolean(nullable: false));
            AddColumn("dbo.Movies", "PosterUrl", c => c.String());
            DropColumn("dbo.Movies", "PosterFilePath");
        }
    }
}
