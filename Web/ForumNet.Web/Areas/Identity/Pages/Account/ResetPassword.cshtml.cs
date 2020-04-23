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

    using Data.Models;
    using Services.Users;

    using static Common.ErrorMessages;
    using static Common.GlobalConstants;

    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ForumUser> userManager;
        private readonly IUsersService usersService;

        public ResetPasswordModel(
            UserManager<ForumUser> userManager, 
            IUsersService usersService)
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
            [StringLength(UserPasswordMaxLength, ErrorMessage = UserPasswordLengthErrorMessage, MinimumLength = UserPasswordMinLength)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = UserConfirmPasswordDisplayName)]
            [Compare(nameof(Password), ErrorMessage = UserPasswordsDoNotMatchErrorMessage)]
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
                this.Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };

                return this.Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return this.RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await this.userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                await this.usersService.ModifyAsync(user.Id);
                return this.RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return this.Page();
        }
    }
}
