namespace CodeEditorApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class objectives : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Objectives");
        }
    }
}
