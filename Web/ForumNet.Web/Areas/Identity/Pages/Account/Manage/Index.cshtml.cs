namespace ForumNet.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using Data.Models;
    using Data.Models.Enums;
    using Infrastructure.Attributes;

    using static Common.ErrorMessages;
    using static Common.GlobalConstants;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ForumUser> userManager;
        private readonly SignInManager<ForumUser> signInManager;

        public IndexModel(
            UserManager<ForumUser> userManager,
            SignInManager<ForumUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [DataType(DataType.Date)]
            [Display(Name = UserBirthDateDisplayName)]
            [MinAge(UserMinAge, ErrorMessage = UserAgeRestrictionErrorMessage)]
            public DateTime BirthDate { get; set; }

            [Required]
            [EnumDataType(typeof(GenderType), ErrorMessage = UserInvalidGenderType)]
            [Display(Name = UserGenderDisplayName)]
            public GenderType Gender { get; set; }

            [MaxLength(UserBiographyMaxLength)]
            public string Biography { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync();
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync();
                return this.Page();
            }

            user.BirthDate = this.Input.BirthDate;
            user.Gender = this.Input.Gender;
            user.Biography = this.Input.Biography;

            var updateResult = await this.userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var userId = await this.userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        private async Task LoadAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            this.Username = user.UserName;

            this.Input = new InputModel
            {
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Biography = user.Biography
            };
        }
    }
}
