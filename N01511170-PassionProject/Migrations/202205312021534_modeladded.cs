namespace N01511170_PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modeladded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        AlbumId = c.Int(nullable: false, identity: true),
                        AlbumName = c.String(),
                        Createdby = c.String(),
                        RelaseDate = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.AlbumId);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        SongId = c.Int(nullable: false, identity: true),
                        SongName = c.String(),
                        SingerName = c.String(),
                        ReleaseDate = c.DateTime(nullable: false, storeType: "date"),
                        Language = c.String(),
                    })
                .PrimaryKey(t => t.SongId);
            
            CreateTable(
                "dbo.SongAlbums",
                c => new
                    {
                        Song_SongId = c.Int(nullable: false),
                        Album_AlbumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_SongId, t.Album_AlbumId })
                .ForeignKey("dbo.Songs", t => t.Song_SongId, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_AlbumId, cascadeDelete: true)
                .Index(t => t.Song_SongId)
                .Index(t => t.Album_AlbumId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SongAlbums", "Album_AlbumId", "dbo.Albums");
            DropForeignKey("dbo.SongAlbums", "Song_SongId", "dbo.Songs");
            DropIndex("dbo.SongAlbums", new[] { "Album_AlbumId" });
            DropIndex("dbo.SongAlbums", new[] { "Song_SongId" });
            DropTable("dbo.SongAlbums");
            DropTable("dbo.Songs");
            DropTable("dbo.Albums");
        }
    }
}
