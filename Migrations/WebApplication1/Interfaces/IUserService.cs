using WebApplication1.Models.DTOs;

namespace WebApplication1.Interfaces
{
    public interface IUserService
    {
        public Task<LoginReturnDTO> Login(UserLoginDTO loginDTO);
        public Task<LoginReturnDTO> Register(UserDTO userDTO);
    }
}
