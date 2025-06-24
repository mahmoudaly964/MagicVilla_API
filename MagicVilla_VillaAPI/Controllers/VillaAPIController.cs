using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapperConfig;
        public VillaAPIController(ILogger<VillaAPIController> logger,ApplicationDbContext db, IMapper mapperConfig)
        {
            _logger = logger;
            _db = db;
            _mapperConfig = mapperConfig;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas() 
        {
            _logger.LogInformation("Get all villas");
            var villa= await _db.Villas.ToListAsync();
            return Ok(_mapperConfig.Map<List<VillaDTO>>(villa));
        }
        [HttpGet("{id}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if(id==0)
            {
                _logger.LogError("Error in villa with Id "+id);
                return BadRequest();
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
            if (villa == null)
                return NotFound();

            return Ok(_mapperConfig.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody]VillaDTOCreate createdVilla)
        {
            if (createdVilla == null)
            {
                return BadRequest();
            }
            //if (villa.Id != 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}
            if (await _db.Villas.FirstOrDefaultAsync(u => u.Name == createdVilla.Name) != null)
            {
                ModelState.AddModelError("", "villa Name should be unique");
                return BadRequest(ModelState);
            }
           var model= _mapperConfig.Map<Villa>(createdVilla);
            
            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();
            //return Ok(villa);
            return CreatedAtRoute("GetVilla", new {id=model.Id}, model);

        }
        [HttpDelete("{id}",Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            if(id==0)
                return BadRequest();
            var villa=await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
                return NotFound();
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            return Ok("item deleted successfully");
        }
        [HttpPut("{id}",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateVilla(int id, [FromBody] VillaDTOUpdate updatedVilla) 
        {
            if(id==0|| id!=updatedVilla.Id)
                return BadRequest();
            
            if(updatedVilla == null)
                return NotFound();
            var model = _mapperConfig.Map<Villa>(updatedVilla);
            //Villa model = new ()
            //{
            //    Id = updatedVilla.Id,
            //    Name = updatedVilla.Name,
            //    Amenity = updatedVilla.Amenity,
            //    Area = updatedVilla.Area,
            //    Details = updatedVilla.Details,
            //    ImageUrl = updatedVilla.ImageUrl,
            //    Occupancy = updatedVilla.Occupancy,
            //    Rate =  updatedVilla.Rate,
            //};
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return Ok("Villa Updated Successfully");

        }

        [HttpPatch("{id}", Name = "UpdateVillaPatch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async  Task<ActionResult> UpdateVilla(int id, JsonPatchDocument<VillaDTOUpdate>patchVilla)
        {
            if (id == 0 || patchVilla==null)
                return BadRequest();
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (villa == null)
                return NotFound();
            var model = _mapperConfig.Map<VillaDTOUpdate>(villa);
            //VillaDTOUpdate model = new ()
            //{
            //    Id = villa.Id,
            //    Name = villa.Name,
            //    Amenity = villa.Amenity,
            //    Area = villa.Area,
            //    Details = villa.Details,
            //    ImageUrl = villa.ImageUrl,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //};
            patchVilla.ApplyTo(model,ModelState);
            var modelV=_mapperConfig.Map<Villa>(model);
            //Villa modelV = new ()
            //{
            //    Id = model.Id,
            //    Name = model.Name,
            //    Amenity = model.Amenity,
            //    Area = model.Area,
            //    Details = model.Details,
            //    ImageUrl = model.ImageUrl,
            //    Occupancy = model.Occupancy,
            //    Rate = model.Rate,
            //};
            _db.Villas.Update(modelV);
            await _db.SaveChangesAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok("Villa Updated Successfully");

        }

    }
}
