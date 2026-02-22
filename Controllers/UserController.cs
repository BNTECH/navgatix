using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace navgatix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAccountTypeService _accountTypeService;
        private readonly IUserInfoService _userInfoService;
        private readonly ITransportService _transportService;
        private readonly IAppCustormer _appCustormer;
        private string subPath = @"~/uploaddocs/";
        private static readonly string[] Summaries = new[]
      {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        public UserController(IUserService userService, IAccountTypeService accountTypeService, IUserInfoService userInfoService, ITransportService transportService, IAppCustormer appCustormer)
        {
            _userService = userService;
            _accountTypeService = accountTypeService;
            _userInfoService = userInfoService;
            _transportService = transportService;
            _appCustormer = appCustormer;
        }
        // GET: UserController
        [HttpPost]
        [AllowAnonymous]
        [HttpPost("Registration")]
        public async Task<IActionResult> UserRegisterAsync([FromBody] UserInfoViewModel model)
        {
            UserViewModel userModel = new UserViewModel
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                DOB = model.DOB,
                Password = model.Password,
            };

            var result = await _userService.UserRegisterAsync(userModel);
            //FileUpload fileUpload=Request.;
            AccountTypeViewModel accountTypeView = new AccountTypeViewModel { Name = model.RoleName };

            var roles = await _accountTypeService.SaveChangeAsync(accountTypeView);

            var accountType = await _accountTypeService.GetById(0, model.RoleName);

            model.AccountTypeId = accountType.Id;
            var user = await _userService.FindByEmailAsync(model.Email);
            if (user == null)
            { user = await _userService.FindUserByUserName(model.UserName); }
            model.UserId = user.Id;
            var userRoleVM = new RoleViewModel { Name = model.RoleName, Email = model.Email, Password = model.Password };
            var userRoles = await _userService.UpdateUserRoleAsync(userRoleVM);

            //Helpers.FileUploadExtension.SaveAs(fileUpload, subPath, true, fileUpload);
            var userResult = await _userInfoService.SaveAsync(model);
            switch (model.RoleName)
            {
                case "Driver": await _transportService.SaveDriverAsync(new DriverViewModel { UserId = model.UserId, FirstName = model.FirstName, MiddleName = model.MiddleName, LastName = model.LastName, Mobile = model.Mobile, TransporterId = model.TransporterId, DOB = model.DOB, Gender = model.Gender, LicenseExpiry = model.LicenseExpiry, LicenseNumber = model.LicenseNumber, ProfilePic = model.ProfilePic }); break;
                case "Transporter": await _transportService.SaveTransporterAsync(new TransporterViewModel { UserId = model.UserId, FirstName = model.FirstName, MiddleName = model.MiddleName, LastName = model.LastName, Mobile = model.Mobile, DOB = model.DOB, Gender = model.Gender, LicenseExpiry = model.LicenseExpiry, LicenseNumber = model.LicenseNumber, ProfilePic = model.ProfilePic, GSTNumber = model.GSTNumber, PANNumber = model.PANNumber, BankAccountNumber = model.BankAccountNumber, IFSCCode = model.IFSCCode, ProfileVerified = model.ProfileVerified }); break;
                case "Customer":
                    await _appCustormer.SaveChangeAsync(new CustomerDetailViewModel { UserId = model.UserId, GSTNumber = model.GSTNumber, CompanyName = !string.IsNullOrEmpty(model.Company) ? model.Company : model.FirstName + " " + model.LastName, City = model.City, State = model.State, Pincode = model.Pincode, Address = model.Address });
                    break;
                default:
                    break;
            }
            model.ProfilePic = userResult.ProfilePic;
            return Ok(model);
        }

        [HttpPost("uploadProfile")]
        [AllowAnonymous]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UploadProfilePic([FromBody] UserProfilePicViewModel userProfile)
        {
            var uploadFolder = Path.Combine(subPath);

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var uniqueFileName = $"{userProfile.UserId}_{DateTime.Now.Ticks}_{userProfile.File.FileName}";
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            var userResult = await _userInfoService.UpdateProfilePic(userProfile.UserId.ToString(), filePath);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await userProfile.File.CopyToAsync(stream);
            }
            userProfile.ProfilePic = filePath;
            return Ok(userProfile);

        }


        [HttpPost("AddDriver")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveDriverDetail([FromBody] DriverViewModel model)
        {
            var userModel = new UserViewModel
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                DOB = model.DOB,
                Password = model.Password,
            };

            var userStatus = await _userService.UserRegisterAsync(userModel);
            var accountTypeView = new AccountTypeViewModel { Name = model.RoleName };

            var roles = await _accountTypeService.SaveChangeAsync(accountTypeView);

            var accountType = await _accountTypeService.GetById(0, model.RoleName);

            model.AccountTypeId = accountType.Id;
            var user = await _userService.FindByEmailAsync(model.Email);
            if (user == null)
            { user = await _userService.FindUserByUserName(model.UserName); }
            model.UserId = user.Id;
            var userRoleVM = new RoleViewModel { Name = model.RoleName, Email = model.Email, Password = model.Password };
            var userRoles = await _userService.UpdateUserRoleAsync(userRoleVM);
            string subPath = @"~/uploaddocs/";

            var userResult = await _userInfoService.SaveAsync(new UserInfoViewModel { TransporterId = model.TransporterId, UserId = model.UserId, FirstName = model.FirstName, AccountTypeId = model.AccountTypeId, AppUserId = model.AppUserId.GetValueOrDefault(), AccountTypeName = model.AccountTypeName, Company = model.Company, DOB = model.DOB, Email = model.Email, GenderId = model.GenderId, FacebookLink = model.FacebookLink, PhoneNumber = model.PhoneNumber, LastName = model.LastName, LicenseExpiry = model.LicenseExpiry, LicenseNumber = model.LicenseNumber, MiddleName = model.MiddleName, Mobile = model.Mobile, Name = model.Name });
            var result = await _transportService.SaveDriverAsync(model);
            return Ok(result);
        }

        [HttpPost("updateDriverDetail")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateDriveDetail([FromBody] DriverViewModel model)
        {
            var user = await _userService.FindUserByUserId(model.UserId);
            _userService.UpdateUser(new UserViewModel { UserName = user.UserName, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, PhoneNumber = user.PhoneNumber, DOB = model.DOB });
            var result = await _transportService.SaveDriverAsync(model);
            return Ok(result);
        }
        [HttpPost("updateTransporterDetail")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateTransporterDetail([FromBody] TransporterViewModel model)
        {
            var user = await _userService.FindUserByUserId(model.UserId);
            _userService.UpdateUser(new UserViewModel { UserName = user.UserName, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, PhoneNumber = user.PhoneNumber, DOB = model.DOB });
            var result = await _transportService.SaveTransporterAsync(model); return Ok(result);
        }
        [HttpPost("updateCustomerDetail")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateCustomerDetail([FromBody] CustomerDetailViewModel model)
        {
            var user = await _userService.FindUserByUserId(model.UserId);
            _userService.UpdateUser(new UserViewModel { UserName = user.UserName, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, PhoneNumber = user.PhoneNumber });
            var result = _appCustormer.SaveChangeAsync(model);
            return Ok(result);
        }



        [HttpPost("createToken")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestViewModel model)
        {
            var result = await _userService.GetTokenAsync(model);
            return Ok(result);
        }
        [HttpPost("addUserRole")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUserRole([FromBody] RoleViewModel model)
        {
            var result = "";// await _userService.UpdateUserRoleAsync(model);
            return Ok(result);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var result = await _userService.Login(model);
            return Ok(result);
        }
        [HttpGet("getRole")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRole(string roleName)
        {
            var result = await _userService.GetRoleAsync(roleName);
            return Ok(result);
        }
        [HttpGet("getRoles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoles(string roleName = "")
        {
            var result = await _userService.GetRolesAsync(roleName);
            return Ok(result);
        }
        [HttpGet("createRoles")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var result = await _userService.CreateRoles(roleName);
            return Ok(result);
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost("updateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser([FromBody] UserInfoViewModel model)
        {
            UserViewModel userModel = new UserViewModel
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                DOB = model.DOB,
            };
            var result = await _userService.UpdateUser(userModel);
            if (result == "Success")
            {
                var userResult = await _userInfoService.SaveAsync(model);
                if (userResult.Id != null && userResult.Id != Guid.Empty)
                { model.Status = result; }
            }
            return Ok(model);
        }
        [HttpGet("getUserbyId")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserbyId(string userId)
        {
            var result = await _userService.FindUserByUserId(userId);
            var model = await _userInfoService.GetUserDetailbyId(userId);
            model.UserName = result.UserName;
            model.Email = result.Email;
            return Ok(model);
        }
        [HttpPost("getUserList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserList([FromBody] UserSearchViewModel userSearch = null)
        {
            var model = await _userInfoService.GetUserList(userSearch);
            return Ok(model);
        }




        [HttpPost("getUserDetailList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserDetailList([FromBody] UserSearchViewModel userSearch = null)
        {
            var model = await _userInfoService.GetUserDetailList(userSearch);
            return Ok(model);
        }
        [HttpGet("getUserDetail/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserDetail(string userId)
        {
            var model = await _userInfoService.GetUserDetail(userId);
            switch (model.AccountTypeName)
            {
                case "Driver":
                    var driverdetail = _transportService.GetDriverDetails(userId);
                    break;
                case "Transporter":
                    var transportDetail = _transportService.GetTransporterDetails(userId);
                    break;
                case "Customer": break;
                default:
                    break;
            }

            return Ok(model);
        }
        [HttpGet("changePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] UserInfoViewModel userInfoView)
        {
            UserViewModel userModel = new UserViewModel
            {
                UserName = userInfoView.UserName,
                Email = userInfoView.Email,
                FirstName = userInfoView.FirstName,
                LastName = userInfoView.LastName,
                PhoneNumber = userInfoView.PhoneNumber,
                DOB = userInfoView.DOB,
                Password = userInfoView.Password,
                NewPassword = userInfoView.NewPassword,
            };
            var model = await _userService.UpdateUser(userModel);
            return Ok(model);
        }
        [HttpPost("contactSupport")]
        [AllowAnonymous]
        public async Task<IActionResult> ContactSupport([FromBody] ContactUsViewModel contactUsView)
        {
            var model = await _userService.SaveContactUsSupport(contactUsView);
            return Ok(model);
        }


    }
}
