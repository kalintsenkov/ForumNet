namespace ForumNet.Web.ViewModels.Messages
{
    using System.ComponentModel.DataAnnotations;

    using Data.Common;

    public class MessagesSendInputModel
    {
        [Required]
        [MaxLength(DataConstants.MessageContentMaxLength)]
        public string Content { get; set; }

        public string ReceiverId { get; set; }
    }
}
