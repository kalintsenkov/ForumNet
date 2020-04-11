namespace ForumNet.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    public class ChatWithUserViewModel
    {
        public ChatUserViewModel User { get; set; }

        public IEnumerable<ChatMessagesWithUserViewModel> Messages { get; set; }
    }
}
