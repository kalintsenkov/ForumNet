namespace ForumNet.Web
{
    using System.Globalization;
    using System.Linq;

    using AutoMapper;

    using Data.Models;
    using Data.Models.Enums;
    using ViewModels.Categories;
    using ViewModels.PostReports;
    using ViewModels.Posts;
    using ViewModels.Replies;
    using ViewModels.ReplyReports;
    using ViewModels.Tags;
    using ViewModels.Users;

    public class ForumNetProfile : Profile
    {
        public ForumNetProfile()
        {
            #region Categories
            this.CreateMap<Category, CategoriesInfoViewModel>();
            this.CreateMap<Category, CategoriesEditInputModel>();
            this.CreateMap<Category, PostsCategoryDetailsViewModel>();
            this.CreateMap<Category, UsersThreadsCategoryViewModel>();
            #endregion

            #region Posts
            this.CreateMap<PostsEditInputModel, PostsEditViewModel>();
            this.CreateMap<Post, PostsDeleteConfirmedViewModel>();
            this.CreateMap<Post, PostReportsInputModel>()
                .ForMember(
                dest => dest.Description,
                dest => dest.Ignore());
            this.CreateMap<Post, UsersThreadsAllViewModel>()
                .ForMember(
                    dest => dest.Likes,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Like)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Replies.Count(r => !r.IsDeleted)));
            this.CreateMap<Post, PostsDeleteDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)))
                .ForMember(
                    dest => dest.Likes,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Like)))
                .ForMember(
                    dest => dest.Loves,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Love)))
                .ForMember(
                    dest => dest.HahaCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Haha)))
                .ForMember(
                    dest => dest.WowCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Wow)))
                .ForMember(
                    dest => dest.SadCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Sad)))
                .ForMember(
                    dest => dest.AngryCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Angry)));
            this.CreateMap<Post, PostsListingViewModel>()
                .ForMember(
                    dest => dest.Likes,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Like)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Replies.Count(r => !r.IsDeleted)));
            this.CreateMap<Post, PostsEditViewModel>()
                .ForMember(
                    dest => dest.TagIds,
                    dest => dest.MapFrom(src => src.Tags.Select(t => t.TagId)));
            this.CreateMap<Post, PostsDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)))
                .ForMember(
                    dest => dest.Likes,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Like)))
                .ForMember(
                    dest => dest.Loves,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Love)))
                .ForMember(
                    dest => dest.HahaCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Haha)))
                .ForMember(
                    dest => dest.WowCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Wow)))
                .ForMember(
                    dest => dest.SadCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Sad)))
                .ForMember(
                    dest => dest.AngryCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Angry)));
            #endregion

            #region PostReports
            this.CreateMap<PostReport, PostReportsListingViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)));
            this.CreateMap<PostReport, PostReportsDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)));
            #endregion

            #region Tags
            this.CreateMap<Tag, TagsInfoViewModel>();
            this.CreateMap<Tag, PostsTagsDetailsViewModel>();
            this.CreateMap<Tag, UsersThreadsTagsViewModel>();
            this.CreateMap<PostTag, TagsInfoViewModel>();
            this.CreateMap<PostTag, PostsTagsDetailsViewModel>();
            this.CreateMap<PostTag, UsersThreadsTagsViewModel>();
            #endregion

            #region Replies
            this.CreateMap<Reply, RepliesEditInputModel>();
            this.CreateMap<Reply, RepliesDeleteConfirmedViewModel>();
            this.CreateMap<Reply, ReplyReportsInputModel>()
                .ForMember(
                    dest => dest.Description,
                    dest => dest.Ignore());
            this.CreateMap<Reply, UsersRepliesAllViewModel>()
                .ForMember(
                    dest => dest.Activity,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)));
            this.CreateMap<Reply, RepliesDeleteDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)))
                .ForMember(
                    dest => dest.Likes,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Like)))
                .ForMember(
                    dest => dest.Loves,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Love)))
                .ForMember(
                    dest => dest.HahaCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Haha)))
                .ForMember(
                    dest => dest.WowCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Wow)))
                .ForMember(
                    dest => dest.SadCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Sad)))
                .ForMember(
                    dest => dest.AngryCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Angry)));
            this.CreateMap<Reply, RepliesDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)))
                .ForMember(
                    dest => dest.Likes,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Like)))
                .ForMember(
                    dest => dest.Loves,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Love)))
                .ForMember(
                    dest => dest.HahaCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Haha)))
                .ForMember(
                    dest => dest.WowCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Wow)))
                .ForMember(
                    dest => dest.SadCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Sad)))
                .ForMember(
                    dest => dest.AngryCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Angry)));
            this.CreateMap<Reply, PostsRepliesDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)))
                .ForMember(
                    dest => dest.Likes,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Like)))
                .ForMember(
                    dest => dest.Loves,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Love)))
                .ForMember(
                    dest => dest.HahaCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Haha)))
                .ForMember(
                    dest => dest.WowCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Wow)))
                .ForMember(
                    dest => dest.SadCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Sad)))
                .ForMember(
                    dest => dest.AngryCount,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType == ReactionType.Angry)));
            #endregion

            #region Users
            this.CreateMap<ForumUser, RepliesAuthorDetailsViewModel>();
            this.CreateMap<ForumUser, PostsAuthorDetailsViewModel>();
            this.CreateMap<ForumUser, UsersThreadsViewModel>()
                .ForMember(
                    dest => dest.FollowersCount,
                    dest => dest.MapFrom(src => src.Followers.Count(u => !u.IsDeleted && u.UserId == src.Id)))
                .ForMember(
                    dest => dest.FollowingCount,
                    dest => dest.MapFrom(src => src.Followers.Count(u => !u.IsDeleted && u.FollowerId == src.Id)));
            this.CreateMap<ForumUser, UsersRepliesViewModel>()
                .ForMember(
                    dest => dest.FollowersCount,
                    dest => dest.MapFrom(src => src.Followers.Count(u => !u.IsDeleted && u.UserId == src.Id)))
                .ForMember(
                    dest => dest.FollowingCount,
                    dest => dest.MapFrom(src => src.Followers.Count(u => !u.IsDeleted && u.FollowerId == src.Id)));
            this.CreateMap<ForumUser, UsersFollowersViewModel>()
                .ForMember(
                    dest => dest.FollowersCount,
                    dest => dest.MapFrom(src => src.Followers.Count(u => !u.IsDeleted && u.UserId == src.Id)))
                .ForMember(
                    dest => dest.FollowingCount,
                    dest => dest.MapFrom(src => src.Followers.Count(u => !u.IsDeleted && u.FollowerId == src.Id)));

            this.CreateMap<UserFollower, UsersFollowersAllViewModel>()
                .ForMember(
                    dest => dest.ThreadsCount,
                    dest => dest.MapFrom(src => src.Follower.Posts.Count(p => !p.IsDeleted)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Follower.Replies.Count(p => !p.IsDeleted)));

            this.CreateMap<ForumUser, UsersFollowersAllViewModel>()
                .ForMember(
                    dest => dest.ThreadsCount,
                    dest => dest.MapFrom(src => src.Posts.Count(p => !p.IsDeleted)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Replies.Count(p => !p.IsDeleted)));
            #endregion
        }
    }
}