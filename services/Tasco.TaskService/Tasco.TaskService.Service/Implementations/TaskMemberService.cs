using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasco.TaskService.Repository.Entities;
using Tasco.TaskService.Repository.UnitOfWork;
using Tasco.TaskService.Service.Interfaces;

namespace Tasco.TaskService.Service.Implementations
{
    public class TaskMemberService : BaseService<TaskMemberService>, ITaskMemberService
    {
        public TaskMemberService(IUnitOfWork<TaskManagementDbContext> unitOfWork, ILogger<TaskMemberService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public Task AssignWorkTaskToUser(Guid workTaskId, Guid userId)
        {
            var taskMember = new TaskMember
            {
                WorkTaskId = workTaskId,
                UserId = userId,
                UserName = GetUserEmailFromJwt(),
                Role = "Assignee",
                AssignedByUserId = Guid.Parse(GetUserIdFromJwt()),
                AssignedDate = DateTime.UtcNow,
                IsActive = true
            };
            _unitOfWork.GetRepository<TaskMember>().InsertAsync(taskMember);
            return _unitOfWork.CommitAsync();
        }
    }
}
