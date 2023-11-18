using Npgsql;
using NuGet.Protocol.Plugins;
using System.Data;

namespace MJV.DALPg
{
    public class DALPostegres
    {
        string strConnection = "Server=127.0.0.1;Port=5432;Database=TesteDBMjv;User Id = postgres; Password = ovo12345";
        public List<Models.Usuario> ListarUsuario()
        {
            List<Models.Usuario> listaUsuario = new List<Models.Usuario>();

            using (NpgsqlConnection conn = new NpgsqlConnection(strConnection))
            {
                conn.Open();
                Console.WriteLine("Abriu conexao");
                //ADO 
                NpgsqlCommand sqlCommand = new NpgsqlCommand();
                sqlCommand.CommandText = "SELECT * FROM public.\"alunos\""; ; //codigo sql
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.Connection = conn;

                Models.Usuario aluno;

                //ler 
                NpgsqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    aluno = new Models.Usuario();
                    if (!reader.IsDBNull(1))
                    {
                        aluno.Nome = reader.GetString(1);
                        Console.WriteLine(reader.GetString(1));
                    }
                    if (!reader.IsDBNull(2))
                    {
                        aluno.SobreNome = reader.GetString(2);
                        Console.Write(" " + reader.GetString(2));
                    }
                    Console.WriteLine("");

                    //Console.WriteLine(reader["nome"]);

                    listaUsuario.Add(aluno);
                }

                return listaUsuario;
            }

        }
        public Models.Usuario SelecionarUsuario(int id)
        {
            Models.Usuario listaUsuario = new();

            using (NpgsqlConnection conn = new NpgsqlConnection(strConnection))
            {
                conn.Open();
                Console.WriteLine("Abriu conexao");
                //ADO 
                NpgsqlCommand sqlCommand = new NpgsqlCommand();
                sqlCommand.CommandText = "SELECT * FROM public.\"alunos\" where id = @id"; //codigo sql
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.Connection = conn;

                Models.Usuario aluno;

                //ler 
                NpgsqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    aluno = new Models.Usuario();
                    if (!reader.IsDBNull(1))
                    {
                        aluno.Nome = reader.GetString(1);
                        Console.WriteLine(reader.GetString(1));
                    }
                    if (!reader.IsDBNull(2))
                    {
                        aluno.SobreNome = reader.GetString(2);
                        Console.Write(" " + reader.GetString(2));
                    }
                    Console.WriteLine("");

                    //Console.WriteLine(reader["nome"]);

              
                }

                return listaUsuario;
            }

        }
        public bool InserirAluno(string nome, string sobrenome, string email, string conexao)
        {
            NpgsqlConnection conn = new NpgsqlConnection(conexao);

            //ADO 
            NpgsqlCommand sqlCommand = new NpgsqlCommand();
            sqlCommand.CommandText = "INSERT INTO usuario (nome, sobrenome, email) VALUES (@nome, @sobrenome, @email);";  //codigo sql
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.Connection = conn;

            //fefinir parametros
            sqlCommand.Parameters.AddWithValue("@nome", nome);
            sqlCommand.Parameters.AddWithValue("@sobrenome", sobrenome);
            sqlCommand.Parameters.AddWithValue("@sobrenome", email);
            //abrir conexao
            sqlCommand.Connection.Open();
            //quantas linhas foram afetadas

            int linhasAfetadas = sqlCommand.ExecuteNonQuery();


            sqlCommand.Connection.Close();


            return linhasAfetadas > 0;
        }

        public bool AtualizarAluno(int id, string nome, string sobrenome)
        {
            NpgsqlConnection conn = new NpgsqlConnection(strConnection);

            //ADO 
            NpgsqlCommand sqlCommand = new NpgsqlCommand();
            sqlCommand.CommandText = @"UPDATE Usuario set nome = @nome, sobrenome= @ sobrenome) 
                                        where id = @id";  //codigo sql

            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.Connection = conn;

            //fefinir parametros

            sqlCommand.Parameters.AddWithValue("nome", nome);
            sqlCommand.Parameters.AddWithValue("sobrenome", sobrenome);
            sqlCommand.Parameters.AddWithValue("id", id);

            //abrir conexao
            sqlCommand.Connection.Open();
            //quantas linhas foram afetadas

            int linhasAfetadas = sqlCommand.ExecuteNonQuery();


            sqlCommand.Connection.Close();


            return linhasAfetadas > 0;
        }
        public bool DeletarAluno(int id, string nome, string sobrenome)
        {
            NpgsqlConnection conn = new NpgsqlConnection(strConnection);

            //ADO 
            NpgsqlCommand sqlCommand = new NpgsqlCommand();
            sqlCommand.CommandText = @"delete from Usuario 
                                        where id = @id";  //codigo sql

            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.Connection = conn;

            //fefinir parametros
            sqlCommand.Parameters.AddWithValue("id", id);

            //abrir conexao
            sqlCommand.Connection.Open();
            //quantas linhas foram afetadas

            int linhasAfetadas = sqlCommand.ExecuteNonQuery();


            sqlCommand.Connection.Close();


            return linhasAfetadas > 0;
        }

    }
}
