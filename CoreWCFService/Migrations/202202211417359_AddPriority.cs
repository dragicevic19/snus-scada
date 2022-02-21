namespace CoreWCFService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriority : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlarmDTOes", "Priority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AlarmDTOes", "Priority");
        }
    }
}
