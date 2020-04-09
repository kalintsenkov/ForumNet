namespace ForumNet.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    public class ChatAllViewModel
    {
        public IEnumerable<ChatUserViewModel> Chats { get; set; }
    }
}
