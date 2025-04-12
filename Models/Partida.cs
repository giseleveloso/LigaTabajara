using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LigaTabajara.Models
{
    public class Partida
    {
        public int Id { get; set; }
        public int Rodada { get; set; }
        public DateTime DataHora { get; set; }
        public int TimeCasaId { get; set; }
        public Time TimeCasa { get; set; }
        public int TimeVisitanteId { get; set; }
        public Time TimeVisitante { get; set; }
        public int GolsTimeCasa { get; set; }
        public int GolsTimeVisitante { get; set; }
        public ICollection<Gol> Gols { get; set; }

    }
}