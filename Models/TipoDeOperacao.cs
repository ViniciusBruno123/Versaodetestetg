using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCPMetalurgicaInter.Models
{
    public class TipoDeOperacao
    {
        public required int Id { get; set; }
        public required string Descricao { get; set; }
        public virtual List<Operacao>? Operacoes { get; init; } = new();
    }
}