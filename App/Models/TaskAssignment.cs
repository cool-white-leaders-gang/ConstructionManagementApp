using System;

namespace ConstructionManagementApp.App.Models
{
    internal class TaskAssignment
    {
        private int _taskId;
        private Task _task;
        private int _userId;
        private User _user;

        public int TaskId
        {
            get => _taskId;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Id zadania musi być większe od zera.");
                _taskId = value;
            }
        }

        public Task Task
        {
            get => _task;
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Task), "Zadanie nie może być null.");
                _task = value;
            }
        }

        public int UserId
        {
            get => _userId;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Id użytkownika musi być większe od zera.");
                _userId = value;
            }
        }

        public User User
        {
            get => _user;
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(User), "Użytkownik nie może być null.");
                _user = value;
            }
        }

        public TaskAssignment(int taskId, Task task, int userId, User user)
        {
            TaskId = taskId;
            Task = task;
            UserId = userId;
            User = user;
        }

    }
}
