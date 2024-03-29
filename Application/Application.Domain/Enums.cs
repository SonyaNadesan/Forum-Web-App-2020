﻿namespace Application.Domain
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
        };

        public enum ReactionTypes
        {
            LIKE,
            LOVE,
            WOW,
            SAD,
            ANGRY,
            NONE
        };

        public enum MatchConditions
        {
            MatchAll,
            MatchAny
        }
    }
}
