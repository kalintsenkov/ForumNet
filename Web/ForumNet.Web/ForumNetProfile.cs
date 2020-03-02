namespace ForumNet.Web
{
    using AutoMapper;

    using Data.Models;
    using ViewModels.Categories;
    using ViewModels.Posts;
    using ViewModels.Tags;

    public class ForumNetProfile : Profile
    {
        public ForumNetProfile()
        {
            #region Categories

            this.CreateMap<Category, CategoriesInfoViewModel>();

            #endregion

            #region Tags

            this.CreateMap<Tag, TagsInfoViewModel>();
            this.CreateMap<PostTag, TagsInfoViewModel>();

            #endregion

            #region Posts

            this.CreateMap<Post, PostsDetailsViewModel>();
            this.CreateMap<Post, PostsListingViewModel>();

            #endregion

            #region Replies

            

            #endregion
        }
    }
}