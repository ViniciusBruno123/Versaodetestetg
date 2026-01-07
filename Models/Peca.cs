
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace PCPMetalurgicaInter.Models
{
    public class Peca
    {
        public required int Id { get; set; }
        public required string Descricao { get; set; } 
        public required DateTime Data_Cadastro { get; set; }
        public double Valor { get; set; }
        public required bool Situacao { get; set; }
        public string? Imagem { get; set; }
        public byte[]? DadosImagem { get; set; }
        public string? CodigoUniversal { get; set; }
        public virtual List<OrdemDeProducao>? Ops{ get; set; }
        public virtual List<PecaOperacao>? PecaOperacoes { get; set; }
        public virtual List<Subproduto>? Subprodutos { get; set; } = new();
    }
}