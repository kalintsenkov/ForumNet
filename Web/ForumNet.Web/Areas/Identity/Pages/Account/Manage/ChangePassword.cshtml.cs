namespace ForumNet.Web.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using Common;
    using Data.Models;
    using Services.Contracts;

    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ForumUser> userManager;
        private readonly SignInManager<ForumUser> signInManager;
        private readonly IUsersService usersService;

        public ChangePasswordModel(
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

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = GlobalConstants.UserCurrentPasswordDisplayName)]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(GlobalConstants.UserPasswordMaxLength, ErrorMessage = ErrorMessages.UserPasswordLengthErrorMessage, MinimumLength = GlobalConstants.UserPasswordMinLength)]
            [DataType(DataType.Password)]
            [Display(Name = GlobalConstants.UserNewPasswordDisplayName)]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = GlobalConstants.UserConfirmNewPasswordDisplayName)]
            [Compare(nameof(NewPassword), ErrorMessage = ErrorMessages.UserChangePasswordDoNotMatchErrorMessage)]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            var hasPassword = await this.userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return Page();
            }

            var user = await this.userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{this.userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await this.userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await this.signInManager.RefreshSignInAsync(user);
            await this.usersService.ModifyAsync(user.Id);
            this.StatusMessage = "Your password has been changed.";

            return RedirectToPage();
        }
    }
}
