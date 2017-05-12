namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentsogobjectives : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Comments");
            DropTable("dbo.Objectives");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Objectives",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        finished = c.Boolean(nullable: false),
                        AspNetUserID = c.String(),
                        GoalID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        content = c.String(),
                        AspNetUserID = c.String(),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
