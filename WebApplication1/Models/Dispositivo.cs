using System.Data.SqlClient;

namespace ESP32.Models
{
    public class Dispositivo : MQTT
    {
        public int id {  get; set; }
        public string nome { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public int excluido { get; set; }
        /*
        public string clienteID { get; set; }
        public string servidor { get; set; }
        public int porta { get; set; }
        public string topico { get; set; }
        */

        public Dispositivo(string nome, string latitude, string longitude, string clienteID, string servidor, int porta, string topico) : base(clienteID,  servidor,  porta,  topico)
        {
            this.nome = nome;
            this.latitude = latitude;
            this.longitude = longitude;

        }

        public Dispositivo(int id, string nome, string latitude, string longitude, string clienteID, string servidor, int porta, string topico) : base(clienteID, servidor, porta, topico)
        {
            this.id = id;
            this.nome = nome;
            this.latitude = latitude;
            this.longitude = longitude;

        }

        public Dispositivo()
        {

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
                    string query = "INSERT INTO dispositivos (nome, latitude, longitude, excluido) VALUES (@nome, @latitude, @longitude, 0); SELECT SCOPE_IDENTITY();";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nome", dispositivo.nome);
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

                        if (rowsAffected > 0)
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
                Console.WriteLine("Erro ao cadastrar dispositivo: " + ex.Message);
                return false;
            }
        }

        public bool alterDispositivo()
        {
            Banco banco = new Banco();
            SqlConnection connection;
            try
            {

                using (connection = new SqlConnection(banco.stringConexao()))
                {
                    connection.Open();
                    string query = $"update dispositivos set latitude = {this.latitude}, longitude = {this.longitude}, nome = '{this.nome}' where id_dispositivo = {this.id} ";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.ExecuteNonQuery();
                    Console.WriteLine($"Dispositivo: ID - {this.id},  NOME - {this.nome}");
                    return true;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao cadastrar dispositivo: " + ex.Message);
                return false;
            }
        }


        public bool excluirDispositivo(int id)
        {
            Banco banco = new Banco();
            SqlConnection connection;
            try
            {

                using (connection = new SqlConnection(banco.stringConexao()))
                {
                    connection.Open();
                    int dispositivoId = 0;
                    string query = $"update dispositivos set excluido = 1 where id_dispositivo = {id} ";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.ExecuteNonQuery();
                    Console.WriteLine($"Dispositivo excluido: ID - {this.id}");
                    return true;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir dispositivo: " + ex.Message);
                return false;
            }

        }

        public List<Dispositivo> selectDispositivoAll()
        {
            Banco banco = new Banco();
            List<Dispositivo> dispositivos = new List<Dispositivo>();
            SqlConnection connection;
            try
            {

                using (connection = new SqlConnection(banco.stringConexao()))
                {

                    connection.Open();
                    string query = "select * from dispositivos d left join MQTT_Dispositivo mqtt on d.id_dispositivo = mqtt.id_dispositivo where d.excluido <> 1";
                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dispositivo dispositivo = new Dispositivo
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id_dispositivo")),
                                nome = reader.GetString(reader.GetOrdinal("nome")),
                                latitude = reader.GetString(reader.GetOrdinal("latitude")),
                                longitude = reader.GetString(reader.GetOrdinal("longitude")),
                                id_dispositivo = reader.GetInt32(reader.GetOrdinal("id_dispositivo")),
                                clienteID = reader.GetString(reader.GetOrdinal("clienteID")),
                                servidor = reader.GetString(reader.GetOrdinal("servidor")),
                                porta = reader.GetInt32(reader.GetOrdinal("porta")),
                                topico = reader.GetString(reader.GetOrdinal("topico")),

                            };

                            Console.WriteLine(dispositivo.id + dispositivo.nome);

                            dispositivos.Add(dispositivo);
                        }
                        reader.Close();
                    }

                    connection.Close();

                }

                
                return dispositivos;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }

}
