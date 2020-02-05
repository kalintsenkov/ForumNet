// ReSharper disable VirtualMemberCallInConstructor
namespace ForumNet.Data.Models
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Common;

    public class ForumRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public ForumRole()
            : this(null)
        {
        }

        public ForumRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}