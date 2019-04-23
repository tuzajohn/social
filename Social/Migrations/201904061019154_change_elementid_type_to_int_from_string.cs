namespace Social.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_elementid_type_to_int_from_string : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "ElementId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "ElementId", c => c.String());
        }
    }
}
