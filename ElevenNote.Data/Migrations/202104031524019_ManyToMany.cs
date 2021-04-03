namespace ElevenNote.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Note", "CategoryId", "dbo.Category");
            DropIndex("dbo.Note", new[] { "CategoryId" });
            CreateTable(
                "dbo.CategoryNote",
                c => new
                    {
                        NoteId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NoteId, t.CategoryId })
                .ForeignKey("dbo.Note", t => t.NoteId, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.NoteId)
                .Index(t => t.CategoryId);
            
            DropColumn("dbo.Note", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Note", "CategoryId", c => c.Int());
            DropForeignKey("dbo.CategoryNote", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.CategoryNote", "NoteId", "dbo.Note");
            DropIndex("dbo.CategoryNote", new[] { "CategoryId" });
            DropIndex("dbo.CategoryNote", new[] { "NoteId" });
            DropTable("dbo.CategoryNote");
            CreateIndex("dbo.Note", "CategoryId");
            AddForeignKey("dbo.Note", "CategoryId", "dbo.Category", "CategoryId");
        }
    }
}
