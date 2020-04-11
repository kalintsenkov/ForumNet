namespace ForumNet.Web.ViewModels.Chat
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class ChatSendMessageInputModel
    {
        [Required]
        [MaxLength(GlobalConstants.MessageContentMaxLength)]
        public string Message { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public IEnumerable<ChatUserViewModel> Users { get; set; }
    }
}
