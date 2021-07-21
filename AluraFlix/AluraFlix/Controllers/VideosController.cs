using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Models.Responses;
using AluraFlix.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AluraFlix.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly IVideosService _videosService;

        public VideosController(IVideosService videosService)
        {
            _videosService = videosService;
        }

        [HttpGet]
        public async Task<IActionResult> ListAll()
        {
            return Ok(await _videosService.ListAll());
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Find([FromRoute] long id)
        {
            try
            {
                var (success, video, errors) = await _videosService.Find(id);
                if (success)
                {
                    return Ok(video);
                }
                else
                {
                    return BadRequest(errors);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new List<ErrorResponse>{
                    new ErrorResponse
                    {
                        Code = ErrorEnum.GENERIC_ERROR,
                        Description = "An unexpected error has occurred."
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] VideoRequest request)
        {
            try
            {
                var (success, video, errors) = await _videosService.Register(request);
                if (success)
                {
                    return Ok(video);
                }
                else
                {
                    return BadRequest(errors);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new List<ErrorResponse>{
                    new ErrorResponse
                    {
                        Code = ErrorEnum.GENERIC_ERROR,
                        Description = "An unexpected error has occurred."
                    }
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Change([FromBody] VideoUpdateRequest request)
        {
            try
            {
                var (success, video, errors) = await _videosService.Change(request);
                if (success)
                {
                    return Ok(video);
                }
                else
                {
                    return BadRequest(errors);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new List<ErrorResponse>{
                    new ErrorResponse
                    {
                        Code = ErrorEnum.GENERIC_ERROR,
                        Description = "An unexpected error has occurred."
                    }
                });
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                var (success, errors) = await _videosService.Delete(id);
                if (success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(errors);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new List<ErrorResponse>{
                    new ErrorResponse
                    {
                        Code = ErrorEnum.GENERIC_ERROR,
                        Description = "An unexpected error has occurred."
                    }
                });
            }
        }
    }
}
