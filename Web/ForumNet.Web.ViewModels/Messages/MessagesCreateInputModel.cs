namespace ForumNet.Web.ViewModels.Messages
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;

    public class MessagesCreateInputModel
    {
        [Required]
        [MaxLength(DataConstants.MessageContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string ReceiverId { get; set; }
    }
}
