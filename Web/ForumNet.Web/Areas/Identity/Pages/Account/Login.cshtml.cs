namespace ForumNet.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using Common;
    using Data.Models;
    using Services.Users;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ForumUser> userManager;
        private readonly SignInManager<ForumUser> signInManager;
        private readonly IUsersService usersService;

        public LoginModel(
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

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = GlobalConstants.UserLoginRememberMeDisplayName)]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            if (returnUrl == "/Identity/Account/Logout")
            {
                returnUrl = "/";
            }
            else
            {
                returnUrl = returnUrl ?? Url.Content("~/");
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (returnUrl == "/Identity/Account/Logout")
            {
                returnUrl = "/";
            }
            else
            {
                returnUrl = returnUrl ?? Url.Content("~/");
            }

            if (this.ModelState.IsValid)
            {
                var isDeleted = await this.usersService.IsDeletedAsync(Input.Username);
                if (isDeleted)
                {
                    this.ModelState.AddModelError(string.Empty, "The username or password is incorrect.");
                    return Page();
                }

                var result = await signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "The username or password is incorrect.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
