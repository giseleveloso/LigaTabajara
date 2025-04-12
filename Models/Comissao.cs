using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LigaTabajara.Models
{
    public class Comissao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Cargo Cargo { get; set; }
        public DateTime DataNascimento { get; set; }
        public Time Time { get; set; }
        public int TimeId { get; set; }
    }
}