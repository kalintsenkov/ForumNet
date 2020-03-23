namespace ForumNet.Web.ViewModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;

    public class RepliesCreateInputModel
    {
        public int? ParentId { get; set; }

        public int PostId { get; set; }

        [Required]
        [MaxLength(DataConstants.ReplyDescriptionMaxLength)]
        public string Description { get; set; }
    }
}