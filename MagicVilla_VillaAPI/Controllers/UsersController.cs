using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/UsersAuth")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly APIResponse _response;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new();
        }
        [HttpPost("register")]
        
        public async Task<ActionResult<APIResponse>>Register([FromBody]RegisterationRequestDTO model)
        {
            bool isUnique=_userRepository.IsUniqueUser(model.UserName);
            if (!isUnique)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Username already in use");
                return BadRequest(_response);
            }
            var register = await _userRepository.Register(model);
            if (register == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("registeration failed");
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = register;
            return Ok(_response);


        }

        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);
            if (loginResponse == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode=HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);

        }



    }
}
