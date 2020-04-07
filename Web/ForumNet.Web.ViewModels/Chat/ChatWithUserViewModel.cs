namespace ForumNet.Web.ViewModels.Messages
{
    using System.Collections.Generic;

    public class ChatWithUserViewModel
    {
        public string ReceiverId { get; set; }

        public IEnumerable<ChatConversationsViewModel> Chats { get; set; }

        public IEnumerable<ChatMessagesWithUserViewModel> Messages { get; set; }
    }
}
