namespace SneakerDrop.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class baseEntityUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "Name", c => c.String());
        }
    }
}
