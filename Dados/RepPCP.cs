using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PCPMetalurgicaInter.Models;
using Microsoft.EntityFrameworkCore;
namespace PCPMetalurgicaInter.Dados
{
    public class RepPCP
    {
        private BancoContext _context;
        public RepPCP(BancoContext context)
        {
            this._context = context;
        }
        public virtual List<PCP> GetAll()
        {
            return _context.PCPs.OrderBy(t=>t.Id).ToList();
        }
        public virtual PCP GetById(int id)
        {
            var x = _context.PCPs.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhum operador de PCP encontrado para este id");
            }
            return x;
        }
        public virtual void Delete(int id)
        {
            var x = _context.PCPs.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhum operador de PCP encontrado para este id");
            }
            _context.PCPs.Remove(x);
        }
        public virtual void Update(PCP x)
        {
            _context.Entry(x).State = EntityState.Modified;
        }
        public virtual void Insert(PCP x)
        {
            _context.PCPs.Add(x);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}