using Newtonsoft.Json;
using System.Data.SqlClient;

namespace ESP32.Models
{
    public class Luminosidade
    {
        int id_luminosidade {  get; set; }
        int id_dispositivo { get; set; }
        int luminosidade { get; set; }
        DateTime data {  get; set; }

        public Luminosidade(int id_dispositivo, int luminosidade, DateTime data)
        {
            this.id_dispositivo = id_dispositivo;
            this.luminosidade = luminosidade;
            this.data = data;
        }

        public Luminosidade() { 

        }

        public string selectLuminosidadeJson()
        {
            Banco banco = new Banco();
            SqlConnection connection;
            try
            {
                List<object> JsonLuminosidade = new List<object>();

                using (connection = new SqlConnection(banco.stringConexao()))
                {

                    connection.Open();
                    string query = "select d.id_dispositivo, d.nome, l.luminosidade, l.data from luminosidade l " +
                        "left join dispositivos d on l.id_dispositivo = d.id_dispositivo";
                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var luminosidade = new
                            {
                                id_dispositivo = reader["id_dispositivo"],
                                nome = reader["nome"],
                                luminosidade = reader["luminosidade"],
                                data = reader["data"]
                            };
                            JsonLuminosidade.Add(luminosidade);


                        }
                        reader.Close();
                    }

                    connection.Close();

                }

                string json = JsonConvert.SerializeObject(JsonLuminosidade);
                return json;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public void salvarLuminosidade()
        {
            Banco banco = new Banco();
            SqlConnection connection;
            try
            {

                using (connection = new SqlConnection(banco.stringConexao()))
                {
                    connection.Open();
                    int dispositivoId = 0;
                    string query = "insert into luminosidade (id_dispositivo, luminosidade, data) VALUES (@id_dispositivo, @luminosidade, @data);";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_dispositivo", this.id_dispositivo);
                    command.Parameters.AddWithValue("@luminosidade", this.luminosidade);
                    command.Parameters.AddWithValue("@data", DateTime.Now);

                    command.ExecuteNonQuery();

                    Console.WriteLine("SALVO LUMINOSIDADE");

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao cadastrar luminosidade: " + ex.Message);
            }
        }

    }
}
