using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PCPMetalurgicaInter.Models;
using PCPMetalurgicaInter.Services;

namespace PCPMetalurgicaInter.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PCPService _pcpservice;
    private readonly OperadorService _operadorservice;
    private readonly FuncionarioService _funcionarioservice;

    public HomeController(ILogger<HomeController> logger, PCPService pcpservice,
    OperadorService operadorservice, FuncionarioService funcionarioservice)
    {
        _logger = logger;
        _pcpservice = pcpservice;
        _operadorservice = operadorservice;
        _funcionarioservice = funcionarioservice;
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(string Login, string Senha)
    {
        var ListaFuncs = _funcionarioservice.GetAll();
        bool existe = false;
        Funcionario funcionario = null;
        foreach (var func in ListaFuncs)
        {
            if (func.Login == Login)
            {
                existe = true;
                funcionario = func;
                break;
            }
        }
        if (!existe)
        {
            ViewBag.Erro = "Login inexistente";
            return View();
        }
        if (funcionario.Senha == Senha)
        {
            var ListaPCP = _pcpservice.GetAll();
            var ListaOperadores = _operadorservice.GetAll();
            foreach (var PCP in ListaPCP)
            {
                if (PCP.Funcionario == funcionario)
                {
                    HttpContext.Session.SetInt32("FuncionarioId", funcionario.Id);
                    HttpContext.Session.SetInt32("PCPId", PCP.Id);
                    return RedirectToAction("Index");
                }
            }
            foreach (var Operador in ListaOperadores)
            {
                if (Operador.Funcionario == funcionario)
                {
                    HttpContext.Session.SetInt32("FuncionarioId", funcionario.Id);
                    HttpContext.Session.SetInt32("OperadorId", Operador.Id);
                    return RedirectToAction("ApontarOperador", "Apontamento", new { area = "" });
                }
            }
            ViewBag.Erro = "Funcionário não cadastrado como operador ou PCP";
            return View();
        }
        ViewBag.Erro = "Senha incorreta";
        return View();
    }
    public ActionResult Logoff()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", new { area = "" });
    }


    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
