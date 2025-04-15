using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionManagementApp.Events
{
    public class LogEventArgs : EventArgs
    {
        public string UserName { get; }
        public string Message { get; }
        public DateTime Timestamp { get; }

        public LogEventArgs(string username, string message)
        {
            UserName = username;
            Message = message;
            Timestamp = DateTime.Now;
        }
    }
}
