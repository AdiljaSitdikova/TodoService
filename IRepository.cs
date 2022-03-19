namespace TodoApi
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T> where T : IEntity
    {
        IEnumerable<T> Get();

        Task<T> Get(long id);

        Task Update(T dto);

        Task Update(long id, Func<T,T> updateFn);

        Task Create(T dto);

        Task Delete(long id);
    }
}