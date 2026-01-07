using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;
using PCPMetalurgicaInter.Services;

namespace PCPMetalurgicaInter.Controllers;

public class OrdemDeProducaoController : Controller
{
    private readonly OrdemDeProducaoService _service;
    private readonly PecaService _pecaservice;
    private readonly ApontamentoService _apontamentoservice;
    private readonly PCPService _pcpservice;
    public OrdemDeProducaoController(OrdemDeProducaoService service, PecaService pecaService,
    ApontamentoService apontamentoservice, PCPService pcpservice)
    {
        _service = service;
        _pecaservice = pecaService;
        _apontamentoservice = apontamentoservice;
        _pcpservice = pcpservice;
    }
    [HttpGet]
    public IActionResult Consultar()
    {
        var Lista = _service.GetAll();
        return View(Lista);
    }
    [HttpGet]
    public IActionResult Emitir()
    {
        ViewBag.Id = _service.GetAll().Count() + 1;
        ViewBag.SelectPeca = _pecaservice.GetAll().Select(peca => new SelectListItem
        {
            Text = peca.Descricao,
            Value = peca.Id.ToString()
        });
        return View("Cadastrar");
    }
    [HttpPost]
    public IActionResult Emitir(OrdemDeProducao x)
    {
        try
        {
            x.Fechada = false;
            x.Cancelada = false;
            x.Peca = _pecaservice.GetById(x.PecaId);
            x.PCPEmissorId = HttpContext.Session.GetInt32("PCPId").Value;
            bool resp = _service.Insert(x);
            TempData["Sucesso"] = "Ordem aberta com sucesso";
            return RedirectToAction("Consultar");
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction();
        }
    }
    [HttpGet]
    [Route("OrdemDeProducao/Editar/{id}")]
    public IActionResult Editar(string id)
    {
        if (!int.TryParse(id, out var idOP))
        {
            TempData["Erro"] = "O ID fornecido não é valido.";
            return RedirectToAction("Consultar");
        }
        try
        {
            var OP = _service.GetById(idOP);
            ViewBag.SelectPeca = _pecaservice.GetAll().Select(tipo => new SelectListItem
            {
                Text = tipo.Descricao,
                Value = tipo.Id.ToString()
            });
            return View(OP);

        } catch (Exception ex) 
        {
            TempData["Erro"] = ex.Message;
            return RedirectToAction("Consultar");
        }
    }
    [HttpPost]
    public IActionResult Fechar(OrdemDeProducao x)
    {
        try
        {
            var resp = _service.Fechar(x);
            TempData["Sucesso"] = "Op fechada com sucesso";
            return RedirectToAction("Consultar");
        }
        catch (Exception e)
        {
            TempData["Erro"] = e.Message;
            return RedirectToAction("Editar", "OrdemDeProducao", new { id = x.Id });
        }
    }
    [HttpPost]
    public IActionResult Cancelar(OrdemDeProducao x)
    {
        try
        {
            var resp = _service.Cancelar(x);
            TempData["Sucesso"] = "Op cancelada com sucesso";
            return RedirectToAction("Consultar");
        }
        catch (Exception e)
        {
            TempData["Erro"] = e.Message;
            return RedirectToAction("Editar", "OrdemDeProducao", new { id = x.Id });
        }
    }
    [HttpGet]
    public IActionResult Imprimir(int id)
    {
        var x = _service.GetByIdToPrint(id);
        return View(x);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
