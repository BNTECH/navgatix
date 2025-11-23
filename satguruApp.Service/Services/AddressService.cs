using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class AddressService : Repository<Address>, IAddressService
    {
        public AddressService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public System.Data.DataTable GetdtLatLong(string address)



        {
            DataSet dsResult = new DataSet();
            string url = "https://maps.google.com/maps/api/geocode/xml?address=" + address + "&sensor=false&key=AIzaSyBHXNt7opkXdOuV82DQ9gpy5_ovKdXkIS0";//"https://maps.googleapis.com/maps/api/geocode/xml?key=AIzaSyBHXNt7opkXdOuV82DQ9gpy5_ovKdXkIS0&address={address}&sensor=true_or_false";
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);



            using (System.Net.WebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    dsResult.ReadXml(reader);
                }
            }
            if (Convert.ToString(dsResult.Tables["geocoderesponse"].Rows[0]["status"]) == "ZERO_RESULTS")
            {
                return dsResult.Tables["geocoderesponse"];
            }
            return dsResult.Tables["location"];
        }



        public async Task<bool> UpdAddressLongLat()
        {



            try
            {
                var results = await (from a in _db.Addresses
                                     join cty in _db.Cities on a.State_Id equals cty.StateId
                                     where a.Latitude == null
                                     select new { a, cty })
                .ToListAsync();
                if (results != null)
                {
                    foreach (var item in results)
                    {
                        var stNm = _db.States.Where(x => x.Id == item.a.State_Id).Select(x => x.Name).FirstOrDefault();
                        string address = item.a.Address1 + item.cty.CityName + stNm + item.a.Zip_Code;
                        var res = GetdtLatLong(address);
                        if (res != null)
                        {
                            if (Convert.ToBoolean(res.Columns.Contains("status")))
                            {
                                continue;
                            }
                            item.a.Latitude = Convert.ToDouble(res.Rows[0]["lat"]);
                            item.a.Longitude = Convert.ToDouble(res.Rows[0]["lng"]);
                            item.a.Updated_By = AppUserId;
                            item.a.Updated_Datetime = DateTime.UtcNow;
                            await _db.SaveChangesAsync();
                        }
                    }
                }



            }
            catch (Exception)
            {



            }



            return true;
        }
        public async Task<List<StateViewModel>> GetStateList()
        {
            return await (from b in _db.States
                          select new StateViewModel
                          {
                              Id = b.Id,
                              Name = b.Name,
                              Code = b.StateCode
                          }).ToListAsync();
        }



        public async Task<AddressViewModel> AddUpdateAddress(AddressViewModel vmModel)
        {
            vmModel.StateName = await _db.States.Where(x => x.Id == vmModel.StateId).Select(x => x.Name).FirstOrDefaultAsync();
            var latLong = GetdtLatLong(vmModel.Address1 + vmModel.Unit + vmModel.ZipCode + vmModel.City + vmModel.StateName);
            if (latLong.TableName == "location")
            {
                vmModel.Latitude = Convert.ToDouble(latLong.Rows[0]["lat"]);
                vmModel.Longitude = Convert.ToDouble(latLong.Rows[0]["lng"]);
            }
            if (vmModel.Id != 0)
            {
                var add = _db.Addresses.Where(x => x.Id == vmModel.Id && x.Is_Deleted == false).FirstOrDefault();
                vmModel.RKUpateTo(add);
                add.Updated_By = AppUserId;
                add.Updated_Datetime = DateTime.UtcNow;
            }

            if (vmModel.Id == 0)
            {
                var address = new Address();
                vmModel.RKUpateTo(address);
                address.Table_Row_Id = vmModel.TableRowId;
                address.Ct_Table_Id = vmModel.CttableId;
                address.Created_By = AppUserId;
                address.Created_Datetime = DateTime.UtcNow;
                _db.Addresses.Add(address);
            }
            await _db.SaveChangesAsync();
            return vmModel;



        }
        public async Task<RKAddressDetailViewModel> GetAddressCompanyById(int ID)
        {
            RKAddressDetailViewModel address = new RKAddressDetailViewModel();

            if (ID > 0)
            {
                address = await (from add in _db.Addresses
                                 join cty in _db.Cities on add.City_Id equals cty.Id
                                 where add.Id == ID && add.Is_Deleted == false
                                 select new RKAddressDetailViewModel
                                 {
                                     Id = add.Id,
                                     CttableId = add.Ct_Table_Id.GetValueOrDefault(),
                                     TableRowId = add.Table_Row_Id,
                                     CTTypeID = add.Ct_Type_Id,
                                     City = cty.CityName,
                                     Address1 = add.Address1,
                                     ZipCode = add.Zip_Code,
                                     StateId = add.State_Id,
                                     //Unit = add.Unit,
                                     //Latitude = add.Latitude,
                                     //Longitude = add.Longitude



                                 }).FirstOrDefaultAsync();
            }
            return address;
        }





        public async Task< RKAddressDetailViewModel> DoActiveAndInactiveAddress(RKAddressDetailViewModel VMModel)
        {
            if (VMModel.Id > 0)
            {
                var _address = await _db.Addresses.OrderByDescending(x => x.Id).Where(x => x.Id == VMModel.Id).FirstOrDefaultAsync();
                _address.Is_Deleted = !_address.Is_Deleted;
                _address.Updated_By = AppUserId;
                _address.Updated_Datetime = DateTime.Now;
                 await _db.SaveChangesAsync();
            }



            return VMModel;
        }



        public async Task<List<AddressViewModel>> FilterAddressList(AddressViewModel vmModel)
        {
            var res = await (from add in _db.Addresses
                             join stat in _db.States on add.State_Id equals stat.Id
                             join ctry in _db.Countries on stat.CountryId equals ctry.Id
                             join cty in _db.Cities on add.City_Id equals cty.Id
                             where add.Is_Deleted == vmModel.IsDeleted && add.Table_Row_Id == vmModel.TableRowId
                             select new AddressViewModel
                             {
                                 Id = add.Id,
                                 CttableId = add.Ct_Table_Id.GetValueOrDefault(),
                                 TableRowId = add.Table_Row_Id,
                                 CTTypeID = add.Ct_Type_Id,
                                 City = cty.CityName,
                                 Address1 = add.Address1,
                                 //Address2 = add.Address2,
                                 Name = add.Name,
                                 ZipCode = add.Zip_Code,
                                 CountryId = ctry.Id,
                                 StateId = add.State_Id,
                                 StateName = stat.Name,
                                 //Unit = add.Unit,
                                 //Latitude = add.Latitude,
                                 //Longitude = add.Longitude,
                                 IsDeleted = add.Is_Deleted.GetValueOrDefault()
                             }).ToListAsync();
            vmModel.TotalCount = res.Count();
            return res;
        }
        public async Task<AddressViewModel> AddUpdateAddressCompany(AddressViewModel vmModel)
        {
            if (vmModel.Id != 0)
            {
                var add = await _db.Addresses.Where(x => x.Id == vmModel.Id && !x.Is_Deleted.GetValueOrDefault()).FirstOrDefaultAsync();
                vmModel.RKUpateTo(add);
                //add.Address2 = vmModel.Address2;
                add.Name = vmModel.Name;
                add.Table_Row_Id = vmModel.TableRowId;
                add.Ct_Table_Id = vmModel.CttableId;
                add.Updated_By = AppUserId;
                add.Updated_Datetime = DateTime.UtcNow;
            }



            if (vmModel.Id == 0)
            {
                var address = new Address();
                vmModel.RKUpateTo(address);
                //address.Address2 = vmModel.Address2;
                address.Table_Row_Id = vmModel.TableRowId;
                address.Name = vmModel.Name;
                address.Ct_Table_Id = vmModel.CttableId;
                address.Created_By = AppUserId;
                address.Created_Datetime = DateTime.UtcNow;
                vmModel.Id = address.Id;
                _db.Addresses.Add(address);
            }
            await _db.SaveChangesAsync();
            return vmModel;



        }
        public NetTopologySuite.Geometries.Geometry getGeoPoint(GeoLocationPointViewModel getPoint)
        {
            var point = string.Empty;
            var hasPoint = !String.IsNullOrWhiteSpace(getPoint.Longitude) && !String.IsNullOrWhiteSpace(getPoint.Latitude);
            if (hasPoint)
                point = string.Format(System.Globalization.CultureInfo.InvariantCulture.NumberFormat,
                "POINT({0} {1})", getPoint.Longitude, getPoint.Latitude);
            return null;
        }
    }
}
