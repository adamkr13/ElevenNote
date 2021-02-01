namespace ElevenNote.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedModelBuilder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Note", "CategoryId", "dbo.Category");
            AddColumn("dbo.Note", "Category_CategoryId", c => c.Int());
            CreateIndex("dbo.Note", "Category_CategoryId");
            AddForeignKey("dbo.Note", "Category_CategoryId", "dbo.Category", "CategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Note", "Category_CategoryId", "dbo.Category");
            DropIndex("dbo.Note", new[] { "Category_CategoryId" });
            DropColumn("dbo.Note", "Category_CategoryId");
            AddForeignKey("dbo.Note", "CategoryId", "dbo.Category", "CategoryId");
        }
    }
}
