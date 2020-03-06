namespace ForumNet.Web.ViewModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    public class RepliesReplyInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}