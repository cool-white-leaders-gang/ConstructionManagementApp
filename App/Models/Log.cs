using System;

public class Log
{
    public int Id { get; private set; } // Id logu
    private string _userEmail;
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

    public string UserEmail
    {
        get => _userEmail;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email użytkownika nie może być pusty");
            _userEmail = value;
        }
    }

    public Log(string message, DateTime timestamp, string userEmail)
    {
        Message = message;
        Timestamp = timestamp;
        UserEmail = userEmail;
    }
}