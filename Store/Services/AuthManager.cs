using System.Security.Principal;
using AutoMapper;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Services.Contracts;
using SQLitePCL;

namespace Services
{
    public class AuthManager : IAuthService
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IMapper _mapper;

        public AuthManager(RoleManager<IdentityRole> roleManager,
         UserManager<IdentityUser> userManager,
          IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public IEnumerable<IdentityRole> Roles =>
             _roleManager.Roles;

        public async Task<IdentityResult> CreateUser(UserDtoForCreation userDto)
        {
            var user = _mapper.Map<IdentityUser>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
                throw new Exception("User couldnt created");

            if (userDto.Roles.Count > 0)
            {
                var roleResult = await _userManager.AddToRolesAsync(user, userDto.Roles);
                if (!roleResult.Succeeded)
                    throw new Exception("System Error");
            }

            return result;
        }

        public IEnumerable<IdentityUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<IdentityUser> GetOneUser(string UserName)
        {
            return await _userManager.FindByNameAsync(UserName);
        }

        public async Task<UserDtoForUpdate> GetOneUserForUpdate(string userName)
        {
            var user = await GetOneUser(userName);
            if (user is not null)
            {
                var userDto = _mapper.Map<UserDtoForUpdate>(user);
                userDto.Roles = new HashSet<string>(Roles.Select(r => r.Name).ToList());
                userDto.UserRoles = new HashSet<string>(await _userManager.GetRolesAsync(user));
                return userDto;
            }
            throw new Exception("An error occured.");
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await GetOneUser(model.UserName);
            if (user is not null)
            {
                await _userManager.RemovePasswordAsync(user);
                var result = await _userManager.AddPasswordAsync(user, model.Password);
                return result;
            }
            throw new Exception("User couldnt found.");
        }

        public async Task Update(UserDtoForUpdate userDto)
        {
            var user = await GetOneUser(userDto.UserName);
            user.PhoneNumber = userDto.PhoneNumber;
            user.Email = userDto.Email;

            if (user is not null)
            {
                var result = await _userManager.UpdateAsync(user);
                if (userDto.Roles.Count > 0)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var r1 = await _userManager.RemoveFromRolesAsync(user, userRoles);
                    var r2 = await _userManager.AddToRolesAsync(user, userDto.Roles);
                }
                return;
            }
            throw new Exception("System has problem with user update");
        }

        public async Task<IdentityResult> DeleteUser(string userName)
        {
            var user = await GetOneUser(userName);
            if (user is not null)
            {
                var result = await _userManager.DeleteAsync(user);
                return result;
            }
            throw new Exception("Kullanıcı bulunamadı.");
        }
    }
}