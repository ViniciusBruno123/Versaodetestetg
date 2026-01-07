using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;

namespace PCPMetalurgicaInter.Services
{
    public class OperadorService
    {
        public required RepOperador _repository;
        public OperadorService(RepOperador repository)
        {
            _repository = repository;
        }
        public List<Operador> GetAll()
        {
            return _repository.GetAll();
        }
          public Operador GetById(int? id)
        {
            if (id != null)
            {
            return _repository.GetById(id.Value);  
            }
            throw new ArgumentException("Erro: Nenhum id inserido");
        }
        public bool Insert(Operador x)
        {
            bool resp = false;
            if(x != null)
            {
                _repository.Insert(x);
                _repository.Save();
                resp = true;
            }
            return resp;
        }
        public bool Update(Operador x) 
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