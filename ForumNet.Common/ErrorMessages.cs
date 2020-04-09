namespace ForumNet.Common
{
    public class ErrorMessages
    {
        public const string CategoryExistingNameErrorMessage = "This category name is already used";
        public const string CategoryNonExistingIdErrorMessage = "Invalid category";

        public const string CategoryNameLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";

        public const string TagNameLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
        public const string TagIsRequiredErrorMessage = "At least one tag needs to be selected";
        public const string TagExistingNameErrorMessage = "This tag name is already used";
        public const string TagNonExistingIdErrorMessage = "Invalid tag";

        public const string PostTitleLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
        public const string PostReportDescriptionLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";

        public const string ReplyReportDescriptionLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";

        public const string UserUsernameLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
        public const string UserPasswordLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
        public const string UserPasswordsDoNotMatchErrorMessage = "The password and confirmation password do not match.";
        public const string UserChangePasswordDoNotMatchErrorMessage = "The new password and confirmation password do not match.";
        public const string UserInvalidGenderType = "Not valid gender.";
    }
}
