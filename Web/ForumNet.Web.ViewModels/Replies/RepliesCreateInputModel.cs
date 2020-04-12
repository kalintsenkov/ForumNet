namespace ForumNet.Web.ViewModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    using static Common.GlobalConstants;

    public class RepliesCreateInputModel
    {
        public int? ParentId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        [MaxLength(ReplyDescriptionMaxLength)]
        public string Description { get; set; }
    }
}