using RCLAPI.DTO;

namespace RCLAPI.Services;

public interface IApiServices
{
    public Task<(List<ProdutoDTO>? Produtos, string? ErrorMessage)> GetProdutos(string tipoProduto, string categoriaId);
    public Task<ProdutoDTO> GetDetalheProduto(int IdProduto);
    public Task<List<ProdutoDTO>> GetProdutosEspecificos(string produtoTipo, int? IdCategoria);
    public Task<(T? Data, string? ErrorMessage)> GetAsync<T>(string endpoint);
    public Task<List<Categoria>> GetCategorias();

    //FAV
    public Task<(bool Data, string? ErrorMessage)> ActualizaFavorito(string acao, int produtoId);
    public Task<List<ProdutoFavorito>> GetFavoritos(string utilizadorId);

    //REGISTAR E LOGAR
    public Task<ApiResponse<bool>> RegistarUtilizador(string username, string email,string password, string telemovel);
    public Task<ApiResponse<bool>> Login(string email, string password);
    
    //CARRINHO
    Task<List<ItemCarrinhoCompra>> GetItensDoCarrinho(string clienteId);
    Task<bool> RemoveItemDoCarrinho(int itemCarrinhoId);
    Task<bool> AdicionaItemNoCarrinho(ItemCarrinhoCompra itemCarrinho);
}
