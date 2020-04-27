﻿namespace ForumNet.Web
{
    using System.Globalization;
    using System.Linq;

    using AutoMapper;

    using Common;
    using Data.Models;
    using Data.Models.Enums;
    using InputModels.Categories;
    using InputModels.PostReports;
    using InputModels.Posts;
    using InputModels.Replies;
    using InputModels.ReplyReports;
    using ViewModels.Categories;
    using ViewModels.Chat;
    using ViewModels.Home;
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
            this.CreateMap<Category, CategoriesInfoViewModel>()
                .ForMember(
                    dest => dest.PostsCount,
                    dest => dest.MapFrom(src => src.Posts.Count(p => !p.IsDeleted)));
            this.CreateMap<Category, CategoriesEditInputModel>();
            this.CreateMap<Category, PostsCategoryDetailsViewModel>();
            this.CreateMap<Category, UsersThreadsCategoryViewModel>();
            #endregion

            #region Messages
            this.CreateMap<Message, ChatMessagesWithUserViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
            #endregion

            #region Posts
            this.CreateMap<Post, PostsDeleteConfirmedViewModel>();
            this.CreateMap<Post, PostReportsInputModel>()
                .ForMember(
                dest => dest.Description,
                dest => dest.Ignore());
            this.CreateMap<Post, UsersThreadsViewModel>()
                .ForMember(
                    dest => dest.Likes,
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType != ReactionType.Neutral)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Replies.Count(r => !r.IsDeleted)));
            this.CreateMap<Post, PostsDeleteViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Replies.Count(r => !r.IsDeleted)))
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
                    dest => dest.MapFrom(src => src.Reactions.Count(r => r.ReactionType != ReactionType.Neutral)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Replies.Count(r => !r.IsDeleted)));
            this.CreateMap<Post, PostsEditInputModel>()
                .ForMember(
                    dest => dest.TagIds,
                    dest => dest.MapFrom(src => src.Tags.Select(t => t.TagId)));
            this.CreateMap<Post, PostsDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Replies.Count(r => !r.IsDeleted)))
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
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeShortFormat, CultureInfo.InvariantCulture)));
            this.CreateMap<PostReport, PostReportsDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
            #endregion

            #region Tags
            this.CreateMap<Tag, TagsInfoViewModel>()
                .ForMember(
                    dest => dest.PostsCount,
                    dest => dest.MapFrom(src => src.Posts.Count(p => !p.Post.IsDeleted)));
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
            this.CreateMap<Reply, UsersRepliesViewModel>()
                .ForMember(
                    dest => dest.Activity,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeShortFormat, CultureInfo.InvariantCulture)));
            this.CreateMap<Reply, RepliesDeleteViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)))
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
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)))
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
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)))
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

            #region ReplyReports
            this.CreateMap<ReplyReport, ReplyReportsListingViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeShortFormat, CultureInfo.InvariantCulture)));
            this.CreateMap<ReplyReport, ReplyReportsDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString(GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture)));
            #endregion

            #region Users
            this.CreateMap<ForumUser, ChatUserViewModel>();
            this.CreateMap<ForumUser, ChatConversationsViewModel>();
            this.CreateMap<ForumUser, UsersLoginStatusViewModel>();
            this.CreateMap<ForumUser, HomeAdminViewModel>();
            this.CreateMap<ForumUser, RepliesAuthorDetailsViewModel>();
            this.CreateMap<ForumUser, PostsAuthorDetailsViewModel>();
            this.CreateMap<ForumUser, UsersDetailsViewModel>();
            this.CreateMap<ForumUser, UsersFollowersViewModel>()
                .ForMember(
                    dest => dest.ThreadsCount,
                    dest => dest.MapFrom(src => src.Posts.Count(p => !p.IsDeleted)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Replies.Count(r => !r.IsDeleted && !r.Post.IsDeleted)));
            this.CreateMap<UserFollower, UsersFollowersViewModel>()
                .ForMember(
                    dest => dest.ThreadsCount,
                    dest => dest.MapFrom(src => src.Follower.Posts.Count(p => !p.IsDeleted)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Follower.Replies.Count(r => !r.IsDeleted && !r.Post.IsDeleted)));
            this.CreateMap<ForumUser, UsersFollowingViewModel>()
                .ForMember(
                    dest => dest.ThreadsCount,
                    dest => dest.MapFrom(src => src.Posts.Count(p => !p.IsDeleted)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Replies.Count(r => !r.IsDeleted && !r.Post.IsDeleted)));
            this.CreateMap<UserFollower, UsersFollowingViewModel>()
                .ForMember(
                    dest => dest.ThreadsCount,
                    dest => dest.MapFrom(src => src.Follower.Posts.Count(p => !p.IsDeleted)))
                .ForMember(
                    dest => dest.RepliesCount,
                    dest => dest.MapFrom(src => src.Follower.Replies.Count(r => !r.IsDeleted && !r.Post.IsDeleted)));
            #endregion
        }
    }
}