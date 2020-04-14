namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using InputModels.Replies;
    using Services.Contracts;
    using ViewModels.Replies;

    [Authorize]
    public class RepliesController : Controller
    {
        private readonly IRepliesService repliesService;

        public RepliesController(IRepliesService repliesService)
        {
            this.repliesService = repliesService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(RepliesCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Details", "Posts", new { id = input.PostId });
            }

            await this.repliesService.CreateAsync(input.Description, input.ParentId, input.PostId, this.User.GetId());

            return !input.ParentId.HasValue
                ? this.RedirectToAction("Details", "Posts", new { id = input.PostId })
                : this.RedirectToAction(nameof(Details), new { id = input.ParentId.Value });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesEditInputModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            if (reply.AuthorId != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            return this.View(reply);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RepliesEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var replyAuthorId = await this.repliesService.GetAuthorIdByIdAsync(input.Id);
            if (replyAuthorId != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            await this.repliesService.EditAsync(input.Id, input.Description);

            return this.RedirectToAction(nameof(Details), new { id = input.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesDetailsViewModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            reply.Replies = await this.repliesService.GetAllByPostIdAsync<RepliesDetailsViewModel>(reply.PostId);

            return this.View(reply);
        }

        public async Task<IActionResult> BestAnswer(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesDetailsViewModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }
            
            if (reply.PostAuthorId != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            await this.repliesService.MakeBestAnswerAsync(id);

            return this.RedirectToAction("Details", "Posts", new { id = reply.PostId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesDeleteDetailsViewModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            if (reply.Author.Id != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            return this.View(reply);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesDeleteConfirmedViewModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            if (reply.AuthorId != this.User.GetId() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            await this.repliesService.DeleteAsync(id);

            return this.RedirectToAction("Details", "Posts", new { id = reply.PostId });
        }
    }
}