using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class UserInfoService : Repository<ApplicationUser>, IUserInfoService
    {
        public UserInfoService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public async Task<int> SaveAsync(UserInfoViewModel userInfo)
        {
            var gender = await _db.Genders.Where(x => x.Name == userInfo.Gender || x.Id == userInfo.GenderId).FirstOrDefaultAsync();
            if (gender == null && !string.IsNullOrEmpty(userInfo.Gender))
            {
                gender = new Gender();
                gender.Name = userInfo.Gender;
                gender.IsDeleted = false;
                _db.Genders.Add(gender);
                await _db.SaveChangesAsync();
            }
            if (userInfo.UserId != null)
            {
                var usrInfo = await _db.UserInformations.Where(x => x.Email == userInfo.Email || x.UserId == userInfo.UserId || x.PhoneNumber == userInfo.PhoneNumber).FirstOrDefaultAsync();
                if ((userInfo.Id == null || userInfo.Id == Guid.Empty) && usrInfo == null)
                {
                    UserInformation userinfo = new UserInformation();
                    userinfo.Id = Guid.NewGuid();
                    userinfo.FirstName = userInfo.FirstName;
                    userinfo.LastName = userInfo.LastName;
                    userinfo.Email = userInfo.Email;
                    userinfo.MiddleName = "";
                    userinfo.Mobile = userInfo.Mobile;
                    userinfo.PhoneNumber = userInfo.PhoneNumber;
                    userinfo.FacebookLink = userInfo.FacebookLink;
                    userinfo.InstagramLink = userInfo.InstagramLink;
                    userinfo.WhatsAppLink = userInfo.WhatsAppLink;
                    userinfo.AccountTypeId = userInfo.AccountTypeId;
                    userinfo.Company = userInfo.Company;
                    userinfo.DOB = userInfo.DOB;
                    userinfo.GenderId = userInfo.GenderId;
                    userinfo.Description = userInfo.Description;
                    userinfo.ProfilePic = userInfo.ProfilePic;
                    userinfo.IsDeleted = false;
                    userinfo.UserId = userInfo.UserId;
                    _db.UserInformations.Add(userinfo);
                }
                else
                {
                    UserInformation userinfo = await _db.UserInformations.Where(x => x.Id == userInfo.Id || x.Email == userInfo.Email || x.UserId == userInfo.UserId).FirstOrDefaultAsync();
                    userinfo.FirstName = userinfo.FirstName;
                    userinfo.LastName = userinfo.LastName;
                    userinfo.Email = userinfo.Email;
                    userinfo.MiddleName = userinfo.MiddleName;
                    userinfo.Mobile = userinfo.Mobile;
                    userinfo.PhoneNumber = userinfo.PhoneNumber;
                    userinfo.FacebookLink = userinfo.FacebookLink;
                    userinfo.InstagramLink = userinfo.InstagramLink;
                    userinfo.WhatsAppLink = userinfo.WhatsAppLink;
                    userinfo.AccountTypeId = userinfo.AccountTypeId;
                    userinfo.Company = userinfo.Company;
                    userinfo.DOB = userinfo.DOB;
                    userinfo.GenderId = userinfo.GenderId;
                    userinfo.IsDeleted = false;
                }

                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<UserInfoViewModel> GetUserDetailbyId(string userId)
        {
            try
            {
                return await (from user in _db.UserInformations
                              join accountType in _db.AccountTypes on user.AccountTypeId equals accountType.Id into accountTypes
                              from accountType in accountTypes.DefaultIfEmpty()
                              join gender in _db.Genders on user.GenderId equals gender.Id into genders
                              from gender in genders.DefaultIfEmpty()
                              where user.UserId == userId && (user.IsDeleted == null || user.IsDeleted == false)
                              select new UserInfoViewModel
                              {
                                  FirstName = user.FirstName,
                                  LastName = user.LastName,
                                  Name = user.FirstName + (!string.IsNullOrEmpty(user.LastName) ? (" " + user.LastName) : ""),
                                  MiddleName = user.MiddleName,
                                  Email = user.Email,
                                  PhoneNumber = user.PhoneNumber,
                                  Mobile = user.Mobile,
                                  AccountTypeId = user.AccountTypeId,
                                  UserId = user.UserId,
                                  DOB = user.DOB,
                                  IsDeleted = user.IsDeleted,
                                  CreatedDate = user.CreatedDate,
                                  UpdatedDate = user.UpdatedDate,
                                  WhatsAppLink = user.WhatsAppLink,
                                  TwiterLink = user.TwiterLink,
                                  FacebookLink = user.FacebookLink,
                                  InstagramLink = user.InstagramLink,
                                  WebsiteLink = user.WebsiteLink,
                                  Company = user.Company,
                                  GenderId = user.GenderId,
                                  Gender = gender.Name,
                                  AccountTypeName = accountType.Name
                              }).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public async Task<List<UserInfoViewModel>> GetUserList(UserSearchViewModel userSearch)
        {
            return await (from user in _db.UserInformations
                          join accountType in _db.AccountTypes on user.AccountTypeId equals accountType.Id into accountTypes
                          from accountType in accountTypes.DefaultIfEmpty()
                          join gender in _db.Genders on user.GenderId equals gender.Id into genders
                          from gender in genders.DefaultIfEmpty()
                          where (user.IsDeleted == false || user.IsDeleted == null) && (userSearch.Mobile == null || userSearch.Mobile == user.Mobile) && (string.IsNullOrEmpty(userSearch.Email) || userSearch.Email.ToLower() == user.Email.ToLower()) && string.IsNullOrEmpty(userSearch.Name) || (user.FirstName + " " + (string.IsNullOrEmpty(user.MiddleName) ? "" : user.MiddleName) + " " + user.LastName).ToLower().Contains(userSearch.Name.ToLower()) && (string.IsNullOrEmpty(userSearch.PhoneNumber) || user.PhoneNumber.Contains(userSearch.PhoneNumber)) && (string.IsNullOrEmpty(userSearch.Search) || (user.FirstName + " " + (string.IsNullOrEmpty(user.MiddleName) ? "" : user.MiddleName) + " " + user.LastName).ToLower().Contains(userSearch.Search.ToLower()) || (user.Email.ToLower().Contains(userSearch.Search.ToLower())))
                          select new UserInfoViewModel
                          {
                              FirstName = user.FirstName,
                              LastName = user.LastName,
                              Name = user.FirstName + (!string.IsNullOrEmpty(user.LastName) ? (" " + user.LastName) : ""),
                              MiddleName = user.MiddleName,
                              Email = user.Email,
                              PhoneNumber = user.PhoneNumber,
                              Mobile = user.Mobile,
                              AccountTypeId = user.AccountTypeId,
                              UserId = user.UserId,
                              DOB = user.DOB,
                              IsDeleted = user.IsDeleted,
                              CreatedDate = user.CreatedDate,
                              UpdatedDate = user.UpdatedDate,
                              WhatsAppLink = user.WhatsAppLink,
                              TwiterLink = user.TwiterLink,
                              FacebookLink = user.FacebookLink,
                              InstagramLink = user.InstagramLink,
                              WebsiteLink = user.WebsiteLink,
                              Company = user.Company,
                              GenderId = user.GenderId,
                              Gender = gender.Name,
                              AccountTypeName = accountType.Name
                          }).ToListAsync();
        }
        public async Task<List<UserInfoViewModel>> GetUserDetailList(UserSearchViewModel userSearch)
        {
            return await (from user in _db.Users
                          join userInfo in _db.UserInformations on user.Id equals userInfo.UserId
                          join accountType in _db.AccountTypes on userInfo.AccountTypeId equals accountType.Id into accountTypes
                          from accountType in accountTypes.DefaultIfEmpty()
                          join gender in _db.Genders on userInfo.GenderId equals gender.Id into genders
                          from gender in genders.DefaultIfEmpty()
                          where ///accountType.Name.ToUpper() == "WRITER" &&
                         (string.IsNullOrEmpty(userSearch.Search) || (userInfo.PhoneNumber.Contains(userSearch.Search) ||
                         userInfo.FirstName.ToLower().Contains(userSearch.Search.ToLower()) || userInfo.LastName.ToLower().Contains(userSearch.Search.ToLower()))
                           || userInfo.Email.ToLower().Contains(userSearch.Search.ToLower())
                      && (userInfo.IsDeleted == false || userInfo.IsDeleted == null))
                          select new UserInfoViewModel
                          {
                              FirstName = userInfo.FirstName,
                              LastName = userInfo.LastName,
                              Name = userInfo.FirstName + (!string.IsNullOrEmpty(userInfo.LastName) ? (" " + userInfo.LastName) : ""),
                              MiddleName = userInfo.MiddleName,
                              Email = user.Email,
                              PhoneNumber = user.PhoneNumber,
                              Mobile = userInfo.Mobile,
                              AccountTypeId = userInfo.AccountTypeId,
                              UserId = userInfo.UserId,
                              DOB = userInfo.DOB,
                              IsDeleted = userInfo.IsDeleted,
                              CreatedDate = userInfo.CreatedDate,
                              UpdatedDate = userInfo.UpdatedDate,
                              WhatsAppLink = userInfo.WhatsAppLink,
                              TwiterLink = userInfo.TwiterLink,
                              FacebookLink = userInfo.FacebookLink,
                              InstagramLink = userInfo.InstagramLink,
                              WebsiteLink = userInfo.WebsiteLink,
                              Company = userInfo.Company,
                              GenderId = userInfo.GenderId,
                              Gender = gender.Name,
                              Description = userInfo.Description,
                              ProfilePic = userInfo.ProfilePic,
                              AccountTypeName = accountType.Name,
                          }).ToListAsync();

        }
        public async Task<UserInfoViewModel> GetUserDetail(string userId)
        {

            var userDeail = await (from user in _db.Users
                                   join userInfo in _db.UserInformations on user.Id equals userInfo.UserId
                                   join accountType in _db.AccountTypes on userInfo.AccountTypeId equals accountType.Id into accountTypes
                                   from accountType in accountTypes.DefaultIfEmpty()
                                   join gender in _db.Genders on userInfo.GenderId equals gender.Id into genders
                                   from gender in genders.DefaultIfEmpty()
                                   where (userInfo.IsDeleted == false || userInfo.IsDeleted == null) && user.Id == userId
                                   select new UserInfoViewModel
                                   {
                                       FirstName = userInfo.FirstName,
                                       LastName = userInfo.LastName,
                                       Name = userInfo.FirstName + (!string.IsNullOrEmpty(userInfo.LastName) ? (" " + userInfo.LastName) : ""),
                                       MiddleName = userInfo.MiddleName,
                                       Email = user.Email,
                                       PhoneNumber = user.PhoneNumber,
                                       Mobile = userInfo.Mobile,
                                       AccountTypeId = userInfo.AccountTypeId,
                                       UserId = userInfo.UserId,
                                       DOB = userInfo.DOB,
                                       IsDeleted = userInfo.IsDeleted,
                                       CreatedDate = userInfo.CreatedDate,
                                       UpdatedDate = userInfo.UpdatedDate,
                                       WhatsAppLink = userInfo.WhatsAppLink,
                                       TwiterLink = userInfo.TwiterLink,
                                       FacebookLink = userInfo.FacebookLink,
                                       InstagramLink = userInfo.InstagramLink,
                                       WebsiteLink = userInfo.WebsiteLink,
                                       Company = userInfo.Company,
                                       GenderId = userInfo.GenderId,
                                       Gender = gender.Name,
                                       AccountTypeName = accountType.Name,
                                       Description = userInfo.Description,
                                       ProfilePic = userInfo.ProfilePic,
                                   }).FirstOrDefaultAsync();

            return userDeail;
        }
    }
}
