namespace ForumNet.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using ViewModels.PostReports;
    using Services.Contracts;

    public class PostReportsController : AdminController
    {
        private readonly IPostReportsService postReportsService;

        public PostReportsController(IPostReportsService postReportsService)
        {
            this.postReportsService = postReportsService;
        }

        public async Task<IActionResult> All()
        {
            var postReports = await this.postReportsService.GetAllAsync<PostReportsListingViewModel>();

            var viewModel = new PostReportAllViewModel
            {
                PostReports = postReports
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var postReport = await this.postReportsService.GetByIdAsync<PostReportsDetailsViewModel>(id);
            if (postReport == null)
            {
                return this.NotFound();
            }

            return this.View(postReport);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isExisting = await this.postReportsService.IsExistingAsync(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.postReportsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }
    }
}
