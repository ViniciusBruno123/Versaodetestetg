using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCPMetalurgicaInter.Models
{
    public class Subproduto
    {
        public Peca? Peca { get; set; }
        public required int PecaId { get; set; }
        public Peca? PecaSub { get; set; }
        public required int PecaSubId { get; set; }
        public required double Quantidade { get; set; }
    }
}