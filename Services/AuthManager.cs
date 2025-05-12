using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class AuthManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AuthManager> _logger;

        public AuthManager(UserManager<IdentityUser> userManager, ILogger<AuthManager> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IdentityUser> RegisterUser(UserDto userDto)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = userDto.Email,
                    Email = userDto.Email
                };

                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (!result.Succeeded)
                    throw new Exception("Kullanıcı oluşturma hatası: " + string.Join(", ", result.Errors.Select(e => e.Description)));

                if (userDto.Roles.Count > 0)
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, userDto.Roles);
                    if (!roleResult.Succeeded)
                        throw new Exception("Rol atama hatası: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı kaydetme hatası");
                throw;
            }
        }
    }
} 