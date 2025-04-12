using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LigaTabajara.Models
{
    public class Gol
    {
        public int Id { get; set; }
        public int JogadorId { get; set; }
        public Jogador Jogador { get; set; }
        public int PartidaId { get; set; }
        public Partida Partida { get; set; }
        public int Minuto { get; set; }
    
    }
}