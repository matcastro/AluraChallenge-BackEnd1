using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Models.Responses;
using AluraFlix.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AluraFlix.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriasService _categoriasService;

        public CategoriasController(ICategoriasService categoriasService)
        {
            _categoriasService = categoriasService;
        }

        [HttpGet]
        public async Task<IActionResult> ListAll()
        {
            return Ok(await _categoriasService.ListAll());
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Find([FromRoute] long id)
        {
            try
            {
                var (success, categoria, errors) = await _categoriasService.Find(id);
                if (success)
                {
                    return Ok(categoria);
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
        public async Task<IActionResult> Register([FromBody] CategoriaRequest request)
        {
            try
            {
                var (success, categoria, errors) = await _categoriasService.Register(request);
                if (success)
                {
                    return Ok(categoria);
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
        public async Task<IActionResult> Change([FromBody] CategoriaUpdateRequest request)
        {
            try
            {
                var (success, categoria, errors) = await _categoriasService.Change(request);
                if (success)
                {
                    return Ok(categoria);
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
                var (success, errors) = await _categoriasService.Delete(id);
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
