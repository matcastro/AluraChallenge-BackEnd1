using AluraFlix.Core.Models;
using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Models.Responses;
using AluraFlix.Core.Repositories.Interfaces;
using AluraFlix.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AluraFlix.Core.Services
{
    public class CategoriasService : ICategoriasService
    {
        private readonly ICategoriasRepository _categoriasRepository;
        private readonly IVideosRepository _videosRepository;

        public CategoriasService(ICategoriasRepository categoriasRepository,
            IVideosRepository videosRepository)
        {
            _categoriasRepository = categoriasRepository;
            _videosRepository = videosRepository;
        }

        public async Task<(bool success, Categoria categoria, IList<ErrorResponse> errors)> Change(CategoriaUpdateRequest request)
        {
            var errors = ValidateCategoriaRequest(request);
            errors.AddRange(ValidateId(request.Id));

            if (errors.Count > 0)
            {
                return (false, null, errors);
            }

            var (success, categoria) = await _categoriasRepository.Change(request);
            if (!success)
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.GENERIC_ERROR,
                    Description = "Error changing movie register."
                });
            }
            return (success, categoria, errors);
        }

        public async Task<(bool success, IList<ErrorResponse> errors)> Delete(long id)
        {
            var errors = ValidateId(id);

            if (errors.Count > 0)
            {
                return (false, errors);
            }
            var success = await _categoriasRepository.Delete(id);
            if (!success)
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.GENERIC_ERROR,
                    Description = $"Error deleting video {id}"
                });
            }
            return (success, errors);
        }

        public async Task<(bool success, Categoria categoria, IList<ErrorResponse> errors)> Find(long id)
        {
            var errors = ValidateId(id);
            if (errors.Count > 0)
            {
                return (false, null, errors);
            }
            var categoria = await _categoriasRepository.Find(id);
            if (categoria.Id == 0)
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.NOT_FOUND,
                    Description = $"Video {id} not found."
                });
            }
            return (!(categoria.Id == 0), categoria, errors);
        }

        public async Task<IList<Categoria>> ListAll()
        {
            return await _categoriasRepository.ListAll();
        }

        public async Task<(bool success, Categoria categoria, IList<ErrorResponse> errors)> Register(CategoriaRequest request)
        {
            var errors = ValidateCategoriaRequest(request);
            if (errors.Count > 0)
            {
                return (false, null, errors);
            }
            var (success, video) = await _categoriasRepository.Register(request);

            if (!success)
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.GENERIC_ERROR,
                    Description = "Error registering movie"
                });
            }
            return (true, video, errors);
        }

        public async Task<(bool success, IList<Video> videos, IList<ErrorResponse> errors)> FindVideos(long id)
        {
            var errors = ValidateId(id);
            if (errors.Count > 0)
            {
                return (false, null, errors);
            }
            var videos = await _videosRepository.FindByCategoriaId(id);
            
            if(videos.Count > 0)
            {
                return (true, videos, null);
            }
            else
            {
                return (false, videos, new List<ErrorResponse> { new ErrorResponse
                {
                    Code = ErrorEnum.NOT_FOUND,
                    Description = $"No videos found for the category {id}"
                } });
            }
        }

        private List<ErrorResponse> ValidateId(long id)
        {
            var errors = new List<ErrorResponse>();
            if (id <= 0)
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.INVALID_ID,
                    Description = "Id must be greater than zero."
                });
            }

            return errors;
        }

        private List<ErrorResponse> ValidateCategoriaRequest(CategoriaRequest request)
        {
            var errors = new List<ErrorResponse>();
            if (string.IsNullOrWhiteSpace(request.Titulo))
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.TITLE_NOT_INFORMED,
                    Description = $"Field {nameof(request.Titulo)} must be informed."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Cor))
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.COLOR_NOT_INFORMED,
                    Description = $"Field {nameof(request.Cor)} must be informed."
                });
            }

            return errors;
        }
    }
}
