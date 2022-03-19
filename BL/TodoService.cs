namespace TodoApiDTO.BL
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TodoApi;
    using TodoApi.DAL;
    using TodoApi.Models;

    public class TodoService 
    {
        private readonly IRepository<TodoItemDTO> repository;

        public TodoService(IRepository<TodoItemDTO> repository)
        {
            this.repository = repository;
        }

        public async Task SetIsComplete(long id, bool isComplete)
        {
            await repository.Update(id, x =>
            {
                x.IsComplete = isComplete;
                return x;
            });
        }
    }
}
