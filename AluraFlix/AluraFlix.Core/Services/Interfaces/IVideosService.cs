using AluraFlix.Core.Models;
using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluraFlix.Core.Services.Interfaces
{
    public interface IVideosService
    {
        Task<IList<Video>> ListAll();
        Task<(bool success, Video video, IList<ErrorResponse> errors)> Find(long id);
        Task<(bool success, Video video, IList<ErrorResponse> errors)> Register(VideoRequest request);
        Task<(bool success, Video video, IList<ErrorResponse> errors)> Change(VideoUpdateRequest request);
        Task<(bool success, IList<ErrorResponse> errors)> Delete(long id);
    }
}
