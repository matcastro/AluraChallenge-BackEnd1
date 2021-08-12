using AluraFlix.Controllers;
using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Services;
using AluraFlix.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace AluraFlix.Tests.Controllers
{
    public class CategoriasControllerRegister
    {
        [Fact]
        public async Task DadaCategoriaValidaDeveInserirNoRepositorio()
        {
            //arrange
            var repository = new CategoriaRepositoryFake();
            var service = new CategoriasService(repository, null);
            var controller = new CategoriasController(service);
            var categoriaRequest = new CategoriaRequest
            {
                Cor = "#FFFFFF",
                Titulo = "Titulo de teste"
            };

            //act
            var result = (ObjectResult)await controller.Register(categoriaRequest);

            //assert
            Assert.Equal(200, result.StatusCode);
            Assert.Single(repository.Categorias);
        }

        [Fact]
        public async Task DadaCategoriaInvalidoDeveRetornar400()
        {
            //arrange
            var repository = new CategoriaRepositoryFake();
            var service = new CategoriasService(repository, null);
            var controller = new CategoriasController(service);
            var categoria = new CategoriaRequest
            {
                Titulo = "Titulo de teste",
                Cor = null
            };

            //act
            var result = (ObjectResult)await controller.Register(categoria);

            //assert
            Assert.Equal(400, result.StatusCode);
        }
    }
}
