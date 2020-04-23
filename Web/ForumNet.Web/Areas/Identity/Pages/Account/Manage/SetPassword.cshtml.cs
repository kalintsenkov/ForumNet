namespace ForumNet.Web.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using Data.Models;

    using static Common.ErrorMessages;
    using static Common.GlobalConstants;

    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<ForumUser> userManager;
        private readonly SignInManager<ForumUser> signInManager;

        public SetPasswordModel(
            UserManager<ForumUser> userManager,
            SignInManager<ForumUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(UserPasswordMaxLength, ErrorMessage = UserPasswordLengthErrorMessage, MinimumLength = UserPasswordMinLength)]
            [DataType(DataType.Password)]
            [Display(Name = UserNewPasswordDisplayName)]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = UserConfirmNewPasswordDisplayName)]
            [Compare(nameof(NewPassword), ErrorMessage = UserPasswordsDoNotMatchErrorMessage)]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var hasPassword = await this.userManager.HasPasswordAsync(user);
            if (hasPassword)
            {
                return this.RedirectToPage("./ChangePassword");
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await this.userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.Page();
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your password has been set.";

            return this.RedirectToPage();
        }
    }
}
