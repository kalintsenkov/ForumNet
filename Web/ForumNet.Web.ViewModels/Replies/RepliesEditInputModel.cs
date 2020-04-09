namespace ForumNet.Web.ViewModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class RepliesEditInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ReplyDescriptionMaxLength)]
        public string Description { get; set; }

        public string AuthorId { get; set; }
    }
}
