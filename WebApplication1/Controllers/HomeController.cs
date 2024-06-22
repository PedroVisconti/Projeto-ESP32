using ESP32.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ESP32.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult Autenticar(string username, string password)
        {
            Usuario usuario = new Usuario();
            if(usuario.realizarLogin(username, password)){
                return RedirectToAction("Acessado", "Principal");
            }
            ViewBag.ErrorMessage = "Usuário ou senha inválidos";
            return View("Login");
        }

        public IActionResult Cadastrar(Usuario usuario)
        {
            Console.WriteLine(usuario.data_nascimento);
            if (usuario.inserirUsuario())
            {
                return View("Index");
            }
            ViewBag.ErrorMessage = "Usuario não cadastrado";
            return View("Cadastro");
        }
    }
}
