using AluraFlix.Controllers;
using AluraFlix.Core.Models;
using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Models.Responses;
using AluraFlix.Core.Services;
using AluraFlix.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AluraFlix.Tests.Controllers
{
    public class VideosControllerGet
    {
        [Fact]
        public async Task DadaConsultaSemParametrosDeveRetornarTodosOsVideos()
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
            await controller.Register(videoRequest);
            videoRequest = new VideoRequest
            {
                CategoriaId = 1,
                Descricao = "Descricao de teste",
                Titulo = "Titulo de teste",
                Url = "https://www.urldeteste.com"
            };
            await controller.Register(videoRequest);
            //act
            var result = (ObjectResult)await controller.Get(null);
            var retorno = result.Value as List<Video>;

            //assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(3, retorno.Count);
        }

        [Theory]
        [InlineData("teste")]
        public async Task DadoConsultaSemVideosDeveRetornarNaoEncontrado(string search)
        {
            //arrange
            var erroEsperado = ErrorEnum.NOT_FOUND;
            var repository = new VideosRepositoryFake();
            var service = new VideosService(repository);
            var controller = new VideosController(service);
            //act
            var result = (ObjectResult)await controller.Get(search);
            var retorno = result.Value as List<ErrorResponse>;
            //assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(erroEsperado, retorno[0].Code);
        }

        [Fact]
        public async Task DadaConsultaComParametroDeveRetornarTodosOsVideosCorrespondentes()
        {
            //arrange
            var search = "testa";
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
                Titulo = "Titulo testado",
                Url = "https://www.urldeteste.com"
            };
            await controller.Register(videoRequest);
            videoRequest = new VideoRequest
            {
                CategoriaId = 1,
                Descricao = "Descricao de teste",
                Titulo = "Titulo testador",
                Url = "https://www.urldeteste.com"
            };
            await controller.Register(videoRequest);
            //act
            var result = (ObjectResult)await controller.Get(search);
            var retorno = result.Value as List<Video>;
            //assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(2, retorno.Count);
        }
    }
}
