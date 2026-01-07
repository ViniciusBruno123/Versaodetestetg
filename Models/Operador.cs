using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCPMetalurgicaInter.Models
{
    public class Operador
    {
        public required int Id { get; set; }
        public virtual Funcionario? Funcionario { get; set; }
        public required int FuncionarioId { get; set; }
        public virtual List<Apontamento>? Apontamentos { get; set; }
    }
}