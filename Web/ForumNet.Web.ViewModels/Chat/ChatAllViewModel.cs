namespace ForumNet.Web.ViewModels.Messages
{
    using System.Collections.Generic;

    public class ChatAllViewModel
    {
        public IEnumerable<ChatConversationsViewModel> Chats { get; set; }
    }
}
