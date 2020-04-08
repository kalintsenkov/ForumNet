namespace ForumNet.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using Common;
    using Data.Common;
    using Data.Models;
    using Data.Models.Enums;

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
            [Required]
            [DataType(DataType.Date)]
            [Display(Name = ModelConstants.BirthDateDisplayName)]
            public DateTime? BirthDate { get; set; }

            [Required]
            [EnumDataType(typeof(GenderType), ErrorMessage = ErrorMessages.InvalidGenderType)]
            [Display(Name = ModelConstants.GenderDisplayName)]
            public GenderType Gender { get; set; }

            [MaxLength(DataConstants.UserBiographyMaxLength)]
            public string Biography { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            await this.LoadAsync();
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync();
                return this.Page();
            }

            if (this.Input.BirthDate != user.BirthDate || this.Input.Gender != user.Gender)
            {
                user.BirthDate = this.Input.BirthDate;
                user.Gender = this.Input.Gender;
                user.Biography = this.Input.Biography;

                var updateResult = await this.userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    var userId = await this.userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
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
