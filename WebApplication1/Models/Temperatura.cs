using Newtonsoft.Json;
using System.Data.SqlClient;

namespace ESP32.Models
{
    public class Temperatura
    {
        int id_temperatura {  get; set; }
        int id_dispositivo { get; set; }
        int temperatura { get; set; }
        DateTime data {  get; set; }


        public Temperatura(int id_dispositivo, int temperatura, DateTime data)
        {
            this.id_dispositivo = id_dispositivo;
            this.temperatura = temperatura;
            this.data = data;
        }

        public Temperatura() { 
        
        }

        public string selectTemperaturaJson()
        {
            Banco banco = new Banco();
            SqlConnection connection;
            try
            {
                List<object> JsonTemperatura = new List<object>();

                using (connection = new SqlConnection(banco.stringConexao()))
                {

                    connection.Open();
                    string query = "select d.id_dispositivo, d.nome, t.temperatura, t.data from temperatura t " +
                        "left join dispositivos d on t.id_dispositivo = d.id_dispositivo where d.excluido <> 1";
                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var temepratura = new
                            {
                                id_dispositivo = reader["id_dispositivo"],
                                nome = reader["nome"],
                                temperatura = reader["temperatura"],
                                data = reader["data"]
                            };
                            JsonTemperatura.Add(temepratura);


                        }
                        reader.Close();
                    }

                    connection.Close();

                }

                string json = JsonConvert.SerializeObject(JsonTemperatura);
                return json;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public void salvarTemperatura()
        {
            Banco banco = new Banco();
            SqlConnection connection;
            try
            {

                using (connection = new SqlConnection(banco.stringConexao()))
                {
                    connection.Open();
                    int dispositivoId = 0;
                    string query = "insert into temperatura (id_dispositivo, temperatura, data) VALUES (@id_dispositivo, @temperatura, @data);";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_dispositivo", this.id_dispositivo);
                    command.Parameters.AddWithValue("@temperatura", this.temperatura);
                    command.Parameters.AddWithValue("@data", DateTime.Now);

                    command.ExecuteNonQuery();

                    Console.WriteLine("SALVO TEMPERATURA");

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao cadastrar temperatura: " + ex.Message);
            }
        }
    }
    
}
