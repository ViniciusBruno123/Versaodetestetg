using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;

namespace PCPMetalurgicaInter.Services
{
    public class PecaOperacaoService
    {
        public required RepPecaOperacao _repository;
        public PecaOperacaoService(RepPecaOperacao repository)
        {
            _repository = repository;
        }
        public List<PecaOperacao> GetByPeca(Peca peca)
        {
            return _repository.GetByPeca(peca);
        }
        public PecaOperacao GetByEtapa(int? etapa, Peca? peca)
        {
            if (peca == null)
            {
                throw new ArgumentException("Erro: nenhuma peça foi selecionada"); 
            }
            if (etapa == null)
            {
                throw new ArgumentException("Erro: nenhuma etapa foi selecionada");
            }

            return _repository.GetByEtapa(etapa.Value, peca); 
            
        }
        public bool Insert(PecaOperacao x)
        {
            bool resp = false;

            if(x == null)
            {
                throw new ArgumentException("Erro: a entidade Peca-Operacao não pode ser inicialiada");
            }

            if (x.Peca == null)
            {
                throw new ArgumentException("Erro: Nenhum id inserido");
            }

            if (x.Peca.PecaOperacoes != null)
            {
                if (_repository.GetByPeca(x.Peca).Any(op => op.etapa == x.etapa))
                {
                    throw new ArgumentException("Erro: já existe operação registrada nesta etapa para esta peça");
                }
            }
            
            _repository.Insert(x);
            _repository.Save();
            
            resp = true;
            return resp;
        }
            
        public bool Update(PecaOperacao x) 
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
        public bool Delete(PecaOperacao x)
        {
            bool resp = false;
            if (x != null)
            {
                _repository.Delete(x);
                _repository.Save();
                resp = true;
            }
            return resp;
        }
    }
}