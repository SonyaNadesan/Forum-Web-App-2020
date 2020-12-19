namespace Application.Domain
{
    public class Enums
    {
        public enum LoginStatus
        {
            Success,
            ConfirmedButNeedsPasswordChange,
            LockedOut,
            Failed
        };
    }
}
