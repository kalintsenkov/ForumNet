namespace ForumNet.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    public class ChatSendMessageViewModel
    {
        public string ReceiverId { get; set; }

        public IEnumerable<ChatUserViewModel> Users { get; set; }

        public IEnumerable<ChatUserViewModel> Chats { get; set; }
    }
}
