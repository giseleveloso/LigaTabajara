namespace LigaTabajara.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using LigaTabajara.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<LigaTabajara.Data.LigaTabajaraDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LigaTabajara.Data.LigaTabajaraDbContext context)
        {
            if (context.Times.Any()) return;

            for (int i = 1; i <= 20; i++)
            {
                var time = new Time
                {
                    Nome = $"Time {i}",
                    Cidade = "Cidade " + i,
                    Estado = "Estado " + i,
                    AnoFundacao = 1950 + i,
                    Estadio = "Estádio " + i,
                    CapacidadeEstadio = 30000 + i * 100,
                    CorPrimaria = "Azul",
                    CorSecundaria = "Branco",
                    Status = Status.Inapto, // Será avaliado depois
                    Jogadores = new List<Jogador>(),
                    ComissaoTecnica = new List<Comissao>()
                };

                // Adiciona 30 jogadores
                for (int j = 1; j <= 30; j++)
                {
                    time.Jogadores.Add(new Jogador
                    {
                        Nome = $"Jogador {j} do Time {i}",
                        Posicao = Posicao.Atacante,
                        NumCamisa = j,
                        DataNascimento = new DateTime(1990, 1, 1).AddDays(j)
                    });
                }

                Cargo[] cargos = new[]
                {
                    Cargo.Treinador,
                    Cargo.Auxiliar,
                    Cargo.PreparadorFisico,
                    Cargo.Fisiologista,
                    Cargo.Fisioterapeuta
                };

                for (int k = 0; k < cargos.Length; k++)
                {
                    time.ComissaoTecnica.Add(new Comissao
                    {
                        Nome = $"Profissional {k + 1} do Time {i}",
                        Cargo = cargos[k],
                        DataNascimento = new DateTime(1980, 1, 1).AddYears(k)
                    });
                }

                context.Times.Add(time);
            }

            context.SaveChanges();
        }

 
    }   
}
