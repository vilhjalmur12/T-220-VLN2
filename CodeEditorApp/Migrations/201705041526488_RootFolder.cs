namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RootFolder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RootFolders", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RootFolders", "UserID", c => c.Int(nullable: false));
        }
    }
}
