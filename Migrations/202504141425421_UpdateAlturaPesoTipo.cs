namespace LigaTabajara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAlturaPesoTipo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Jogadors", "Altura", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Jogadors", "Peso", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jogadors", "Peso", c => c.Single(nullable: false));
            AlterColumn("dbo.Jogadors", "Altura", c => c.Single(nullable: false));
        }
    }
}
