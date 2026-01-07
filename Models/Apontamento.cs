using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCPMetalurgicaInter.Models
{
    public class Apontamento
    {
        public required int Id { get; set; }
        public virtual OrdemDeProducao? OrdemDeProducao { get; set; }
        public required int OrdemDeProducaoId { get; set; }
        public virtual Operador? Operador{ get; set; }
        public required int OperadorId { get; set; }
        public virtual Operacao? Operacao { get; set; }
        public required int OperacaoId { get; set; }
        public int? Quantidade { get; set; }
        public required DateTime Inicio { get; set; }
        public DateTime? Fim { get; set; }
    }
}   