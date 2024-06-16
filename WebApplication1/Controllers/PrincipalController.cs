using ESP32.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ESP32.Controllers
{
    public class PrincipalController : Controller
    {
        public IActionResult Acessado()
        {
            return View();
        }

        public IActionResult CadastroDispositivos()
        {
            return View();
        }

        public bool salvarDispositivo(Dispositivo dispositivo)
        {
            Banco banco = new Banco();
            SqlConnection connection;
            try
            {

                using (connection = new SqlConnection(banco.stringConexao()))
                {
                    connection.Open();
                    int dispositivoId = 0;
                    string query = "INSERT INTO dispositivos (nome, latitude, longitude) VALUES (@nome, @latitude, @longitude); SELECT SCOPE_IDENTITY();";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nome",dispositivo.nome);
                    command.Parameters.AddWithValue("@latitude", dispositivo.latitude);
                    command.Parameters.AddWithValue("@longitude", dispositivo.longitude);

                    dispositivoId = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"ID do dispositivo inserido: {dispositivoId}");

                    if (dispositivoId != 0)
                    {
                        
                        Console.WriteLine("Dispositivo inserido com sucesso. ID: " + dispositivoId);
                        Console.WriteLine("Inserção do dispositivo bem-sucedida!");

                        query = "INSERT INTO MQTT_Dispositivo (id_dispositivo, clienteID, servidor, porta, topico) VALUES " +
                            "(@id_dispositivo, @clienteID, @servidor, @porta, @topico)";
                        SqlCommand command2 = new SqlCommand(query, connection);
                        command2.Parameters.AddWithValue("@id_dispositivo", dispositivoId);
                        command2.Parameters.AddWithValue("@clienteID", dispositivo.clienteID);
                        command2.Parameters.AddWithValue("@servidor", dispositivo.servidor);
                        command2.Parameters.AddWithValue("@porta", dispositivo.porta);
                        command2.Parameters.AddWithValue("@topico", dispositivo.topico);

                        int rowsAffected = command2.ExecuteNonQuery();

                        if(rowsAffected > 0)
                        {
                            Console.WriteLine("inserido dados do MQTT com sucesso");
                            connection.Close();
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Erro ao inserir dados do MQTT");
                            connection.Close();
                            return false;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Falha ao inserir o usuário.");
                        connection.Close();
                        return false;
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao cadastrar usuario: " + ex.Message);
                return false;
            }
        }
    }
}
