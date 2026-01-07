using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;
using PCPMetalurgicaInter.Services;
namespace PCPMetalurgicaInter.Controllers;
    
public class PecaController: Controller
{
    private readonly PecaService _service;
    private readonly SubprodutoService _subprodutoservice;
    private readonly OperacaoService _operacaoService;
    private readonly PecaOperacaoService _pecaOperacaoService;

    public PecaController(PecaService service, SubprodutoService subprodutoService,
    OperacaoService operacaoService, PecaOperacaoService pecaOperacaoService)
    {
        _service = service;
        _subprodutoservice = subprodutoService;
        _operacaoService = operacaoService;
        _pecaOperacaoService = pecaOperacaoService;
    }

    [HttpGet]
    public IActionResult Consultar()
    {
        try
        {
            List<Peca> pecaList = _service.GetAll();
            return View(pecaList);
        }
        catch(ArgumentException e)
        {
            TempData["Erro"] = "Peça: " + e.Message;
            return RedirectToAction("Home", "Index");
        }
    }
    [HttpGet]
    public IActionResult Cadastrar()
    {
       var select = _service.GetAll().Select(tipo => new SelectListItem
        {
            Text = tipo.Descricao,
            Value = tipo.Id.ToString()
        });
        ViewBag.SelectPeca = select;
        ViewBag.Id = select.Count() + 1;
        ViewBag.SelectOperacao = _operacaoService.GetAll();
        return View();
    }
    [HttpPost]
    public IActionResult Cadastrar(Peca x)
    {
        if(x.Data_Cadastro < new DateTime(1753, 1, 1) || x.Data_Cadastro > new DateTime(9999, 12, 31))
        {
            TempData["Erro"] = "A data inserida está fora do intervalo permitido.";
            return RedirectToAction("Cadastrar");
        }
        if (string.IsNullOrWhiteSpace(x.Descricao))
        {
            TempData["Erro"] = "A descrição não pode ser nula.";
            return RedirectToAction("Cadastrar");
        }
        bool resp = _service.Insert(x);
        if (!resp)
        {
            TempData["Erro"] = "Erro no cadastro da peça";
            return RedirectToAction("Cadastrar");
        }
        try
        {
            if (x.Subprodutos != null && x.Subprodutos.Count != 0)
            {
                for (int i = 0; i < x.Subprodutos.Count(); i++)
                {
                    var sub = x.Subprodutos[i];
                    try
                    {
                        Subproduto subproduto = new Subproduto()
                        {
                            Peca = x,
                            PecaId = x.Id,
                            PecaSub = _service.GetById(sub.PecaSubId),
                            PecaSubId = sub.PecaSubId,
                            Quantidade = sub.Quantidade
                        };
                        bool respsub = _subprodutoservice.Insert(subproduto);
                    }
                    catch (ArgumentException e)
                    {
                        TempData["Erro"] = "Peça Não Existe: " + e.Message;
                        return RedirectToAction("Cadastrar");
                    }
                }
            }
            if (x.PecaOperacoes != null && x.PecaOperacoes.Count != 0)
            {
                for (int i = 0; i < x.PecaOperacoes.Count(); i++)
                {
                    var idOper = x.PecaOperacoes[i].OperacaoId;
                    try
                    {
                        PecaOperacao pecaOperacao = new PecaOperacao
                        {
                            OperacaoId = idOper,
                            PecaId = x.Id,
                            etapa = x.PecaOperacoes[i].etapa
                        };
                        _pecaOperacaoService.Insert(pecaOperacao);
                    }
                    catch (ArgumentException e)
                    {
                        TempData["Erro"] = "Operação Não Existe: " + e.Message;
                        return RedirectToAction("Cadastrar");
                    }
                }
            }
            TempData["Sucesso"] = "Cadastro realizado com sucesso";
            return RedirectToAction("Consultar");
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction("Cadastrar");
        }
    }
    [HttpGet]
    [Route("Peca/Editar/{idPeca}")]
    public IActionResult Editar(string idPeca)
    {
        if (!int.TryParse(idPeca, out var idPecaIn))
        {
            TempData["Erro"] = "O ID fornecido não é valido.";
            return RedirectToAction("Consultar");
        }
        try
        {
            Peca x = _service.GetById(idPecaIn);
            ViewBag.SelectPeca = _service.GetAll().Select(tipo => new SelectListItem
            {
                Text = tipo.Descricao,
                Value = tipo.Id.ToString()
            });
            ViewBag.SelectOperacao = _operacaoService.GetAll();
            //List<Subproduto> y = _service.GetSubprodutos(idPeca);
            //List<PecaOperacao> z = _pecaOperacaoService.GetByPeca(x);
            //ViewBag.Subprodutos = y;
            //ViewBag.Processos = z;
            return View(x);
        }
        catch(ArgumentException e)
        {
            TempData["Erro"] = e.Message;
            return RedirectToAction("Consultar");
        }
    }
    [HttpPost]
    public async Task<IActionResult> Anexar(Peca x, [FromForm] IFormFile imagem)
    {
        try
        {
            using var ms = new MemoryStream();
            await imagem.CopyToAsync(ms);
            x.DadosImagem = ms.ToArray();
            x.Imagem = imagem.FileName;
            TempData["Sucesso"] = "Imagem anexada com sucesso";
            return View("Editar", x.Id);
        }
        catch
        {
            TempData["Erro"] = "Não foi possível anexar a imagem";
            return View("Editar", x.Id);
        }
    }
    [HttpGet]
    public IActionResult Imprimir(Peca x)
    {
        try
        {
            var z = _service.Imprimir(x);
            return View(z);
        }
        catch (ArgumentException e)
        {
            TempData["Erro"] = e.Message;
            return View("Editar", x.Id);
        }
    }
    [HttpPost]
    public IActionResult Editar(Peca x)
    {
        try
        {
            // Atualiza dados principais da peça
            bool resp = _service.Update(x);
            if (!resp)
            {
                TempData["Erro"] = "Erro na edição da peça";
                return RedirectToAction("Editar", new { idPeca = x.Id });
            }

            // ----- SUBPRODUTOS -----
            var subprodutosAtuais = _subprodutoservice.GetByPeca(x).ToList(); // garantir lista concreta
            var novosSubprodutos = (x.Subprodutos ?? new List<Subproduto>()).ToList(); // garantir lista concreta

            // Copiar IDs para evitar modificação durante iteração
            var idsNovosSub = novosSubprodutos.Select(s => s.PecaSubId).ToHashSet();

            // Remover os que foram excluídos
            var subprodutosParaRemover = subprodutosAtuais
                .Where(existente => !idsNovosSub.Contains(existente.PecaSubId))
                .ToList();

            foreach (var sub in subprodutosParaRemover)
            {
                _subprodutoservice.Delete(sub);
            }

            foreach (var novo in novosSubprodutos)
            {
                var existente = subprodutosAtuais.FirstOrDefault(s => s.PecaSubId == novo.PecaSubId);
                if (existente == null)
                {
                    var novaEntidade = new Subproduto
                    {
                        Peca = x,
                        PecaId = x.Id,
                        PecaSubId = novo.PecaSubId,
                        PecaSub = _service.GetById(novo.PecaSubId),
                        Quantidade = novo.Quantidade
                    };
                    _subprodutoservice.Insert(novaEntidade);
                }
                else if (existente.Quantidade != novo.Quantidade)
                {
                    existente.Quantidade = novo.Quantidade;
                    _subprodutoservice.Update(existente);
                }
            }

            // ----- OPERAÇÕES -----
            var operacoesAtuais = _pecaOperacaoService.GetByPeca(x).ToList();
            var novasOperacoes = x.PecaOperacoes ?? new List<PecaOperacao>();

            // Remove operações que não estão mais associadas
            var operacoesParaRemover = operacoesAtuais
            .Where(o => !novasOperacoes.Any(n => n.OperacaoId == o.OperacaoId))
            .ToList();
            

            foreach (var operacao in operacoesParaRemover)
            {
                _pecaOperacaoService.Delete(operacao);
            }

            // Atualiza ou adiciona novas operações
            foreach (var nova in novasOperacoes)
            {
                var existente = operacoesAtuais.FirstOrDefault(o => o.OperacaoId == nova.OperacaoId);

                if (existente == null)
                {
                    // Nova operação
                    var operacaoEntidade = _operacaoService.GetById(nova.OperacaoId);
                    _pecaOperacaoService.Insert(new PecaOperacao
                    {
                        PecaId = x.Id,
                        Peca = x,
                        OperacaoId = nova.OperacaoId,
                        Operacao = operacaoEntidade,
                        etapa = nova.etapa
                    });
                }
                else if (existente.etapa != nova.etapa)
                {
                    // Atualização de etapa
                    existente.etapa = nova.etapa;
                    _pecaOperacaoService.Update(existente);
                }
            }
            TempData["Sucesso"] = "Peça editada com sucesso";
            return RedirectToAction("Editar", new { idPeca = x.Id });
        }
        catch (Exception e)
        {
            TempData["Erro"] = "Erro inesperado: " + e.Message;
            return RedirectToAction("Editar", new { idPeca = x.Id });
        }
    }
    [HttpDelete]
    public IActionResult DeletarSubproduto(int idPeca, int idSub)
    {
        try
        {
            
            TempData["Sucesso"] = "Subproduto removido com sucesso";
            return RedirectToAction("Editar", new { idPeca = idPeca });
        }
        catch (Exception e)
        {
            TempData["Erro"] = "Erro inesperado: " + e.Message;
            return RedirectToAction("Editar", new { idPeca = idPeca});
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
