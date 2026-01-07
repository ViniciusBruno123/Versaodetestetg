using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;

namespace PCPMetalurgicaInter.Services
{
    public class ApontamentoService
    {
        public required RepApontamento _repository;
        public ApontamentoService(RepApontamento repository)
        {
            _repository = repository;
        }
        public List<Apontamento> GetByOp(OrdemDeProducao x)
        {
            return _repository.GetByOp(x.Id);
        }
        public Apontamento GetById (int? id)
        {
            if (id != null && id != 0)
            {
                return _repository.GetById(id.Value);  
            }
            throw new ArgumentException("Erro: Nenhum id inserido");
        }
        public bool UpdateOrInsertOperador(Apontamento x,  int OpQtd, bool Ativa)
        {
            //testa se OP está ativa
            if (!Ativa)
            {
                throw new ArgumentException("OP já finalizada ou cancelada!");
            }
            var apontamentos = _repository.GetByOp(x.OrdemDeProducaoId);
            // teste quantidade já apontada para a op
            int quantjafeita = 0;
            foreach (var y in apontamentos)
            {
                if (x.OperacaoId == y.OperacaoId && y.Quantidade != null)
                {
                    quantjafeita += y.Quantidade.Value;
                }
            }
            //teste abrir ou fechar
            //tenta fechar:
            foreach (var y in apontamentos)
            {
                if (x.OperacaoId == y.OperacaoId && x.OperadorId == y.OperadorId)
                {
                    if (y.Fim == null)
                    {
                        if (x.Quantidade == null)
                        {
                            throw new ArgumentException("Impossível fechar apontamento sem quantidade!");
                        }
                        else if (quantjafeita + x.Quantidade > OpQtd)
                        {
                            throw new ArgumentException("A quantidade apontada supera o total da OP!");
                        }
                        else
                        {
                            y.Fim = DateTime.Now;
                            y.Quantidade = x.Quantidade;
                            _repository.Update(y);
                            _repository.Save();
                            return true;
                        }
                    }
                }
            }
            //se não fechar acima, insere novo:
            if (quantjafeita == OpQtd)
            {
                throw new ArgumentException("A quantidade total para esta operação já foi atendida!");
            }
            x.Inicio = DateTime.Now;
            _repository.Insert(x);
            _repository.Save();
            return false;
        }
        public bool UpdatePCP(Apontamento x)
        {
            _repository.Update(x);
            _repository.Save();
            return true;
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