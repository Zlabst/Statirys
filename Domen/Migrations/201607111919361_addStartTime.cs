namespace Domen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStartTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Start", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Start");
        }
    }
}
