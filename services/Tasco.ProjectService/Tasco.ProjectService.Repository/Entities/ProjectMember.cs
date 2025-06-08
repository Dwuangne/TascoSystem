using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasco.ProjectService.Repository.Entities
{
    public class ProjectMember
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; } = "member";

        public Project Project { get; set; } = null!;
    }
}
