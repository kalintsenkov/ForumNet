namespace ForumNet.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Posts;

    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly IUsersService usersService;

        public PostsController(IPostsService postsService, IUsersService usersService)
        {
            this.postsService = postsService;
            this.usersService = usersService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostsInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var authorId = await this.usersService.GetId(this.User);
            await this.postsService.CreateAsync(
                input.Title,
                input.PostType,
                input.Description,
                authorId, 
                input.CategoryId,
                input.ImageOrVideoUrl);

            return RedirectToAction("Index", "Home");
        }

        //// GET: Posts/Details/5
        //public IActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Posts/Edit/5
        //public IActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// GET: Posts/Delete/5
        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}
    }
}