using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCPMetalurgicaInter.Models;
namespace PCPMetalurgicaInter.Dados
{
    public class RepSubproduto
    {
        private BancoContext _context;
        public RepSubproduto(BancoContext context)
        {
            this._context = context;
        }
        public virtual List<Subproduto> GetByPeca(Peca peca)
        {
            var Lista = 
            from Subproduto in _context.Subprodutos
            where (Subproduto.Peca == peca)
            select Subproduto;
            return Lista.OrderBy(t=>t.PecaSubId).ToList();
        }
        public virtual void Delete(Subproduto x)
        {
            _context.Subprodutos.Remove(x);
        }
        public virtual void Update(Subproduto x)
        {
            _context.Entry(x).State = EntityState.Modified;
        }
        public virtual void Insert(Subproduto x)
        {
            _context.Subprodutos.Add(x);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}