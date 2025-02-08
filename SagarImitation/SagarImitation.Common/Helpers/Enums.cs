namespace SagarImitation.Common.Helpers
{
    public static class Status
    {
        public const int Success = 0;
        public const int Failed = 1;
        public const int Insert = 2;
        public const int Updated = 3;
        public const int AlredyExists = 4;
        public const int InvalidToken = 5;
    }

    public static class ActiveStatus
    {
        public const int Active = 1;
        public const int Inactive = 0;
    }

    public static class LoginFrom
    {
        public const int AdminPortal = 1;
        public const int CustomerPortal = 2;
    }
}