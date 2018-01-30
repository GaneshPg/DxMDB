namespace DxMDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPosterURLToMovie : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Movies", "posterUrl");
            AddColumn("dbo.Movies", "PosterUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "PosterUrl");
        }
    }
}
