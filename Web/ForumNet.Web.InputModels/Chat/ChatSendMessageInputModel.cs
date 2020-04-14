namespace ForumNet.Web.InputModels.Chat
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ViewModels.Chat;

    using static Common.GlobalConstants;

    public class ChatSendMessageInputModel
    {
        [Required]
        [MaxLength(MessageContentMaxLength)]
        public string Message { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public IEnumerable<ChatUserViewModel> Users { get; set; }
    }
}
