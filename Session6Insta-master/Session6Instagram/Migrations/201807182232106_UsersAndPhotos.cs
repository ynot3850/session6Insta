namespace Session6Instagram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersAndPhotos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Picture = c.String(),
                        Date = c.DateTime(nullable: false),
                        Caption = c.String(),
                        PhotoUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.PhotoUser_Id)
                .Index(t => t.PhotoUser_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "PhotoUser_Id", "dbo.Users");
            DropIndex("dbo.Photos", new[] { "PhotoUser_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Photos");
        }
    }
}
