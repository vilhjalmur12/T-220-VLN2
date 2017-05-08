namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileUpdate_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "Content", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "Content");
        }
    }
}
