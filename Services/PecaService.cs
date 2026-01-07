using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;

namespace PCPMetalurgicaInter.Services
{
    public class PecaService
    {
        public required RepPeca _repository;
        public PecaService(RepPeca repository)
        {
            _repository = repository;
        }
        public List<Peca> GetAll()
        {
            return _repository.GetAll();
        }
        public Peca GetById(int? id)
        {
            if (id != null)
            {
            return _repository.GetById(id.Value);  
            }
            throw new ArgumentException("Erro: Nenhum id inserido");
        }
        public List<Subproduto> GetSubprodutos(int? id)
        {
            if (id != null)
            {
                Peca peca = _repository.GetById(id.Value);
                return _repository.GetSubprodutos(peca);
            }
            throw new ArgumentException("");
        }
        public string Imprimir(Peca x)
        {
            if (x == null)
            {
                throw new ArgumentNullException("Erro: nenhuma peça inserida na requisição");
            }
            if (x.Imagem == null)
            {
                throw new ArgumentException("Erro: A peça não possui imagem cadastrada");
            }
            var y = _repository.Imprimir(x);
            return y;
        }
        public bool Insert(Peca x)
        {
            bool resp = false;
            if (x != null)
            {
                _repository.Insert(x);
                _repository.Save();
                resp = true;
            }
            return resp;
        }
        public bool Update(Peca x) 
        {
            if (x == null || x.Id == 0)
                throw new ArgumentException("Peça inválida.");

            // Carrega do banco a peça completa com os includes
            var antiga = _repository.GetById(x.Id);

            if (antiga == null)
                throw new ArgumentException("Peça não encontrada.");

            // Atualiza os campos básicos
            antiga.Descricao = x.Descricao;
            antiga.CodigoUniversal = x.CodigoUniversal;
            antiga.Situacao = x.Situacao;
            antiga.Valor = x.Valor;
            antiga.Data_Cadastro = x.Data_Cadastro;

            // Remove listas antigas
            _repository.DeleteListas(antiga);

            // Recria Subprodutos válidos
            antiga.Subprodutos = new List<Subproduto>();
            if (x.Subprodutos != null)
            {
                foreach (var sub in x.Subprodutos.Where(s => s.Quantidade > 0))
                {
                    antiga.Subprodutos.Add(new Subproduto
                    {
                        PecaId = antiga.Id,
                        PecaSubId = sub.PecaSubId,
                        Quantidade = sub.Quantidade
                    });
                }
            }

            // Recria Operações válidas
            antiga.PecaOperacoes = new List<PecaOperacao>();
            if (x.PecaOperacoes != null)
            {
                foreach (var op in x.PecaOperacoes.Where(o => o.etapa > 0))
                {
                    antiga.PecaOperacoes.Add(new PecaOperacao
                    {
                        PecaId = antiga.Id,
                        OperacaoId = op.OperacaoId,
                        etapa = op.etapa
                    });
                }
            }

            // Atualiza a entidade rastreada
            _repository.Update(antiga);
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