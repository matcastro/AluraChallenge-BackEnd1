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
    public class VideosControllerFind
    {
        [Fact]
        public async Task DadoIdExistenteDeveRetornarVideo()
        {
            //arrange
            var tituloEsperado = "Titulo de teste";

            var repository = new VideosRepositoryFake();
            var service = new VideosService(repository);
            var controller = new VideosController(service);
            var videoRequest = new VideoRequest
            {
                CategoriaId = 1,
                Descricao = "Descricao de teste",
                Titulo = tituloEsperado,
                Url = "https://www.urldeteste.com"
            };
            var resultRegistro = (ObjectResult)await controller.Register(videoRequest);
            var video = resultRegistro.Value as Video;

            //act
            var result = (ObjectResult) await controller.Find(video.Id);
            var retorno = result.Value as Video;
            //assert
            Assert.Equal(tituloEsperado, retorno.Titulo);
        }

        [Fact]
        public async Task DadoIdInexistenteDeveRetornarCodigoDeErroNaoEncontrado()
        {
            //arrange
            var erroEsperado = ErrorEnum.NOT_FOUND;

            var repository = new VideosRepositoryFake();
            var service = new VideosService(repository);
            var controller = new VideosController(service);
            
            //act
            var result = (ObjectResult)await controller.Find(100);
            var retorno = result.Value as List<ErrorResponse>;
            //assert
            Assert.Equal(erroEsperado, retorno[0].Code);
        }
    }
}
