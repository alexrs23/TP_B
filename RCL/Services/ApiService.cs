using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xamarin.Essentials;

using RCLAPI.DTO;

namespace RCLAPI.Services;
public class ApiService : IApiServices
{
    private readonly ILogger<ApiService> _logger;
    private readonly HttpClient _httpClient = new();
    JsonSerializerOptions _serializerOptions;

    private List<ProdutoDTO> produtos;

    private List<Categoria> categorias;

    private ProdutoDTO _detalhesProduto;
    public ApiService(ILogger<ApiService> logger)
    {
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _detalhesProduto = new ProdutoDTO();

        categorias = new List<Categoria>();
    }

    private void AddAuthorizationHeader()
    {
        var token = Preferences.Get("accesstoken", string.Empty);  //nao sei se é para estar aqui

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }


    public async Task<(ImagemPerfil? ImagemPerfil, string? ErrorMessage)> GetImagemPerfilUsuario()
    {
        string endpoint = "$api/Usuarios/imagemperfil";

        return await GetAsync<ImagemPerfil>(endpoint);
    }

    // ********************* Categorias  **********

    public async Task<List<Categoria>?> GetCategorias()
    {
        string endpoint = $"api/Categorias";

        try
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string content = await httpResponseMessage.Content.ReadAsStringAsync();

                _logger.LogInformation($"GetCategorias: Response content: {content}"); // log
                if (!string.IsNullOrWhiteSpace(content))
                {
                    List<Categoria> categorias = JsonSerializer.Deserialize<List<Categoria>>(content, _serializerOptions)!;
                    return categorias;
                }
                else
                {
                    _logger.LogWarning($"GetCategorias: API retornou conteúdo vazio.");
                    return null;
                }

            }

            return await HandleErrorResponse<List<Categoria>>(httpResponseMessage);
        }
        catch (Exception ex)
        {
            return HandleException<List<Categoria>>(ex);
        }
    }
    private async Task<T?> HandleErrorResponse<T>(HttpResponseMessage response) where T : class
    {
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            string errorMessage = "Unauthorized";
            _logger.LogWarning(errorMessage);
            return null;
        }
        string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
        _logger.LogError(generalErrorMessage);
        return null;

    }

    private T? HandleException<T>(Exception ex, string? customMessage = null) where T : class
    {
        string errorMessage = customMessage ?? $"Erro inesperado: {ex.Message}";
        _logger.LogError(errorMessage);
        return null;
    }

    // ********************* Produtos  **********
    public async Task<(List<ProdutoDTO>?Produtos, string? ErrorMessage)> GetProdutos(string tipoProduto, string categoriaId)
    {
        string endpoint = $"api/Produtos?tipoProduto={tipoProduto}&categoriaId={categoriaId}";
        return await GetAsync<List<ProdutoDTO>>(endpoint);
    }

    public async Task<List<ProdutoDTO>> GetProdutosEspecificos(string produtoTipo, int? IdCategoria)
    {

        string endpoint = $"/";

        if (produtoTipo == "categoria" && IdCategoria != null)
        {
            endpoint = $"api/Produtos?tipoProduto=categoria&categoriaId={IdCategoria}";

        }
        else if (produtoTipo == "detalhe" && IdCategoria != null)
        {
            endpoint = $"api/Produtos?tipoProduto=categoria&categoriaId={IdCategoria}";
        }
        else if (produtoTipo == "promocao")
        {
            endpoint = $"api/Produtos?tipoProduto=promocao";
        }
        else if (produtoTipo == "maisvendido")
        {
            endpoint = $"api/Produtos?tipoProduto=maisvendido";
        }
        else if (produtoTipo == "todos")
        {
            endpoint = $"api/Produtos?tipoProduto=todos";
        }
        else if (produtoTipo == "populares")
        {
            endpoint = $"api/Produtos?tipoProduto=populares";
        }
        else
        {
            return null;
        }

        try
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string content = "";

                content = await httpResponseMessage.Content.ReadAsStringAsync();
                produtos = JsonSerializer.Deserialize<List<ProdutoDTO>>(content, _serializerOptions)!;  
               
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return null;
        }

        return produtos;
    }

    public async Task<ProdutoDTO> GetDetalheProduto(int IdProduto)
    {
        string endpoint = $"api/Produtos/{IdProduto}";

        string caminho = $"{AppConfig.BaseUrl}{endpoint}";

        try
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(caminho);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string content = "";

                content = await httpResponseMessage.Content.ReadAsStringAsync();
                _detalhesProduto = JsonSerializer.Deserialize<ProdutoDTO>(content, _serializerOptions)!;

                return _detalhesProduto;
            }
            else return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return null;
        }
    }
    public async Task<(T?Data, string?ErrorMessage)>GetAsync<T>(string endpoint)
    {
        try
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync(AppConfig.BaseUrl + endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(responseString, _serializerOptions);
                return (data ?? Activator.CreateInstance<T>(), null);
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    string errorMessage = "Unauthorized";
                    _logger.LogWarning(errorMessage);
                    return (default, errorMessage);
                }

                string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
                _logger.LogError(generalErrorMessage);
                return (default, generalErrorMessage);
            }
        }
        catch (HttpRequestException ex)
        {
            string errrMessage = $"Erro de requisição HTTP: {ex.Message}";
            _logger.LogError(errrMessage);
            return (default, errrMessage);
        }
        catch (JsonException ex)
        {
            string errorMessage = $"Erro de desserialização JSON: {ex.Message}";
            _logger.LogError(ex.Message);
            return (default, errorMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Erro inesperado: {ex.Message}";
            _logger.LogError(ex.Message);
            return (default, errorMessage);
        }
    }


    // ****************** Utilizadores ********************

    public async Task<ApiResponse<bool>> RegistarUtilizador(string usarname, string email,
                                                          string password, string telemovel)
    {
        try
        {
            var newuser = new Register()
            {
                Username= usarname,
                Email = email,
                Telefone = telemovel,
                Password = password
            };

            var json = JsonSerializer.Serialize(newuser, _serializerOptions);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/Acc/register", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisitos Http: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao registar o utiizador: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<bool>> Login(string email, string password)
    {
        try
        {
            var login = new Login()
            {
                Email = email,
                Password = password
            };

            var json = JsonSerializer.Serialize(login, _serializerOptions);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/Acc/login", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição Http: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro no login: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }
    private async Task<HttpResponseMessage> PostRequest(string uri, HttpContent content)
    {
        var enderecoURL = AppConfig.BaseUrl + uri;
        
        try
        {
            var result = await _httpClient.PostAsync(enderecoURL, content);
            return result;
        }
        catch (Exception ex)
        {
            // Log o erro ou trata conforme necessario
            _logger.LogError($"Erro ao enviar requisição POST para {uri}: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }

    internal async Task<ApiResponse<bool>> UploadImagemUtilizador(byte[] imageArray)
    {
        try
        {
            var content = new MultipartFormDataContent();

            content.Add(new ByteArrayContent(imageArray), "imagem", "image.jpg");

            var response = await PostRequest("api/Usuarios/uploadfoto", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.StatusCode == HttpStatusCode.Unauthorized ? "Unauthorized"
                    : "Erro ao enviar requisição HTTP: {response.StatusCode}";

                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");

                return new ApiResponse<bool> { ErrorMessage = errorMessage };
            }
            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao fazer o upload da imagem do utilizador: {ex.Message}");

            return new ApiResponse<bool> {ErrorMessage= ex.Message};
        }
    }

    // *************** Gerir Favoritos ******************

    public async Task<List<ProdutoFavorito>> GetFavoritos(string utilizadorId)
    {
        string endpoint = $"api/Favoritos/{utilizadorId}";

        HttpResponseMessage response = await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

        var responseString = await response.Content.ReadAsStringAsync();
        List<ProdutoFavorito> data = JsonSerializer.Deserialize<List<ProdutoFavorito>>(responseString, _serializerOptions);

        return data;

    }

    public async Task<(bool Data, string? ErrorMessage)> ActualizaFavorito(string acao, int produtoId)
    {
        try
        {
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            var response = await FavoritosPutRequest($"api/Favoritos/{produtoId}/{acao}", content );

            if (!response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string errorMessage = "Unauthorized";
                    _logger.LogWarning(errorMessage);
                    return (false, errorMessage);
                }
                string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
                _logger.LogError(generalErrorMessage);
                return (false, generalErrorMessage);
            }
        }
        catch (HttpRequestException ex)
        {
            string errorMessage = $"Erro de requisição HTTP: {ex.Message}";
            _logger.LogError(errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Erro inesperado: {ex.Message}";
            _logger.LogError(errorMessage);
            return (false, errorMessage);
        }
    }

    private async Task<HttpResponseMessage> FavoritosPutRequest(string uri, HttpContent content)
    {
        var enderecoUrl = AppConfig.BaseUrl + uri;
        try
        {
          //  AddAuthorizationHeader();
            var result = await _httpClient.PutAsync(enderecoUrl, content);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao enviar requisição PUT para {uri}: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
