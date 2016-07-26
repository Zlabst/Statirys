namespace Domen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMedia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Media", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Media");
        }
    }
}
