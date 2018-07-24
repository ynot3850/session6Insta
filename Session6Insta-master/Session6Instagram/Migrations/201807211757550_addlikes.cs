namespace Session6Instagram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlikes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Photo_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Photo_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Likes", "Photo_Id", "dbo.Photos");
            DropIndex("dbo.Likes", new[] { "User_Id" });
            DropIndex("dbo.Likes", new[] { "Photo_Id" });
            DropTable("dbo.Likes");
        }
    }
}
