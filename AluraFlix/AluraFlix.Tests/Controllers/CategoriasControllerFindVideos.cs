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
    public class CategoriasControllerFindVideos
    {
        [Fact]
        public async Task DadoIdExistenteDeveRetornarVideos()
        {
            //arrange
            var videoRepository = new VideosRepositoryFake();
            var repository = new CategoriaRepositoryFake();
            var service = new CategoriasService(repository, videoRepository);
            var controller = new CategoriasController(service);
            var categoriaRequest = new CategoriaRequest
            {
                Titulo = "Titulo de teste",
                Cor = "#FFFFFF"
            };
            var resultRegistro = (ObjectResult)await controller.Register(categoriaRequest);
            var categoria = resultRegistro.Value as Categoria;

            var videoService = new VideosService(videoRepository);
            var videoController = new VideosController(videoService);
            var videoRequest = new VideoRequest
            {
                CategoriaId = categoria.Id,
                Descricao = "Descricao de teste",
                Titulo = "Titulo de teste",
                Url = "https://www.urldeteste.com"
            };
            await videoController.Register(videoRequest);
            videoRequest = new VideoRequest
            {
                CategoriaId = categoria.Id + 1,
                Descricao = "Descricao de teste",
                Titulo = "Titulo de teste",
                Url = "https://www.urldeteste.com"
            };
            await videoController.Register(videoRequest);
            videoRequest = new VideoRequest
            {
                CategoriaId = categoria.Id,
                Descricao = "Descricao de teste",
                Titulo = "Titulo de teste",
                Url = "https://www.urldeteste.com"
            };
            await videoController.Register(videoRequest);

            //act
            var result = (ObjectResult) await controller.FindVideos(categoria.Id);
            var retorno = result.Value as List<Video>;
            //assert
            Assert.Equal(2, retorno.Count);
        }

        [Fact]
        public async Task DadoIdInexistenteDeveRetornarCodigoDeErroNaoEncontrado()
        {
            //arrange
            var erroEsperado = ErrorEnum.NOT_FOUND;

            var repository = new VideosRepositoryFake();
            var service = new CategoriasService(null, repository);
            var controller = new CategoriasController(service);
            
            //act
            var result = (ObjectResult)await controller.FindVideos(100);
            var retorno = result.Value as List<ErrorResponse>;
            //assert
            Assert.Equal(erroEsperado, retorno[0].Code);
        }
    }
}
