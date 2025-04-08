using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Session
    {
        public User User { get; set; }
        public DateTime LoginTime { get; private set; }

        public Session(User user)
        {
            this.User = user;
            LoginTime = DateTime.Now;
        }

    }
}