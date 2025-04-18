using ConstructionManagementApp.Events;

namespace ConstructionManagementApp.App.Interfaces
{
    public interface ILogService
    {
        void OnActionOccurred(object sender, LogEventArgs e);
    }
}