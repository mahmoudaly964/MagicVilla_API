using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public UserRepository(ApplicationDbContext db,IConfiguration configuration) 
        {
            _db=db;
            _configuration = configuration;
        }
        public bool IsUniqueUser(string username)
        {
            var user= _db.Users.FirstOrDefault(u=>u.UserName == username);
            return (user == null);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO login)
        {

            var user = _db.Users.FirstOrDefault(u => ((u.UserName == login.UserName) && (u.Password == login.Password)));
            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    token = "",
                    user = null
                };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["ApiSettings:Secret"];
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                                   new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
                                   )
            };
            var loginToken = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponse = new LoginResponseDTO()
            {
                token = tokenHandler.WriteToken(loginToken),
                user = user
            };
            return loginResponse;
        }

        public async Task<User> Register(RegisterationRequestDTO register)
        {
            User user = new User()
            {
                UserName=register.UserName,
                Name=register.Name,
                Password=register.Password,
                Role=register.Role
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            
            return user;
        }
    }
}
