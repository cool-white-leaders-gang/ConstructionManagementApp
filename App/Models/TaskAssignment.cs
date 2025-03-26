using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionManagementApp.App.Models
{
    internal class TaskAssignment
    {
        public int UserId { get; set; }
        public User user { get; set; }

        public int TaskId { get; set; }
        public Task task { get; set; }

        public TaskAssignment(int taskId, int userId)
        {
            TaskId = taskId;
            UserId = userId;
        }

    }

}
