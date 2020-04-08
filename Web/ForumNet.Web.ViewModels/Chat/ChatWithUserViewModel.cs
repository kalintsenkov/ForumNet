namespace ForumNet.Web.ViewModels.Messages
{
    using System.Collections.Generic;

    public class ChatWithUserViewModel
    {
        public string ReceiverId { get; set; }

        public IEnumerable<ChatUserViewModel> Chats { get; set; }

        public IEnumerable<ChatMessagesWithUserViewModel> Messages { get; set; }
    }
}
