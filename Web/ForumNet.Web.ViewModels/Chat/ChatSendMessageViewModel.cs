namespace ForumNet.Web.ViewModels.Messages
{
    using System.Collections.Generic;

    public class ChatSendMessageViewModel
    {
        public string ReceiverId { get; set; }

        public IEnumerable<ChatUserViewModel> Users { get; set; }

        public IEnumerable<ChatConversationsViewModel> Chats { get; set; }
    }
}
