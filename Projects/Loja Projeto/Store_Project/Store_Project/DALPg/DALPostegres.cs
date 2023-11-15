using Npgsql;
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

                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM public.Loja", conn))
                {
                    Models.Loja loja;

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            loja = new Models.Loja();
                            if (!reader.IsDBNull(0))
                            {
                                loja.Id = reader.GetInt32(0);
                            
                            }
                            if (!reader.IsDBNull(1))
                            {
                                loja.Nome = reader.GetString(1);
                          
                            }
                            if (!reader.IsDBNull(2))
                            {
                                loja.Cidade = reader.GetString(2);
                            }
     
                            listLoja.Add(loja);
                        }

                        return listLoja;
                    }
                }
            }
        }
    }
}
