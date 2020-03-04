namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Categories;

    public class CategoriesController : Controller
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> All()
        {
            var viewModel = new CategoriesAllViewModel
            {
                Categories = await this.categoriesService.GetAllAsync<CategoriesInfoViewModel>()
            };

            foreach (var category in viewModel.Categories)
            {
                category.Threads = await this.categoriesService.GetThreadsCountByIdAsync(category.Id);
            }

            return View(viewModel);
        }

        //// GET: Categories/Details/5
        //public IActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Categories/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Categories/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Categories/Edit/5
        //public IActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Categories/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Categories/Delete/5
        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Categories/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}