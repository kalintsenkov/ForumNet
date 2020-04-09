namespace ForumNet.Web.ViewModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class RepliesCreateInputModel
    {
        public int? ParentId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ReplyDescriptionMaxLength)]
        public string Description { get; set; }
    }
}