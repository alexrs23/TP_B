namespace GestaoLoja
{
    public class AppConfig
    {
        // Nome da aplicação
        public string AppName { get; set; } = "Gestão de Loja";

        // Configuração do banco de dados
        public string DatabaseName { get; set; } = "GestaoLoja";

        // Configuração de upload de imagens
        public string UploadImagePath { get; set; } = "/wwwroot/img";

        // Outras configurações gerais
        public string DefaultCulture { get; set; } = "pt-PT"; // Idioma padrão
        public int PageSize { get; set; } = 10; // Paginação

        // Exemplo de configuração de API externa (se necessário)
        public string ExternalApiKey { get; set; }
    }

}
