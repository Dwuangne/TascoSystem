using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasco.TaskService.Service.Interfaces
{
    public interface ITaskMemberService
    {
        Task AssignWorkTaskToUser(Guid workTaskId, Guid userId);
    }
}
