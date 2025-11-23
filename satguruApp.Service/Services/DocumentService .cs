using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;

namespace satguruApp.Service.Services
{
    public class DocumentService : Repository<Document>,  IDocumentService
    {
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ISystemConfigurationService _SystemConfigurationBiz;



        public DocumentService(SatguruDBContext context, IConfiguration configuration, ISystemConfigurationService SystemConfigurationBiz, IHostEnvironment environment) : base(context)
        {
            _configuration = configuration;
            _environment = environment;
            _SystemConfigurationBiz = SystemConfigurationBiz;
        }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "UploadedDocuments");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/UploadedDocuments/" + uniqueFileName;
        }

        public async Task<DocumentViewModel> GetByIdAsync(int id)
        {
            var doc = await _db.Documents
                //.Include(d => d.Vehicle)
                .FirstOrDefaultAsync(d => d.Id == id && (d.IsDeleted == false || d.IsDeleted == null));

            if (doc == null) return null;

            return DocumentViewModel.ModelMapFrom.Compile().Invoke(doc);
        }

        public async Task<List<DocumentViewModel>> GetAllAsync()
        {
            return await _db.Documents
                //.Include(d => d.Vehicle)
                .Where(d => d.IsDeleted == false || d.IsDeleted == null)
                .Select(DocumentViewModel.ModelMapFrom)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(DocumentViewModel model)
        {
            var document = new Document();
            model.ModelMapTo(document);
            document.IsDeleted = false;

            if (model.UploadFile != null)
            {
                document.DocumentUrl = await UploadFileAsync(model.UploadFile);
            }

            _db.Documents.Add(document);
            var saved = await _db.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateAsync(DocumentViewModel model)
        {
            var document = await _db.Documents.FirstOrDefaultAsync(d => d.Id == model.Id);
            if (document == null || document.IsDeleted == true) return false;

            model.ModelMapTo(document);

            if (model.UploadFile != null)
            {
                document.DocumentUrl = await UploadFileAsync(model.UploadFile);
            }

            _db.Documents.Update(document);
            var saved = await _db.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var document = await _db.Documents.FirstOrDefaultAsync(d => d.Id == id);
            if (document == null || document.IsDeleted == true) return false;

            // Soft delete
            document.IsDeleted = true;
            _db.Documents.Update(document);
            var saved = await _db.SaveChangesAsync();
            return saved > 0;
        }

        

        public async Task<DocumentViewModel> SaveUpdateDocument(DocumentViewModel model)

        {

            if (model.Id != null && model.Id >0)
            {
                var _doc =  await _db.Documents.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

                _doc.TableRowId = model.TableRowId;

                _doc.DocumentName = model.DocumentName;

                _doc.DocumentPathKey = model.DocumentPathKey;

                _doc.DocumentExt = model.DocumentExt;

                _doc.DocumentPath = model.DocumentPath;

                _doc.ExpiryDate = model.ExpiryDate;

                _doc.EffectiveDate = model.EffectiveDate;

                _doc.CategoryType = model.CategoryType;


                _doc.UpdateDatetime = DateTime.Now;

              //  _doc.UpdatedBy = AppUserId;


                _doc.IsDeleted = false;

                

              //  model.DocKey = _doc.CreatedDatetime.Ticks.ToString();

            }

            else

            {

                model.CTTableId = _db.CommonTypes.Where(x => x.Code == model.CttableType && x.IsDeleted == false).Select(x => x.Id).FirstOrDefault();

                Document _doc = new Document();

                model.ModelMapTo(_doc);

                _doc.IsDeleted = false;

                _doc.CreatedDatetime = DateTime.Now;

               // _doc.CreatedBy = AppUserId;

                _db.Documents.Add(_doc);

                _db.SaveChanges();

               // model.DocKey = _doc.CreatedDateTime.Ticks.ToString();

                model.Id = _doc.Id;

            }
         await   _db.SaveChangesAsync();

            return model;

        }

        public bool DeleteDocumentList(int documentId, bool isDeleted)

        {

            var record = (from doc in _db.Documents

                          where doc.Id == documentId

                          select doc).FirstOrDefault();

            if (record != null)

            {

                record.IsDeleted = isDeleted;

                //record.UpdatedBy = AppUserId;

                record.UpdateDatetime = DateTime.Now;

                _db.SaveChanges();

            }

            List<string> extensions = new List<string>() { "jpg", "png", "jpeg", "gif", "tiff", "raw", "ai" };

            DocumentViewModel model = new DocumentViewModel();

            if (record != null && record.VehicleId != null)

                model.VehicleId = record.VehicleId;

            if (record != null && record.TableRowId != null)

                model.TableRowId = record.TableRowId.GetValueOrDefault();

            model.CttableType = "ATL";

            var docs = _db.Documents.Where(x => !x.IsDeleted.GetValueOrDefault() && (extensions.Contains(x.DocumentExt))).OrderByDescending(x => x.IsDefault).FirstOrDefault();

            model.Id = docs.Id;

            SetDefaultDocument(model);

            return isDeleted;

        }

        public List<DocumentViewModel> GetDocumentList(DocumentViewModel model)

        {

            int ctTableID = _db.CommonTypes.Where(x => x.Code.ToLower() == model.CttableType.ToLower()).Select(x => x.Id).FirstOrDefault();

            var result = (from document in _db.Documents

                          where document.IsDeleted == model.IsDeleted && document.CTTableId == ctTableID
      && ((model.TableRowId != 0 && document.TableRowId == model.TableRowId && model.TableRowId != null)

                               || (model.VehicleId != null && document.VehicleId == model.VehicleId))

                          orderby document.IsDefault descending

                          select new DocumentViewModel

                          {

                              Id = document.Id,

                              DocumentName = document.DocumentName,

                              //DocKey = document.CreatedDatetime.Ticks.ToString(),

                              DocumentPath = document.DocumentPath,

                              ExpiryDate = document.ExpiryDate,

                              EffectiveDate = document.EffectiveDate,

                              CategoryType = document.CategoryType,

                              DocumentExt = document.DocumentExt,

                              IsDefault = (bool)document.IsDefault,

                             // WfLogID = document.WfLogID,

                              IsDeleted = document.IsDeleted,

                          }).ToList();

            return result;

        }

        public DocumentViewModel GetDocument(int id, string fileName, string docKey="", bool isDeleted = false)

        {

            var ticks = docKey != "" ? Convert.ToInt64(docKey) : 0;

            var dt = ticks != 0 ? new DateTime(ticks) : (DateTime?)null;

            var result = (from document in _db.Documents

                          where document.IsDeleted == isDeleted && document.Id == id && document.DocumentName == fileName

                          select new DocumentViewModel

                          {

                              Id = document.Id,

                              DocumentName = document.DocumentName,

                              DocumentPath = document.DocumentPath,

                              ExpiryDate = document.ExpiryDate,

                              EffectiveDate = document.EffectiveDate,

                              CategoryType = document.CategoryType,

                              DocumentExt = document.DocumentExt,

                              IsDeleted = document.IsDeleted,

                              CreatedDatetime = document.CreatedDatetime

                          }).FirstOrDefault();

            if (result != null)

            {

              //  result.DocKey = result.CreatedDateTime.Ticks.ToString();

            }

            return result;

        }

        public DocumentViewModel SetDefaultDocument(DocumentViewModel model)
        {
            try
            {

                model.CTTableId = _db.CommonTypes.Where(x => x.Code == model.CttableType && x.IsDeleted== false).Select(x => x.Id).FirstOrDefault();

                var documentList = _db.Documents.Where(x => x.CTTableId == model.CTTableId && x.VehicleId == model.VehicleId && x.IsDeleted==false).ToList();

                if (documentList != null)

                {

                    foreach (var item in documentList)

                    {

                        if (item.Id == model.Id)

                        {

                            item.IsDefault = true;

                        }

                        else

                        {

                            item.IsDefault = false;

                        }

                      //  item.UpdatedBy = AppUserId;

                        item.UpdateDatetime = DateTime.UtcNow;

                        _db.SaveChanges();

                    }

                }

            }

            catch (Exception)

            {

            }

            return model;

        }

        public DocumentViewModel GetLogo(int id, string type)

        {

            var rec = (from doc in _db.Documents

                       join comType in _db.CommonTypes on doc.CategoryType equals comType.Id

                       where doc.Id == id && comType.Name == type

                       select new DocumentViewModel { DocumentPath = doc.DocumentPath, DocumentName = doc.DocumentName, DocumentExt = doc.DocumentExt }).FirstOrDefault();

            if (rec == null && type == "logodark")

            {

                rec = (from doc in _db.Documents

                       join comType in _db.CommonTypes on doc.CategoryType equals comType.Id

                       where doc.Id == id && comType.Name == "logo"

                       select new DocumentViewModel { DocumentPath = doc.DocumentPath, DocumentName = doc.DocumentName, DocumentExt = doc.DocumentExt }).FirstOrDefault();

            }

            return rec;

        }

        public DocumentViewModel UpdateDocsItem(DocumentViewModel VMModel)

        {

            var record = _db.Documents.Where(x => x.Id == VMModel.Id && x.IsDeleted==false).FirstOrDefault();

            if (record != null)

            {

                record.ExpiryDate = VMModel.ExpiryDate != null ? Convert.ToDateTime(VMModel.ExpiryDate).ToUniversalTime() : (DateTime?)null;

                record.EffectiveDate = VMModel.EffectiveDate != null ? Convert.ToDateTime(VMModel.EffectiveDate).ToUniversalTime() : (DateTime?)null;

                record.CategoryType = VMModel.CategoryType;

                //record.TrackUser(AppUserId);

                _db.SaveChanges();

            }

            VMModel.ExpiryDate = VMModel.ExpiryDate != null ? Convert.ToDateTime(VMModel.ExpiryDate).ToLocalTime() : (DateTime?)null;

            VMModel.EffectiveDate = VMModel.EffectiveDate != null ? Convert.ToDateTime(VMModel.EffectiveDate).ToLocalTime() : (DateTime?)null;

            return VMModel;

        }

    }
}
