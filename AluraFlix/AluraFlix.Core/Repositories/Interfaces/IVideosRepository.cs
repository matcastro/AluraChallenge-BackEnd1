using AluraFlix.Core.Models;
using AluraFlix.Core.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluraFlix.Core.Repositories.Interfaces
{
    public interface IVideosRepository
    {
        Task<IList<Video>> ListAll();
        Task<Video> Find(long id);
        Task<bool> Delete(long id);
        Task<(bool success, Video video)> Change(VideoUpdateRequest request);
        Task<(bool success, Video video)> Register(VideoRequest request);
    }
}
