using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PCPMetalurgicaInter.Models;
using Microsoft.EntityFrameworkCore;

namespace PCPMetalurgicaInter.Dados
{
    public class RepApontamento
    {
        private BancoContext _context;
        public RepApontamento(BancoContext context)
        {
            this._context = context;
        }
        public virtual List<Apontamento> GetByOp(int idOp)
        {
            var Lista = 
            from Apontamento in _context.Apontamentos
            where (Apontamento.OrdemDeProducaoId == idOp)
            select Apontamento;
            return Lista.Include(t=>t.Operador).Include(t=>t.Operacao).OrderBy(t=>t.Inicio).ToList();
        }
        public virtual Apontamento GetById(int id)
        {
            var x = _context.Apontamentos.Include(t=>t.Operador).Include(t=>t.Operacao)
            .FirstOrDefault(t=>t.Id == id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhum apontamento encontrado para este id");
            }
            return x;
        }
        public virtual void Delete(int id)
        {
            var x = _context.Apontamentos.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhum apontamento encontrado para este id");
            }
            _context.Apontamentos.Remove(x);
        }
        public virtual void Update(Apontamento x)
        {
            _context.Entry(x).State = EntityState.Modified;
        }
        public virtual void Insert(Apontamento x)
        {
            if (x.Quantidade != null)
            {
                throw new ArgumentException("Erro: quantidade processada só pode ser informada no fechamento");
            }
            _context.Apontamentos.Add(x);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
 }
