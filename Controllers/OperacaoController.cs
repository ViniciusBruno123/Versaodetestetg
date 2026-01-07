using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCPMetalurgicaInter.Dados;
using PCPMetalurgicaInter.Models;
using PCPMetalurgicaInter.Services;

namespace PCPMetalurgicaInter.Controllers;

public class OperacaoController: Controller
{
    private readonly OperacaoService _service;
    private readonly TipoDeOperacaoService _servicetipo;
    public OperacaoController(OperacaoService service, TipoDeOperacaoService servicetipo)
    {
        _service = service;
        _servicetipo = servicetipo;
    }

    [HttpGet]
    public IActionResult Consultar()
    {
        try
        {
            List<Operacao> operacaoList = _service.GetAll();
            return View(operacaoList);
        }
        catch(ArgumentException e)
        {
            TempData["Erro"] = "Consultar Operações:" + e.Message;
            return RedirectToAction("~/Home/Index");
        }
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        ViewBag.Id = _service.GetAll().Count() + 1;
        ViewBag.TipoDeOperacao = _servicetipo.GetAll().Select(tipo => new SelectListItem
        {
            Text = tipo.Descricao,
            Value = tipo.Id.ToString()
        }).ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Cadastrar(Operacao x)
    {
        try
            {
                _service.Insert(x);
                TempData["Sucesso"] = "Operação cadastrada com sucesso";
                return RedirectToAction();
            }
            catch (ArgumentException e)
            {
                TempData["Erro"] = e.Message;
                return RedirectToAction();
            }
    }
    [HttpGet]
    [Route("Operacao/Editar/{idOper}")]
    public IActionResult Editar(string idOper)
    {
        if (!int.TryParse(idOper, out var idOperacao))
        {
            TempData["Erro"] = "O ID fornecido não é valido.";
            return RedirectToAction("Consultar");
        }
        try
        {
            Operacao x = _service.GetById(idOperacao);
            ViewBag.TipoDeOperacao = _servicetipo.GetAll().Select(tipo => new SelectListItem
            {
                Text = tipo.Descricao,
                Value = tipo.Id.ToString()
            }).ToList();
            return View(x);
        }
        catch(ArgumentException e)
        {
            TempData["Erro"] = e.Message;
            return RedirectToAction("Consultar");
        }
    }
    [HttpPost]
    public IActionResult Editar(Operacao x)
    {
        bool resp = _service.Update(x);
        if (!resp)
        {
            TempData["Erro"] = "Erro na edição da operacao";
            return RedirectToAction("Editar", new { idOper = x.Id });
        }
        TempData["Sucesso"] = "Operação editada com sucesso";
        return RedirectToAction("Consultar");
    }
    [HttpDelete]
    public IActionResult Delete(int idOper)
    {;
        bool resp = _service.Delete(idOper);
        if (!resp)
        {
            TempData["Erro"] = "Erro na exclusão da operação";
            return RedirectToAction("Consultar");
        }
        TempData["Sucesso"] = "Operação excluída com sucesso";
        return RedirectToAction("Consultar");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
