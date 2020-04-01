namespace ForumNet.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Tags;

    public class TagsController : AdminController
    {
        private readonly ITagsService tagsService;

        public TagsController(ITagsService tagsService)
        {
            this.tagsService = tagsService;
        }

        public async Task<IActionResult> All(string search = null)
        {
            var tags = await this.tagsService.GetAllAsync<TagsInfoViewModel>(search);
            var viewModel = new TagsAllViewModel
            {
                Tags = tags,
                Search = search
            };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TagsCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.tagsService.CreateAsync(input.Name);

            return this.RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isExisting = await this.tagsService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.tagsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }
    }
}
