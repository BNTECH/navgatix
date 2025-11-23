using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected SatguruDBContext _context;
        public DbSet<T> dbSet;
        //protected readonly IExceptionService _exceptionService;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public Func<int> GetAppUserId { get; set; }
        public Func<int> GetDefaultDivisionID { get; set; }
        private int _appUserId;
        private int _defaultDivisionID;
        public int AppUserId
        {
            get { return GetAppUserId == null ? _appUserId : GetAppUserId(); }
            set { var _appUserId = value; }
        }
        public int DefaultDivisionID
        {
            get { return GetDefaultDivisionID == null ? _defaultDivisionID : GetDefaultDivisionID(); }
            set { var _defaultDivisionID = value; }
        }
        public Repository(SatguruDBContext context)
        {
            this._context = context;
            this.dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> All()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public virtual Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        Task IRepository<T>.Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
