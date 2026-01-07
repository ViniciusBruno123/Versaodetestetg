using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PCPMetalurgicaInter.Models;
using Microsoft.EntityFrameworkCore;
namespace PCPMetalurgicaInter.Dados
{
    public class RepFuncionario
    {
        private BancoContext _context;
        public RepFuncionario(BancoContext context)
        {
            this._context = context;
        }
        public virtual List<Funcionario> GetAll()
        {
            return _context.Funcionarios.Include(t => t.PCP)
            .Include(t=>t.Operador)
            .OrderBy(t=>t.Id).ToList();
        }
        public virtual Funcionario GetById(int id)
        {
            var x = _context.Funcionarios.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: Nenhum funcionário encontrado para este id");
            }
            return x;
        }
        public virtual void Delete(int id)
        {
            var x = _context.Funcionarios.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: Nenhum funcionário encontrado para este id");
            }
            _context.Funcionarios.Remove(x);
        }
        public virtual void Update(Funcionario x)
        {
            _context.Entry(x).State = EntityState.Modified;
        }
        public virtual void Insert(Funcionario x)
        {
            _context.Funcionarios.Add(x);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}