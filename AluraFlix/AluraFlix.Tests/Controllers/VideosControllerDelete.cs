using AluraFlix.Controllers;
using AluraFlix.Core.Models;
using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Services;
using AluraFlix.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace AluraFlix.Tests.Controllers
{
    public class VideosControllerDelete
    {
        [Fact]
        public async Task DadoIdValidoDeveRemoverDoRepositorio()
        {
            //arrange
            var repository = new VideosRepositoryFake();
            var service = new VideosService(repository);
            var controller = new VideosController(service);
            var videoRequest = new VideoRequest
            {
                CategoriaId = 1,
                Descricao = "Descricao de teste",
                Titulo = "Titulo de teste",
                Url = "https://www.urldeteste.com"
            };
            await controller.Register(videoRequest);

            videoRequest = new VideoRequest
            {
                CategoriaId = 1,
                Descricao = "Descricao de teste",
                Titulo = "Titulo de teste",
                Url = "https://www.urldeteste.com"
            };
            var resultRegistro = (ObjectResult)await controller.Register(videoRequest);
            var video = resultRegistro.Value as Video;
            //act
            var result = (StatusCodeResult)await controller.Delete(video.Id);

            //assert
            Assert.Equal(200, result.StatusCode);
            Assert.Single(repository.Videos);
        }

        [Fact]
        public async Task DadoIdInvalidoDeveRetornar400()
        {
            //arrange
            var repository = new VideosRepositoryFake();
            var service = new VideosService(repository);
            var controller = new VideosController(service);

            //act
            var result = (ObjectResult) await controller.Delete(100);

            //assert
            Assert.Equal(400, result.StatusCode);
        }
    }
}
