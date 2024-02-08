using Microsoft.AspNetCore.Mvc;
using TodoApp.Services;
using TodoDB.Todos;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly TodosService _todosService;

        public TodosController(TodosService TodosService) =>
            _todosService = TodosService;

        [HttpGet]
        public async Task<List<Todos>> Get() =>
            await _todosService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Todos>> Get(string id)
        {
            var book = await _todosService.GetAsync(id);

        	if (book is null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Todos>> Post([FromBody] string task)
        {
            Todos newTodo = new Todos(task);

            await _todosService.CreateAsync(newTodo);

            return CreatedAtAction(nameof(Get), new { id = newTodo.Id }, newTodo);
        }

        //[HttpPut("{id:length(24)}, {task:length(24)}, {isdone:bool}")]
        [HttpPut]
        public async Task<ActionResult<Todos>> Update([FromBody] Todos updatedTodo)
        {
            if (updatedTodo == null || updatedTodo.Id == null)
            {
                return BadRequest("Invalid request. Todo data is missing.");
            }
            var todo = await _todosService.GetAsync(updatedTodo.Id);

            if (todo is null)
            {
                return NotFound();
            }

            if(updatedTodo.Task != null)
                todo.Task = updatedTodo.Task;

            todo.IsDone = updatedTodo.IsDone;


            await _todosService.UpdateAsync(updatedTodo.Id, todo);

            return todo;
        }

        [HttpDelete]
        public async Task<ActionResult<Todos>> Delete([FromBody]string id)
        {
            if(id == null)
            {
                return BadRequest("Invalid request. Todo data is missing.");
            }

            var todo = await _todosService.GetAsync(id);

            if (todo is null)
            {
                return NotFound("ID passed is not found.");
            }

            await _todosService.RemoveAsync(id);

            return todo;
        }
    }
}
