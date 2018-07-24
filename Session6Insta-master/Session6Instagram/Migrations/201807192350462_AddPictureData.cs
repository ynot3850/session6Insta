namespace Session6Instagram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPictureData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "PictureData", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "PictureData");
        }
    }
}
