using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LigaTabajara.Models
{
    public class Partida
    {
        public int Id { get; set; }
        public int Rodada { get; set; }
        [Display(Name = "Data e Hora")]
        public DateTime DataHora { get; set; }
        [Display(Name = "Time da Casa")]
        public int TimeCasaId { get; set; }
        public Time TimeCasa { get; set; }
        [Display(Name = "Time Visitante")]
        public int TimeVisitanteId { get; set; }
        public Time TimeVisitante { get; set; }
        [Display(Name = "Gols time da casa")]
        public int GolsTimeCasa { get; set; }
        [Display(Name = "Gols time visitante")]
        public int GolsTimeVisitante { get; set; }
        public ICollection<Gol> Gols { get; set; }

    }
}