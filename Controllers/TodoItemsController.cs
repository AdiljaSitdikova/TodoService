namespace TodoApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TodoApi.Models;
    using TodoApiDTO.BL;

    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IRepository<TodoItemDTO> repository;
        private readonly TodoService todoService;
        private readonly ILogger<IEntity> logger;

        public TodoItemsController(IRepository<TodoItemDTO> repository, TodoService todoService, ILogger<IEntity> logger)
        {
            this.repository = repository;
            this.todoService = todoService;
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<TodoItemDTO> GetTodoItems() => this.repository.Get().ToList();

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var res = await this.repository.Get(id);

            if (res == null)
            {
                return NotFound();
            }

            return res;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            try
            {
                await this.repository.Update(todoItemDTO);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            await this.repository.Create(todoItemDTO);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItemDTO.Id }, todoItemDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            try
            {
                await this.repository.Delete(id);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SetIsComplete(long id, [FromBody]bool isComplete)
        {
            try
            {
                await this.todoService.SetIsComplete(id, isComplete);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }

            return NoContent();
        }
    }
}