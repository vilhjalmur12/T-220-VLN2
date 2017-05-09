namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "ContentType", c => c.String(maxLength: 100));
            AlterColumn("dbo.Files", "name", c => c.String(maxLength: 255));
            DropColumn("dbo.Files", "content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "content", c => c.String());
            AlterColumn("dbo.Files", "name", c => c.String());
            DropColumn("dbo.Files", "ContentType");
        }
    }
}
