using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LigaTabajara.Models
{
    public class Time
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public int AnoFundacao { get; set; }
        public string Estadio { get; set; }
        public int CapacidadeEstadio { get; set; }
        public string CorPrimaria { get; set; }
        public string CorSecundaria { get; set; }
        public Status Status { get; set; }

        public ICollection<Jogador> Jogadores { get; set; }
        public ICollection<Comissao> ComissaoTecnica { get; set; }

    }
}