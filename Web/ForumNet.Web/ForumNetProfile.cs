namespace ForumNet.Web
{
    using System.Globalization;

    using AutoMapper;

    using Data.Models;
    using ViewModels.Categories;
    using ViewModels.Posts;
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
            this.CreateMap<Post, PostsListingViewModel>();
            this.CreateMap<Post, PostsEditViewModel>();
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