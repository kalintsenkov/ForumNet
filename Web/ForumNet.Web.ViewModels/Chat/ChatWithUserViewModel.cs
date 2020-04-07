namespace ForumNet.Web.ViewModels.Messages
{
    using System.Collections.Generic;

    public class ChatWithUserViewModel
    {
        public string ReceiverId { get; set; }

        public IEnumerable<ChatConversationsViewModel> Conversations { get; set; }

        public IEnumerable<ChatMessagesWithUserViewModel> All { get; set; }
    }
}
