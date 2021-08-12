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
    public class CategoriasControllerDelete
    {
        [Fact]
        public async Task DadoIdValidoDeveRemoverDoRepositorio()
        {
            //arrange
            var repository = new CategoriaRepositoryFake();
            var service = new CategoriasService(repository, null);
            var controller = new CategoriasController(service);
            var categoriaRequest = new CategoriaRequest
            {
                Titulo = "Titulo de teste",
                Cor = "#FFFFFF"
            };
            await controller.Register(categoriaRequest);

            categoriaRequest = new CategoriaRequest
            {
                Titulo = "Titulo de teste",
                Cor = "#FFFFFF"
            };
            var resultRegistro = (ObjectResult)await controller.Register(categoriaRequest);
            var categoria = resultRegistro.Value as Categoria;
            //act
            var result = (StatusCodeResult)await controller.Delete(categoria.Id);

            //assert
            Assert.Equal(200, result.StatusCode);
            Assert.Single(repository.Categorias);
        }

        [Fact]
        public async Task DadoIdInvalidoDeveRetornar400()
        {
            //arrange
            var repository = new CategoriaRepositoryFake();
            var service = new CategoriasService(repository, null);
            var controller = new CategoriasController(service);

            //act
            var result = (ObjectResult) await controller.Delete(100);

            //assert
            Assert.Equal(400, result.StatusCode);
        }
    }
}
