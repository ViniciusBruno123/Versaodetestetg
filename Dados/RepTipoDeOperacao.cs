using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PCPMetalurgicaInter.Models;

namespace PCPMetalurgicaInter.Dados
{
    public class RepTipoDeOperacao
    {
        private BancoContext _context;
        public RepTipoDeOperacao(BancoContext context)
        {
            this._context = context;
        }
        public List<TipoDeOperacao> GetAll()
        {
            return _context.TipoDeOperacoes.OrderBy(t=>t.Id).ToList();
        }
        public virtual TipoDeOperacao GetById(int? id)
        {
            var x = _context.TipoDeOperacoes.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhum tipo de operação encontrado para este id");
            }
            return x;
        }
        public virtual void Delete(int id)
        {
            var x = _context.TipoDeOperacoes.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhum tipo de operação encontrado para este id");
            }
            _context.TipoDeOperacoes.Remove(x);
        }
        public virtual void Update(TipoDeOperacao x)
        {
            _context.TipoDeOperacoes.Entry(x).State = EntityState.Modified;
        }
        public virtual void Insert(TipoDeOperacao x)
        {
            _context.TipoDeOperacoes.Add(x);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}