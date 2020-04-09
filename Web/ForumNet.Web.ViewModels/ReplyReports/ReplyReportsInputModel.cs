namespace ForumNet.Web.ViewModels.ReplyReports
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class ReplyReportsInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.ReplyReportDescriptionMaxLength, ErrorMessage = ErrorMessages.ReplyReportDescriptionLengthErrorMessage, MinimumLength = GlobalConstants.ReplyReportDescriptionMinLength)]
        public string Description { get; set; }
    }
}
