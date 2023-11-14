using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using Microsoft.AspNetCore.Hosting.Server;
using Npgsql;

namespace ConexaoBancodeDados.dal
{
    public class PostegresDAL
    {
        string strConnection = "Server=127.0.0.1;Port=5432;Database=Mjv;User Id = postgres; Password = ovo12345";
        public List<Models.Alunos> ListarAlunos()
        {
            List<Models.Alunos> listaAlunos = new List<Models.Alunos>();

            using (NpgsqlConnection conn = new NpgsqlConnection(strConnection))
            {
                conn.Open();
                Console.WriteLine("Abriu conexao");
                //ADO 
                NpgsqlCommand sqlCommand = new NpgsqlCommand();
                sqlCommand.CommandText = "SELECT * FROM public.\"alunos\""; ; //codigo sql
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.Connection = conn;

                Models.Alunos aluno;

                //ler 
                NpgsqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    aluno = new Models.Alunos();
                    if (!reader.IsDBNull(1))
                    {
                        aluno.Nome = reader.GetString(1);
                        Console.WriteLine(reader.GetString(1));
                    }
                    if (!reader.IsDBNull(2))
                    {
                        aluno.Sobrenome = reader.GetString(2);
                        Console.Write(" " + reader.GetString(2));
                    }
                    Console.WriteLine("");

                    //Console.WriteLine(reader["nome"]);

                    listaAlunos.Add(aluno);
                }

                return listaAlunos;
            }
        
        }
        public bool InserirAlunos(string nome, string sobrenome)
        {
            NpgsqlConnection conn = new NpgsqlConnection(strConnection);

            //ADO 
            NpgsqlCommand sqlCommand = new NpgsqlCommand();
            sqlCommand.CommandText = "INSERT INTO Alunos (nome, sobrenome) VALUES (@nome, @sobrenome);";  //codigo sql
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.Connection = conn;

            //fefinir parametros
            sqlCommand.Parameters.AddWithValue("@nome", nome);
            sqlCommand.Parameters.AddWithValue("@sobrenome", sobrenome);
            //abrir conexao
            sqlCommand.Connection.Open();
            //quantas linhas foram afetadas

            int linhasAfetadas =  sqlCommand.ExecuteNonQuery();


            sqlCommand.Connection.Close();


            return linhasAfetadas > 0;
        }
    }

}
