using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;
using PCPMetalurgicaInter.Services;

namespace PCPMetalurgicaInter.Controllers;

public class ApontamentoController : Controller
{

    //alterado 09/06 16:16
    private readonly ApontamentoService _service;
    private readonly OrdemDeProducaoService _serviceop;
    private readonly OperadorService _serviceoperador;
    private readonly OperacaoService _serviceoperacao;
    public ApontamentoController(ApontamentoService service, OrdemDeProducaoService serviceop,
    OperacaoService serviceoperacao, OperadorService serviceoperador)
    {
        _service = service;
        _serviceop = serviceop;
        _serviceoperacao = serviceoperacao;
        _serviceoperador = serviceoperador;
    }
    [HttpGet]
    public IActionResult Consultar(int? idOp)
    {
        if (idOp != null && idOp != 0)
        {
            try
            {
                OrdemDeProducao op = _serviceop.GetById(idOp);
                List<Apontamento> apontamentos = _service.GetByOp(op);
                ViewBag.ConsultaPorOp = apontamentos;
                return View();
            }
            catch (ArgumentException e)
            {
                ViewBag.Erro = e.Message;
                return View();
            }
        }
        return View();
    }
    [HttpGet]
    public IActionResult Editar(int idApont)
    {
        try
        {
            Apontamento x = _service.GetById(idApont);
            ViewBag.ApontamentoParaEdicao = x;
            return View("Cadastrar");
        }
        catch(ArgumentException e)
        {
            ViewBag.Erro = e.Message;
            return View("Consultar");
        }
    }
    [HttpPut]
    public IActionResult Editar(Apontamento x)
    {
        bool resp = _service.UpdatePCP(x);
        if (!resp)
        {
            ViewBag.ConfirmacaoAberto = "Erro na edição do apontamento";
            return View(x);
        }
        ViewBag.ConfirmacaoAberto = "Apontamento editado com sucesso";
        return View("Consultar", x.OrdemDeProducao.Id);
    }
    [HttpDelete]
    public IActionResult Delete(int idApont)
    {
        Apontamento x = _service.GetById(idApont);
        int y = x.OrdemDeProducao.Id;
        bool resp = _service.Delete(idApont);
        if (!resp)
        {
            ViewBag.ConfirmacaoAberto = "Erro na edição do apontamento";
            return View(x);
        }
        return View("Consulta", y);
    }
    [HttpGet]
    public IActionResult ApontarOperador()
    {
        ViewBag.SelectOperacao = _serviceoperacao.GetAll().Select(tipo => new SelectListItem
        {
            Text = tipo.Descricao,
            Value = tipo.Id.ToString()
        });
        return View();
    }
    [HttpPost]
    public IActionResult ApontarOperador(Apontamento x)
    {
        try
        {
            x.Fim = null;
            int? OperadorId = HttpContext.Session.GetInt32("OperadorId");
            if (!OperadorId.HasValue)
            {
                TempData["Erro"] = "Não foi possivel registrar o apontamento. Verifique se o usuário logado é um operador";
                return RedirectToAction();
            }
            x.OperadorId = OperadorId.Value;
            int OpQtd = _serviceop.GetById(x.OrdemDeProducaoId).Quantidade;
            bool Ativa = true;
            if (_serviceop.GetById(x.OrdemDeProducaoId).Cancelada)
            {
                Ativa = false;
            }
            if (_serviceop.GetById(x.OrdemDeProducaoId).Fechada)
            {
                Ativa = false;
            }
            bool resp = _service.UpdateOrInsertOperador(x, OpQtd, Ativa);
            if (resp)
            {
                TempData["Sucesso"] = "Apontamento fechado com sucesso";
                return RedirectToAction();
            }
            else
            {
                TempData["Sucesso"] = "Apontamento aberto com sucesso";
                return RedirectToAction();
            }
            
        }
        catch (ArgumentException ex)
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
