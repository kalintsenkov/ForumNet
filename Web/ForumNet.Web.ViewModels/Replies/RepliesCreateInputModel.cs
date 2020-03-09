namespace ForumNet.Web.ViewModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;

    public class RepliesCreateInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.ReplyDescriptionMaxLength)]
        public string Description { get; set; }
    }
}