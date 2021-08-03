using AluraFlix.Core.Models;
using AluraFlix.Core.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluraFlix.Core.Repositories.Interfaces
{
    public interface ICategoriasRepository
    {
        Task<(bool success, Categoria categoria)> Register(CategoriaRequest request);
        Task<IList<Categoria>> ListAll();
        Task<Categoria> Find(long id);
        Task<bool> Delete(long id);
        Task<(bool success, Categoria categoria)> Change(CategoriaUpdateRequest request);
    }
}
