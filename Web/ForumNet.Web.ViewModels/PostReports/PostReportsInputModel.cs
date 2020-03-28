namespace ForumNet.Web.ViewModels.PostReports
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;

    public class PostReportsInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.PostReportDescriptionMaxLength)]
        public string Description { get; set; }
    }
}
