using Microsoft.Extensions.Configuration;
using Npgsql;
using Store_Project.Models;
using static System.Collections.Specialized.BitVector32;
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
            Models.Section section = null;
            Models.Loja loja = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(strConnection))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(
                           "SELECT loja.id, loja.nome, loja.cidade, section.nome AS nome_secao, produto.nome AS nome_produto, produto.descricao, produto.valor, section.id AS section_id " +
                           "FROM public.loja " +
                           "LEFT JOIN public.section ON loja.id = section.loja_id " +
                           "LEFT JOIN public.produto ON section.id = produto.section_id", conn))
                {

                 
           
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //int lojaId = reader.GetInt32(0);

                            //Models.Loja loja = listLoja.FirstOrDefault(l => l.Id == lojaId);

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
                                int sectionId = reader.GetInt32(7);
                                section = loja.Sections.FirstOrDefault(s => s.Id == sectionId);

                                // Seção não encontrada pelo ID, verificar pelo nome
                                if (section == null || section.Nome != reader.GetString(3))
                                {
                                    section = loja.Sections.FirstOrDefault(s => s.Nome == reader.GetString(3));
                             
                                    if (section == null)
                                    {
                                        section = new Models.Section
                                        {
                                            Id = sectionId,
                                            Nome = reader.GetString(3),
                                            Produtos = new List<Produto>()
                                        };

                                        loja.Sections.Add(section);
                                    }
                                }
                            }

                            if (!reader.IsDBNull(4))
                            {
                                Produto produto = new Produto()
                                {
                                    Nome = reader.GetString(4),
                                    Descricao = reader.GetString(5),
                                    Valor = reader.GetDouble(6),
                             
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

        public int CreateSecao(int lojaId, Models.Section CreatedSection)
        {
           

            using (NpgsqlConnection conn = new NpgsqlConnection(strConnection))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(
                    "INSERT INTO public.section (loja_id, nome) VALUES (@lojaId, @nome) RETURNING id", conn))
                {
                    cmd.Parameters.AddWithValue("lojaId", lojaId);
                    cmd.Parameters.AddWithValue("nome", CreatedSection.Nome);

                    // Obtém o Id da nova seção após a inserção
                    int novaSecaoId = (int)cmd.ExecuteScalar();
                    //adcionar produtos na secao

                    return novaSecaoId;
                }
            }

        }

        public bool CreateProduto(Produto produto)
        {
            int linhasAfetadas = 0;
            using (NpgsqlConnection conn = new NpgsqlConnection(strConnection))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(
                   "INSERT INTO public.produto (nome, descricao, valor, section_id) VALUES (@nome, @descricao, @valor, @section_id)", conn))
                {
                    cmd.Parameters.AddWithValue("nome", produto.Nome);
                    cmd.Parameters.AddWithValue("descricao", produto.Descricao);
                    cmd.Parameters.AddWithValue("valor", produto.Valor);
                    cmd.Parameters.AddWithValue("section_id", produto.Section_id);

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }
          
            return linhasAfetadas > 0;
        }
    }

 }
