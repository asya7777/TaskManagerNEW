using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.Entities
{
    public class Task
    {
        public int taskId { get; set; }
        public string taskName { get; set; }
        public DateTime taskDeadline { get; set; }
        public string? taskDescription { get; set; }
        public int usrId { get; set; }
        public User User { get; set; }

        public bool isFinished { get; set; }

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    }
}
