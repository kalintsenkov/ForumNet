namespace ForumNet.Web.ViewModels.ReplyReports
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class ReplyReportsInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ReplyReportDescriptionMaxLength)]
        public string Description { get; set; }
    }
}
