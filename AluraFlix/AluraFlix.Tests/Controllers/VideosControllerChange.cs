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
    public class VideosControllerChange
    {
        [Fact]
        public async Task DadoVideoValidoDeveAtualizarABase()
        {
            //arrange
            var tituloEsperado = "Titulo de teste alterado";
            var descricaoEsperada = "Descricao de teste";
            var categoriaIdEsperada = 1;
            var urlEsperada = "https://www.urldeteste.com";

            var repository = new VideosRepositoryFake();
            var service = new VideosService(repository);
            var controller = new VideosController(service);
            var videoRequest = new VideoRequest
            {
                CategoriaId = categoriaIdEsperada,
                Descricao = descricaoEsperada,
                Titulo = "Titulo de teste",
                Url = urlEsperada
            };
            var resultRegistro = (ObjectResult)await controller.Register(videoRequest);
            var video = resultRegistro.Value as Video;
            var videoUpdateRequest = new VideoUpdateRequest
            {
                Id = video.Id,
                Titulo = tituloEsperado
            };
            //act
            var result = (ObjectResult)await controller.Change(videoUpdateRequest);
            var retorno = result.Value as Video;

            //assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(tituloEsperado, retorno.Titulo);
            Assert.Equal(descricaoEsperada, retorno.Descricao);
            Assert.Equal(categoriaIdEsperada, retorno.CategoriaId);
            Assert.Equal(urlEsperada, retorno.Url);
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
                Url = "https://www.urldeteste.com"
            };
            var resultRegistro = (ObjectResult)await controller.Register(videoRequest);
            var video = resultRegistro.Value as Video;
            var videoUpdateRequest = new VideoUpdateRequest
            {
                Id = video.Id,
                Url = "url invalida"
            };
            //act
            var result = (ObjectResult)await controller.Change(videoUpdateRequest);

            //assert
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task DadoVideoInexistenteDeveRetornarNaoEncontrado()
        {
            //arrange
            var erroEsperado = ErrorEnum.NOT_FOUND;
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
            var resultRegistro = (ObjectResult)await controller.Register(videoRequest);
            var video = resultRegistro.Value as Video;
            var videoUpdateRequest = new VideoUpdateRequest
            {
                Id = video.Id + 1,
                Titulo = "Titulo 2"
            };
            //act
            var result = (ObjectResult)await controller.Change(videoUpdateRequest);
            var retorno = result.Value as List<ErrorResponse>;
            //assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(erroEsperado, retorno[0].Code);
        }
    }
}
