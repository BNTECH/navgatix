using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class RKAddressListViewModel
    {
        public int Id { get; set; }
        public int CttableId { get; set; }
        public int? TableRowId { get; set; }
        public Guid? TableRowGuid { get; set; }
        public string Address1 { get; set; }
        public string Unit { get; set; }
        public int? StateId { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string StateName { get; set; }
        public int? CTTypeID { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string StateCode { get; set; }
        public string CareProviderName { get; set; }
    }



    public class RKAddressDetailViewModel
    {
        public long Id { get; set; }
        public int CttableId { get; set; }
        public int? TableRowId { get; set; }
        public string Address1 { get; set; }
        public int? StateId { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int? CTTypeID { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string StateName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Rate { get; set; }
    }



    public class AddressViewModel
    {
        public long Id { get; set; }
        public int CttableId { get; set; }
        public int? TableRowId { get; set; }
        public Guid? TableRowGuid { get; set; }
        public string Address1 { get; set; }
        public string Unit { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int? CTTypeID { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedDateTime { get; set; }
        public string StateName { get; set; }
        public string State { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public string Address2 { get; set; }
        public string Name { get; set; }
        public int TotalCount { get; set; }




        public Address RKUpateTo(Address dbModel)
        {
            dbModel.Address1 = Address1;
           // dbModel.Unit = Unit;
           // dbModel.City_Id = city_id;
            dbModel.State_Id = StateId;
            dbModel.Zip_Code = ZipCode;
            dbModel.Latitude = Latitude;
            dbModel.Longitude = Longitude;
            dbModel.Ct_Type_Id = CTTypeID;
            return dbModel;
        }
    }



    public class RKAddressFilterViewModel
    {
        // paging
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get { return (Page * PageSize); } }
        //sorting
        public string SortBy { get; set; }
        public string SortDir { get; set; }



        //filters
        public string Filter { get; set; }
        public int[] NavIds { get; set; }
    }
}

