using APIRestFull.Entities;
namespace APIRestFull.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ProdutoFavorito>> GetFavoritos(string username);
        Task<bool> ActualizaFavorito(string acao, int produtoId, string userName);
    }
}