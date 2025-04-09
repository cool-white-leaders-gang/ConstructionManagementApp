using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Message
    {
        public int Id { get; private set; } // Id wiadomości
        private string _content;
        private int _senderId;
        private int _receiverId;
        private DateTime _sentAt;

        public int SenderId
        {
            get => _senderId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id nadawcy musi być większe od zera");
                _senderId = value;
            }
        }

        public int ReceiverId
        {
            get => _receiverId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id odbiorcy musi być większe od zera.");
                if (value == SenderId)
                    throw new ArgumentException("Nadawca i odbiorca nie mogą być tą samą osobą.");
                _receiverId = value;
            }
        }

        public string Content
        {
            get => _content;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Treść wiadomości nie może być pusta.");
                if (value.Length > 500)
                    throw new ArgumentException("Treść wiadomości nie może przekraczać 500 znaków.");
                _content = value;
            }
        }

        public DateTime SentAt { get; set; } // Data wysłania wiadomości

        public Message(int senderId, int receiverId, string content)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            SentAt = DateTime.Now;
        }
    }
}