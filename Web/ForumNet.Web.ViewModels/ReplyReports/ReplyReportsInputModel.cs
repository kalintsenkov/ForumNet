namespace ForumNet.Web.ViewModels.ReplyReports
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;

    public class ReplyReportsInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.ReplyReportDescriptionMaxLength)]
        public string Description { get; set; }
    }
}
