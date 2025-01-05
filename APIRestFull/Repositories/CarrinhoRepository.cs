using APIRestFull.Context;
using APIRestFull.Data;
using APIRestFull.Entities;
using Microsoft.EntityFrameworkCore;
namespace APIRestFull.Repositories
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private readonly AppDbContext _context;
        public CarrinhoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ItemCarrinhoCompra>> GetCarrinhoByCliente(string clienteId)
        {
            return await _context.ItemCarrinhoCompras.Where(x => x.ClienteId == clienteId).ToListAsync();
        }
        public async Task<bool> DeleteItem(int itemCarrinhoId)
        {
            var item = await _context.ItemCarrinhoCompras.FirstOrDefaultAsync(x => x.Id == itemCarrinhoId);
            if (item is null) return false;
            _context.ItemCarrinhoCompras.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddItem(ItemCarrinhoCompra item)
        {
            Console.WriteLine("ADICIONEI ITEM");
            _context.ItemCarrinhoCompras.Add(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}