using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LigaTabajara.Models
{
    public class Time
    {
        [Display(Name = "Nome do Time")]
        public int Id { get; set; }
        [Display(Name = "Nome do Time")]
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        [Display(Name = "Ano de Fundação")]
        public int AnoFundacao { get; set; }
        public string Estadio { get; set; }
        [Display(Name = "Capacidade do Estádio")]
        public int CapacidadeEstadio { get; set; }
        [Display(Name = "Cor Primária")]
        public string CorPrimaria { get; set; }
        [Display(Name = "Cor Secundária")]
        public string CorSecundaria { get; set; }
        public Status Status { get; set; }

        public ICollection<Jogador> Jogadores { get; set; }
        public ICollection<Comissao> ComissaoTecnica { get; set; }

    }
}