using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCPMetalurgicaInter.Models
{
    public class OrdemDeProducao
    {
        public required int Id { get; set; }
        public required int Quantidade { get; set; }
        public virtual required DateTime DataEmissao { get; set; }
        public virtual Peca? Peca { get; set; }
        public required int PecaId { get; set; }
        public virtual PCP? PCPEmissor { get; set; }
        public required int PCPEmissorId { get; set; }
        public required bool Fechada { get; set; }
        public required bool Cancelada { get; set; }
        public virtual List<Apontamento>? Apontamentos { get; }
    }
}