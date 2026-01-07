using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;
using PCPMetalurgicaInter.Services;

namespace PCPMetalurgicaInter.Controllers;

public class TipoDeOperacaoController: Controller
{
    private readonly TipoDeOperacaoService _service;
    public TipoDeOperacaoController(TipoDeOperacaoService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Consultar()
    {
        try
        {
            var tipodeoperacaoList = _service.GetAll();
            return View(tipodeoperacaoList);
        }
        catch(ArgumentException e)
        {
            TempData["Erro"] = "Tipo de Operação: "+e.Message;
            return RedirectToAction("~/Home/Index");
        }
    }
    [HttpGet]
    public IActionResult Cadastrar()
    {
    	ViewBag.Id = _service.GetAll().Count() + 1;
        return View();
    }
    [HttpPost]
    public IActionResult Cadastrar(TipoDeOperacao x)
    {
        try
        {
            _service.Insert(x);
            TempData["Sucesso"] = "Tipo de operação cadastrada com sucesso";
            return RedirectToAction();
        }
        catch(ArgumentException e)
        {
            TempData["Erro"] = e.Message;
            return RedirectToAction();
        }
    }
    [HttpGet]
    [Route("TipoDeOperacao/Editar/{idTipoOper}")]
    public IActionResult Editar(string idTipoOper)
    {
        if (!int.TryParse(idTipoOper, out var idTipoOperacao)) 
        {   TempData["Erro"] = "O ID fornecido não é valido."; 
            return RedirectToAction("Consultar"); 
        }
        try
        {
            TipoDeOperacao x = _service.GetById(idTipoOperacao);
            return View(x);
        }
        catch (ArgumentException e)
        {
            TempData["Erro"] = e.Message;
            return RedirectToAction("Consultar");
        }
    }
    [HttpPost]
    public IActionResult Editar(TipoDeOperacao x)
    {
        try
        {
            _service.Update(x);
            TempData["Sucesso"] = "Tipo de operação editado com sucesso";
            return RedirectToAction("Consultar");
        }
        catch (ArgumentException e)
        {
            TempData["Erro"] = e.Message;
            return RedirectToAction("Editar", new { idTipoOper = x.Id });
        }
    }
    [HttpDelete]
    public IActionResult Delete(int idTipoOper)
    {
        bool resp = _service.Delete(idTipoOper);
        if (!resp)
        {
            TempData["Erro"] = "Erro na exclusão do tipo de operação";
            return RedirectToAction("Consultar");
        }
        TempData["Sucesso"] = "Tipo de operação excluído com sucesso";
        return RedirectToAction("Consultar");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
