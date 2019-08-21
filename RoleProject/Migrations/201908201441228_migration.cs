namespace RoleProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Aginces", "password");
            DropColumn("dbo.Clients", "password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "password", c => c.String(nullable: false));
            AddColumn("dbo.Aginces", "password", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
