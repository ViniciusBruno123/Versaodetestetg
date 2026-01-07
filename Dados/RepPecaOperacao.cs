using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCPMetalurgicaInter.Models;
namespace PCPMetalurgicaInter.Dados
{
    public class RepPecaOperacao
    {
        private BancoContext _context;
        public RepPecaOperacao(BancoContext context)
        {
            this._context = context;
        }
        public virtual List<PecaOperacao> GetByPeca(Peca peca)
        {
            var Lista = 
            from PecaOperacao in _context.PecaOperacoes 
            where (PecaOperacao.Peca == peca)
            select PecaOperacao;
            return Lista.OrderBy(t=>t.etapa).ToList();
        }
        public virtual PecaOperacao GetByEtapa(int etapa, Peca peca)
        {
            var Lista = 
            from PecaOperacao in _context.PecaOperacoes 
            where (PecaOperacao.Peca == peca)
            select PecaOperacao;
            PecaOperacao? y = null;
            foreach (var pecaOperacao in Lista)
            {
                if (pecaOperacao.etapa == etapa)
                {
                    y = pecaOperacao;
                }
            }
            if (y == null)
            {
                throw new ArgumentException("Erro: nenhuma etapa foi selecionada para esta peça");
            }
            return y;
        }
        public virtual void Delete(PecaOperacao x)
        {
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhuma peça-operação encontrada nesses ids");
            }
            _context.PecaOperacoes.Remove(x);
        }
        public virtual void Update(PecaOperacao x)
        {
            _context.Entry(x).State = EntityState.Modified;
        }
        public virtual void Insert(PecaOperacao x)
        {
            _context.PecaOperacoes.Add(x);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}