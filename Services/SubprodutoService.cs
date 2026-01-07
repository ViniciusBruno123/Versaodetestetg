using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;

namespace PCPMetalurgicaInter.Services
{
    public class SubprodutoService
    {
        public required RepSubproduto _repository;
        public SubprodutoService(RepSubproduto repository)
        {
            _repository = repository;
        }
        public List<Subproduto> GetByPeca(Peca peca)
        {
            return _repository.GetByPeca(peca);
        }
        public bool Insert(Subproduto x)
        {
            bool resp = false;

            if(x == null)
            {
                throw new ArgumentException("Erro: a entidade Subproduto não pode ser inicialiada");
            }

            if (x.PecaId == null)
            {
                throw new ArgumentException("Erro: Nenhum id inserido");
            }
            _repository.Insert(x);
            _repository.Save();
            
            resp = true;
            return resp;
        }
            
        public bool Update(Subproduto x) 
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
        public bool Delete(Subproduto x)
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