using AluraFlix.Core.Models;
using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluraFlix.Tests.Fakes
{
    public class CategoriaRepositoryFake : ICategoriasRepository
    {
        public List<Categoria> Categorias { get; }
        private int _sequency;

        public CategoriaRepositoryFake()
        {
            Categorias = new List<Categoria>();
            _sequency = 1;
        }

        public async Task<bool> Delete(long id)
        {
            await Task.CompletedTask;
            var categoria = await Find(id);
            if(categoria.Id == 0)
            {
                return false;
            }
            Categorias.Remove(categoria);
            return true;
        }

        public async Task<(bool success, Categoria categoria)> Register(CategoriaRequest request)
        {
            await Task.CompletedTask;
            var categoria = new Categoria
            {
                Cor = request.Cor,
                Id = _sequency,
                Titulo = request.Titulo
            };
            Categorias.Add(categoria);
            _sequency++;
            return (true, categoria);
        }

        public async Task<IList<Categoria>> ListAll()
        {
            await Task.CompletedTask;
            return Categorias;
        }

        public async Task<Categoria> Find(long id)
        {
            await Task.CompletedTask;
            var categoria = Categorias.Find(v => v.Id == id);
            if (categoria is null)
            {
                return new Categoria
                {
                    Id = 0
                };
            }
            return categoria;
        }

        public async Task<(bool success, Categoria categoria)> Change(CategoriaUpdateRequest request)
        {
            var categoria = await Find(request.Id);
            if (categoria.Id == 0)
            {
                return (false, null);
            }
            categoria.Cor = request.Cor;
            categoria.Titulo = request.Titulo;
            return (true, categoria);
        }
    }
}
