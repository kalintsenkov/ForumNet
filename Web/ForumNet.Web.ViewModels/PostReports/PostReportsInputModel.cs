namespace ForumNet.Web.ViewModels.PostReports
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class PostReportsInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PostReportDescriptionMaxLength)]
        public string Description { get; set; }
    }
}
