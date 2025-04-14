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
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }
        public string Nacionalidade { get; set; }
        public Posicao Posicao { get; set; }
        [Display(Name = "Número da camisa")]
        public int NumCamisa { get; set; }
        [Display(Name = "Altura")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Altura { get; set; }

        [Display(Name = "Peso")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Peso { get; set; }
        [Display(Name = "Pé preferido")]
        public Pe PePreferido { get; set; }
        [Display(Name = "Time")]
        public int TimeId { get; set; }
        [Display(Name = "Time")]
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