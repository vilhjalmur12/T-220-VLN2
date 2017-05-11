namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileUpdate1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Files", "Content", c => c.String());
            DropColumn("dbo.Files", "ContentType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "ContentType", c => c.String(maxLength: 100));
            AlterColumn("dbo.Files", "Content", c => c.Binary());
        }
    }
}
