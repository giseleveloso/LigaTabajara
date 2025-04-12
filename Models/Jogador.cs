using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LigaTabajara.Models
{
    public class Jogador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Nacionalidade { get; set; }
        public Posicao Posicao { get; set; }
        public int NumCamisa { get; set; }
        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
        public Pe PePreferido { get; set; }
        public int TimeId { get; set; }
        public Time Time { get; set; }
        public int Gols { get; set; }
    }

    /*
    public class LigaTabajaraDbContext : DbContext
    {
        public DbSet<Jogador> Jogadores { get; set; }
    }
    */
}