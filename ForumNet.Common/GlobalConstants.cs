namespace ForumNet.Common
{
    public class GlobalConstants
    {
        public const string SystemName = "Forum.NET";
        public const string SystemEmail = "no-reply@forum.net";

        public const string AdministratorAreaName = "Administration";
        public const string AdministratorRoleName = "Admin";
        public const string AdministratorUserName = "Admin";
        public const string AdministratorEmail = "admin@forum.net";
        public const string AdministratorPassword = "admin123456";
        public const string AdministratorProfilePicture = "#icon-ava-a";

        public const string TestUserUserName = "testuser";
        public const string TestUserEmail = "testuser@forum.net";
        public const string TestUserPassword = "user123456";
        public const string TestUserProfilePicture = "#icon-ava-t";

        public const int UserUsernameMaxLength = 25;
        public const int UserUsernameMinLength = 4;
        public const int UserPasswordMaxLength = 100;
        public const int UserPasswordMinLength = 6;
        public const int UserBiographyMaxLength = 250;

        public const int TagNameMaxLength = 20;
        public const int TagNameMinLength = 3;

        public const int CategoryNameMaxLength = 50;
        public const int CategoryNameMinLength = 3;

        public const int MessageContentMaxLength = 300;

        public const int PostTitleMaxLength = 100;
        public const int PostTitleMinLength = 3;
        public const int PostDescriptionMaxLength = 30000;
        public const int PostReportDescriptionMaxLength = 30000;
        public const int PostReportDescriptionMinLength = 3;


        public const int ReplyDescriptionMaxLength = 30000;
        public const int ReplyReportDescriptionMaxLength = 30000;
        public const int ReplyReportDescriptionMinLength = 3;

        public const int ShortDescriptionAllowedLength = 44;
    }
}