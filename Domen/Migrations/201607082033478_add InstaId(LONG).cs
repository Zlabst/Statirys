namespace Domen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInstaIdLONG : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accounts", "InstaId", c => c.Long(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accounts", "InstaId", c => c.String());
        }
    }
}
