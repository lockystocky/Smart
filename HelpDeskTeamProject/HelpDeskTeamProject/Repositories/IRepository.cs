using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskTeamProject.Repositories
{
    interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetList();
        T GetObject(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}