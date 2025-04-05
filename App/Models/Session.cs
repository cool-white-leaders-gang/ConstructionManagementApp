using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Session
    {
        public User User { get; private set; }
        public DateTime LoginTime { get; private set; }

        public Session(User user)
        {
            User = user;
            LoginTime = DateTime.Now;
        }
    }
}