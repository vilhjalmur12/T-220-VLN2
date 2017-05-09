namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Extension = c.String(),
                        Details = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Files", "FileType_ID", c => c.Int());
            CreateIndex("dbo.Files", "FileType_ID");
            AddForeignKey("dbo.Files", "FileType_ID", "dbo.FileTypes", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "FileType_ID", "dbo.FileTypes");
            DropIndex("dbo.Files", new[] { "FileType_ID" });
            DropColumn("dbo.Files", "FileType_ID");
            DropTable("dbo.FileTypes");
        }
    }
}
