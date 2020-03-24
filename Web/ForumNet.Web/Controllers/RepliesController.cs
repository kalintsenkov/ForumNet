﻿namespace ForumNet.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
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

            var authorId = this.User.GetId();
            await this.repliesService.CreateAsync(input.Description, input.ParentId, input.PostId, authorId);

            return this.RedirectToAction("Details", "Posts", new { id = input.PostId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesEditInputModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.GetId();
            if (reply.AuthorId != currentUserId && !this.User.IsAdministrator())
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

            var currentUserId = this.User.GetId();
            var replyAuthorId = await this.repliesService.GetAuthorIdById(input.Id);
            if (replyAuthorId != currentUserId && !this.User.IsAdministrator())
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

            reply.CurrentUserId = this.User.GetId();

            return this.View(reply);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var reply = await this.repliesService.GetByIdAsync<RepliesDeleteDetailsViewModel>(id);
            if (reply == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.GetId();
            if (reply.AuthorId != currentUserId && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            await this.repliesService.DeleteAsync(id);

            return this.RedirectToAction("Details", "Posts", new { id = reply.PostId });
        }
    }
}