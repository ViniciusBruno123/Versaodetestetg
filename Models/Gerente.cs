using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCPMetalurgicaInter.Models
{
    public class Gerente
    {
        public required int Id { get; set; }
        public required virtual PCP Pcp{ get; set; }
    }
}