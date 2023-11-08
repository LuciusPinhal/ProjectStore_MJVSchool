using System.Text.Json;

class MinhaClasse
{
    public int Numero { get; set; }
    public string Texto { get; set; }
}

class Program
{
    static void Main()
    {
        // Defina o caminho para o arquivo JSON que deseja alterar
        string caminhoArquivoJson = "dados.json";

        // Desserializar o objeto a partir do arquivo JSON
        MinhaClasse objetoDesserializado = DesserializarObjetoDeJson(caminhoArquivoJson);

        // Faça as alterações necessárias no objeto
        objetoDesserializado.Numero = 100;
        objetoDesserializado.Texto = "Nova mensagem";

        // Serializar o objeto modificado de volta para JSON
        string jsonModificado = JsonSerializer.Serialize(objetoDesserializado);

        // Salvar o JSON modificado de volta no arquivo original
        File.WriteAllText(caminhoArquivoJson, jsonModificado);
    }

    static MinhaClasse DesserializarObjetoDeJson(string caminhoArquivoJson)
    {
        // Ler o arquivo JSON e desserializar o objeto
        string json = File.ReadAllText(caminhoArquivoJson);
        return JsonSerializer.Deserialize<MinhaClasse>(json);
    }
}