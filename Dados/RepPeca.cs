using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCPMetalurgicaInter.Models;
namespace PCPMetalurgicaInter.Dados
{
    public class RepPeca
    {
        private BancoContext _context;
        public RepPeca(BancoContext context)
        {
            this._context = context;
        }
        public virtual List<Peca> GetAll()
        {
            return _context.Pecas.OrderBy(t=>t.Id).ToList();
        }
        public virtual List<Subproduto> GetSubprodutos(Peca peca)
        {
            var Lista = 
            from Subproduto in _context.Subprodutos
            where (Subproduto.Peca == peca)
            select Subproduto;
            return Lista.OrderBy(t=>t.PecaSub.Id).ToList();
        }

        public virtual Peca GetById(int id)
        {
            var x = _context.Pecas.Include(t => t.Subprodutos).ThenInclude(st => st.PecaSub)
            .Include(t => t.PecaOperacoes.OrderBy(o => o.etapa))
            .ThenInclude(st=>st.Operacao).ThenInclude(sst=>sst.TipoDeOperacao)
            .FirstOrDefault(t=>t.Id == id);

            if (x == null)
            {
                throw new ArgumentException("Erro: nenhuma peça encontrada para este id");
            }
            return x;
        }
        public virtual string Imprimir(Peca x)
        {
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhuma peça encontrada para este id");
            }
            if (x.Imagem == null)
            {
                throw new ArgumentException("Erro: nenhuma imagem cadastrada para esta peça");
            }
            return x.Imagem;
        }
        public virtual void Delete(int id)
        {
            var x = _context.Pecas.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhuma peça encontrada para este id");
            }
            _context.Pecas.Remove(x);
        }
        public virtual void DeleteListas(Peca x)
        {
            _context.PecaOperacoes.RemoveRange(x.PecaOperacoes);
            _context.Subprodutos.RemoveRange(x.Subprodutos);
        }
        public virtual void Update(Peca x)
        {
            _context.Entry(x).State = EntityState.Modified;
        }
        public virtual void Insert(Peca x)
        {
            _context.Pecas.Add(x);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}