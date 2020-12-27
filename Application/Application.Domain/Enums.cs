namespace Application.Domain
{
    public class Enums
    {
        public enum LoginStatus
        {
            Success,
            ConfirmedButNeedsPasswordChange,
            LockedOut,
            Failed,
            UserNotFound
        };

        public enum EmailBodyType
        {
            RegularString,
            HtmlString,
            HtmlFile
        }
    }
}
