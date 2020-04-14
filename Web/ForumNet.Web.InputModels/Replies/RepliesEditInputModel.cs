namespace ForumNet.Web.InputModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    using static Common.GlobalConstants;

    public class RepliesEditInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ReplyDescriptionMaxLength)]
        public string Description { get; set; }

        public string AuthorId { get; set; }
    }
}
