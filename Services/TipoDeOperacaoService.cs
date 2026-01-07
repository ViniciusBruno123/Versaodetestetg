using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;

namespace PCPMetalurgicaInter.Services
{
    public class TipoDeOperacaoService //:ITipoDeOperacaoService
    {
        public required RepTipoDeOperacao _repository;
        public TipoDeOperacaoService(RepTipoDeOperacao repository)
        {
            _repository = repository;
        }

        public List<TipoDeOperacao> GetAll()
        {
            return _repository.GetAll();
        }
        public TipoDeOperacao GetById(int? id)
        {
            if (id == null)
            {
                throw new ArgumentException("Erro: Nenhum id inserido");
            }

            return _repository.GetById(id);
        }
        public bool Insert(TipoDeOperacao x)
        {
            if (x == null || string.IsNullOrWhiteSpace(x.Descricao))
            {
                throw new ArgumentException("Erro: Nenhum descri��o inserida");
            }

            _repository.Insert(x);
            _repository.Save();
            return true;
        }
        public bool Update(TipoDeOperacao x)
        {
            if (x == null || x.Id == 0 || string.IsNullOrWhiteSpace(x.Descricao))
                return false;

            var xx = _repository.GetById(x.Id);
            if (xx == null)
                return false;

            xx.Descricao = x.Descricao;

            _repository.Update(xx);
            _repository.Save();
            return true;
        }
        public bool Delete(int? id)
        {
            if (id == null)
                return false;

            var xx = _repository.GetById(id);
            if (xx == null)
                return false;

            _repository.Delete(id.Value);
            _repository.Save();
            return true;
        }
    }
}