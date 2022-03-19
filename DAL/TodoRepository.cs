namespace TodoApiDTO.DAL
{
    using Microsoft.Extensions.Logging;
    using TodoApi;
    using TodoApi.DAL;
    using TodoApi.Models;

    internal class TodoRepository : BaseRepository<TodoItem, TodoItemDTO>
    {
        public TodoRepository(TodoContext context) : base(context)
        {
        }

        protected override TodoItem DtoToEntity(TodoItemDTO dto, TodoItem entity = null)
        {
            if (entity == null)
            {
                entity = new TodoItem();
            }

            entity.Id = dto.Id;
            entity.Name = dto.Name;
            entity.IsComplete = dto.IsComplete;

            return entity;
        }

        protected override TodoItemDTO ItemToDTO(TodoItem entity) => ItemToDTOFn(entity);

        private static TodoItemDTO ItemToDTOFn(TodoItem entity) =>
            new TodoItemDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                IsComplete = entity.IsComplete
            };
    }
}
