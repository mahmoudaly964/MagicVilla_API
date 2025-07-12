using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        public Task<LoginResponseDTO> Login(LoginRequestDTO login);
        public Task<User> Register(RegisterationRequestDTO register);
    }
}
