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
    public class CategoriasControllerFind
    {
        [Fact]
        public async Task DadoIdExistenteDeveRetornarCategoria()
        {
            //arrange
            var tituloEsperado = "Titulo de teste";

            var repository = new CategoriaRepositoryFake();
            var service = new CategoriasService(repository, null);
            var controller = new CategoriasController(service);
            var categoriaRequest = new CategoriaRequest
            {
                Titulo = tituloEsperado,
                Cor = "#FFFFFF"
            };
            var resultRegistro = (ObjectResult)await controller.Register(categoriaRequest);
            var categoria = resultRegistro.Value as Categoria;

            //act
            var result = (ObjectResult) await controller.Find(categoria.Id);
            var retorno = result.Value as Categoria;
            //assert
            Assert.Equal(tituloEsperado, retorno.Titulo);
        }

        [Fact]
        public async Task DadoIdInexistenteDeveRetornarCodigoDeErroNaoEncontrado()
        {
            //arrange
            var erroEsperado = ErrorEnum.NOT_FOUND;

            var repository = new CategoriaRepositoryFake();
            var service = new CategoriasService(repository, null);
            var controller = new CategoriasController(service);
            
            //act
            var result = (ObjectResult)await controller.Find(100);
            var retorno = result.Value as List<ErrorResponse>;
            //assert
            Assert.Equal(erroEsperado, retorno[0].Code);
        }
    }
}
