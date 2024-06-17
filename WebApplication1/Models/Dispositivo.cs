using System.Data.SqlClient;

namespace ESP32.Models
{
    public class Dispositivo : MQTT
    {
        public int id {  get; set; }
        public string nome { get; set; }
        public long latitude { get; set; }
        public long longitude { get; set; }
        /*
        public string clienteID { get; set; }
        public string servidor { get; set; }
        public int porta { get; set; }
        public string topico { get; set; }
        */

        public Dispositivo(string nome, long latitude, long longitude, string clienteID, string servidor, int porta, string topico) : base(clienteID,  servidor,  porta,  topico)
        {
            this.nome = nome;
            this.latitude = latitude;
            this.longitude = longitude;

        }
        
        public Dispositivo()
        {

        }

        public void insertDispositivo()
        {

        }

        public void alterDispositivo()
        {

        }

        public void selectDispositivoId(int id)
        {

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
                    string query = "select * from dispositivos d left join MQTT_Dispositivo mqtt on d.id_dispositivo = mqtt.id_dispositivo";
                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dispositivo dispositivo = new Dispositivo
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id_dispositivo")),
                                nome = reader.GetString(reader.GetOrdinal("nome")),
                                latitude = reader.GetInt64(reader.GetOrdinal("latitude")),
                                longitude = reader.GetInt64(reader.GetOrdinal("lontitude")),
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
