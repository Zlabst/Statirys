namespace Domen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSavingTime : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.Accounts DROP CONSTRAINT DF__Accounts__Start__1FCDBCEB");
            AlterColumn("dbo.Accounts", "Start", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accounts", "Start", c => c.DateTime(nullable: false));
        }
    }
}
