using System;

namespace ConstructionManagementApp.App.Models
{
    internal class TeamMembers
    {
        private int _teamId;
        private Team _team;
        private int _userId;
        private User _user;

        public int TeamId
        {
            get => _teamId;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Id zespołu musi być większe od zera.");
                _teamId = value;
            }
        }

        public Team Team
        {
            get => _team;
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Team), "Zespół nie może być null.");
                _team = value;
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

        public TeamMembers(int teamId, int userId)
        {
            TeamId = teamId;
            UserId = userId;
        }

    }
}
