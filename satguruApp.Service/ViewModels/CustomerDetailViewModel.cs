using Microsoft.EntityFrameworkCore.Metadata.Internal;
using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class CustomerDetailViewModel
    {
        public static Expression<Func<CustomerDetail, CustomerDetailViewModel>> ModelMapFrom = (model) => new CustomerDetailViewModel
        {
            Id = model.Id,
            UserId=model.UserId,
            CompanyName=model.CompanyName,
            GSTNumber=model.GSTNumber,
            Address=model.Address,
            City=model.City,
            State=model.State,
            Pincode=model.Pincode,
            IsDeleted=false
            
        };

        public long Id { get; set; }

        public string UserId { get; set; }

        public string CompanyName { get; set; }

        public string GSTNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Pincode { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual UserInfoViewModel User { get; set; }
        public void ModelMapTo(CustomerDetail model) {
            model.Id = Id;
            model.UserId = UserId;
            model.CompanyName = CompanyName;
             model.GSTNumber=GSTNumber;
            model.Address = Address;
            model.City = City;
            model.State = State;
             model.Pincode=Pincode;
            model.IsDeleted = IsDeleted;
        }
    }
}
