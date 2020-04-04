namespace ForumNet.Web.ViewModels.Messages
{
    using System.Collections.Generic;

    public class MessagesWithUserViewModel
    {
        public string ReceiverId { get; set; }

        public IEnumerable<MessagesAllViewModel> All { get; set; }

        public IEnumerable<MessagesDetailsAllViewModel> AllWithUser { get; set; }
    }
}
