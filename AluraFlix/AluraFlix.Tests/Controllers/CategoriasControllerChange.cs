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
    public class CategoriasControllerChange
    {
        [Fact]
        public async Task DadaCategoriaValidaDeveAtualizarABase()
        {
            //arrange
            var tituloEsperado = "Titulo de teste alterado";
            var corEsperada = "#FFFFFF";
            
            var repository = new CategoriaRepositoryFake();
            var service = new CategoriasService(repository, null);
            var controller = new CategoriasController(service);
            var categoriaRequest = new CategoriaRequest
            {
                Titulo = "Titulo de teste",
                Cor = corEsperada
            };
            var resultRegistro = (ObjectResult)await controller.Register(categoriaRequest);
            var categoria = resultRegistro.Value as Categoria;
            var categoriaUpdateRequest = new CategoriaUpdateRequest
            {
                Id = categoria.Id,
                Titulo = tituloEsperado,
                Cor = corEsperada
            };
            //act
            var result = (ObjectResult)await controller.Change(categoriaUpdateRequest);
            var retorno = result.Value as Categoria;

            //assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(tituloEsperado, retorno.Titulo);
            Assert.Equal(corEsperada, retorno.Cor);
        }

        [Fact]
        public async Task DadaCategoriaInvalidoDeveRetornar400()
        {
            //arrange
            var repository = new CategoriaRepositoryFake();
            var service = new CategoriasService(repository, null);
            var controller = new CategoriasController(service);
            var categoriaUpdateRequest = new CategoriaUpdateRequest
            {
                Id = 0,
                Cor = "cor invalida"
            };
            //act
            var result = (ObjectResult)await controller.Change(categoriaUpdateRequest);

            //assert
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task DadaCategoriaInexistenteDeveRetornarNaoEncontrado()
        {
            //arrange
            var erroEsperado = ErrorEnum.GENERIC_ERROR;
            var repository = new CategoriaRepositoryFake();
            var service = new CategoriasService(repository, null);
            var controller = new CategoriasController(service);
            var categoriaRequest = new CategoriaRequest
            {
                Titulo = "Titulo de teste",
                Cor = "#FFFFFF"
            };
            var resultRegistro = (ObjectResult)await controller.Register(categoriaRequest);
            var categoria = resultRegistro.Value as Categoria;
            var categoriaUpdateRequest = new CategoriaUpdateRequest
            {
                Id = categoria.Id + 1,
                Titulo = "Titulo 2",
                Cor = categoriaRequest.Cor
            };
            //act
            var result = (ObjectResult)await controller.Change(categoriaUpdateRequest);
            var retorno = result.Value as List<ErrorResponse>;
            //assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(erroEsperado, retorno[0].Code);
        }
    }
}
