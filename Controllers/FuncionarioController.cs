using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PCPMetalurgicaInter.Models;
using PCPMetalurgicaInter.Services;

namespace PCPMetalurgicaInter.Controllers;

public class FuncionarioController : Controller
{
    private readonly PCPService _pcpservice;
    private readonly OperadorService _operadorservice;
    private readonly FuncionarioService _funcionarioservice;

    public FuncionarioController(PCPService pcpservice,
    OperadorService operadorservice, FuncionarioService funcionarioservice)
    {
        _pcpservice = pcpservice;
        _operadorservice = operadorservice;
        _funcionarioservice = funcionarioservice;
    }

    [HttpGet]
    public IActionResult Consultar()
    {
        var ListaFunc = _funcionarioservice.GetAll();
        return View(ListaFunc);
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Cadastrar(Funcionario x, int tipo)
    {
        try
        {
            _funcionarioservice.Insert(x);
            if (tipo == 0)
            {
                Operador operador = new Operador
                {
                    Id = _operadorservice.GetAll().Count() + 1,
                    FuncionarioId = x.Id,
                };
                _operadorservice.Insert(operador);
            }
            else if (tipo == 1)
            {
                PCP pCP = new PCP
                {
                    Id = _pcpservice.GetAll().Count + 1,
                    FuncionarioId = x.Id
                };
                _pcpservice.Insert(pCP);
            }
            return RedirectToAction("Consultar");
        }
        catch (ArgumentException ex)
        {
            ViewBag.Erro = ex.Message;
            return RedirectToAction("Cadastrar");
        }
    }   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
