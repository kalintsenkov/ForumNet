namespace ForumNet.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;

    using Data.Models;
    using Data.Models.Enums;
    using Infrastructure.Attributes;
    using Services.Providers.DateTime;
    using Services.Providers.Email;
    using Services.Users;

    using static Common.ErrorMessages;
    using static Common.GlobalConstants;

    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ForumUser> signInManager;
        private readonly UserManager<ForumUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly IUsersService usersService;
        private readonly IDateTimeProvider dateTimeProvider;

        public ExternalLoginModel(
            SignInManager<ForumUser> signInManager,
            UserManager<ForumUser> userManager,
            IEmailSender emailSender,
            IUsersService usersService,
            IDateTimeProvider dateTimeProvider)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.usersService = usersService;
            this.dateTimeProvider = dateTimeProvider;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = UserBirthDateDisplayName)]
            [MinAge(UserMinAge, ErrorMessage = UserAgeRestrictionErrorMessage)]
            public DateTime BirthDate { get; set; }

            [Required]
            [StringLength(UserUsernameMaxLength, ErrorMessage = UserUsernameLengthErrorMessage, MinimumLength = UserUsernameMinLength)]
            [Display(Name = UserUsernameDisplayName)]
            public string Username { get; set; }

            [Required]
            [StringLength(UserPasswordMaxLength, ErrorMessage = UserPasswordLengthErrorMessage, MinimumLength = UserPasswordMinLength)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = UserConfirmPasswordDisplayName)]
            [Compare(nameof(Password), ErrorMessage = UserPasswordsDoNotMatchErrorMessage)]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return this.RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                this.ErrorMessage = $"Error from external provider: {remoteError}";
                return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                this.ErrorMessage = "Error loading external login information.";
                return this.RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return this.LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                this.ReturnUrl = returnUrl;
                this.LoginProvider = info.LoginProvider;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    this.Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                this.ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

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
                    BirthDate = Input.BirthDate,
                    ProfilePicture = profilePicture,
                    Gender = GenderType.NotKnown,
                    CreatedOn = this.dateTimeProvider.Now()
                };

                var result = await this.userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    result = await this.userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {

                        var userId = await this.userManager.GetUserIdAsync(user);
                        var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await this.emailSender.SendEmailAsync(
                         SystemEmail,
                         SystemName,
                         Input.Email,
                         "Confirm your email",
                         $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return this.RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await this.signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return this.LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            this.LoginProvider = info.LoginProvider;
            this.ReturnUrl = returnUrl;
            return this.Page();
        }
    }
}
