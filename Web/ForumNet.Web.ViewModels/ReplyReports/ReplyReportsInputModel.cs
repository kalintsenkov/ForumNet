namespace ForumNet.Web.ViewModels.ReplyReports
{
    using System.ComponentModel.DataAnnotations;

    using static Common.ErrorMessages;
    using static Common.GlobalConstants;

    public class ReplyReportsInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(ReplyReportDescriptionMaxLength, ErrorMessage = ReplyReportDescriptionLengthErrorMessage, MinimumLength = ReplyReportDescriptionMinLength)]
        public string Description { get; set; }
    }
}
