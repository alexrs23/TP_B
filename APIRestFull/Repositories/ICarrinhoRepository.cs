using APIRestFull.Entities;
namespace APIRestFull.Repositories
{
    public interface ICarrinhoRepository
    {
        Task<List<ItemCarrinhoCompra>> GetCarrinhoByCliente(string clienteId);
        Task<bool> DeleteItem(int itemCarrinhoId);
        Task<bool> AddItem(ItemCarrinhoCompra item);
    }
}