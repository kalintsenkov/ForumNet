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

        public async Task<IActionResult> All(string search = null)
        {
            var categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>(search);
            var viewModel = new CategoriesAllViewModel
            {
                Search = search,
                Categories = categories
            };

            return View(viewModel);
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

            return this.RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categoriesService.GetByIdAsync<CategoriesEditInputModel>(id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoriesEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.categoriesService.EditAsync(input.Id, input.Name);

            return this.RedirectToAction("Details", "Categories", new { area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isExisting = await this.categoriesService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.categoriesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }
    }
}
