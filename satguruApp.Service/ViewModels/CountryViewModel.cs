using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public partial class CountryViewModel
    {
        public static Expression<Func<Country, CountryViewModel>> ModelMapFrom = (model) => new CountryViewModel
        {
            Id = model.Id,
            Name = model.Name,
            CurrencySymbols = model.CurrencySymbols,
            CountryCodeThree = model.CountryCodeThree,
            CountryCodeTwo = model.CountryCodeTwo,
            RegionName = model.RegionName,
        };
        public void ModelMapTo(Country model)
        {
            model.Id = Id;
            model.Name = Name;
            model.CurrencySymbols = CurrencySymbols;
            model.CountryCodeThree = CountryCodeThree;
            model.CountryCodeTwo = CountryCodeTwo;
            model.RegionName = RegionName;
        }
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? CountryCodeThree { get; set; }

        public string? CountryCodeTwo { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDatetime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDatetime { get; set; }

        public string? RegionName { get; set; }

        public string? CurrencySymbols { get; set; }

        public bool? IsDeleted { get; set; }

        public string? Message { get; set; }
    }
}
