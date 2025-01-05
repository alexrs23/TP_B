using APIRestFull.Entities;

namespace APIRestFull.Repositories
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Produto>> ObterProdutosPromocaoAsync();
        Task<IEnumerable<Produto>> ObterProdutosMaisVendidosAsync();
        Task<Produto> ObterDetalheProdutoAsync(int id);
        Task<IEnumerable<Produto>> ObterTodosProdutosAsync();
        Task<IEnumerable<Produto>> GetProdutosEspecificos(string? especifico, int? idCat);
    }
}
