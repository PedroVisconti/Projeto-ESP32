using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ESP32.Models
{
    public class Usuario
    {
        int id {  get; set; }
        string nome { get; set; }
        string sobrenome { get; set; }
        string login {  get; set; }
        string senha { get; set; }
        string cpf { get; set; }
        DateTime data_nascimento { get; set; }

        public Usuario(string nome, string sobrenome, string login, string senha, string cpf, DateTime data_nascimento)
        {
            this.id = id;
            this.nome = nome;
            this.sobrenome = sobrenome;
            this.login = login;
            this.senha = senha;
            this.cpf = cpf;
            this.data_nascimento = data_nascimento;
        }
        
        public Usuario() { 
        }

        public bool realizarLogin(string login, string senha)
        {
            Banco banco = new Banco();
            SqlConnection connection = banco.conectarBanco();

            try
            {
                // Abre a conexão com o banco de dados
                connection.Open();

                // Consulta SQL para buscar o usuário pelo login e senha
                string query = "SELECT id, nome, sobrenome, cpf, data_nascimento FROM Usuarios WHERE login = @login AND senha = @senha";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@login", login);

                string senhaCript = CriptografarSenha(senha);

                command.Parameters.AddWithValue("@senha", senhaCript);

                // Executa a consulta
                SqlDataReader reader = command.ExecuteReader();

                // Verifica se encontrou um usuário
                if (reader.Read())
                {
                    // Preenche os dados do usuário
                    this.id = Convert.ToInt32(reader["id"]);
                    this.nome = reader["nome"].ToString();
                    this.sobrenome = reader["sobrenome"].ToString();                    
                    this.cpf = reader["cpf"].ToString();
                    this.data_nascimento = Convert.ToDateTime(reader["data_nascimento"]);

                    // Fecha o reader
                    reader.Close();

                    // Retorna true indicando que o login foi bem-sucedido
                    return true;
                }
                else
                {
                    // Fecha o reader
                    reader.Close();

                    // Retorna false indicando que o login falhou
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Trata a exceção, se necessário
                Console.WriteLine("Erro ao realizar login: " + ex.Message);
                return false;
            }
            finally
            {
                // Fecha a conexão com o banco de dados
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }


        }

        public bool inserirUsuario()
        {
            Banco banco = new Banco();
            SqlConnection connection = banco.conectarBanco();

            try
            {
                connection.Open();

                // Consulta SQL para buscar o usuário pelo login e senha
                string query = "INSERT INTO Usuarios VALUES (@nome, @sobrenome, @cpf, @data_nascimento, @login, @senha)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@sobrenome", sobrenome);
                command.Parameters.AddWithValue("@cpf", cpf);
                command.Parameters.AddWithValue("@data_nascimento", data_nascimento);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@senha", senha);

                int rowsAffected = command.ExecuteNonQuery();

                // Verifica se o INSERT foi bem-sucedido
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Inserção bem-sucedida!");
                    return true;
                }
                else
                {
                    Console.WriteLine("Falha ao inserir o usuário.");
                    return false;
                }


            }
            catch(Exception ex)
            {
                Console.WriteLine("Erro ao cadaastrar usuario: " + ex.Message);
                return false;
            }
            finally
            {
                // Fecha a conexão com o banco de dados
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public void alterarUsuario()
        {

        }

        public void selectUsuarioID(int id)
        {

        }

        static string CriptografarSenha(string senha)
        {
            // Converte a string para um array de bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(senha);

            // Calcula o hash MD5
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Converte o array de bytes para uma string hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }
}
