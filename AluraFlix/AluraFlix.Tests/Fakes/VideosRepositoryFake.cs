using AluraFlix.Core.Models;
using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluraFlix.Tests.Fakes
{
    public class VideosRepositoryFake : IVideosRepository
    {
        public List<Video> Videos { get; }
        private int _sequency;

        public VideosRepositoryFake()
        {
            Videos = new List<Video>();
            _sequency = 1;
        }
        public async Task<(bool success, Video video)> Change(VideoUpdateRequest request)
        {
            await Task.CompletedTask;
            var video = await Find(request.Id);
            if(video.Id == 0)
            {
                return (false, null);
            }
            video.Descricao = request.Descricao;
            video.Titulo = request.Titulo;
            video.CategoriaId = request.CategoriaId;
            video.Url = request.Url;
            return (true, video);
        }

        public async Task<bool> Delete(long id)
        {
            await Task.CompletedTask;
            var video = await Find(id);
            if(video.Id == 0)
            {
                return false;
            }
            Videos.Remove(video);
            return true;
        }

        public async Task<Video> Find(long id)
        {
            await Task.CompletedTask;
            var video = Videos.Find(v => v.Id == id);
            if(video is null)
            {
                return new Video
                {
                    Id = 0
                };
            }
            return video;
        }

        public async Task<IList<Video>> FindByCategoriaId(long id)
        {
            await Task.CompletedTask;
            return Videos.FindAll(v => v.CategoriaId == id);
        }

        public async Task<IList<Video>> ListAll()
        {
            await Task.CompletedTask;
            return Videos;
        }

        public async Task<(bool success, Video video)> Register(VideoRequest request)
        {
            await Task.CompletedTask;
            var video = new Video
            {
                CategoriaId = request.CategoriaId,
                Descricao = request.Descricao,
                Id = _sequency,
                Titulo = request.Titulo,
                Url = request.Url
            };
            Videos.Add(video);
            _sequency++;
            return (true, video);
        }

        public async Task<IList<Video>> Search(string search)
        {
            await Task.CompletedTask;
            return Videos.FindAll(v => v.Titulo.ToUpper().Contains(search.ToUpper()));
        }
    }
}
