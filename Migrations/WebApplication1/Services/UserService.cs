
using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;

using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Context;
using WebApplication1.Models.DTOs;
using WebApplication1.Exceptions;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly TodoContext _context;
        private readonly IRepository<int, UserDetail> _UserDetailRepo;
        private readonly IRepository<int, User> _UserRepo;

        private readonly ITokenService _tokenService;

        public UserService(TodoContext context, IRepository<int, UserDetail> UserDetailRepo, IRepository<int, User> UserRepo, ITokenService tokenService)
        {
            _context = context;
            _UserDetailRepo = UserDetailRepo;
            _UserRepo = UserRepo;
            _tokenService = tokenService;

        }
        public async Task<LoginReturnDTO> Login(UserLoginDTO loginDTO)
        {
            var data = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDTO.Username);
            //if (data == null) { throw new UnauthorizedUserException("Invalid UserDetailname or password"); }
            var UserDetailDB = await _UserDetailRepo.Get(data.UserId);
            //if (UserDetailDB == null)
            //{
            //    throw new UnauthorizedUserException("Invalid UserDetailname or password");
            //}
            HMACSHA512 hMACSHA = new HMACSHA512(UserDetailDB.PasswordHashKey);
            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            bool isPasswordSame = ComparePassword(encrypterPass, UserDetailDB.Password);
            if (isPasswordSame)
            {
                var User = await _UserRepo.Get(data.UserId);
                // if(UserDetailDB.Status =="Active")
                //{
                LoginReturnDTO loginReturnDTO = MapUserToLoginReturn(User);
                return loginReturnDTO;
                // }

                //throw new UserNotActiveException("Your account is not activated");
            }
            throw new UnauthorizedUserException("Invalid UserDetailname or password");
        }

        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<LoginReturnDTO> Register(UserDTO UserDTO)
        {
            var found = await _context.Users.FirstOrDefaultAsync(p => p.Username == UserDTO.Username);
            if (found != null)
            {
                throw new Exception("User with this Username Already Exist, Kindly Login");
            }
            User user = null;
            UserDetail UserDetail = null;
            try
            {
                user = UserDTO;
     
                UserDetail = MapUserUserDetailDTOToUserDetail(UserDTO);
                user = await _UserRepo.Add(user);
                UserDetail.UserId = user.UserId;
                UserDetail = await _UserDetailRepo.Add(UserDetail);
               
                ((UserDTO)user).Password = string.Empty;

                var User = await _UserRepo.Get(UserDetail.UserId);
                // if(UserDetailDB.Status =="Active")
                //{
                LoginReturnDTO loginReturnDTO = MapUserToLoginReturn(User);
                return loginReturnDTO;
                //return user;
            }
            catch (Exception) { }
            if (user != null)
                await RevertUserInsert(user);
            if (UserDetail != null && user == null)
                await RevertUserDetailInsert(UserDetail);
            throw new UnableToRegisterException("Not able to register at this moment");
        }

        private LoginReturnDTO MapUserToLoginReturn(User User)
        {
            LoginReturnDTO returnDTO = new LoginReturnDTO();
            returnDTO.UserID = User.UserId;
         
            returnDTO.Token = _tokenService.GenerateToken(User);
            return returnDTO;
        }

        private async Task RevertUserDetailInsert(UserDetail UserDetail)
        {
            await _UserDetailRepo.Delete(UserDetail.UserId);
        }
       
       private async Task RevertUserInsert(User User)
        {

            await _UserRepo.Delete(User.UserId);
        }

        private UserDetail MapUserUserDetailDTOToUserDetail(UserDTO UserDTO)
        {
            UserDetail UserDetail = new UserDetail();
            UserDetail.UserId = UserDTO.UserId;
            UserDetail.RegistrationDate = DateTime.Now;
            UserDetail.Status = "Disabled";
            HMACSHA512 hMACSHA = new HMACSHA512();
            UserDetail.PasswordHashKey = hMACSHA.Key;
            UserDetail.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(UserDTO.Password));
            return UserDetail;
        }

       }
}
