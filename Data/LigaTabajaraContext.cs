using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LigaTabajara.Data
{
    public class LigaTabajaraDbContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public LigaTabajaraDbContext() : base("name=LigaTabajaraContext")
        {
        }

        public System.Data.Entity.DbSet<LigaTabajara.Models.Gol> Gols { get; set; }

        public System.Data.Entity.DbSet<LigaTabajara.Models.Jogador> Jogadors { get; set; }

        public System.Data.Entity.DbSet<LigaTabajara.Models.Time> Times { get; set; }

        public System.Data.Entity.DbSet<LigaTabajara.Models.Comissao> Comissaos { get; set; }

        public System.Data.Entity.DbSet<LigaTabajara.Models.Partida> Partidas { get; set; }

        public System.Data.Entity.DbSet<LigaTabajara.Models.Classificacao> Classificacaos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Desativa Delete em Cascata para TimeCasa
            modelBuilder.Entity<LigaTabajara.Models.Partida>()
                .HasRequired(p => p.TimeCasa)
                .WithMany()
                .HasForeignKey(p => p.TimeCasaId)
                .WillCascadeOnDelete(false);

            // Desativa Delete em Cascata para TimeVisitante
            modelBuilder.Entity<LigaTabajara.Models.Partida>()
                .HasRequired(p => p.TimeVisitante)
                .WithMany()
                .HasForeignKey(p => p.TimeVisitanteId)
                .WillCascadeOnDelete(false);
        }

    }


}
