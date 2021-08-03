using AluraFlix.Core.Models;
using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluraFlix.Core.Services.Interfaces
{
    public interface ICategoriasService
    {
        Task<IList<Categoria>> ListAll();
        Task<(bool success, Categoria categoria, IList<ErrorResponse> errors)> Find(long id);
        Task<(bool success, Categoria categoria, IList<ErrorResponse> errors)> Register(CategoriaRequest request);
        Task<(bool success, Categoria categoria, IList<ErrorResponse> errors)> Change(CategoriaUpdateRequest request);
        Task<(bool success, IList<ErrorResponse> errors)> Delete(long id);
        Task<(bool success, IList<Video> videos, IList<ErrorResponse> errors)> FindVideos(long id);
    }
}
