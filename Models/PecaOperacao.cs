using System;

namespace PCPMetalurgicaInter.Models;

public class PecaOperacao
{
    public virtual Peca? Peca { get; set; }
    public required  int PecaId { get; set; }
    public virtual Operacao? Operacao { get; set; }
    public required int OperacaoId{ get; set; }
    public required int etapa  { get; set; }
}
