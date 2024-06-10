using ESP32.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESP32.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            /*
            Banco banco = new Banco(Environment.MachineName, "PROJETOESP", "sa", "banco");

            banco.conectarBanco();

            Dispositivo dis = new Dispositivo(1, "Dispositivo1", 12345, -67890);

            MQTT mqtt = new MQTT(dis, "Pedro", "test.mosquitto.org", 1883, "espdash/automacao/sensor");
            */
            // int id, string nome, string sobrenome, string login, string senha, string cpf, DateTime data_nascimento
            Usuario usuario = new Usuario("Pedro");

            return View();
        }
    }
}
