namespace LigaTabajara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classificacaos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeId = c.Int(nullable: false),
                        Pontos = c.Int(nullable: false),
                        Vitorias = c.Int(nullable: false),
                        Empates = c.Int(nullable: false),
                        Derrotas = c.Int(nullable: false),
                        GolsPro = c.Int(nullable: false),
                        GolsContra = c.Int(nullable: false),
                        SaldoGols = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Times", t => t.TimeId, cascadeDelete: true)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.Times",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Cidade = c.String(),
                        Estado = c.String(),
                        AnoFundacao = c.Int(nullable: false),
                        Estadio = c.String(),
                        CapacidadeEstadio = c.Int(nullable: false),
                        CorPrimaria = c.String(),
                        CorSecundaria = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comissaos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Cargo = c.Int(nullable: false),
                        DataNascimento = c.DateTime(nullable: false),
                        TimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Times", t => t.TimeId, cascadeDelete: true)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.Jogadors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        DataNascimento = c.DateTime(nullable: false),
                        Nacionalidade = c.String(),
                        Posicao = c.Int(nullable: false),
                        NumCamisa = c.Int(nullable: false),
                        Altura = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Peso = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PePreferido = c.Int(nullable: false),
                        TimeId = c.Int(nullable: false),
                        Gols = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Times", t => t.TimeId, cascadeDelete: true)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.Partidas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rodada = c.Int(nullable: false),
                        DataHora = c.DateTime(nullable: false),
                        TimeCasaId = c.Int(nullable: false),
                        TimeVisitanteId = c.Int(nullable: false),
                        GolsTimeCasa = c.Int(nullable: false),
                        GolsTimeVisitante = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Times", t => t.TimeCasaId)
                .ForeignKey("dbo.Times", t => t.TimeVisitanteId)
                .Index(t => t.TimeCasaId)
                .Index(t => t.TimeVisitanteId);
            
            CreateTable(
                "dbo.Gols",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JogadorId = c.Int(nullable: false),
                        PartidaId = c.Int(nullable: false),
                        Minuto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jogadors", t => t.JogadorId, cascadeDelete: true)
                .ForeignKey("dbo.Partidas", t => t.PartidaId, cascadeDelete: true)
                .Index(t => t.JogadorId)
                .Index(t => t.PartidaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Partidas", "TimeVisitanteId", "dbo.Times");
            DropForeignKey("dbo.Partidas", "TimeCasaId", "dbo.Times");
            DropForeignKey("dbo.Gols", "PartidaId", "dbo.Partidas");
            DropForeignKey("dbo.Gols", "JogadorId", "dbo.Jogadors");
            DropForeignKey("dbo.Classificacaos", "TimeId", "dbo.Times");
            DropForeignKey("dbo.Jogadors", "TimeId", "dbo.Times");
            DropForeignKey("dbo.Comissaos", "TimeId", "dbo.Times");
            DropIndex("dbo.Gols", new[] { "PartidaId" });
            DropIndex("dbo.Gols", new[] { "JogadorId" });
            DropIndex("dbo.Partidas", new[] { "TimeVisitanteId" });
            DropIndex("dbo.Partidas", new[] { "TimeCasaId" });
            DropIndex("dbo.Jogadors", new[] { "TimeId" });
            DropIndex("dbo.Comissaos", new[] { "TimeId" });
            DropIndex("dbo.Classificacaos", new[] { "TimeId" });
            DropTable("dbo.Gols");
            DropTable("dbo.Partidas");
            DropTable("dbo.Jogadors");
            DropTable("dbo.Comissaos");
            DropTable("dbo.Times");
            DropTable("dbo.Classificacaos");
        }
    }
}
