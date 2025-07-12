using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly IVillaRepository _villaDb;
        private readonly IMapper _mapperConfig;
        protected APIResponse _response;
        public VillaAPIController(ILogger<VillaAPIController> logger, IVillaRepository villaDb, IMapper mapperConfig)
        {
            _logger = logger;
            _villaDb=villaDb;
            _mapperConfig = mapperConfig;
            _response = new();
        }
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ResponseCache(CacheProfileName = "Duration30")]

        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name ="occupancyfilter")]int? occupancy,
            [FromQuery] string? search) 
        {
            try
            {
                IEnumerable<Villa> villa;
                _logger.LogInformation("Get all villas");
                if (occupancy > 0)
                {
                     villa = await _villaDb.GetAllAsync(x => x.Occupancy == occupancy);

                }
                else
                {
                     villa = await _villaDb.GetAllAsync();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    villa=villa.Where(x=>x.Name.ToLower().Contains(search));
                }
                _response.Result = _mapperConfig.Map<List<VillaDTO>>(villa);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("{id}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [ResponseCache(Duration =30)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error in villa with Id " + id);
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _villaDb.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapperConfig.Map<VillaDTO>(villa);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages=new List<string>() { ex.ToString()};
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles ="admin")]

        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody]VillaDTOCreate createdVilla)
        {
            try
            {
                if (createdVilla == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //if (villa.Id != 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError);
                //}
                if (await _villaDb.GetAsync(u => u.Name == createdVilla.Name) != null)
                {
                    ModelState.AddModelError("", "villa Name should be unique");
                    return BadRequest(ModelState);
                }
                var model = _mapperConfig.Map<Villa>(createdVilla);
                _response.Result = model;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                await _villaDb.CreateAsync(model);
                //return Ok(villa);
                return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }
        [HttpDelete("{id}",Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "none")]

        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _villaDb.GetAsync(v => v.Id == id);
                if (villa == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _villaDb.RemoveAsync(villa);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPut("{id}",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaDTOUpdate updatedVilla) 
        {
            try
            {
                if (id == 0 || id != updatedVilla.Id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updatedVilla == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                var model = _mapperConfig.Map<Villa>(updatedVilla);

                await _villaDb.UpdateAsync(model);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPatch("{id}", Name = "UpdateVillaPatch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "admin")]

        public async  Task<ActionResult<APIResponse>> UpdateVilla(int id, JsonPatchDocument<VillaDTOUpdate>patchVilla)
        {
            try
            {
                if (id == 0 || patchVilla == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = _villaDb.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                var model = _mapperConfig.Map<VillaDTOUpdate>(villa);

                patchVilla.ApplyTo(model, ModelState);
                var modelV = _mapperConfig.Map<Villa>(model);

                await _villaDb.UpdateAsync(modelV);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                if (!ModelState.IsValid)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

    }
}
