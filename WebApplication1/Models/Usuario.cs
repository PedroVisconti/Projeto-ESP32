using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ESP32.Models
{
    public class Usuario
    {
        public int id {  get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string login {  get; set; }
        public string senha { get; set; }
        public string cpf { get; set; }
        public DateTime data_nascimento { get; set; }

        public Usuario(string nome, string sobrenome, string login, string senha, string cpf, DateTime data_nascimento)
        {
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

            try
            {
                using(SqlConnection connection = new SqlConnection(banco.stringConexao()))
                {
                    connection.Open();

                    string senhaCript = CriptografarSenha(senha);
                    string query = $"SELECT id_usuario, nome, senha, sobrenome, cpf, data_nascimento FROM usuario WHERE login = '{login}' AND senha = '{senhaCript}'";
                    Console.WriteLine("QUERRY DE CONSULTA: " + query);
                    SqlCommand command = new SqlCommand(query, connection);


                    SqlDataReader reader = command.ExecuteReader();


                    if (reader.Read())
                    {

                        this.id = Convert.ToInt32(reader["id_usuario"]);
                        this.nome = reader["nome"].ToString();
                        this.senha = reader["senha"].ToString();
                        this.sobrenome = reader["sobrenome"].ToString();
                        this.cpf = reader["cpf"].ToString();
                        this.data_nascimento = Convert.ToDateTime(reader["data_nascimento"]);


                        reader.Close();
                        connection.Close();
                        Console.WriteLine("realizado login");
                        return true;
                    }
                    else
                    {

                        reader.Close();
                        connection.Close();

                        return false;
                    }
                }


            }
            catch (Exception ex)
            {   
                Console.WriteLine("Erro ao realizar login: " + ex.Message);
                return false;
            }


        }

        public bool inserirUsuario()
        {
            Banco banco = new Banco();
            SqlConnection connection;
            try
            {

                using(connection = new SqlConnection(banco.stringConexao()))
                {
                    connection.Open();

                    string query = "INSERT INTO usuario (nome, sobrenome, cpf, data_nascimento, login, senha) VALUES (@nome, @sobrenome, @cpf, @data_nascimento, @login, @senha)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nome", this.nome);
                    command.Parameters.AddWithValue("@sobrenome", this.sobrenome);
                    command.Parameters.AddWithValue("@cpf", this.cpf);
                    command.Parameters.AddWithValue("@data_nascimento", this.data_nascimento);
                    command.Parameters.AddWithValue("@login", this.login);
                    command.Parameters.AddWithValue("@senha", this.senha = CriptografarSenha(this.senha));

                    int rowsAffected = command.ExecuteNonQuery();


                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Inserção bem-sucedida!");
                        connection.Close();
                        //return realizarLogin(this.login, this.senha);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Falha ao inserir o usuário.");
                        connection.Close();
                        return false;
                    }
                }


            }
            catch(Exception ex)
            {
                Console.WriteLine("Erro ao cadastrar usuario: " + ex.Message);
                return false;
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
            
            byte[] inputBytes = Encoding.UTF8.GetBytes(senha);

            
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                
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
