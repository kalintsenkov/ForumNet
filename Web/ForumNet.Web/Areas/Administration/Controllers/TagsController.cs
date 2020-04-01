namespace ForumNet.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;

    public class TagsController : AdminController
    {
        private readonly ITagsService tagsService;

        public TagsController(ITagsService tagsService)
        {
            this.tagsService = tagsService;
        }

        // public IActionResult Create()
        // {
        //     return this.View();
        // }
           
        // [HttpPost]
        // public async Task<IActionResult> Create(TagsCreateInputModel input)
        // {
        //     if (!this.ModelState.IsValid)
        //     {
        //         return this.View(input);
        //     }
           
        //     await this.tagsService.CreateAsync(input.Name);
           
        //     return this.RedirectToAction(nameof(All));
        // }
    }
}
