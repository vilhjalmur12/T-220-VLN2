namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class projectTOfolder : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Folders", newName: "SubFolders");
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AspNetUserID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Projects", "FolderID", c => c.Int(nullable: false));
            AlterColumn("dbo.SubFolders", "FolderID", c => c.Int(nullable: false));
            DropColumn("dbo.SubFolders", "Location");
            DropColumn("dbo.SubFolders", "SubFolderID");
            DropColumn("dbo.SubFolders", "Discriminator");
            DropColumn("dbo.Projects", "location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "location", c => c.String());
            AddColumn("dbo.SubFolders", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.SubFolders", "SubFolderID", c => c.Int(nullable: false));
            AddColumn("dbo.SubFolders", "Location", c => c.String());
            AlterColumn("dbo.SubFolders", "FolderID", c => c.Int());
            DropColumn("dbo.Projects", "FolderID");
            DropTable("dbo.Folders");
            RenameTable(name: "dbo.SubFolders", newName: "Folders");
        }
    }
}
