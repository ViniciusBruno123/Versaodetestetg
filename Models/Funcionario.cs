using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PCPMetalurgicaInter.Models
{
    public class Funcionario
    {
        public required int Id { get; set; }
        public required string Nome { get; set; }
        public virtual PCP? PCP { get; }
        public int PCPId { get; }
        public virtual Operador? Operador { get; }
        public int OperadorId { get; }
        public required string Login { get; set; }
        public required string Senha { get; set; }
    }
}