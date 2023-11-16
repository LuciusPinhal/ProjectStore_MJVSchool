using Microsoft.Extensions.Configuration;
using Npgsql;
using Store_Project.Models;
//using NuGet.Protocol.Plugins;
//using System.Data;

namespace Store_Project.DALPg
{
    public class DALPostegres
    {

        string strConnection = "Server=127.0.0.1;Port=5432;Database=Projeto MJV;User Id = postgres; Password = ovo12345";

        public List<Models.Loja> ListLojaDB()
        {
            List<Models.Loja> listLoja= new List<Models.Loja>();

            using (NpgsqlConnection conn = new NpgsqlConnection(strConnection))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(
                           "SELECT loja.id, loja.nome, loja.cidade, section.nome AS nome_secao, produto.nome AS nome_produto, produto.descricao, produto.valor, section.id AS section_id " +
                           "FROM public.loja " +
                           "LEFT JOIN public.section ON loja.id = section.loja_id " +
                           "LEFT JOIN public.produto ON section.id = produto.section_id", conn))
                {
                    Models.Loja loja = null;
                    Models.Section section = null;

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                
       
                            if (loja == null || loja.Id != reader.GetInt32(0))
                            {
                                loja = new Loja
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    Cidade = reader.GetString(2),
                                    Sections = new List<Models.Section>()
                                };

                                listLoja.Add(loja);

                            }

                            if (!reader.IsDBNull(3))
                            {
                
                                if (section == null || section.Nome != reader.GetString(3))
                                { 
                                    section = new Section
                                    {
                                        Id = reader.GetInt32(7),
                                        Nome = reader.GetString(3),
                                        Produtos = new List<Produto>()
                                    };

                                    loja.Sections.Add(section);
                                }
                          
                            }
                            
                            if (!reader.IsDBNull(4))
                            {
                                Produto produto = new Produto()
                                {
                                    Nome = reader.GetString(4),
                                    Descricao = reader.GetString(5),
                                    Valor = reader.GetDouble(6)
                                };
                                
                                section.Produtos.Add(produto);
                            }
                        
                        }

                        return listLoja;
                    }
                }
            }
        }


        public bool CreateLoja(Loja loja)
        {
            int linhasAfetadas = 0;
            using (NpgsqlConnection conn = new NpgsqlConnection(strConnection))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO public.loja (id, nome, cidade) VALUES (@id, @nome, @cidade)", conn))
                {
                    cmd.Parameters.AddWithValue("id", loja.Id);
                    cmd.Parameters.AddWithValue("nome", loja.Nome);
                    cmd.Parameters.AddWithValue("cidade", loja.Cidade);

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            return linhasAfetadas > 0;

        }

        public bool CreateSecao(int lojaId, Section Createdsection)
        {
            int linhasAfetadas = 0;

            using (NpgsqlConnection conn = new NpgsqlConnection(strConnection))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(
                    "INSERT INTO public.section (loja_id, nome) VALUES (@lojaId, @nome) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("lojaId", lojaId);
                    cmd.Parameters.AddWithValue("nome", Createdsection.Nome);

                    // Obtém o Id da nova seção após a inserção
                    linhasAfetadas = cmd.ExecuteNonQuery();
                    //adcionar produtos na secao
                }
            }

            return linhasAfetadas > 0;
        }

        public bool CreateProduto()
        {
            int linhasAfetadas = 0;
            return linhasAfetadas > 0;
        }
    }

 }
