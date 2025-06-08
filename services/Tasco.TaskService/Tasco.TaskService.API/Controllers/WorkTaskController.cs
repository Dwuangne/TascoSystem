using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tasco.TaskService.Service.Interfaces;
using Tasco.TaskService.API.Payload.Request;
using Tasco.TaskService.Service.BusinessModels;

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
            var result = await _workTaskService.GetAllWorkTasks(pageSize, pageIndex, search);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkTaskById(Guid id)
        {
            var result = await _workTaskService.GetWorkTaskById(id);
            if (result == null)
            {
                return NotFound($"Work task with ID {id} not found.");
            }
            return Ok(result);
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetMyWorkTasks(int pageSize = 10, int pageIndex = 1, string search = null)
        {
            var result = await _workTaskService.GetMyWorkTasks(pageSize, pageIndex, search);
            return Ok(result);
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
            return CreatedAtAction(nameof(GetWorkTaskById), new { id = createdWorkTask.Id }, createdWorkTask);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkTask(Guid id, [FromBody] WorkTaskRequest workTask)
        {
            if (workTask == null)
            {
                return BadRequest("Work task data is required.");
            }
            var existingWorkTask = await _workTaskService.GetWorkTaskById(id);
            if (existingWorkTask == null)
            {
                return NotFound($"Work task with ID {id} not found.");
            }
            var mappedWorkTask = _mapper.Map<WorkTaskBusinessModel>(workTask);
            await _workTaskService.UpdateWorkTask(id, mappedWorkTask);
            return NoContent();
        }

    }
}
