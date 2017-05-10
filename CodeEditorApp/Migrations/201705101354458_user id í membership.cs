namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useridímembership : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Memberships", "AspNetUserID", c => c.String());
            DropColumn("dbo.Memberships", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Memberships", "UserID", c => c.String());
            DropColumn("dbo.Memberships", "AspNetUserID");
        }
    }
}
