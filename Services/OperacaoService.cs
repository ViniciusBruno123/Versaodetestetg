using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;

namespace PCPMetalurgicaInter.Services
{
    public class OperacaoService
    {
        public required RepOperacao _repository;
        public OperacaoService(RepOperacao repository)
        {
            _repository = repository;
        }

        public List<Operacao> GetAll()
        {
            return _repository.GetAll();
        }
        public Operacao GetById(int? id)
        {
            if (id == null || id == 0)
            {
                throw new ArgumentException("Erro: Nenhum id inserido");
            }
            try
            {
                var x = _repository.GetById(id.Value);
                return x;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }
        public bool Insert(Operacao x)
        {
            if (x == null || string.IsNullOrEmpty(x.Descricao) || x.TipoDeOperacaoId == null)
            {
                throw new ArgumentException("Erro: operação não preenchida corretamente!");
            }
            var y = _repository.GetAll();
            foreach (var item in y)
            {
                if (x.Descricao == item.Descricao)
                {
                    throw new ArgumentException("Erro: já há uma operação cadastrada com essa descrição!");
                }
            }
            _repository.Insert(x);
            _repository.Save();
            return true;
        }
        public bool Update(Operacao x)
        {
            if (x == null || x.Id == 0 || string.IsNullOrEmpty(x.Descricao))
                return false;

            var xx = _repository.GetById(x.Id);
            if (xx == null)
                return false;

            xx.Descricao = x.Descricao;
            xx.TipoDeOperacao = x.TipoDeOperacao;

            _repository.Update(xx);
            _repository.Save();
            return true;
        }
        public bool Delete(int? id)
        {
            if (id == null || id == 0)
                return false;

            var xx = _repository.GetById(id.Value);
            if (xx == null)
                return false;

            _repository.Delete(id.Value);
            _repository.Save();
            return true;
        }
    }
}