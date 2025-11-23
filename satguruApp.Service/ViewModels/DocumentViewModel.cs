using Microsoft.AspNetCore.Http;
using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public partial class DocumentViewModel
    {
        public static Expression<Func<Document, DocumentViewModel>> ModelMapFrom = (model) => new DocumentViewModel
        {
            Id = model.Id,
            VehicleId = model.VehicleId,
            VehicleNumber = model.Vehicle.VehicleNumber,
            DocType = model.DocType,
            DocumentUrl = model.DocumentUrl,
            ExpiryDate = model.ExpiryDate,
            CTTableId = model.CTTableId
        };
        public void ModelMapTo(Document model)
        {
            model.Id = Id;
            model.VehicleId = VehicleId;
            model.DocumentUrl = DocumentUrl;
            model.DocType = DocType;
            model.ExpiryDate = ExpiryDate;
            model.CTTableId = CTTableId;
        }
        public int Id { get; set; }
        public bool? IsDefault { get; set; }

        public Guid? VehicleId { get; set; }
        public int? TableRowId { get; set; }
        public string VehicleNumber { get; set; }
        public string CttableType { get; set; }


        public string DocType { get; set; }

        public string DocumentUrl { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool? IsDeleted { get; set; }

        public int? CTTableId { get; set; }

        public int? CreatedBy { get; set; }
        public IFormFile File { get; set; }

        public DateTime? CreatedDatetime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdateDatetime { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string DocumentName { get; set; }

        public string DocumentExt { get; set; }

        public string DocumentPathKey { get; set; }

        public string DocumentPath { get; set; }

        public byte[] DocStream { get; set; }

        public int? CategoryType { get; set; }

        public int? CTDocumentStatusId { get; set; }

        public DateTime? DocumentIssueDate { get; set; }

        public bool? IsJpass { get; set; }

        public bool? Form312 { get; set; }

        public string Comment { get; set; }

        public IFormFile UploadFile
        {
            get; set;
        }

    }
}
