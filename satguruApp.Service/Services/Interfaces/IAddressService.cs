using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IAddressService : IRepository<Address>
    {
        System.Data.DataTable GetdtLatLong(string address);
       Task< bool> UpdAddressLongLat();
       Task<List<StateViewModel>> GetStateList();
        Task<AddressViewModel> AddUpdateAddress(AddressViewModel vmModel);
       Task< AddressViewModel> AddUpdateAddressCompany(AddressViewModel vmModel);
       Task< RKAddressDetailViewModel> GetAddressCompanyById(int id);
        Task< RKAddressDetailViewModel> DoActiveAndInactiveAddress(RKAddressDetailViewModel VMModel);
        NetTopologySuite.Geometries.Geometry getGeoPoint(GeoLocationPointViewModel getPoint);
       Task< List<AddressViewModel>> FilterAddressList(AddressViewModel vmModel);
    }
}
