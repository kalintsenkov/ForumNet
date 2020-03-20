namespace ForumNet.Web.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using Data.Models;
    using Infrastructure.Extensions;
    using Services.Contracts;

    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ForumUser> userManager;
        private readonly SignInManager<ForumUser> signInManager;
        private readonly IUsersService usersService;

        public DeletePersonalDataModel(
            UserManager<ForumUser> userManager,
            SignInManager<ForumUser> signInManager, 
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.usersService = usersService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            this.RequirePassword = await userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            if (this.RequirePassword)
            {
                if (!await this.userManager.CheckPasswordAsync(user, Input.Password))
                {
                    this.ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            var userId = this.User.GetId();
            await this.usersService.DeleteAsync(userId);

            await this.signInManager.SignOutAsync();

            return Redirect("~/");
        }
    }
}
