namespace ForumNet.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;

    using Data.Common;
    using Data.Models;
    using Infrastructure;
    using Services.Contracts;

    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ForumUser> userManager;
        private readonly IUsersService usersService;

        public ResetPasswordModel(UserManager<ForumUser> userManager, IUsersService usersService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(DataConstants.UserPasswordMaxLength, ErrorMessage = ErrorMessages.PasswordLengthErrorMessage, MinimumLength = DataConstants.UserPasswordMinLength)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = ModelConstants.ConfirmPasswordDisplayName)]
            [Compare(nameof(Password), ErrorMessage = ErrorMessages.PasswordsDoNotMatchErrorMessage)]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return this.BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };

                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return Page();
            }

            var user = await this.userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await this.userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                await this.usersService.ModifyAsync(user.Id);
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
