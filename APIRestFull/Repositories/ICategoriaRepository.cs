using APIRestFull.Entities;

namespace APIRestFull.Repositories;
public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetCategorias();
}
