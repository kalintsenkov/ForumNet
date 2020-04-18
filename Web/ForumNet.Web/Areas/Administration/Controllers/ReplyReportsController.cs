namespace ForumNet.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Reports;
    using ViewModels.ReplyReports;

    public class ReplyReportsController : AdminController
    {
        private readonly IReplyReportsService replyReportsService;

        public ReplyReportsController(IReplyReportsService replyReportsService)
            => this.replyReportsService = replyReportsService;

        public async Task<IActionResult> All()
        {
            var viewModel = new ReplyReportsAllViewModel
            {
                ReplyReports = await this.replyReportsService.GetAllAsync<ReplyReportsListingViewModel>()
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var postReport = await this.replyReportsService.GetByIdAsync<ReplyReportsDetailsViewModel>(id);
            if (postReport == null)
            {
                return this.NotFound();
            }

            return this.View(postReport);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isExisting = await this.replyReportsService.IsExistingAsync(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReportsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }
    }
}
