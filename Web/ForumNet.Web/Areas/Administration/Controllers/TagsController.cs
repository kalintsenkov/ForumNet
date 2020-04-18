namespace ForumNet.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using InputModels.Tags;
    using Services.Tags;
    using ViewModels.Tags;

    public class TagsController : AdminController
    {
        private const int TagsPerPage = 9;

        private readonly ITagsService tagsService;

        public TagsController(ITagsService tagsService)
        {
            this.tagsService = tagsService;
        }

        public async Task<IActionResult> All(int page = 1, string search = null)
        {
            var skip = (page - 1) * TagsPerPage;
            var count = await this.tagsService.GetCountAsync();
            var tags = await this.tagsService.GetAllAsync<TagsInfoViewModel>(search, skip, TagsPerPage);
            var viewModel = new TagsAllViewModel
            {
                Tags = tags,
                Search = search,
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(count / (decimal)TagsPerPage)
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
            var isExisting = await this.tagsService.IsExistingAsync(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.tagsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }
    }
}
