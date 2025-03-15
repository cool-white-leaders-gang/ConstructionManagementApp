using ConstructionManagementApp.App.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionManagementApp.App.Models
{
    internal class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskProgress Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public List<string> Comments { get; set; }

        public Task(int id, string name, string description, TaskPriority priority)
        {
            Id = id;
            Name = name;
            Description = description;

            //Ustawienie wartości domyślnych
            Status = TaskProgress.New;
            Priority = priority;
            CreatedAt = DateTime.Now;
            Comments = new List<string>();
        }

    }
}
