namespace ForumNet.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;

    using Common;
    using Data.Models;
    using Data.Models.Enums;
    using Services.Messaging;
    using Services.Providers;
    using Services.Users;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ForumUser> signInManager;
        private readonly UserManager<ForumUser> userManager;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IUsersService usersService;
        private readonly IEmailSender emailSender;

        public RegisterModel(
            SignInManager<ForumUser> signInManager,
            UserManager<ForumUser> userManager,
            IDateTimeProvider dateTimeProvider,
            IUsersService usersService,
            IEmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.dateTimeProvider = dateTimeProvider;
            this.usersService = usersService;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(GlobalConstants.UserUsernameMaxLength, ErrorMessage = ErrorMessages.UserUsernameLengthErrorMessage, MinimumLength = GlobalConstants.UserUsernameMinLength)]
            [Display(Name = GlobalConstants.UserUsernameDisplayName)]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(GlobalConstants.UserPasswordMaxLength, ErrorMessage = ErrorMessages.UserPasswordLengthErrorMessage, MinimumLength = GlobalConstants.UserPasswordMinLength)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = GlobalConstants.UserConfirmPasswordDisplayName)]
            [Compare(nameof(Password), ErrorMessage = ErrorMessages.UserPasswordsDoNotMatchErrorMessage)]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                var isUsernameUsed = await this.usersService.IsUsernameUsedAsync(Input.Username);
                if (isUsernameUsed)
                {
                    this.ModelState.AddModelError(nameof(Input.Username), "There is already user with that username.");
                    return this.Page();
                }

                var usernameFirstLetter = char.ToLower(Input.Username[0]);
                var profilePicture = $"#icon-ava-{usernameFirstLetter}";

                var user = new ForumUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    ProfilePicture = profilePicture,
                    Gender = GenderType.NotKnown,
                    CreatedOn = this.dateTimeProvider.Now()
                };

                var result = await this.userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await this.emailSender.SendEmailAsync(
                        GlobalConstants.SystemEmail,
                        GlobalConstants.SystemName,
                        Input.Email,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
