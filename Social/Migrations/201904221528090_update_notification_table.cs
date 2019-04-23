namespace Social.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_notification_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Count", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Count");
        }
    }
}
