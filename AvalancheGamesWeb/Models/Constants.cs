using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AvalancheGamesWeb
{
    public class Constants
    {
        public const int AdminRole = 1;
        public const string AdminRoleName = "Administrator";

        public const int PowerRole = 2;
        public const string PowerRoleName = "PowerUser";

        public const int NormalRole = 3;
        public const string NormalRoleName = "NormalUser";
        //might have to delete these
        public const int SnakeGame = 1;
        public const string SnakeGameName = "Snake";
        public const int PongBallGame = 2;
        public const string PongBallGameName = "PongBall";
        public const int FloatyGame = 3;
        public const string FloatyGameName = "Floaty";
        public const int DefaultDefaultPageSize = 10;
        public const int DefaultPageNumber = 0;
        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 20;
        public const int SaltSize = 20;
        public const string PasswordRequirementsMessage = "The Password must contain at Least One Capital letter, One Lowercase letter and One Number";
        public const string PasswordRequirements = @"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$";
    }
}