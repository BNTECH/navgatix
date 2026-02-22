using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using satguruApp.DLL.Models;
using satguruApp.Service.Authorization;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimFactory;
        private readonly SatguruDBContext _db;
        private readonly JWT _jwt;
        public UserService(SatguruDBContext context, UserManager<ApplicationUser> userManager
            , IOptions<JWT> jwt
            , RoleManager<ApplicationRole> roleManager
            , SignInManager<ApplicationUser> signInManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimFactory
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwt = jwt.Value;
            _db = context;
            _userClaimFactory = userClaimFactory;
        }
        public async Task<string> UserRegisterAsync(UserViewModel model) //UserInfoViewModel
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                // DOB = model.DOB,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.UserName.ToUpper(),
                IsActive = true,
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail == null)
            {
                try
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                }
                catch (Exception ex)
                {

                }
                return $"User Registered with username {user.UserName}";
            }
            else
            {
                return $"Email {user.Email} is already registered.";
            }
        }

        public async Task<AuthenticationViewModel> GetTokenAsync(TokenRequestViewModel model)
        {
            var authenticationModel = new AuthenticationViewModel();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.UserName);
            }
            if (user == null)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"No Accounts Registered with {model.Email}.";
                return authenticationModel;
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authenticationModel.UserInfoId = (await _db.UserInformations.FirstOrDefaultAsync(x => x.UserId == user.Id))?.Id;
                authenticationModel.CustomerId = (await _db.CustomerDetails.FirstOrDefaultAsync(x => x.UserId == user.Id))?.Id;
                authenticationModel.DriverId = (await _db.Drivers.FirstOrDefaultAsync(x => x.UserId == user.Id))?.Id;
                authenticationModel.FirstName = user.FirstName;
                authenticationModel.FirstName = user.LastName;
                authenticationModel.UserId = user.Id;
                authenticationModel.AppUserId = user.AppUserId;
                authenticationModel.IsAuthenticated = true;
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationModel.Email = user.Email;
                authenticationModel.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = rolesList.ToList();
                return authenticationModel;
            }
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
            return authenticationModel;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            try
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = new List<Claim>();
                for (int i = 0; i < roles.Count; i++)
                {
                    roleClaims.Add(new Claim("roles", roles[i]));
                }
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
                .Union(userClaims)
                .Union(roleClaims);
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                    signingCredentials: signingCredentials);
                return jwtSecurityToken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> UpdateUserRoleAsync(RoleViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.Email);
            }
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _roleManager.FindByNameAsync(model.Name);
                if (roles == null)
                {
                    await CreateRoles(model.Name);
                }
                if (roles != null)
                {
                    var userRoles = await _userManager.GetUsersInRoleAsync(model.Name);
                    if (!userRoles.Any(x => x.Email == model.Email))
                    {
                        var roleList = new List<string> { model.Name };
                        var userRole = await _userManager.AddToRoleAsync(user, model.Name);
                    }
                    return $"Added {model.Name} to user {model.Email}.";
                }
                return $"Role {model.Name} not found.";
            }
            return $"Incorrect Credentials for user {user.Email}.";
        }
        public async Task<string> UpdateUserRolesAsync(RoleViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.Email);
            }
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _roleManager.FindByNameAsync(model.Name);
                if (roles == null)
                {
                    await CreateRoles(model.Name);
                }
                if (roles != null)
                {
                    var userRoles = await _userManager.GetUsersInRoleAsync(model.Name);
                    if (!userRoles.Any(x => x.Email == model.Email))
                    {
                        var roleList = new List<string> { model.Name };
                        //var userRole = await _userManager.AddToRolesAsync(user, roleList);
                    }
                    return $"Added {model.Name} to user {model.Email}.";
                }
                return $"Role {model.Name} not found.";
            }
            return $"Incorrect Credentials for user {user.Email}.";
        }
        public async Task<string> CreateRoles(string roleName)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleExists = roles.Any(x => x.Name.ToLower() == roleName.ToLower());
            if (!roleExists)
            {
                ApplicationRole role = new ApplicationRole { Name = roleName };
                var saveRole = await _roleManager.CreateAsync(role);
            }
            return "Success";
        }

        public async Task<ApplicationRole> GetRoleAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }
        public async Task<List<ApplicationRole>> GetRolesAsync(string roleName = "")
        {
            return await _roleManager.Roles.Where(x => string.IsNullOrEmpty(roleName) || x.Name.ToLower().Contains(roleName.ToLower())).ToListAsync();
        }

        public async Task<AuthenticationViewModel> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
            }
            if (user != null)
            {
                var passStatus = await _userManager.CheckPasswordAsync(user, model.Password);
                if (passStatus)
                {
                   
                    TokenRequestViewModel tokenModel = new TokenRequestViewModel() { Email = user.Email, UserName = model.UserName, Password = model.Password };
                    var token = await GetTokenAsync(tokenModel);
                    return token;
                }
            }
            return new AuthenticationViewModel();
        }
        public async Task<ApplicationUser> FindUserByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        public async Task<ApplicationUser> FindUserByUserId(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<string> UpdateUser(UserViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                //  DOB = model.DOB,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.UserName.ToUpper(),
            };
            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded) { return "Success"; } else { return "Failed"; }
        }
        public async Task<string> ChangePassword(UserViewModel model)
        {
            var user = await FindByEmailAsync(model.Email);
            if (user == null)
            {
                user = await FindByEmailAsync(model.UserName);
            }
            var updateResult = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
            if (updateResult.Succeeded) { return "Success"; } else { return "Failed"; }
        }
        public async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await _userClaimFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();
            //claims = claims.Where(claim => _db.RequestedClaimTypes.Contains(claim.Type)).ToList();



            if (user.Id != null)
                claims.Add(new Claim(PropertyConstants.UserId, user.Id));

            if (user.FirstName != null)
                claims.Add(new Claim(PropertyConstants.FullName, user.FirstName));

            //if (user.CreatedDateTime != null)
            //    claims.Add(new Claim(PropertyConstants.CreatedDateTime, Convert.ToString(user.CreatedDateTime)));

            if (user.AppUserId != null && user.AppUserId > 0)
                claims.Add(new Claim(PropertyConstants.AppUserId, user.AppUserId.ToString()));

            if (!string.IsNullOrEmpty(user.UserName))
                claims.Add(new Claim(PropertyConstants.UserName, user.UserName.ToString()));
            if (user.PhoneNumber != null)
                claims.Add(new Claim(PropertyConstants.PhoneNumber, user.PhoneNumber));
            if (user.Email != null)
                claims.Add(new Claim(PropertyConstants.Email, user.Email));
            //claims.Add(new Claim(PropertyConstants.CompanyId, Convert.ToString(companyId)));



            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }
            return principal;
        }
        public async Task<int> SaveContactUsSupport(ContactUsViewModel contactUs)
        {
            var contactusEntity = new ContactU
            {
                UserId = contactUs.UserId,
                PhoneNumber = contactUs.PhoneNumber,
                EmailId = contactUs.EmailId,
                Description = contactUs.Description,
                IsDeleted = false,
                CreatedBy = contactUs.CreatedBy,
                CreatedDatetime = DateTime.UtcNow,
            };
            _db.ContactUs.Add(contactusEntity);
            await _db.SaveChangesAsync();
            return contactusEntity.Id;
        }

    }
}
