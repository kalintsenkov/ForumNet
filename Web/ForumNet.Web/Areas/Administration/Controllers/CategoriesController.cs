namespace ForumNet.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Categories;

    public class CategoriesController : AdminController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoriesCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.categoriesService.CreateAsync(input.Name);

            return this.RedirectToAction("All", "Categories");
        }
    }
}
