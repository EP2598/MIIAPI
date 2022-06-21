using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Client.Repositories.Interface
{
    public interface IRepository<T, X> where T : class
    {
        Task<List<T>> Get();
        Task<T> Get(X id);
        HttpStatusCode Post(T ent);
        HttpStatusCode Put(X id, T ent);
        HttpStatusCode Delete(X id);
    }
}
