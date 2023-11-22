using Api_Store;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Store_Project.DALPg
{
    public class DALPostegres : IDisposable
    {
        private NpgsqlConnection connection;


        public DALPostegres()
        {
            string strConnection = "Server=127.0.0.1;Port=5432;Database=Projeto MJV;User Id = postgres; Password = ovo12345";
            connection = new NpgsqlConnection(strConnection);
        }

        public List<Loja> ListLojaDB()
        {
            List<Loja> listLoja= new List<Loja>();
            Api_Store.Section section = null;
            Loja loja = null;

            try
            {
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(
                           "SELECT loja.id, loja.nome, loja.cidade, section.nome AS nome_secao, produto.nome AS nome_produto, produto.descricao, produto.valor, section.id AS section_id " +
                           "FROM public.loja " +
                           "LEFT JOIN public.section ON loja.id = section.loja_id " +
                           "LEFT JOIN public.produto ON section.id = produto.section_id", connection))
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
                                    Sections = new List<Api_Store.Section>()
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
                                        section = new Api_Store.Section
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

            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public int GetUltimoIdLoja()
        {
            try
            {
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT MAX(Id) FROM loja", connection))
                {
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result) + 1;
                    }
                    else
                    {
                        // Tratamento para o caso de a tabela estar vazia
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        public Loja GetLojaById(int lojaId)
        {
            Loja loja = null;

            try
            {
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM public.loja WHERE id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", lojaId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            loja = new Loja
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Cidade = reader.GetString(2),
                                Sections = new List<Section>()

                            };
                            reader.Close();

                            List<Section> secoes = GetSecoesAndProdutosByLojaId(loja.Id);

                            // Atribui as seções à propriedade Sections da loja
                            loja.Sections = secoes;
                      
                         }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return loja;

        }
 
        public List<Section> GetSecoesAndProdutosByLojaId(int lojaId)
        {
            List<Section> secoes = new List<Section>();

            using (NpgsqlCommand cmd = new NpgsqlCommand(
                "SELECT s.id as section_id, s.nome as section_nome, p.nome as produto_nome, p.descricao as produto_descricao, p.valor as produto_valor " +
                "FROM public.section s LEFT JOIN public.produto p ON s.id = p.section_id WHERE s.loja_id = @lojaId", connection))
            {
                cmd.Parameters.AddWithValue("@lojaId", lojaId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        int sectionId = reader.GetInt32(0);
                        string sectionNome = reader.GetString(1);

                        Section secao = secoes.FirstOrDefault(s => s.Id == sectionId);

                        if (secao == null)
                        {
                            // Se a seção ainda não foi adicionada, crie uma nova
                            secao = new Section
                            {
                                Id = sectionId,
                                Nome = sectionNome,
                                Produtos = new List<Produto>()
                            };

                            secoes.Add(secao);
                        }

                        // Verifique se há produtos associados
                        if (!reader.IsDBNull(2))
                        {
                            // Se houver, adicione-os à seção
                            Produto produto = new Produto
                            {
                                Nome = reader.GetString(2),
                                Descricao = reader.GetString(3),
                                Valor = reader.GetDouble(4),
                                Section_id = sectionId
                            };

                            secao.Produtos.Add(produto);
                        }
                    }
                }
            }

            return secoes;
        }

        public bool CreateLoja(Loja loja)
        {
            int linhasAfetadas = 0;
            try
            {
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO public.loja (id, nome, cidade) VALUES (@id, @nome, @cidade)", connection))
                {
                    cmd.Parameters.AddWithValue("id", loja.Id);
                    cmd.Parameters.AddWithValue("nome", loja.Nome);
                    cmd.Parameters.AddWithValue("cidade", loja.Cidade);

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }

                return linhasAfetadas > 0;
            } 
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }


        }

        public int CreateSecao(int lojaId, Api_Store.Section CreatedSection)
        {
           
            try
            {
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(
                    //"INSERT INTO public.section (id, loja_id, nome) VALUES (@id, @lojaId, @nome)", connection))
                    "INSERT INTO public.section (loja_id, nome) VALUES (@lojaId, @nome) RETURNING id", connection))
                {
                    //cmd.Parameters.AddWithValue("id", CreatedSection.Id);
                    cmd.Parameters.AddWithValue("lojaId", lojaId);
                    cmd.Parameters.AddWithValue("nome", CreatedSection.Nome);

                    int novaSecaoId = (int)cmd.ExecuteScalar();
                    //adcionar produtos na secao

                    return novaSecaoId;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        public bool CreateProduto(Produto produto)
        {
            int linhasAfetadas = 0;
            try
            {
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(
                "INSERT INTO public.produto (nome, descricao, valor, section_id) VALUES (@nome, @descricao, @valor, @section_id)", connection))
                {

                    cmd.Parameters.AddWithValue("nome", produto.Nome);
                    cmd.Parameters.AddWithValue("descricao", produto.Descricao);
                    cmd.Parameters.AddWithValue("valor", produto.Valor);
                    cmd.Parameters.AddWithValue("section_id", produto.Section_id);

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
                return linhasAfetadas > 0;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        public bool EditeLoja(Loja loja)
        {
            int linhasAfetadas = 0;
            try
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(
                    "UPDATE public.loja SET nome = @nome, cidade = @cidade WHERE id = @id", connection))
                { 

                    //fefinir parametros

                    cmd.Parameters.AddWithValue("id", loja.Id);
                    cmd.Parameters.AddWithValue("nome", loja.Nome);
                    cmd.Parameters.AddWithValue("cidade", loja.Cidade);

                    //quantas linhas foram afetadas

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
                   
                return linhasAfetadas > 0;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        public bool DeleteLoja(int DeleteId)
        {
            int linhasAfetadas = 0;
            try
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(
                    "DELETE FROM public.loja WHERE id = @id", connection))
                {
                    // Definir parâmetros
                    cmd.Parameters.AddWithValue("id", DeleteId);

                    // Quantas linhas foram afetadas
                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
                return linhasAfetadas > 0;  
            }

            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
            }
        }
    }     
}
