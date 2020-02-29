namespace ForumNet.Web
{
    using AutoMapper;

    using Data.Models;
    using ViewModels.Posts;

    public class ForumNetProfile : Profile
    {
        public ForumNetProfile()
        {
            this.CreateMap<Category, CategoriesListingViewModel>();
            this.CreateMap<Tag, TagsListingViewModel>();
        }
    }
}