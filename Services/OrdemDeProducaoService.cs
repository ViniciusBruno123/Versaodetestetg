using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;

namespace PCPMetalurgicaInter.Services
{
    public class OrdemDeProducaoService
    {
        public required RepOrdemDeProducao _repository;
        public OrdemDeProducaoService(RepOrdemDeProducao repository)
        {
            _repository = repository;
        }
        public List<OrdemDeProducao> GetAll()
        {
            var Lista = _repository.GetAll().AsQueryable();
            return Lista.OrderBy(t=>t.DataEmissao).ToList();
        }
        public OrdemDeProducao GetById(int? id)
        {
            if (id != null)
            {
                return _repository.GetById(id.Value);  
            }
            throw new ArgumentException("Erro: Nenhum id inserido");
        }
        public OrdemDeProducao GetByIdToPrint(int? id)
        {
            if (id != null)
            {
                return _repository.GetByIdToPrint(id.Value);
            }
            throw new ArgumentException("Erro: Nenhum id inserido");
        }
        public bool Insert(OrdemDeProducao x)
        {
            bool resp = false;
            x.DataEmissao = DateTime.Now;
            x.Cancelada = false;
            x.Fechada = false;
            if (x == null)
            {
                throw new ArgumentException("Ordem de produção não preenchida corretamente");
            }
            else if (!x.Peca.Situacao)
            {
                throw new ArgumentException("Impossível abrir OP para peça inativa");
            }
            else if (x.Peca.Subprodutos == null || x.Peca.Subprodutos.Count == 0
            || x.Peca.PecaOperacoes == null || x.Peca.PecaOperacoes.Count == 0)
            {
                throw new ArgumentException("Impossível abrir OP para peças sem componentes ou processos");
            }
            else if (x.Cancelada == true || x.Fechada == true)
            {
                throw new ArgumentException("Impossível abrir OP já cancelada ou fechada");
            }
            _repository.Insert(x);
            _repository.Save();
            resp = true;
            return resp;
        }
        public bool Fechar(OrdemDeProducao x)
        {
            if (x.Cancelada == true)
            {
                throw new ArgumentException("Impossível fechar ordem já cancelada");
            }
            if (x.Cancelada == true)
            {
                throw new ArgumentException("Impossível fechar ordem já cancelada");
            }
            x.Cancelada = true;
            _repository.Fechar(x);
            _repository.Save();
            return true;
        }
        public bool Cancelar(OrdemDeProducao x)
        {
            if (x.Fechada == true)
            {
                throw new ArgumentException("Impossível cancelar ordem já finalizada");
            }
            x.Cancelada = true;
            _repository.Cancelar(x);
            _repository.Save();
            return true;
        }
        public bool Update(OrdemDeProducao x)
        {
            bool resp = false;
            if (x != null)
            {
                _repository.Update(x);
                _repository.Save();
                resp = true;
            }
            return resp;
        }
        public bool Delete(int? id)
        {
            bool resp = false;
            if (id != null)
            {
                _repository.Delete(id.Value);
                _repository.Save();
                resp = true;
            }
            return resp;
        }
    }
}