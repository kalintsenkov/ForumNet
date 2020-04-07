namespace ForumNet.Web.ViewModels.Messages
{
    using System.Collections.Generic;

    public class ChatAllViewModel
    {
        public IEnumerable<ChatConversationsViewModel> Conversations { get; set; }
    }
}
