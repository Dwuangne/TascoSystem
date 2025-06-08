using System.ComponentModel.DataAnnotations;

namespace Tasco.TaskService.API.Payload.Request
{
    public class WorkTaskRequest
    {
        public Guid WorkAreaId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Urgent

        [StringLength(50)]
        public string Status { get; set; } = "Todo"; // Todo, InProgress, Review, Done

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? CompletedDate { get; set; }

        public int DisplayOrder { get; set; } = 0;

    }
}
