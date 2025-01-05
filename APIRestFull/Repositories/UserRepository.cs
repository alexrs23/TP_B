using APIRestFull.Context;
using APIRestFull.Data;
using APIRestFull.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIRestFull.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProdutoFavorito>> GetFavoritos(string username)
        {
            var favs = await _context.ProdutoFavoritos.Where(x => x.UserName == username).ToListAsync();
            if (favs is null) return null;
            return favs;
        }

        public async Task<bool> ActualizaFavorito(string acao, int produtoId, string userName)
        {
            var produto = await _context.ProdutoFavoritos.FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.UserName == userName);
            if (produto is not null && acao == "heartfill")
            {
                produto.Efavorito = true;
                _context.ProdutoFavoritos.Update(produto);
                await _context.SaveChangesAsync();
                return true;
            }
            if (produto is not null && acao == "heartsimples")
            {
                produto.Efavorito = false;
                _context.ProdutoFavoritos.Update(produto);
                await _context.SaveChangesAsync();
                return true;
            }
            else if (produto is null)
            {
                var newFav = new ProdutoFavorito() { ProdutoId = produtoId, UserName = userName, Efavorito = true };
                _context.ProdutoFavoritos.Add(newFav);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}