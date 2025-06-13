using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tasco.TaskService.Service.Interfaces;

namespace Tasco.TaskService.API.Controllers
{
    [Route("api/v1/taskmembers")]
    [ApiController]
    public class TaskMemberController : BaseController<TaskMemberController>
    {
        private readonly ITaskMemberService _taskMemberService;
        public TaskMemberController(ILogger<TaskMemberController> logger, IMapper mapper, ITaskMemberService taskMemberService) : base(logger, mapper)
        {
            _taskMemberService = taskMemberService;
        }
        [HttpPost("{workTaskId}/assign/{userId}")]
        public async Task<IActionResult> AssignWorkTaskToUser(Guid workTaskId, Guid userId)
        {
            await _taskMemberService.AssignWorkTaskToUser(workTaskId, userId);
            return Ok($"Assigned work task with ID: {workTaskId} to user with ID: {userId}.");
        }
    }
}
