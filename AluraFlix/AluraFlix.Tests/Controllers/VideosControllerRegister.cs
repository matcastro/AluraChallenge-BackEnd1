using AluraFlix.Controllers;
using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Services;
using AluraFlix.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace AluraFlix.Tests.Controllers
{
    public class VideosControllerRegister
    {
        [Fact]
        public async Task DadoVideoValidoDeveInserirNoRepositorio()
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

            //act
            var result = (ObjectResult)await controller.Register(videoRequest);

            //assert
            Assert.Equal(200, result.StatusCode);
            Assert.Single(repository.Videos);
        }

        [Fact]
        public async Task DadoVideoInvalidoDeveRetornar400()
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
                Url = "url invalida"
            };

            //act
            var result = (ObjectResult)await controller.Register(videoRequest);

            //assert
            Assert.Equal(400, result.StatusCode);
        }
    }
}
