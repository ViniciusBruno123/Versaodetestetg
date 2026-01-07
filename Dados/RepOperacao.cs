using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCPMetalurgicaInter.Models;
namespace PCPMetalurgicaInter.Dados
{
    public class RepOperacao
    {
        private BancoContext _context;
        public RepOperacao(BancoContext context)
        {
            this._context = context;
        }
        public virtual List<Operacao> GetAll()
        {
            return _context.Operacoes.Include(t => t.TipoDeOperacao).OrderBy(t=>t.Id).ToList();
        }
        public virtual Operacao GetById(int id)
        {
            var x = _context.Operacoes.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhuma operação encontrada para este id");
            }
            return x;
        }
        public virtual void Delete(int id)
        {
            var x = _context.Operacoes.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhuma operação encontrada para este id");
            }
            _context.Operacoes.Remove(x);
        }
        public virtual void Update(Operacao x)
        {
            _context.Entry(x).State = EntityState.Modified;
        }
        public virtual void Insert(Operacao x)
        {
            _context.Operacoes.Add(x);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}