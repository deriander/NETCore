using NETCore.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Repository.Interface
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> Get();
        Task<T> Get(int Id);
        Task<T> Post(T Entity);
        Task<T> Put(T Entity);
        Task<T> Delete(int Id);
    }
}
