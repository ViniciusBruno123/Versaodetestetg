using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PCPMetalurgicaInter.Models;
namespace PCPMetalurgicaInter.Dados
{
    public class RepOperador
    {
        private BancoContext _context;
        public RepOperador(BancoContext context)
        {
            this._context = context;
        }
        public virtual List<Operador> GetAll()
        {
            return _context.Operadores.OrderBy(t=>t.Id).ToList();
        }
        public virtual Operador GetById(int id)
        {
            var x = _context.Operadores.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhum operador encontrado para este id");
            }
            return x;
        }
        public virtual void Delete(int id)
        {
            var x = _context.Operadores.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhum operador encontrado para este id");
            }
            _context.Operadores.Remove(x);
        }
        public virtual void Update(Operador x)
        {
            _context.Entry(x).State = EntityState.Modified;
        }
        public virtual void Insert(Operador x)
        {
            _context.Operadores.Add(x);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}