namespace ForumNet.Web
{
    using System.Globalization;
    using System.Linq;

    using AutoMapper;

    using Data.Models;
    using ViewModels.Categories;
    using ViewModels.Posts;
    using ViewModels.Replies;
    using ViewModels.Tags;
    using ViewModels.Users;

    public class ForumNetProfile : Profile
    {
        public ForumNetProfile()
        {
            #region Categories

            this.CreateMap<Category, CategoriesInfoViewModel>();
            this.CreateMap<Category, CategoriesEditInputModel>();

            #endregion

            #region Posts

            this.CreateMap<PostsEditInputModel, PostsEditViewModel>();
            this.CreateMap<Post, PostsDeleteDetailsViewModel>();
            this.CreateMap<Post, PostsListingViewModel>();
            this.CreateMap<Post, PostsEditViewModel>()
                .ForMember(
                    dest => dest.TagIds,
                    dest => dest.MapFrom(src => src.Tags.Select(t => t.TagId)));
            this.CreateMap<Post, PostsDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)));

            #endregion

            #region Tags

            this.CreateMap<Tag, TagsInfoViewModel>();
            this.CreateMap<PostTag, TagsInfoViewModel>();

            #endregion

            #region Replies

            this.CreateMap<Reply, RepliesDeleteDetailsViewModel>();
            this.CreateMap<Reply, RepliesEditInputModel>();
            this.CreateMap<Reply, RepliesDetailsViewModel>()
                .ForMember(
                    dest => dest.CreatedOn,
                    dest => dest.MapFrom(src => src.CreatedOn.ToString("dd MMM, yyyy", CultureInfo.InvariantCulture)));

            #endregion

            #region Users

            this.CreateMap<ForumUser, UsersInfoViewModel>()
                .ForMember(
                    dest => dest.Name,
                    dest => dest.MapFrom(src => src.UserName));

            #endregion
        }
    }
}