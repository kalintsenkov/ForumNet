namespace ForumNet.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.ReplyReports;

    public class ReplyReportsController : AdminController
    {
        private readonly IReplyReportsService replyReportsService;

        public ReplyReportsController(IReplyReportsService replyReportsService)
        {
            this.replyReportsService = replyReportsService;
        }

        public async Task<IActionResult> All()
        {
            var replyReports = await this.replyReportsService.GetAll<ReplyReportsListingViewModel>();

            var viewModel = new ReplyReportsAllViewModel
            {
                ReplyReports = replyReports
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var postReport = await this.replyReportsService.GetById<ReplyReportsDetailsViewModel>(id);
            if (postReport == null)
            {
                return this.NotFound();
            }

            return this.View(postReport);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isExisting = await this.replyReportsService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.replyReportsService.DeleteAsync(id);

            return this.RedirectToAction(nameof(All));
        }
    }
}
