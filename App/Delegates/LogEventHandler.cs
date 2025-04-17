using System;
using ConstructionManagementApp.Events;

namespace ConstructionManagementApp.App.Delegates
{
    public delegate void LogEventHandler(object sender, LogEventArgs e);
}