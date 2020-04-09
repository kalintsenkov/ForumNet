namespace ForumNet.Web.ViewModels.PostReports
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class PostReportsInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.PostReportDescriptionMaxLength, ErrorMessage = ErrorMessages.PostReportDescriptionLengthErrorMessage, MinimumLength = GlobalConstants.PostReportDescriptionMinLength)]
        public string Description { get; set; }
    }
}
