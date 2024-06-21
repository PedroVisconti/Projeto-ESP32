using ESP32.Models;
using ESP32.Models.ModelsView;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ESP32.Controllers
{
    public class PrincipalController : Controller
    {

        AcessadoViewModel model = new AcessadoViewModel();

        Dispositivo dispositivo = new Dispositivo();

        Luminosidade luminosidade = new Luminosidade();

        Temperatura temperatura = new Temperatura();
        public IActionResult Acessado()
        {

            model.dispositivos = dispositivo.selectDispositivoAll();

            foreach(Dispositivo dispositivo1 in model.dispositivos)
            {
                dispositivo1.conectarMQTT();
            }

            model.JsonLuminosidade = luminosidade.selectLuminosidadeJson();
            model.JsonTemperatura =  temperatura.selectTemperaturaJson();

            Console.WriteLine("luminosidade: " + model.JsonLuminosidade);
            Console.WriteLine("temperatura: " + model.JsonTemperatura);

            return View(model);
        }

        public IActionResult CadastroDispositivos()
        {
            return View();
        }

        public IActionResult EditarDispositivo(Dispositivo dispositivoEditar)
        {
            return View(dispositivoEditar);
        }

        public IActionResult salvarDispositivo(Dispositivo dispositivo)
        {
           bool salvo =  dispositivo.salvarDispositivo(dispositivo);

            if(salvo == true)
            {
               return RedirectToAction("Acessado", "Principal");
            }
            else
            {
                Console.WriteLine("Ocorreu um erro ao salvar o Dispositivo");
                return RedirectToAction("salvarDispositivo", "Principal");
            }
        }
    }
}
