namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lagagoals : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Goals", "goalType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Goals", "goalType", c => c.Int(nullable: false));
        }
    }
}
