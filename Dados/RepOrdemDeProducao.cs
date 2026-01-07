using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PCPMetalurgicaInter.Models;
using Microsoft.EntityFrameworkCore;
namespace PCPMetalurgicaInter.Dados
{
    public class RepOrdemDeProducao
    {
        private BancoContext _context;
        public RepOrdemDeProducao(BancoContext context)
        {
            this._context = context;
        }
        public virtual List<OrdemDeProducao> GetAll()
        {
            return _context.OPs.Include(t => t.Apontamentos.OrderBy(o => o.Inicio))
            .ThenInclude(st => st.Operacao)
            .Include(t => t.PCPEmissor).ThenInclude(st => st.Funcionario)
            .Include(t=>t.Peca)
            .OrderBy(t=>t.DataEmissao).ToList();
        }
        public virtual OrdemDeProducao GetById(int id)
        {
            var x = _context.OPs.Include(t => t.Peca)
                .Include(t => t.PCPEmissor).ThenInclude(st => st.Funcionario)
                .Include(t => t.Apontamentos).ThenInclude(st => st.Operacao).ThenInclude(sst => sst.TipoDeOperacao)
                .Include(t => t.Apontamentos).ThenInclude(st => st.Operador).ThenInclude(sst => sst.Funcionario)
                .FirstOrDefault(t => t.Id == id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhuma ordem de produção encontrada para este id");
            }
            return x;
        }
        public virtual OrdemDeProducao GetByIdToPrint(int id)
        {
            var x = _context.OPs.Include(t => t.Peca).ThenInclude(st => st.PecaOperacoes)
            .ThenInclude(sst => sst.Operacao).ThenInclude(ssst => ssst.TipoDeOperacao)
            .Include(t => t.PCPEmissor)
            .ThenInclude(st => st.Funcionario)
            .FirstOrDefault(t=>t.Id == id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhuma ordem de produção encontrada para este id");
            }
            return x;
        }
        public virtual void Delete(int id)
        {
            var x = _context.OPs.Find(id);
            if (x == null)
            {
                throw new ArgumentException("Erro: nenhuma ordem de produção encontrada para este id");
            }
            if (x.Apontamentos != null && x.Apontamentos.Count > 0)
            {
                throw new ArgumentException("Erro: ordem de produção já possui apontamentos, impossível deletar");
            }
            _context.OPs.Remove(x);
        }
        public virtual void Update(OrdemDeProducao x)
        {
            _context.Entry(x).State = EntityState.Modified;
        }
        public virtual void Insert(OrdemDeProducao x)
        {
            _context.OPs.Add(x);
        }

        public virtual void Cancelar(OrdemDeProducao x)
        {
            _context.Attach(x);
            _context.Entry(x).Property(o => o.Cancelada).IsModified = true;
        }

        public virtual void Fechar(OrdemDeProducao x)
        {
            _context.Attach(x);
            _context.Entry(x).Property(o => o.Fechada).IsModified = true;
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}