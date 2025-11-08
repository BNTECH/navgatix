using Microsoft.AspNetCore.Http;
using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IDocumentService: IRepository<Document>
    {
        Task<DocumentViewModel> GetByIdAsync(int id);
        Task<List<DocumentViewModel>> GetAllAsync();
        Task<bool> AddAsync(DocumentViewModel model);
        Task<bool> UpdateAsync(DocumentViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<string> UploadFileAsync(IFormFile file);
        public  Task<DocumentViewModel> SaveUpdateDocument(DocumentViewModel model);
        public bool DeleteDocumentList(int documentId, bool isDeleted);
        public List<DocumentViewModel> GetDocumentList(DocumentViewModel model);
        public DocumentViewModel GetDocument(int id, string fileName, string docKey="", bool isDeleted = false);
        public DocumentViewModel SetDefaultDocument(DocumentViewModel model);
        public DocumentViewModel GetLogo(int id, string type);
        public DocumentViewModel UpdateDocsItem(DocumentViewModel VMModel);
    }
}
