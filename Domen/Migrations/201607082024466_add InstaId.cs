namespace Domen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInstaId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "InstaId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "InstaId");
        }
    }
}
