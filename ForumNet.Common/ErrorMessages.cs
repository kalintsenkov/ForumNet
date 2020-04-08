namespace ForumNet.Common
{
    public class ErrorMessages
    {
        public const string ExistingCategoryNameErrorMessage = "This category name is already used";

        public const string NonExistingCategoryIdErrorMessage = "Invalid category";

        public const string ExistingTagNameErrorMessage = "This tag name is already used";

        public const string NonExistingTagIdErrorMessage = "Invalid tag";

        public const string RequiredTagErrorMessage = "At least one tag needs to be selected";

        public const string UsernameLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";

        public const string PasswordLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";

        public const string PasswordsDoNotMatchErrorMessage = "The password and confirmation password do not match.";

        public const string ChangePasswordPasswordsDoNotMatchErrorMessage = "The new password and confirmation password do not match.";

        public const string InvalidGenderType = "Not valid gender.";
    }
}
