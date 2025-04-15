using System;

public class Log
{
    public int Id { get; private set; } // Id logu
    private string _userName;
    private string _message;
    private DateTime _timestamp;

    public string Message
    {
        get => _message;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Wiadomość nie może być pusta");
            _message = value;
        }
    }

    public DateTime Timestamp
    {
        get => _timestamp;
        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Czas zdarzenia nie może być w przyszłości.");
            _timestamp = value;
        }
    }

    public string UserName
    {
        get => _userName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Nazwa użytkownika nie może być pusta");
            _userName = value;
        }
    }

    public Log(string message, DateTime timestamp, string userName)
    {
        Message = message;
        Timestamp = timestamp;
        UserName = userName;
    }
}