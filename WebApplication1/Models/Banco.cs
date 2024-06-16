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
            try
            {
                using(SqlConnection connection = new SqlConnection(stringConexao()))
                {
                    //connection.ConnectionString = nome;
                    return connection;

                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public string stringConexao()
        {
            string nome = $"{hostname}" + @"\" + $"{instancia}";
            //string nome = "DESKTOP-RHMLQTH\\SQLEXPRESS";
            string conexao = $"Server={nome};Database={database};User Id={user};Password={senha};";
            Console.WriteLine(conexao);
            return conexao;
        }
    }
}
