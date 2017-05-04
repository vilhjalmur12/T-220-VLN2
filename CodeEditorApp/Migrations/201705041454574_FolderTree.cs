namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FolderTree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RootFolders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Files", "HeadFolderID", c => c.Int(nullable: false));
            AddColumn("dbo.Folders", "ProjectID", c => c.Int(nullable: false));
            AddColumn("dbo.Folders", "HeadFolderID", c => c.Int(nullable: false));
            AddColumn("dbo.Folders", "IsSolutionFolder", c => c.Boolean(nullable: false));
            AddColumn("dbo.Projects", "HeadFolderID", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "SolutionFolderID", c => c.Int(nullable: false));
            DropColumn("dbo.Files", "location");
            DropColumn("dbo.Files", "FolderID");
            DropColumn("dbo.Projects", "FolderID");
            DropTable("dbo.SubFolders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubFolders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AspNetUserID = c.String(),
                        FolderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Projects", "FolderID", c => c.Int(nullable: false));
            AddColumn("dbo.Files", "FolderID", c => c.Int(nullable: false));
            AddColumn("dbo.Files", "location", c => c.String());
            DropColumn("dbo.Projects", "SolutionFolderID");
            DropColumn("dbo.Projects", "HeadFolderID");
            DropColumn("dbo.Folders", "IsSolutionFolder");
            DropColumn("dbo.Folders", "HeadFolderID");
            DropColumn("dbo.Folders", "ProjectID");
            DropColumn("dbo.Files", "HeadFolderID");
            DropTable("dbo.RootFolders");
        }
    }
}
