using System.Data.SqlClient;

namespace ESP32.Models
{
    public class Banco
    {

        string hostname = Environment.MachineName;
        string instancia = "SQLEXPRESS";
        string database = "PROJETOESP";
        string user = "sa";
        string senha = "banco";

        public SqlConnection conectarBanco()
        {
            string nome = hostname + @"\" + instancia;
            string conexao = $"Server={nome};Database={database};User Id={user};Password={senha};";

            Console.WriteLine(conexao);

            try
            {
                using(SqlConnection connection = new SqlConnection(conexao))
                {
                    connection.Open();
                    return connection;

                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
