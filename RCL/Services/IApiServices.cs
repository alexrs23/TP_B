using RCLAPI.DTO;

namespace RCLAPI.Services;

public interface IApiServices
{
    public Task<(List<ProdutoDTO>? Produtos, string? ErrorMessage)> GetProdutos(string tipoProduto, string categoriaId);
    public Task<ProdutoDTO> GetDetalheProduto(int IdProduto);
    public Task<List<ProdutoDTO>> GetProdutosEspecificos(string produtoTipo, int? IdCategoria);
    public Task<(T? Data, string? ErrorMessage)> GetAsync<T>(string endpoint);
    public Task<List<Categoria>> GetCategorias();
    public Task<(bool Data, string? ErrorMessage)> ActualizaFavorito(string acao, int produtoId);
    public Task<List<ProdutoFavorito>> GetFavoritos(string utilizadorId);
    public Task<ApiResponse<bool>> RegistarUtilizador(string nome, string apelido, string email,string password, long? nif);
    public Task<ApiResponse<bool>> Login(string email, string password);
}
