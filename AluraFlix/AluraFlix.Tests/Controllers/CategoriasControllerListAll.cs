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
    public class CategoriasControllerListAll
    {
        [Fact]
        public async Task DadaConsultaSemParametrosDeveRetornarTodasAsCategorias()
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
            await controller.Register(categoriaRequest);
            categoriaRequest = new CategoriaRequest
            {
                Cor = "#FFFFFF",
                Titulo = "Titulo de teste"
            };
            await controller.Register(categoriaRequest);
            categoriaRequest = new CategoriaRequest
            {
                Cor = "#FFFFFF",
                Titulo = "Titulo de teste"
            };
            await controller.Register(categoriaRequest);
            //act
            var result = (ObjectResult)await controller.ListAll();
            var retorno = result.Value as List<Categoria>;

            //assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(3, retorno.Count);
        }
    }
}
