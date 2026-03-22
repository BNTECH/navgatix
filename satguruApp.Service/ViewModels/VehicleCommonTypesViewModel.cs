using System.Collections.Generic;

namespace satguruApp.Service.ViewModels
{
    public class VehicleCommonTypesViewModel
    {
        public List<CommonTypeWithKeyViewModel> ProductTypes { get; set; } = new();
        public List<CommonTypeWithKeyViewModel> VehicleTypes { get; set; } = new();
        public List<CommonTypeWithKeyViewModel> BodyTypes { get; set; } = new();
        public List<CommonTypeWithKeyViewModel> TyreTypes { get; set; } = new();
    }
}
