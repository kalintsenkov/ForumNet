namespace ForumNet.Web.ViewModels.Messages
{
    using System.Collections.Generic;

    public class MessagesCreateViewModel
    {
        public string ReceiverId { get; set; }

        public IEnumerable<MessagesCreateUserViewModel> Users { get; set; }

        public IEnumerable<MessagesAllViewModel> All { get; set; }
    }
}
