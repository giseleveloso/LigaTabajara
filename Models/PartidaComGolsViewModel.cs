using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LigaTabajara.Models
{
    public class PartidaComGolsViewModel
    {
        public Partida Partida { get; set; }

        public int GolsTimeCasa { get; set; }
        public int GolsTimeVisitante { get; set; }

        public List<GolInfo> Gols { get; set; }
    }

    public class GolInfo
    {
        public int JogadorId { get; set; }
        public int Minuto { get; set; }
    }
}