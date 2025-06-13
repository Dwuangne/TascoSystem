using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tasco.TaskService.API.Payload.Request;
using Tasco.TaskService.Service.BusinessModels;
using Tasco.TaskService.Service.Interfaces;

namespace Tasco.TaskService.API.Controllers
{
    [Route("api/v1/worktasks")]
    [ApiController]
    public class WorkTaskController : BaseController<WorkTaskController>
    {
        private readonly IWorkTaskService _workTaskService;
        public WorkTaskController(ILogger<WorkTaskController> logger, IMapper mapper, IWorkTaskService workTaskService) : base(logger, mapper)
        {
            _workTaskService = workTaskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkTasks(int pageSize = 10, int pageIndex = 1, string search = null)
        {
            var worktasks = await _workTaskService.GetAllWorkTasks(pageSize, pageIndex, search);
            return Ok(worktasks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkTaskById(Guid id)
        {
            var worktask = await _workTaskService.GetWorkTaskById(id);
            if (worktask == null)
            {
                return NotFound($"Work task with ID {id} not found.");
            }
            return Ok(worktask);

        }
        [HttpGet("me")]
        public async Task<IActionResult> GetMyWorkTasks(int pageSize = 10, int pageIndex = 1, string search = null)
        {
            var worktasks = await _workTaskService.GetMyWorkTasks(pageSize, pageIndex, search);
            return Ok(worktasks);
        }
        [HttpPost]
        public async Task<IActionResult> CreateWorkTask([FromBody] WorkTaskRequest workTask)
        {
            if (workTask == null)
            {
                return BadRequest("Work task data is required.");
            }
            var mappedWorkTask = _mapper.Map<WorkTaskBusinessModel>(workTask);
            var createdWorkTask = await _workTaskService.CreateWorkTask(mappedWorkTask);
            return CreatedAtAction(nameof(GetWorkTaskById), new { id = Guid.NewGuid() }, workTask);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkTask(Guid id, [FromBody] WorkTaskRequest workTask)
        {
            if (workTask == null)
            {
                return BadRequest("Work task data is required.");
            }
            var mappedWorkTask = _mapper.Map<WorkTaskBusinessModel>(workTask);
            await _workTaskService.UpdateWorkTask(id, mappedWorkTask);
            return Ok($"Updated work task with ID: {id}");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkTask(Guid id)
        {
            await _workTaskService.DeleteWorkTask(id);
            return Ok($"Deleted work task with ID: {id}.");
        }
    }
}
