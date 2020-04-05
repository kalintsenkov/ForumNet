namespace ForumNet.Web.ViewModels.Messages
{
    using System.Collections.Generic;

    public class MessagesWithUserViewModel
    {
        public string ReceiverId { get; set; }

        public IEnumerable<MessagesConversationsViewModel> Conversations { get; set; }

        public IEnumerable<MessagesAllWithUserViewModel> AllWithUser { get; set; }
    }
}
