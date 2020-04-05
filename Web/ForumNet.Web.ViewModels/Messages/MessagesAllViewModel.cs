namespace ForumNet.Web.ViewModels.Messages
{
    using System.Collections.Generic;

    public class MessagesAllViewModel
    {
        public IEnumerable<MessagesConversationsViewModel> Conversations { get; set; }
    }
}
