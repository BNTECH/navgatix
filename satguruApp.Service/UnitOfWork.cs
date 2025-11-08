using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service
{
    public class UnitOfWork :IUnitOfWork
    {
        readonly SatguruDBContext _context;
        public UnitOfWork(SatguruDBContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
