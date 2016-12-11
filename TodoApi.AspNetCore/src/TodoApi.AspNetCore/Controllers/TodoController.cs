using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.AspNetCore.Models;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        public TodoController(ITodoRepository repository, ILogger<TodoController> logger)
        {
            _repository = repository;
            _logger = logger;

        }

        public ITodoRepository _repository { get; set; }
        public ILogger<TodoController> _logger { get; set; }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            _logger.LogInformation("Getting all items count: {0}", _repository.GetAll().Count());
            
            return _repository.GetAll();
        }


        [HttpGet("id", Name = "GetTodo")]
        public IActionResult GetById(string id)
        {
            var item = _repository.Find(id);

            if (item == null)
                return NotFound();

            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
                return BadRequest();

            _repository.Add(item);
            
            return CreatedAtAction("GetById", new { id = item.Key }, item);
        }


        [HttpPut]
        public IActionResult Update(string id, [FromBody] TodoItem item)
        {
            if (item == null || item.Key != id)
                return BadRequest();

            var todo = _repository.Find(id);

            if (todo == null)
                return NotFound();

            _repository.Update(item);

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var item = _repository.Find(id);

            if (item == null)
                return NotFound();

            _repository.Remove(id);

            return new NoContentResult();
        }
    }
}
