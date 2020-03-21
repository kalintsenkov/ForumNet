namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Data.Models.Enums;
    using Infrastructure.Extensions;
    using Services.Contracts;
    using ViewModels.Posts;

    [Authorize]
    public class PostsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITagsService tagsService;
        private readonly IPostsService postsService;
        private readonly IRepliesService repliesService;
        private readonly ICategoriesService categoriesService;
        private readonly IPostReactionsService postReactionsService;

        public PostsController(
            IMapper mapper,
            ITagsService tagsService,
            IPostsService postsService,
            IRepliesService repliesService,
            ICategoriesService categoriesService,
            IPostReactionsService postReactionsService)
        {
            this.mapper = mapper;
            this.tagsService = tagsService;
            this.postsService = postsService;
            this.repliesService = repliesService;
            this.categoriesService = categoriesService;
            this.postReactionsService = postReactionsService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All(int id, string search)
        {
            var viewModel = new PostsAllViewModel
            {
                Posts = await this.postsService.GetAllAsync<PostsListingViewModel>(search),
                PinnedPosts = await this.postsService.GetAllPinnedAsync<PostsListingViewModel>()
            };

            foreach (var post in viewModel.PinnedPosts)
            {
                post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(post.Id);
            }

            foreach (var post in viewModel.Posts)
            {
                post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(post.Id);
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await this.categoriesService.GetAllAsync<PostsCategoryDetailsViewModel>();
            var tags = await this.tagsService.GetAllAsync<PostsTagsDetailsViewModel>();
            var viewModel = new PostsCreateInputModel
            {
                Categories = categories,
                Tags = tags
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostsCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Categories = await this.categoriesService.GetAllAsync<PostsCategoryDetailsViewModel>();
                input.Tags = await this.tagsService.GetAllAsync<PostsTagsDetailsViewModel>();

                return this.View(input);
            }

            var authorId = this.User.GetId();
            var postId = await this.postsService.CreateAsync(
                input.Title,
                input.PostType,
                input.Description,
                authorId,
                input.CategoryId);

            await this.postsService.AddTagsAsync(postId, input.TagIds);

            return this.RedirectToAction(nameof(Details), new { id = postId });
        }

        public async Task<IActionResult> Details(int id, string sort)
        {
            var post = await this.postsService.GetByIdAsync<PostsDetailsViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            await this.postsService.ViewAsync(id);

            post.CurrentUserId = this.User.GetId();
            post.Tags = await this.tagsService.GetAllByPostIdAsync<PostsTagsDetailsViewModel>(id);
            post.Replies = await this.repliesService.GetAllByPostIdAsync<PostsRepliesDetailsViewModel>(id, sort);

            return this.View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostsEditViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.GetId();
            if (post.AuthorId != currentUserId && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            post.Categories = await this.categoriesService.GetAllAsync<PostsCategoryDetailsViewModel>();
            post.Tags = await this.tagsService.GetAllAsync<PostsTagsDetailsViewModel>();

            return this.View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostsEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var viewModel = this.mapper.Map<PostsEditViewModel>(input);

                viewModel.Categories = await this.categoriesService.GetAllAsync<PostsCategoryDetailsViewModel>();
                viewModel.Tags = await this.tagsService.GetAllAsync<PostsTagsDetailsViewModel>();

                return this.View(viewModel);
            }

            var currentUserId = this.User.GetId();
            var postAuthorId = await this.postsService.GetAuthorIdById(input.Id);
            if (postAuthorId != currentUserId && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            await this.postsService.EditAsync(input.Id, input.Title, input.Description, input.CategoryId, input.TagIds);

            return this.RedirectToAction(nameof(Details), new { id = input.Id });
        }

        [HttpPost]
        public async Task<IActionResult> LikeReact(int id)
        {
            var isExisting = await this.postsService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.postReactionsService.ReactAsync(ReactionType.Like, id, userId);
            var (likes, loves, haha, wow, sad, angry) = await this.postReactionsService.GetCountByPostIdAsync(id);
            var viewModel = new PostsReactionsCountViewModel
            {
                Likes = likes,
                Loves = loves,
                Wow = wow,
                Haha = haha,
                Sad = sad,
                Angry = angry
            };

            return this.Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LoveReact(int id)
        {
            var isExisting = await this.postsService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.postReactionsService.ReactAsync(ReactionType.Love, id, userId);
            var (likes, loves, haha, wow, sad, angry) = await this.postReactionsService.GetCountByPostIdAsync(id);
            var viewModel = new PostsReactionsCountViewModel
            {
                Likes = likes,
                Loves = loves,
                Wow = wow,
                Haha = haha,
                Sad = sad,
                Angry = angry
            };

            return this.Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> HahaReact(int id)
        {
            var isExisting = await this.postsService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.postReactionsService.ReactAsync(ReactionType.Haha, id, userId);
            var (likes, loves, haha, wow, sad, angry) = await this.postReactionsService.GetCountByPostIdAsync(id);
            var viewModel = new PostsReactionsCountViewModel
            {
                Likes = likes,
                Loves = loves,
                Wow = wow,
                Haha = haha,
                Sad = sad,
                Angry = angry
            };

            return this.Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> WowReact(int id)
        {
            var isExisting = await this.postsService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.postReactionsService.ReactAsync(ReactionType.Wow, id, userId);
            var (likes, loves, haha, wow, sad, angry) = await this.postReactionsService.GetCountByPostIdAsync(id);
            var viewModel = new PostsReactionsCountViewModel
            {
                Likes = likes,
                Loves = loves,
                Wow = wow,
                Haha = haha,
                Sad = sad,
                Angry = angry
            };

            return this.Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SadReact(int id)
        {
            var isExisting = await this.postsService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.postReactionsService.ReactAsync(ReactionType.Sad, id, userId);
            var (likes, loves, haha, wow, sad, angry) = await this.postReactionsService.GetCountByPostIdAsync(id);
            var viewModel = new PostsReactionsCountViewModel
            {
                Likes = likes,
                Loves = loves,
                Haha = haha,
                Wow = wow,
                Sad = sad,
                Angry = angry
            };

            return this.Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AngryReact(int id)
        {
            var isExisting = await this.postsService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            await this.postReactionsService.ReactAsync(ReactionType.Angry, id, userId);
            var (likes, loves, haha, wow, sad, angry) = await this.postReactionsService.GetCountByPostIdAsync(id);
            var viewModel = new PostsReactionsCountViewModel
            {
                Likes = likes,
                Loves = loves,
                Wow = wow,
                Haha = haha,
                Sad = sad,
                Angry = angry
            };

            return this.Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await this.postsService.GetByIdAsync<PostsDeleteDetailsViewModel>(id);
            if (post == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.GetId();
            if (post.AuthorId != currentUserId && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            await this.postsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }
    }
}