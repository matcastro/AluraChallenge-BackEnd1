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
    public class VideosService : IVideosService
    {
        private readonly IVideosRepository _videosRepository;

        public VideosService(IVideosRepository videosRepository)
        {
            _videosRepository = videosRepository;
        }

        public async Task<(bool success, Video video, IList<ErrorResponse> errors)> Change(VideoUpdateRequest request)
        {
            
            var errors = ValidateId(request.Id);
            
            if(errors.Count > 0)
            {
                return (false, null, errors);
            }

            var (successFind, foundVideo, errorsFind) = await Find(request.Id);

            if (!successFind)
            {
                return (false, null, errorsFind);
            }
            
            request.Titulo = string.IsNullOrWhiteSpace(request.Titulo) ? foundVideo.Titulo : request.Titulo;
            request.Descricao = string.IsNullOrWhiteSpace(request.Descricao) ? foundVideo.Descricao : request.Descricao;
            request.Url = string.IsNullOrWhiteSpace(request.Url) ? foundVideo.Url : request.Url;
            request.CategoriaId = request.CategoriaId == 0 ? foundVideo.CategoriaId : request.CategoriaId;

            errors.AddRange(ValidateVideoRequest(request));
            if (errors.Count > 0)
            {
                return (false, null, errors);
            }

            var (success, video) = await _videosRepository.Change(request);
            if (!success)
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.GENERIC_ERROR,
                    Description = "Error changing movie register."
                });
            }
            return (success, video, errors);
        }

        public async Task<(bool success, IList<ErrorResponse> errors)> Delete(long id)
        {
            var errors = ValidateId(id);

            if(errors.Count > 0)
            {
                return (false, errors);
            }
            var success = await _videosRepository.Delete(id);
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

        public async Task<(bool success, Video video, IList<ErrorResponse> errors)> Find(long id)
        {
            var errors = ValidateId(id);
            if(errors.Count > 0)
            {
                return (false, null, errors);
            }
            var video = await _videosRepository.Find(id);
            if(video.Id == 0)
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.NOT_FOUND,
                    Description = $"Video {id} not found."
                });
            }
            return (!(video.Id == 0), video, errors);
        }

        public async Task<IList<Video>> ListAll()
        {
            return await _videosRepository.ListAll();
        }

        public async Task<(bool success, Video video, IList<ErrorResponse> errors)> Register(VideoRequest request)
        {
            var errors = ValidateVideoRequest(request);
            if(errors.Count > 0)
            {
                return (false, null, errors);
            }

            if(request.CategoriaId <= 0)
            {
                request.CategoriaId = 1;
            }

            var (success, video) = await _videosRepository.Register(request);

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

        public async Task<(bool success, IList<Video> video, IList<ErrorResponse> errors)> Search(string search)
        {
            var errors = new List<ErrorResponse>();
            if (string.IsNullOrWhiteSpace(search))
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.SEARCH_PARAMETER_NOT_INFORMED,
                    Description = "Search parameter should not be empty."
                });
            }

            if(errors.Count > 0)
            {
                return (false, null, errors);
            }

            var videos = await _videosRepository.Search(search);
            if (videos.Count == 0)
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.NOT_FOUND,
                    Description = $"No videos found for search string {search}."
                });
            }
            return (!(videos.Count == 0), videos, errors);
        }

        private List<ErrorResponse> ValidateVideoRequest(VideoRequest request)
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

            if (string.IsNullOrWhiteSpace(request.Descricao))
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.DESCRIPTION_NOT_INFORMED,
                    Description = $"Field {nameof(request.Descricao)} must be informed."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Url))
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.URL_NOT_INFORMED,
                    Description = $"Field {nameof(request.Url)} must be informed."
                });
            }
            else if (!(Uri.TryCreate(request.Url, UriKind.Absolute, out var uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
            {
                errors.Add(new ErrorResponse
                {
                    Code = ErrorEnum.MALFORMED_URL,
                    Description = $"Field {nameof(request.Url)} must be a valid web address."
                });
            }

            return errors;
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
    }
}
