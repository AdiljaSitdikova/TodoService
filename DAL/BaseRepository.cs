namespace TodoApi.DAL
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TodoApi.Models;

    public abstract class BaseRepository<T, TDTO> : IRepository<TDTO> 
        where T : class, IEntity 
        where TDTO : class, IEntity
    {
        private readonly TodoContext context;

        public BaseRepository(TodoContext context)
        {
            this.context = context;
        }

        public IEnumerable<TDTO> Get()
        {
            return this.context.Set<T>().Select(ItemToDTO);
        }

        public async Task<TDTO> Get(long id)
        {
            var entity = await this.context.Set<T>().FindAsync(id);

            if (entity == null)
            {
                return null;
            }

            return ItemToDTO(entity);
        }

        public async Task Update(TDTO dto)
        {
            await this.Update(dto.Id, x => dto);
        }

        public async Task Update(long id, Func<TDTO, TDTO> updateFn)
        {
            var entity = await this.context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                this.NotFound(id);
            }

            var dto = updateFn(this.ItemToDTO(entity));

            this.DtoToEntity(dto, entity);

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!Exists(dto.Id))
            {
                this.NotFound(dto.Id);
            }
        }

        public async Task Create(TDTO dto)
        {
            var entity = this.DtoToEntity(dto);

            this.context.Set<T>().Add(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var entity = await this.context.Set<T>().FindAsync(id);

            if (entity == null)
            {
                this.NotFound(id);
            }

            this.context.Set<T>().Remove(entity);
            await this.context.SaveChangesAsync();
        }

        protected virtual bool Exists(long id) => this.context.Set<T>().Any(e => e.Id == id);

        protected virtual void NotFound(long id)
        {
            var m = $"Запись с id={id} не найдена";
            throw new KeyNotFoundException(m);
        }

        protected abstract TDTO ItemToDTO(T entity);

        protected abstract T DtoToEntity(TDTO dto, T entity = null);
    }
}