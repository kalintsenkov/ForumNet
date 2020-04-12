namespace ForumNet.Web.ViewModels.PostReports
{
    using System.ComponentModel.DataAnnotations;

    using static Common.ErrorMessages;
    using static Common.GlobalConstants;

    public class PostReportsInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(PostReportDescriptionMaxLength, ErrorMessage = PostReportDescriptionLengthErrorMessage, MinimumLength = PostReportDescriptionMinLength)]
        public string Description { get; set; }
    }
}
