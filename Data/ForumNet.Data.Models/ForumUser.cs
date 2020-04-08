// ReSharper disable VirtualMemberCallInConstructor
namespace ForumNet.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    using Microsoft.AspNetCore.Identity;
    
    using Common;
    using Enums;

    public class ForumUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ForumUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public GenderType Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public int Points { get; set; }

        public string Biography { get; set; }

        public string ProfilePicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();

        public ICollection<Message> ReceivedMessages { get; set; } = new HashSet<Message>();

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public ICollection<Reply> Replies { get; set; } = new HashSet<Reply>();

        public ICollection<PostReaction> PostReactions { get; set; } = new HashSet<PostReaction>();

        public ICollection<PostReport> PostReports { get; set; } = new HashSet<PostReport>();

        public ICollection<ReplyReaction> ReplyReactions { get; set; } = new HashSet<ReplyReaction>();

        public ICollection<ReplyReport> ReplyReports { get; set; } = new HashSet<ReplyReport>();

        public ICollection<UserFollower> Followers { get; set; } = new HashSet<UserFollower>();

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; } = new HashSet<IdentityUserRole<string>>();

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = new HashSet<IdentityUserClaim<string>>();

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; } = new HashSet<IdentityUserLogin<string>>();
    }
}